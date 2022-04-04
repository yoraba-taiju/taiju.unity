using UnityEngine;

namespace Reversible.Unity {
  public abstract class ReversibleBehaviour: MonoBehaviour {
    protected ClockHolder clockHolder;
    protected Clock clock;
    protected PlayerInput playerInput;
    protected abstract void OnStart();
    private void Start() {
      clockHolder = GameObject.FindGameObjectWithTag("Clock").GetComponent<ClockHolder>();
      playerInput = clockHolder.PlayerInput;
      clock = clockHolder.Clock;
      OnStart();
    }
    protected abstract void OnForward();

    protected virtual void OnReverse() {
      
    }
    private void Update() {
      if (clock.IsTicking) {
        OnForward();
      } else {
        OnReverse();
      }
    }
  }
}