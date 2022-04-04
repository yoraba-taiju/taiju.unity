using UnityEngine;

namespace Reversible.Unity {
  public abstract class ReversibleBehaviour: MonoBehaviour {
    private ClockHolder clockHolder_;
    protected Clock clock;
    protected PlayerInput playerInput;
    protected abstract void OnStart();
    private void Start() {
      clockHolder_ = GameObject.FindGameObjectWithTag("Clock").GetComponent<ClockHolder>();
      playerInput = clockHolder_.PlayerInput;
      clock = clockHolder_.Clock;
      OnStart();
    }
    protected abstract void OnForward();

    protected virtual void OnReverse() {}
    private void Update() {
      if (clock.IsTicking) {
        OnForward();
      } else {
        OnReverse();
      }
    }
  }
}