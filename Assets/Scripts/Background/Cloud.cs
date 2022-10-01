using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Background {
  public class Cloud: ReversibleBase {
    [SerializeField] private float speed;
    private Dense<float> xPosition_;
    protected new void Start() {
      base.Start();
      xPosition_ = new Dense<float>(clock, transform.localPosition.x);
    }
    protected override void OnUpdate() {
      var trans = transform;
      ref var x = ref xPosition_.Mut;
      switch (x) {
        case > 40.1f:
          x = -40.0f;
          break;
        case < -40.1f:
          x = 40.0f;
          break;
        default:
          x += speed * Time.deltaTime;
          break;
      }
      var pos = trans.localPosition;
      pos.x = x;
      trans.localPosition = pos;
    }

    public override void OnDeactivated() {
    }

    public override void OnReactivated() {
    }
  }
}