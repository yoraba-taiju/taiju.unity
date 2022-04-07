using UnityEngine;

namespace Reversible.Unity {
  public abstract class ReversibleBehaviour: MonoBehaviour {
    private ClockHolder clockHolder_;
    private Graveyard graveyard_;
    protected Clock clock;
    protected PlayerInput playerInput;

    protected abstract void OnStart();
    protected abstract void OnForward();

    public void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      clockHolder_ = clockObj.GetComponent<ClockHolder>();
      graveyard_ = clockObj.GetComponent<Graveyard>();
      playerInput = clockHolder_.PlayerInput;
      clock = clockHolder_.Clock;
      OnStart();
    }

    protected virtual void OnReverse() {}
    public void Update() {
      if (clock.IsTicking) {
        OnForward();
      } else {
        OnReverse();
      }
    }

    protected void Destroy() {
      graveyard_.Destroy(gameObject);
    }
  }
}
