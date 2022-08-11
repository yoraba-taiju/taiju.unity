using UnityEngine;

namespace Reversible.Unity {
  public abstract class ReversibleBehaviour: MonoBehaviour {
    protected ClockHolder clockHolder;
    protected World world;
    protected Clock clock;
    protected PlayerInput playerInput;

    protected abstract void OnStart();
    protected abstract void OnForward();

    public void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      clockHolder = clockObj.GetComponent<ClockHolder>();
      world = clockObj.GetComponent<World>();
      playerInput = clockHolder.PlayerInput;
      clock = clockHolder.Clock;
      OnStart();
    }

    protected virtual void OnReverse() {}
    protected virtual void OnLeap() {}
    public void Update() {
      if (clockHolder.Leaped) {
        OnLeap();
        return;
      }
      if (clockHolder.IsLeaping) {
        OnReverse();
      } else {
        OnForward();
      }
    }

    protected void Destroy() {
      world.Destroy(gameObject);
    }
  }
}
