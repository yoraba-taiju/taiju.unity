using System.Collections.Generic;
using Donut.Unity;
using Donut.Values;
using UnityEngine;

namespace Enemy.Motion {
  public abstract class MotionBehaviour<T>: DonutBehaviour where T: struct {
    private Dense<State> state_;

    private struct State {
      public T state;
      public float totalDuration;
      public Motion motion;
    }

    protected override void OnStart() {
      var first = new State();
      first.motion = OnStart(out first.state);
      first.totalDuration = 0.0f;
      state_ = new Dense<State>(clock, first);
    }

    protected override void OnForward() {
      ref var state = ref state_.Mut;
      if (clockHolder.Ticked) {
        var nextMotion = OnDispatch(ref state.state);
        if (nextMotion != null) {
          state.motion = nextMotion;
          state.totalDuration = 0.0f;
        }
      }
      state.motion.Move(gameObject, Time.deltaTime, state.totalDuration);
      state.totalDuration += Time.deltaTime;
    }

    protected abstract Motion OnStart(out T self);
    protected abstract Motion OnDispatch(ref T self);
    
    protected abstract class Motion {
      public abstract void Move(GameObject self, float deltaTime, float fromStart);

      public virtual Chained Then(Motion nextMotion) {
        return new Chained(this, nextMotion);
      }
    }

    protected class While: Motion {
      private readonly Motion motion_;
      private readonly float duration_;
      public While(Motion motion, float duration) {
        motion_ = motion;
        duration_ = duration;
      }
      public override void Move(GameObject self, float deltaTime, float fromStart) {
        if (fromStart <= duration_) {
          motion_.Move(self, deltaTime, fromStart);
        }
      }
    }

    protected class Chained: Motion {
      private readonly List<Motion> motions_ = new ();

      public Chained(Motion a, Motion b) {
        motions_.Add(a);
        motions_.Add(b);
      }

      public override void Move(GameObject self, float deltaTime, float fromStart) {
        foreach (var motion in motions_) {
          motion.Move(self, deltaTime, fromStart);
        }
      }

      public override Chained Then(Motion nextMotion) {
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
      public override void Move(GameObject self, float deltaTime, float fromStart) {
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
      public override void Move(GameObject self, float deltaTime, float fromStart) {
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
      public override void Move(GameObject self, float deltaTime, float fromStart) {
        self.transform.rotation *= Quaternion.Lerp(Quaternion.identity,quaternion_, deltaTime * speed_);
      }
    }
  }
}