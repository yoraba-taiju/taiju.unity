﻿using UnityEngine;

namespace Enemy {
  public interface IAnimatorEventSubscriber {
    public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {}
    public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {}
    public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {}
  }
}
