using System.Runtime.CompilerServices;
using UnityEngine;

namespace Reversible.Unity.Specialized {
  public abstract class SpecializedReversibleComponentBehaviour : MonoBehaviour {
    private uint bornAt_;
    protected ClockController clockController;
    protected World world;
    protected Clock clock;

    protected float CurrentTime {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => clockController.CurrentTime;
    }

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
      if (clockController.Ticked) {
        OnTick();
      } else if (clockController.Backed) {
        OnBack();
      } else if (clockController.Leaped) {
        OnLeap();
      }
    }

    protected abstract void OnStart();

    protected abstract void OnTick();

    protected abstract void OnBack();

    protected abstract void OnLeap();
  }
}