using UnityEngine;

namespace Enemy.Drone.Motion {
  public class Drone1Motion : StateMachineBehaviour {
    private static readonly int Seeking = Animator.StringToHash("Seeking");
    private static readonly int Fighting = Animator.StringToHash("Fighting");
    private static readonly int ToFighting = Animator.StringToHash("ToFighting");
    private static readonly int ToSeeking = Animator.StringToHash("ToSeeking");
    private GameObject droneObj_;
    private Drone1 drone_;
    private GameObject sora_;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      droneObj_ ??= animator.gameObject;
      drone_ ??= droneObj_.GetComponent<Drone1>();
      sora_ ??= drone_.sora;
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      var trans = droneObj_.transform;
      var currentHash = stateInfo.shortNameHash;
      if (currentHash == Seeking) {
        var delta = sora_.transform.position - trans.position;
        if (delta.magnitude <= 5.0f) {
          animator.SetTrigger(ToFighting);
        } else {
          trans.localPosition += delta.normalized * 2.5f * Time.deltaTime;;
        }
      } else if (currentHash == Fighting) {
        var e = Instantiate(drone_.bullet, trans.parent);
        e.transform.localPosition = trans.localPosition;
        animator.SetTrigger(ToSeeking);
      }
    }

  }
}
