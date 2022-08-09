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
      world.Register(gameObject);
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
      world.Destroy(gameObject);
    }
  }
}
