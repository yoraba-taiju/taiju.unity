using Util;
using UnityEngine;

namespace Enemy.Bullet {
  public sealed class TargetingSoraInFixedSpeed : EnemyBulletBehaviour {
    [SerializeField] public float speed = 10.0f;
    [SerializeField] public float maxDegreeDeltaPerSecond = 30.0f;
    private GameObject sora_;
    private bool soraMissing_;
    private Vector3 direction_;
    private float angle_;
    protected override void OnStart() {
      sora_ = GameObject.FindWithTag("Player");
      if (sora_ == null) {
        direction_ = Vector3.left * speed;
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
        direction_ = Vector3.left * speed;
        angle_ = 180;
        return;
      }
      direction_ = vec / magnitude * speed;
      angle_ = VecUtil.AngleDegreeOf(direction_);
    }

    protected override void OnForward() {
      if (soraMissing_) {
        transform.position += direction_ * Time.deltaTime;
        return;
      }
      var trans = transform;
      var pos = trans.position;
      var vec = (Vector2)(sora_.transform.position - pos);

      var maxDeg = Time.deltaTime * maxDegreeDeltaPerSecond;
      var nextDeg = Mathf.Clamp(VecUtil.AngleDegreeOf(vec) - angle_, -maxDeg, maxDeg);
      direction_ = VecUtil.RotateByAngleDegree(direction_, nextDeg);
      angle_ = VecUtil.AngleDegreeOf(direction_);
      transform.position += direction_ * Time.deltaTime;
    }

    protected override void OnCollideWithWitch(GameObject other) {
      Destroy();
    }
  }
}
