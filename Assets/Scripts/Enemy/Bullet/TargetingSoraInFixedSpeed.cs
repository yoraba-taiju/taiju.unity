using Utility;
using UnityEngine;

namespace Enemy.Bullet {
  public sealed class TargetingSoraInFixedSpeed : EnemyBulletBehaviour {
    [SerializeField] public float speed = 10.0f;
    [SerializeField] public float maxDegreeDeltaPerSecond = 30.0f;
    private GameObject sora_;
    private bool soraMissing_;
    private Vector2 direction_;
    private float angle_;
    protected override void OnStart() {
      sora_ = GameObject.FindWithTag("Player");
      if (sora_ == null) {
        direction_ = Vector2.left * speed;
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
        direction_ = Vector2.left * speed;
        angle_ = 180;
        return;
      }
      direction_ = vec / magnitude * speed;
      angle_ = VecMath.Atan2(direction_);
    }

    protected override void OnForward() {
      if (soraMissing_) {
        transform.position += (Vector3)(direction_ * Time.deltaTime);
        return;
      }
      var trans = transform;
      var position = trans.position;
      var targetDiff = (Vector2)(sora_.transform.position - position);
      var maxDeg = Time.deltaTime * maxDegreeDeltaPerSecond;

      direction_ = Mover.Follow(targetDiff, direction_, maxDeg);
      angle_ = VecMath.Atan2(direction_);
      position += (Vector3)(direction_ * Time.deltaTime);
      transform.position = position;
    }

    protected override void OnCollideWithWitch(GameObject other) {
      Destroy();
    }
  }
}
