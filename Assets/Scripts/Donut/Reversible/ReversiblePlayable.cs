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
    
    private struct State {
      public double time;
      public bool isPlaying;
    }

    // Transform records
    private Dense<State> states_;
    private void Start() {
      holder_ = GameObject.FindGameObjectWithTag("Clock").GetComponent<ClockHolder>();
      clock_ = holder_.Clock;
      bornAt_ = clock_.CurrentTick;
      playableDirector_ = gameObject.GetComponent<PlayableDirector>();
      states_ = new Dense<State>(clock_, new State {
        time = playableDirector_.time,
        isPlaying = playableDirector_.state == PlayState.Playing,
      });
    }

    private void Update() {
      if (bornAt_ > clock_.CurrentTick) {
        Destroy(gameObject);
        return;
      }
      if (holder_.Ticked) {
        ref var state = ref states_.Mut;
        state.time = playableDirector_.time;
        state.isPlaying = playableDirector_.state == PlayState.Playing;
      } else if (holder_.Backed) {
        // Debug.Log($"Back: {clock_.CurrentTick}: {playableDirector_.time} -> {time_.Ref}");
        // time_.Debug();
        ref readonly var state = ref states_.Ref;
        playableDirector_.time = state.time;
        if (state.isPlaying) {
          playableDirector_.Play();
        } else {
          playableDirector_.Pause();
        }
      }
    }
  }
}
