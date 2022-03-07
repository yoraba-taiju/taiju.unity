using System;
using System.Collections;
using System.Collections.Generic;
using Donut.Unity;
using Donut.Values;
using UnityEngine;

namespace Enemy.Motion {
  public abstract class MotionBehaviour<T>: DonutBehaviour where T: struct {
    private Dense<State> state_;

    private struct State {
      public T state;
      public Motion motion;
    }

    protected override void OnStart() {
      var first = new State();
      first.motion = OnStart(out first.state);
      state_ = new Dense<State>(clock, first);
    }

    protected override void OnUpdate() {
      ref var state = ref state_.Mut;
      if (clockHolder.Ticked) {
        var nextMotion = OnDispatch(ref state.state);
        if (nextMotion != null) {
          state.motion = nextMotion;
        }
      }
      state.motion?.Move(gameObject, Time.deltaTime);
    }

    protected abstract Motion OnStart(out T self);
    protected abstract Motion OnDispatch(ref T self);
    
    protected abstract class Motion {
      public abstract void Move(GameObject self, float deltaTime);

      public virtual Motion Chain(Motion nextMotion) {
        return new Chained(this, nextMotion);
      }
    }

    protected class Chained: Motion {
      private readonly List<Motion> motions_ = new ();

      public Chained(Motion a, Motion b) {
        motions_.Add(a);
        motions_.Add(b);
      }

      public override void Move(GameObject self, float deltaTime) {
        foreach (var motion in motions_) {
          motion.Move(self, deltaTime);
        }
      }

      public override Motion Chain(Motion nextMotion) {
        motions_.Add(nextMotion);
        return this;
      }
    }

    protected class MoveToTarget: Motion {
      private readonly GameObject target_;
      private readonly float speed_;
      public MoveToTarget(GameObject target, float speed) {
        target_ = target;
        speed_ = speed;
      }
      public override void Move(GameObject self, float deltaTime) {
        var trans = self.transform;
        var vec = (target_.transform.position - trans.position).normalized;
        trans.Translate(vec * deltaTime * speed_);
      }
    }

    protected class MoveConstant: Motion {
      private readonly Vector3 direction_;
      private readonly float speed_;
      public MoveConstant(Vector3 direction, float speed) {
        direction_ = direction;
        speed_ = speed;
      }
      public override void Move(GameObject self, float deltaTime) {
        self.transform.position += direction_ * deltaTime * speed_;
      }
    }

    protected class RotateConstant: Motion {
      private readonly Quaternion quaternion_;
      private readonly float speed_;
      public RotateConstant(Quaternion quaternion, float speed) {
        quaternion_ = quaternion;
        speed_ = speed;
      }
      public override void Move(GameObject self, float deltaTime) {
        self.transform.rotation *= Quaternion.Lerp(Quaternion.identity,quaternion_, deltaTime * speed_);
      }
    }
  }
}