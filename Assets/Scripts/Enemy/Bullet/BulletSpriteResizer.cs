﻿using System;
using Donut.Unity;
using UnityEngine;

namespace Enemy.Bullet {
  public sealed class BulletSpriteResizer : DonutBehaviour {
    [SerializeField] public GameObject foreground;

    protected override void OnStart() {
    }

    protected override void OnUpdate() {
      var scale = 0.85f + ((float)Math.Sin(clock.CurrentTick / 30.0f * Math.PI * 2) * 0.05f);
      foreground.transform.localScale = Vector3.one * scale;
    }
  }
}