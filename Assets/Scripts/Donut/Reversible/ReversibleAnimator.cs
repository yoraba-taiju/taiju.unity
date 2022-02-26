using Donut.Unity;
using Donut.Values;
using UnityEngine;

namespace Donut.Reversible {
  public class ReversibleAnimator: MonoBehaviour {
    // Clock
    private Clock clock_;
    private uint BornAt { get; set; }
    private Animator animator_;

    // Animation records
    private Dense<int> state_;
    private Dense<float> time_;
    private void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      clock_ = clockObj.GetComponent<ClockHolder>().Clock;
      animator_ = gameObject.GetComponent<Animator>();
      BornAt = clock_.CurrentTick;

      var info = animator_.GetCurrentAnimatorStateInfo(0);
      state_ = new Dense<int>(clock_,  info.shortNameHash);
      time_ = new Dense<float>(clock_, info.normalizedTime);
    }

    private void Update() {
      if (BornAt > clock_.CurrentTick) {
        Destroy(gameObject);
        return;
      }
      if (clock_.IsTicking) {
        var info = animator_.GetCurrentAnimatorStateInfo(0);
        state_.Value = info.shortNameHash;
        time_.Value = info.normalizedTime;
      } else {
        animator_.Play(state_.Value, -1, time_.Value);
      }
    }
  }
}
