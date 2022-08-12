using UnityEngine;

namespace Enemy.StateMachine {
  public class AnimationEventPublisher : StateMachineBehaviour {
    private IAnimatorEventSubscriber subscriber_;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      subscriber_ ??= animator.GetComponent<IAnimatorEventSubscriber>();
      subscriber_.OnStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      subscriber_ ??= animator.GetComponent<IAnimatorEventSubscriber>();
      subscriber_.OnStateUpdate(animator, stateInfo, layerIndex);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      subscriber_ ??= animator.GetComponent<IAnimatorEventSubscriber>();
      subscriber_.OnStateExit(animator, stateInfo, layerIndex);
    }
  }
}