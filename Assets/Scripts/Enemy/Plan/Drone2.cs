using Donut.Unity;
using UnityEngine;

namespace Enemy.Plan {
  public class Drone2: PlannerBehaviour<Drone2.State> {
    public enum State {
      Seek,
      Watching,
      Quit,
    }

    private GameObject sora_;

    protected override void OnStart(out State first) {
      first = State.Seek;
      sora_ = GameObject.FindWithTag("Player");
    }

    protected override void OnDispatch(ref State self) {
      switch (self) {
        case State.Seek: {
          var delta = sora_.transform.position - transform.position;
          if (delta.magnitude > 0.5f) {
            transform.Translate(delta.normalized * 0.166f);
          } else {
            self = State.Watching;
          }
          break;
        }
        case State.Watching:
          self = State.Quit;
          break;
        case State.Quit:
          transform.Translate(Vector3.right* 0.166f);
          break;
        default:
          self = State.Quit;
          break;
      }
    }
  }
}