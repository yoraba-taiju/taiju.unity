using UnityEngine;

namespace Donut.Unity {
  public abstract class DonutBehaviour: MonoBehaviour {
    protected Clock clock;
    protected PlayerInput playerInput;
    protected abstract void OnStart();
    private void Start() {
      var clockHolder = GameObject.FindGameObjectWithTag("Clock").GetComponent<ClockHolder>();
      playerInput = clockHolder.PlayerInput;
      clock = clockHolder.Clock;
      OnStart();
    }
    protected abstract void OnUpdate();
    private void Update() {
      OnUpdate();
    }
  }
}