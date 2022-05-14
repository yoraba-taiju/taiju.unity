using UnityEngine;

namespace Enemy.StateMachine {
  public class AnimationEventPublisher : StateMachineBehaviour  {
    
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash) {
      Debug.Log("OnStateMachineEnter");
    }
    
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      animator.GetComponent<IAnimatorEventSubscriber>().OnStateEnter(animator, stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      animator.GetComponent<IAnimatorEventSubscriber>().OnStateUpdate(animator, stateInfo, layerIndex);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      animator.GetComponent<IAnimatorEventSubscriber>().OnStateExit(animator, stateInfo, layerIndex);
    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    }
  }
}
