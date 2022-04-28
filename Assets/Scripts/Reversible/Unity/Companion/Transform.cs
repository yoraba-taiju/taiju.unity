using System.Linq;
using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Reversible.Unity.Companion {
  public struct Transform: ICompanion {
    private readonly UnityEngine.Transform transform_;

    private struct Record {
      public Vector3 position;
      public Vector3 scale;
      public Quaternion rot;
    }
    private Dense<Record> record_;

    public Transform(ClockHolder holder, UnityEngine.Transform transform) {
      transform_ = transform;
      record_ = new Dense<Record>(holder.Clock, new Record() {
        position = transform_.localPosition,
        scale = transform_.localScale,
        rot = transform_.localRotation,
      });
    }

    public void OnLeap() {
    }
    public void OnTick() {
      var trans = transform_;
      ref var record = ref record_.Mut;
      record.position = trans.localPosition;
      record.scale = trans.localScale;
      record.rot = trans.localRotation;
    }

    public void OnBack() {
      var trans = transform_;
      ref readonly var record = ref record_.Ref;
      trans.localPosition = record.position;
      trans.localScale = record.scale;
      trans.localRotation = record.rot;
    }
  }
}
