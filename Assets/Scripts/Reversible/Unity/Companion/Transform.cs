using Reversible.Value;
using UnityEngine;

namespace Reversible.Unity.Companion {
  public struct Transform : ICompanion {
    private readonly UnityEngine.Transform transform_;

    private struct Record {
      public Vector3 localPosition;
      public Vector3 localScale;
      public Quaternion localRotation;
    }

    private Dense<Record> record_;

    public Transform(Player player, UnityEngine.Transform transform) {
      var clock = player.Clock;
      transform_ = transform;
      record_ = new Dense<Record>(clock, new Record() {
        localPosition = transform_.localPosition,
        localScale = transform_.localScale,
        localRotation = transform_.localRotation,
      });
    }

    public void OnLeap() {
    }

    public void OnTick() {
      var trans = transform_;
      ref var record = ref record_.Mut;
      record.localPosition = trans.localPosition;
      record.localScale = trans.localScale;
      record.localRotation = trans.localRotation;
    }

    public void OnBack() {
      var trans = transform_;
      ref readonly var record = ref record_.Ref;
      trans.localPosition = record.localPosition;
      trans.localScale = record.localScale;
      trans.localRotation = record.localRotation;
    }
  }
}