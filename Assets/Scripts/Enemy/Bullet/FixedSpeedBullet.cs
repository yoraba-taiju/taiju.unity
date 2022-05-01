using System;
using UnityEngine;
using Reversible.Unity;

namespace Enemy.Bullet {
  public sealed class FixedSpeedBullet : EnemyBulletBehaviour {
    private Vector2 direction_;

    public Vector2 Direction {
      get => direction_;
      set {
        transform.localRotation = Quaternion.Euler(0, 0, (float)Math.Atan2(value.x, value.y));
        direction_ = value;
      }
    }

    protected override void OnStart() {
    }

    protected override void OnForward() {
      transform.position += (Vector3)(Direction * Time.deltaTime);
    }

    protected override void OnCollideWithWitch(GameObject other) {
      Destroy();
    }
  }
}