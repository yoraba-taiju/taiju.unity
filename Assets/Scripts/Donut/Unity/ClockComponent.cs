using UnityEngine;
using UnityEngine.Playables;

namespace Donut.Unity {
  public sealed class ClockComponent: MonoBehaviour {
    // getter
    public PlayerInput PlayerInput { get; } = new();
    public Clock Clock { get; } = new();

    /* player info */
    private PlayerInput playerInput_;
    private PlayableDirector playableDirector_;

    /* Timeline */
    private readonly double[] timeHistory_ = new double[Clock.HISTORY_LENGTH];

    private void Start() {
      playableDirector_ = gameObject.GetComponent<PlayableDirector>();
      PlayerInput.Enable();
    }

    private void LateUpdate() {
      var player = playerInput_.Player;
      var backPressed = player.BackClock;
      if (backPressed.IsPressed()) {
        Clock.Back();
        playableDirector_.time = timeHistory_[Clock.CurrentTick % Clock.HISTORY_LENGTH];
      } else {
        Clock.Tick();
        timeHistory_[Clock.CurrentTick % Clock.HISTORY_LENGTH] = playableDirector_.time;
      }
    }
  }
}