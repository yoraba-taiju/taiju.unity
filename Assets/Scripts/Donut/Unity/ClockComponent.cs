using UnityEngine;
using UnityEngine.Playables;

namespace Donut.Unity {
  public sealed class ClockComponent: MonoBehaviour {
    /* Clock */
    private readonly Clock clock_ = new();

    /* player info */
    private PlayerInput playerInput_;
    private PlayableDirector playableDirector_;

    /* Timeline */
    private readonly double[] timeHistory_ = new double[Clock.HISTORY_LENGTH];

    private void Start() {
      playableDirector_ = gameObject.GetComponent<PlayableDirector>();
      playerInput_ = new PlayerInput();
      playerInput_.Enable();
    }

    private void LateUpdate() {
      var player = playerInput_.Player;
      var backPressed = player.BackClock;
      if (backPressed.IsPressed()) {
        clock_.Back();
        playableDirector_.time = timeHistory_[clock_.CurrentTick % Clock.HISTORY_LENGTH];
      } else {
        clock_.Tick();
        timeHistory_[clock_.CurrentTick % Clock.HISTORY_LENGTH] = playableDirector_.time;
      }
    }

    // getter
    public PlayerInput PlayerInput => playerInput_;
    public Clock Clock => clock_;
  }
}