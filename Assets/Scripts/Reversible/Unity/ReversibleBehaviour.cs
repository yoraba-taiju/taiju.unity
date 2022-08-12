using System.Runtime.CompilerServices;
using UnityEngine;

namespace Reversible.Unity {
  public abstract class ReversibleBehaviour : MonoBehaviour {
    protected ClockController clockController;
    protected World world;
    protected Clock clock;
    protected PlayerInput playerInput;

    protected float CurrentTime {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => clockController.CurrentTime;
    }

    protected abstract void OnStart();
    protected abstract void OnForward();

    public void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      clockController = clockObj.GetComponent<ClockController>();
      world = clockObj.GetComponent<World>();
      playerInput = clockController.PlayerInput;
      clock = clockController.Clock;
      OnStart();
    }

    protected virtual void OnReverse() {
    }

    protected virtual void OnLeap() {
    }

    public void Update() {
      if (clockController.Leaped) {
        OnLeap();
        return;
      }

      if (clockController.IsForwarding) {
        OnForward();
      } else {
        OnReverse();
      }
    }

    protected void Destroy() {
      world.Destroy(gameObject);
    }
  }
}