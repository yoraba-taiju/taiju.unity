using Donut;
using Donut.Unity;
using Donut.Values;
using UnityEngine;

namespace Enemy.Plan {
  public abstract class StateBehaviour<T>: DonutBehaviour where T: struct {
    private Dense<T> current_;
    protected Dense<Vector3> speed;
    protected Dense<Quaternion> rotationSpeed;

    protected override void OnStart() {
      OnStart(out var first);
      current_ = new Dense<T>(clock, first);
      speed = new Dense<Vector3>(clock, Vector3.zero);
      rotationSpeed = new Dense<Quaternion>(clock, Quaternion.identity);
    }

    protected override void OnUpdate() {
      if (clockHolder.Ticked) {
        OnDispatch(ref current_.Mut);
      }
      var trans = transform;
      trans.position += speed.Ref * Time.deltaTime;
      trans.rotation *= Quaternion.Lerp(Quaternion.identity, rotationSpeed.Ref, Time.deltaTime);
    }

    protected abstract void OnStart(out T first);
    protected abstract void OnDispatch(ref T self);
  }
}