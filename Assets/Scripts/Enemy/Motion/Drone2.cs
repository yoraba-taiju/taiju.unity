using Donut.Unity;
using UnityEngine;

namespace Enemy.Motion {
  public class Drone2: MotionBehaviour<Drone2.State> {
    public enum State {
      Seek,
      Watching,
      Quit,
    }

    private GameObject sora_;

    protected override Motion OnStart(out State self) {
      sora_ = GameObject.FindWithTag("Player");
      self = State.Seek;
      return new MoveToTarget(sora_, 3.0f);
    }

    protected override Motion OnDispatch(ref State self) {
      var trans = transform;
      Motion motion = null;
      switch (self) {
        case State.Seek: {
          var delta = sora_.transform.position - transform.position;
          var mag = delta.magnitude;
          if (mag <= 3.0f) {
            self = State.Watching;
            motion = new MoveToTarget(sora_, 0.5f).Then(new RotateConstant(Quaternion.Euler(0.0f, 180.0f, 0.0f), 1f));
          }
          break;
        }
        case State.Watching:
          // FIXME
          if (trans.rotation.eulerAngles.y < 180.0f) {
            self = State.Quit;
            motion = new MoveConstant(Vector3.right, 3.0f);
          }
          break;
        case State.Quit:
          break;
        default:
          self = State.Quit;
          break;
      }
      return motion;
    }
  }
}