using UnityEngine;

namespace Reversible.Unity {
  public abstract class RawReversibleBehaviour : MonoBehaviour {
    protected ClockHolder clockHolder;
    protected World world;
    protected Clock clock;

    protected abstract void OnStart();

    public void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      clockHolder = clockObj.GetComponent<ClockHolder>();
      world = clockObj.GetComponent<World>();
      clock = clockHolder.Clock;
      OnStart();
    }

    public void Update() {
      if (clockHolder.Ticked) {
        OnTick();
      } else if (clockHolder.Backed) {
        OnBack();
      } else if (clockHolder.Leaped) {
        OnLeap();
      }
    }

    protected abstract void OnTick();

    protected abstract void OnBack();

    protected abstract void OnLeap();
  }
}