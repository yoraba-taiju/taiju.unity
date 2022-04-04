using UnityEngine;
using Reversible.Unity;
using Reversible.Value;

namespace Reversible.Companion {
  public struct Rigidbody2D : ICompanion {
    private readonly UnityEngine.Rigidbody2D body_;

    private struct Record {
      public Vector2 position;
      public Vector2 velocity;
      public float rotation;
      public float angularVelocity;
    }
    private Dense<Record> record_;

    public Rigidbody2D(ClockHolder holder, UnityEngine.Rigidbody2D body) {
      body_ = body;
      record_ = new Dense<Record>(holder.Clock, new Record {
        position = body_.position,
        velocity = body_.velocity,
        rotation = body_.rotation,
        angularVelocity = body_.angularVelocity,
      });
    }

    public void OnLeap() {
    }

    public void OnTick() {
      ref var record = ref record_.Mut;
      record = new Record {
        position = body_.position,
        velocity = body_.velocity,
        rotation = body_.rotation,
        angularVelocity = body_.angularVelocity,
      };
    }

    public void OnBack() {
      ref readonly var record = ref record_.Ref;
      body_.position = record.position;
      body_.velocity = record.velocity;
      body_.rotation = record.rotation;
      body_.angularVelocity = record.angularVelocity;
    }
  }
}
