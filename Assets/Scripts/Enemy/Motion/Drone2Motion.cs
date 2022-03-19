using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.Motion {
  public class Drone2Motion : StateMachineBehaviour {
    private static readonly int Seeking = Animator.StringToHash("Seeking");
    private static readonly int Watching = Animator.StringToHash("Watching");
    private static readonly int Return = Animator.StringToHash("Return");
    private static readonly int ToWatching = Animator.StringToHash("ToWatching");

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
        var delta = sora_.transform.position - trans.position;
        if (delta.magnitude <= 3.0f) {
          animator.SetTrigger(ToWatching);
        } else {
          trans.localPosition += delta.normalized * 3.0f * Time.deltaTime;;
        }
      } else if (currentHash == Watching) {
        var delta = sora_.transform.position - trans.position;
        var d = Math.Clamp(delta.magnitude - 4.0f, -0.9f, 0.9f);
        trans.localPosition += delta.normalized * d * d * d * Time.deltaTime;
      } else if (currentHash == Return) {
        var pos = trans.localPosition;
        pos.x += 3.0f * Time.deltaTime;
        trans.localPosition = pos;
      }
    }

  }
}
