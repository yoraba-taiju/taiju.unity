using Lib;
using UnityEngine;
using Utility;

namespace Enemy.Bullet {
  public sealed class TargetingSoraInFixedSpeed : EnemyBulletBehaviour {
    [SerializeField] public float speed = 10.0f;
    [SerializeField] public float maxDegreeDeltaPerSecond = 30.0f;
    private GameObject sora_;
    private bool soraMissing_;
    private Vector2 velocity_;
    private float angle_;

    protected override void OnStart() {
      sora_ = GameObject.FindWithTag("Player");
      if (sora_ == null) {
        velocity_ = Vector2.left * speed;
        angle_ = 180;
        soraMissing_ = true;
        return;
      }

      soraMissing_ = false;
      var trans = transform;
      var pos = trans.position;
      var vec = (Vector2) (sora_.transform.position - pos);
      var magnitude = vec.magnitude;
      if (magnitude <= Mathf.Epsilon) {
        velocity_ = Vector2.left * speed;
        angle_ = 180;
        return;
      }

      velocity_ = vec / magnitude * speed;
      angle_ = VecMath.Atan2(velocity_);
    }

    protected override void OnForward() {
      if (soraMissing_) {
        transform.position += (Vector3) (velocity_ * Time.deltaTime);
        return;
      }

      var trans = transform;
      var position = trans.position;
      var targetDirection = (Vector2) (sora_.transform.position - position);
      var maxDeg = Time.deltaTime * maxDegreeDeltaPerSecond;

      velocity_ = Mover.Follow(targetDirection, velocity_, maxDeg);
      angle_ = VecMath.Atan2(velocity_);
      position += (Vector3) (velocity_ * Time.deltaTime);
      transform.position = position;
    }

    protected override void OnCollideWithWitch(GameObject other) {
      Deactivate();
    }
  }
}