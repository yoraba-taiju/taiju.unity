using System;
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
    [SerializeField] public float maxRotateDegreePerSecond = 120.0f;
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
        Quaternion nextRot;
        { // rotation
          var rot = trans.localRotation;
          var currentAngle = rot.eulerAngles.z;
          var targetDegree = TargetAngleDegreeOf(delta);
          var degreeDelta = VecUtil.NormalizeAngleDegree(targetDegree - currentAngle);
          if (Mathf.Abs(degreeDelta) > 1) {
            var maxDegree = maxRotateDegreePerSecond * Time.deltaTime;
            nextRot = Quaternion.Euler(0, 0, currentAngle + Mathf.Clamp(degreeDelta, -maxDegree, maxDegree));
            trans.localRotation = nextRot;
          } else {
            nextRot = trans.localRotation;
          }
        }
        if (delta.magnitude >= 7.5f) {
          rigidbody_.velocity = nextRot * Vector2.left * 5.0f;
        } else {
          rigidbody_.velocity = Vector2.zero;
        }
      } else if (currentHash == Fighting) {
        ref var timeToFire = ref timeToFire_.Mut;
        timeToFire -= Time.deltaTime;
        if (timeToFire <= 0.0f) {
          var direction = trans.localRotation * Vector3.left;
          var b = Instantiate(bullet, trans.parent);
          b.transform.localPosition = trans.localPosition + direction * 1.3f;
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
