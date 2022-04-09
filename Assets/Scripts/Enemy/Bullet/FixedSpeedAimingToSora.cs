using UnityEngine;
using Reversible.Unity;

namespace Enemy.Bullet {
  public sealed class FixedSpeedAimingToSora : EnemyBulletBehaviour {
    [SerializeField] public float speed = 5.0f;
    [SerializeField] public float angle;
    private GameObject sora_;
    private Vector3 direction_;
    protected override void OnStart() {
      sora_ = GameObject.FindWithTag("Player");
      if (sora_ == null) {
        direction_ = Vector3.left * speed;
        return;
      }
      var trans = transform;
      var pos = trans.position;
      var vec = sora_.transform.position - pos;
      vec.z = 0.0f;
      var magnitude = vec.magnitude;
      if (magnitude > float.Epsilon) {
        vec /= magnitude;
        direction_ = vec * speed;
        direction_ = Quaternion.AngleAxis(angle, Vector3.forward) * direction_;
      } else {
        direction_ = Vector3.left * speed;
      }
    }

    protected override void OnForward() {
      transform.position += direction_ * Time.deltaTime;
    }

    protected override void OnCollideWithWitch(GameObject other) {
      Destroy();
    }
  }
}
