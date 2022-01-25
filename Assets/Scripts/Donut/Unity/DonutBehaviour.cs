using UnityEngine;

namespace Donut.Unity {
  public abstract class DonutBehaviour: MonoBehaviour {
    protected Clock clock;
    protected PlayerInput playerInput;
    protected abstract void OnStart();
    private void Start() {
      var obj = GameObject.FindGameObjectWithTag("Clock");
      var clockComponent = obj.GetComponent<ClockComponent>();
      playerInput = clockComponent.PlayerInput;
      clock = clockComponent.Clock;
      OnStart();
    }
    protected abstract void OnUpdate();
    private void Update() {
      OnUpdate();
    }
  }
}