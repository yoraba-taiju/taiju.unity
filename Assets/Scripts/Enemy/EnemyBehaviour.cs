﻿using System;
using Reversible.Unity;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Util;

namespace Enemy {
  public abstract class EnemyBehaviour: ReversibleBehaviour {
    private int damageLayerMask_;
    private new void Start() {
      var self = this as ReversibleBehaviour;
      self.Start();
      damageLayerMask_ = LayerMask.GetMask("Witch", "WitchBullet");
    }

    private void OnCollisionEnter2D(Collision2D other) {
      OnCollision2D(other);
    }

    private void OnCollisionStay2D(Collision2D other) {
      OnCollision2D(other);
    }

    private void OnCollision2D(Collision2D other) {
      if (clockHolder.IsLeaping) {
        return;
      }
      var pos = transform.localPosition;
      if (Mathf.Abs(pos.x) >= 18.0f || Mathf.Abs(pos.y) >= 10.0f) {
        return;
      }
      var layer = other.gameObject.layer;
      if (((1 << layer) & damageLayerMask_) != 0) {
        OnCollide(other);
      }
    }

    protected abstract void OnCollide(Collision2D collision);
    
    // Angle utils
    protected static float AngleDegOf(Vector2 direction) {
      return VecUtil.AngleDegOf(direction) - 180.0f;
    }
  }
}