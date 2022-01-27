using UnityEngine;
using UnityEngine.Playables;

namespace Donut.Unity {
  public sealed class ClockComponent: MonoBehaviour {
    // getter
    public PlayerInput PlayerInput { get; private set; }
    public Clock Clock { get; } = new();

    /* player info */
    private PlayableDirector playableDirector_;

    /* Timeline */
    private readonly double[] timeHistory_ = new double[Clock.HISTORY_LENGTH];
    
    /* Ticking */
    private float timeToTick_;

    private void Start() {
      PlayerInput = new PlayerInput();
      PlayerInput.Enable();
      playableDirector_ = gameObject.GetComponent<PlayableDirector>();
      timeToTick_ = 1.0f / 30.0f;
    }

    private void LateUpdate() {
      timeToTick_ -= Time.deltaTime;
      if (timeToTick_ > 0.0f) {
        return;
      }

      var player = PlayerInput.Player;
      var backPressed = player.BackClock;
      while (timeToTick_ < 0) {
        timeToTick_ += 1.0f / 30.0f;
      }
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