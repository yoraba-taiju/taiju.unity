﻿using System;
using Donut.Unity;
using UnityEngine;

namespace Enemy.Bullet.Graphic {
  public sealed class CircleBulletSpriteResizer : DonutBehaviour {
    [SerializeField] public GameObject foreground;
    private uint bornAt_;

    protected override void OnStart() {
      bornAt_ = clock.CurrentTick;
    }

    protected override void OnUpdate() {
      var scale = 0.88f + ((float)Math.Sin((clock.CurrentTick - bornAt_) / 30.0f * Math.PI * 2) * 0.05f);
      foreground.transform.localScale = Vector3.one * scale;
    }
  }
}