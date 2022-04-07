using UnityEngine;

namespace Enemy.Drone.Motion {
  public class Drone1Motion : StateMachineBehaviour {
    private static readonly int Seeking = Animator.StringToHash("Seeking");
    
    private GameObject droneObj_;
    private Drone2 drone_;
    private GameObject sora_;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      droneObj_ ??= animator.gameObject;
      drone_ ??= droneObj_.GetComponent<Drone2>();
      sora_ ??= drone_.sora;
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      var trans = droneObj_.transform;
      var currentHash = stateInfo.shortNameHash;
      if (currentHash == Seeking) {

      }
    }

  }
}
