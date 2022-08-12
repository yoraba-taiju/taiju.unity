using System.Runtime.CompilerServices;
using UnityEngine;

namespace Reversible.Unity {
  public abstract class RawReversibleBehaviour : MonoBehaviour {
    protected ClockController clockController;
    protected World world;
    protected Clock clock;
    private uint bornAt_;

    protected float CurrentTime {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => clockController.CurrentTime;
    }

    protected abstract void OnStart();

    public void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      clockController = clockObj.GetComponent<ClockController>();
      world = clockObj.GetComponent<World>();
      clock = clockController.Clock;
      bornAt_ = clock.CurrentTick;
      OnStart();
    }

    public void Update() {
      if (bornAt_ > clock.CurrentTick) {
        Destroy(gameObject);
        return;
      }

      if (clockController.IsForwarding) {
        OnForward();
      } else {
        OnReverse();
      }
    }

    protected abstract void OnForward();
    protected abstract void OnReverse();
  }
}