using System.Linq;
using Donut.Unity;
using Donut.Values;
using UnityEngine;
using UnityEngine.Playables;

namespace Donut.Reversible {
  public class ReversiblePlayable : MonoBehaviour {
  // Clock
  private ClockHolder holder_;
    private Clock clock_;
    private PlayableDirector playableDirector_;
    private uint bornAt_;

    // Transform records
    private Dense<double> time_;
    private void Start() {
      holder_ = GameObject.FindGameObjectWithTag("Clock").GetComponent<ClockHolder>();
      clock_ = holder_.Clock;
      bornAt_ = clock_.CurrentTick;
      playableDirector_ = gameObject.GetComponent<PlayableDirector>();
      time_ = new Dense<double>(clock_, playableDirector_.time);
    }

    private void Update() {
      if (bornAt_ > clock_.CurrentTick) {
        Destroy(gameObject);
        return;
      }
      if (holder_.Ticked) {
        time_.Mut = playableDirector_.time;
      } else if (holder_.Backed) {
        // Debug.Log($"Back: {clock_.CurrentTick}: {playableDirector_.time} -> {time_.Ref}");
        // time_.Debug();
        playableDirector_.time = time_.Ref;
        if (playableDirector_.state != PlayState.Playing) {
          playableDirector_.Play();
        }
      }
    }
  }
}
