using Donut.Unity;
using UnityEngine;

namespace Enemy.Plan {
  public class Drone2: StateBehaviour<Drone2.State> {
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
      var trans = transform;
      switch (self) {
        case State.Seek: {
          var delta = sora_.transform.position - transform.position;
          var mag = delta.magnitude;
          if (mag > 2.0f) {
            speed.Mut = delta.normalized * 20.0f / mag;
            rotationSpeed.Mut = Quaternion.identity;
          } else {
            self = State.Watching;
            speed.Mut = Vector3.zero;
            rotationSpeed.Mut = Quaternion.Euler(0.0f, 90.0f, 0.0f);
          }
          break;
        }
        case State.Watching:
          if (trans.transform.rotation.eulerAngles.y > 180.0f) {
            self = State.Quit;
            trans.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            rotationSpeed.Mut = Quaternion.identity;
          }
          break;
        case State.Quit:
          speed.Mut = Vector3.right * 5.0f;
          break;
        default:
          self = State.Quit;
          break;
      }
    }
  }
}