using System;
using UnityEngine;
using Transform = Reversible.Companion.Transform;

namespace Enemy.Drone.Motion {
  public class Drone0Motion : StateMachineBehaviour {
    private static readonly int Seeking = Animator.StringToHash("Seeking");
    private static readonly int Watching = Animator.StringToHash("Watching");
    private static readonly int Return = Animator.StringToHash("Return");
    private static readonly int ToWatching = Animator.StringToHash("ToWatching");

    private GameObject droneObj_;
    private Transform transform_;
    private Drone0 drone_;
    private Rigidbody2D rigidbody_;
    private GameObject sora_;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      droneObj_ ??= animator.gameObject; 
      drone_ ??= droneObj_.GetComponent<Drone0>();
      rigidbody_ ??= droneObj_.GetComponent<Rigidbody2D>();
      sora_ ??= drone_.sora;
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      var currentHash = stateInfo.shortNameHash;
      var delta = (Vector2)(sora_.transform.position - droneObj_.transform.position);
      if (currentHash == Seeking) {
        if (delta.magnitude <= 6.0f) {
          animator.SetTrigger(ToWatching);
        } else {
          rigidbody_.velocity = delta.normalized * 7.0f;
        }
      } else if (currentHash == Watching) {
        var d = Math.Clamp(delta.magnitude - 4.0f, -0.9f, 0.9f);
        rigidbody_.velocity = delta.normalized * d;
      } else if (currentHash == Return) {
        rigidbody_.velocity = Vector2.right * 3.0f;
      }
    }
  }
}
