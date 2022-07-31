using UnityEngine;

namespace Reversible.Unity {
  public sealed class ClockHolder: MonoBehaviour {
    // getter
    public PlayerInput PlayerInput { get; private set; }
    public Clock Clock { get; } = new();
    public bool Ticked { get; private set; }
    public bool Backed { get; private set; }
    public bool Leaped { get; private set; }
    public bool IsLeaping { get; private set; }

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

    private bool CountDownToTick() {
      if (IsLeaping) {
        timeToTick_ -= Time.unscaledDeltaTime;
      } else {
        timeToTick_ -= Time.deltaTime;
      }
      if (timeToTick_ > 0.0f) {
        return false;
      }
      while (timeToTick_ < 0) {
        timeToTick_ += SecondPerFrame;
      }
      return true;
    }

    private void Update() {
      Ticked = false;
      Backed = false;
      Leaped = false;

      var backPressed = PlayerInput.Player.BackClock;
      if (backPressed.WasPressedThisFrame()) {
        IsLeaping = true;
        Time.timeScale = 0.0f;
        timeToTick_ = SecondPerFrame;
        return;
      }
      if (backPressed.IsPressed()) {
        Backed = CountDownToTick();
        if (Backed) {
          Clock.Back();
        }
        return;
      }
      if (backPressed.WasReleasedThisFrame()) {
        Leaped = true;
        IsLeaping = false;
        timeToTick_ = SecondPerFrame;
        Time.timeScale = 1.0f;
        Clock.Leap();
        Clock.Tick();
        IsLeaping = false;
        return;
      }
      Ticked = CountDownToTick();
      if (Ticked) {
        Clock.Tick();
      }
    }
  }
}