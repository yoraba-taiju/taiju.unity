using System;
using System.Runtime.CompilerServices;
using Reversible.Value;
using UnityEngine;

namespace Reversible.Unity {
  public sealed class ClockController: MonoBehaviour {
    // getter
    public PlayerInput PlayerInput { get; private set; }
    public Clock Clock { get; } = new();
    public bool Ticked { get; private set; }
    public bool Backed { get; private set; }
    public bool Leaped { get; private set; }
    public bool IsForwarding { get; private set; }
    private Dense<float> currentTime_;

    public float CurrentTime {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => currentTime_.Ref;
    }

    /* Ticking */
    private float timeToTick_;
    public const float SecondPerFrame = 1.0f / 30.0f;

    private void Start() {
      PlayerInput = new PlayerInput();
      PlayerInput.Enable();
      currentTime_ = new Dense<float>(Clock, 0.0f);
      timeToTick_ = SecondPerFrame;
      Ticked = false;
      Leaped = false;
    }

    private bool CountDownToTick() {
      if (IsForwarding) {
        timeToTick_ -= Time.deltaTime;
      } else {
        timeToTick_ -= Time.unscaledDeltaTime;
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
      IsForwarding = false;

      Ticked = false;
      Backed = false;
      Leaped = false;

      var backPressed = PlayerInput.Player.BackClock;
      if (backPressed.WasPressedThisFrame()) {
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
        timeToTick_ = SecondPerFrame;
        Clock.Leap();
        Clock.Tick();
        return;
      }
      IsForwarding = true;
      currentTime_.Mut += Time.deltaTime;
      Ticked = CountDownToTick();
      if (Ticked) {
        Clock.Tick();
      }
    }

    private void LateUpdate() {
      if (Leaped) {
        Time.timeScale = 1.0f;
      }
    }
  }
}
