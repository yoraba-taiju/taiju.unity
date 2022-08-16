using Reversible.Value;
using UnityEngine.Playables;

namespace Reversible.Unity.Companion {
  public struct PlayableDirector : ICompanion {
    private readonly UnityEngine.Playables.PlayableDirector playableDirector_;

    private struct State {
      public double time;
      public bool isPlaying;
    }

    private Dense<State> states_;

    public PlayableDirector(Player player, UnityEngine.Playables.PlayableDirector playableDirector) {
      var clock = player.Clock;
      playableDirector_ = playableDirector;
      states_ = new Dense<State>(clock, new State {
        time = playableDirector_.time,
        isPlaying = playableDirector_.state == PlayState.Playing,
      });
    }

    public void OnTick() {
      ref var state = ref states_.Mut;
      state.time = playableDirector_.time;
      state.isPlaying = playableDirector_.state == PlayState.Playing;
    }

    public void OnBack() {
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

    public void OnLeap() {
    }
  }
}