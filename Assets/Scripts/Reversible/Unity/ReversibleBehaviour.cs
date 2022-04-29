using UnityEngine;

namespace Reversible.Unity {
  public abstract class ReversibleBehaviour: MonoBehaviour {
    protected ClockHolder clockHolder;
    private Graveyard graveyard_;
    protected Clock clock;
    protected PlayerInput playerInput;

    protected abstract void OnStart();
    protected abstract void OnForward();

    public void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      clockHolder = clockObj.GetComponent<ClockHolder>();
      graveyard_ = clockObj.GetComponent<Graveyard>();
      playerInput = clockHolder.PlayerInput;
      clock = clockHolder.Clock;
      OnStart();
    }

    protected virtual void OnReverse() {}
    public void Update() {
      if (clockHolder.IsLeaping) {
        OnReverse();
      } else {
        OnForward();
      }
    }

    protected void Destroy() {
      graveyard_.Destroy(gameObject);
    }
  }
}
