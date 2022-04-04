using UnityEngine;
using UnityEngine.Playables;

namespace Reversible.Unity {
  public sealed class ClockHolder: MonoBehaviour {
    // getter
    public PlayerInput PlayerInput { get; private set; }
    public Clock Clock { get; } = new();
    public bool Ticked { get; private set; }
    public bool Backed { get; private set; }
    public bool Leaped { get; private set; }

    /* Ticking */
    private float timeToTick_;
    private const float SecondPerFrame = 1.0f / 30.0f;

    private void Start() {
      PlayerInput = new PlayerInput();
      PlayerInput.Enable();
      timeToTick_ = SecondPerFrame;
      Ticked = false;
      Leaped = false;
    }

    private void LateUpdate() {
      Ticked = false;
      Backed = false;
      Leaped = false;
      timeToTick_ -= Time.unscaledDeltaTime;
      if (timeToTick_ > 0.0f) {
        return;
      }
      while (timeToTick_ < 0) {
        timeToTick_ += SecondPerFrame;
      }
      var backPressed = PlayerInput.Player.BackClock;
      if (backPressed.IsPressed()) {
        Clock.Back();
        Backed = true;
        Time.timeScale = 0.0f;
      } else {
        if (Clock.IsLeaping) {
          Leaped = true;
        }
        Clock.Tick();
        Ticked = true;
        Time.timeScale = 1.0f;
      }
    }
  }
}