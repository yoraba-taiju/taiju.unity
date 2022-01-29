using UnityEngine;
using UnityEngine.Playables;

namespace Donut.Unity {
  public sealed class ClockHolder: MonoBehaviour {
    // getter
    public PlayerInput PlayerInput { get; private set; }
    public Clock Clock { get; } = new();

    /* Ticking */
    private float timeToTick_;

    private void Start() {
      PlayerInput = new PlayerInput();
      PlayerInput.Enable();
      timeToTick_ = 1.0f / 30.0f;
    }

    private void LateUpdate() {
      timeToTick_ -= Time.deltaTime;
      if (timeToTick_ > 0.0f) {
        return;
      }
      while (timeToTick_ < 0) {
        timeToTick_ += 1.0f / 30.0f;
      }
      var backPressed = PlayerInput.Player.BackClock;
      if (backPressed.IsPressed()) {
        Clock.Back();
      } else {
        Clock.Tick();
      }
    }
  }
}