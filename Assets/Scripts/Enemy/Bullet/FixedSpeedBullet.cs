using UnityEngine;
using Utility;

namespace Enemy.Bullet {
  public sealed class FixedSpeedBullet : EnemyBulletBehaviour {
    private Vector2 velocity_;

    public Vector2 Velocity {
      get => velocity_;
      set {
        var angle = VecMath.Atan2(value);
        transform.localRotation = Quaternion.Euler(0, 0, angle);
        velocity_ = value;
      }
    }

    protected override void OnStart() {
    }

    protected override void OnForward() {
      transform.position += (Vector3) (Velocity * Time.deltaTime);
    }

    protected override void OnCollideWithWitch(GameObject other) {
      Destroy();
    }
  }
}