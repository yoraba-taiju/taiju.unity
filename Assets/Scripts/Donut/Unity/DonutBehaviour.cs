using UnityEngine;

namespace Donut.Unity {
  public abstract class DonutBehaviour: MonoBehaviour {
    private ClockComponent clockComponent_;
    protected Clock clock;
    protected PlayerInput PlayerInput => clockComponent_.PlayerInput;
    protected abstract void OnStart();
    private void Start() {
      var obj = GameObject.FindGameObjectWithTag("Clock");
      clockComponent_ = obj.GetComponent<ClockComponent>();
      clock = clockComponent_.Clock;
      OnStart();
    }
    protected abstract void OnUpdate();
    private void Update() {
      OnUpdate();
    }
  }
}