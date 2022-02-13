﻿using UnityEngine;
using UnityEngine.Playables;

namespace Donut.Unity {
  public sealed class ClockHolder: MonoBehaviour {
    // getter
    public PlayerInput PlayerInput { get; private set; }
    public Clock Clock { get; } = new();

    /* Ticking */
    private float timeToTick_;
    private const float MillsPerFrame = 1.0f / 30.0f;

    private void Start() {
      PlayerInput = new PlayerInput();
      PlayerInput.Enable();
      timeToTick_ = MillsPerFrame;
    }

    private void LateUpdate() {
      timeToTick_ -= Time.deltaTime;
      if (timeToTick_ > 0.0f) {
        return;
      }
      while (timeToTick_ < 0) {
        timeToTick_ += MillsPerFrame;
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