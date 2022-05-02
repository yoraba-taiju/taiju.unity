using Enemy.Bullet;
using Reversible.Value;
using UnityEngine;
using Util;

namespace Enemy.Drone {
  public class Drone2: EnemyBehaviour {
    private static readonly int Seeking = Animator.StringToHash("Seeking");
    private static readonly int Fighting = Animator.StringToHash("Fighting");

    private GameObject sora_;
    private Animator animator_;
    private Rigidbody2D rigidbody_;

    [SerializeField] public float initialShield = 10.0f;
    [SerializeField] public float maxRotateDegreePerSecond = 3600.0f;
    private Sparse<float> shield_;
    private Dense<float> timeToFire_;
    [SerializeField] public GameObject explosionEffect;
    [SerializeField] public GameObject bullet;
    protected override void OnStart() {
      sora_ = GameObject.FindWithTag("Player");
      animator_ = GetComponent<Animator>();
      rigidbody_ = GetComponent<Rigidbody2D>();
      shield_ = new Sparse<float>(clock, initialShield);
      timeToFire_ = new Dense<float>(clock, 0.3f);
    }

    protected override void OnForward() {
      var trans = transform;
      var currentHash = animator_.GetCurrentAnimatorStateInfo(0).shortNameHash;
      var delta = (Vector2)(sora_.transform.position - trans.position);
      if (currentHash == Seeking) {
        { // rotation
          var rot = trans.localRotation;
          var current = rot.eulerAngles.z;
          var degreeDelta = VecUtil.AngleDegOf(delta) - rot.eulerAngles.z;
          var maxDegree = maxRotateDegreePerSecond * Time.deltaTime;
          trans.localRotation = Quaternion.Euler(0, 0, current + Mathf.Clamp(degreeDelta, -maxDegree, maxDegree));
        }
        if (delta.magnitude >= 15.0f) {
          rigidbody_.velocity = delta.normalized * 5.0f;
        } else {
          rigidbody_.velocity = Vector2.zero;
        }
      } else if (currentHash == Fighting) {
        ref var timeToFire = ref timeToFire_.Mut;
        timeToFire -= Time.deltaTime;
        if (timeToFire <= 0.0f) {
          var direction = trans.localRotation * Vector3.right;
          var b = Instantiate(bullet, trans.parent);
          b.transform.localPosition = trans.localPosition + direction * 1.2f;
          var aim = b.GetComponent<FixedSpeedBullet>();
          aim.Direction = direction * 15.0f;
          timeToFire += 0.3f;
        }
        rigidbody_.velocity = Vector2.zero;
      }
    }

    protected override void OnCollide(Collision2D collision) {
      ref var shield = ref shield_.Mut;
      shield -= 1.0f;
      if (shield <= 0) {
        Destroy();
        var explosion = Instantiate(explosionEffect, transform.parent);
        explosion.transform.localPosition = transform.localPosition;
      }
    }
  }
}
