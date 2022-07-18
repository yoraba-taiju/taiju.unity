using Enemy.Bullet;
using Reversible.Value;
using UnityEngine;
using Utility;

namespace Enemy.Drone {
  public class Drone2: EnemyBehaviour, IAnimatorEventSubscriber {
    private struct State {
      public static readonly int Seeking = Animator.StringToHash("Seeking");
      public static readonly int Fighting = Animator.StringToHash("Fighting");
      public static readonly int Escaping = Animator.StringToHash("Escaping");
    }
    
    private struct Param {
      public static readonly int NextAction = Animator.StringToHash("NextAction");
    }
    
    private GameObject sora_;
    private Animator animator_;
    private Rigidbody2D rigidbody_;

    [SerializeField] private float initialShield = 10.0f;
    [SerializeField] private float maxRotateDegreePerSecond = 150.0f;
    [SerializeField] private float velocity = 7.0f;
    private Sparse<float> shield_;
    private Sparse<int> fireCount_;
    private Dense<float> timeToFire_;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject bullet;
    protected override void OnStart() {
      sora_ = GameObject.FindWithTag("Player");
      animator_ = GetComponent<Animator>();
      rigidbody_ = GetComponent<Rigidbody2D>();
      shield_ = new Sparse<float>(clock, initialShield);
      fireCount_ = new Sparse<int>(clock, 0);
      timeToFire_ = new Dense<float>(clock, 0.3f);
    }

    protected override void OnForward() {
      var trans = transform;
      var deltaTime = Time.deltaTime;
      var currentHash = animator_.GetCurrentAnimatorStateInfo(0).shortNameHash;
      var delta = (Vector2)(sora_.transform.position - trans.position);
      var distance = delta.magnitude;
      var currentRot = trans.localRotation;
      var angleDelta = VecUtil.DeltaDegreeToTarget(
        currentRot.eulerAngles.z + 180.0f,
        delta);

      if (currentHash == State.Seeking) {
        // Rotate to the target
        var maxAngleDegree = maxRotateDegreePerSecond * deltaTime;
        var moveAngleDegree = Mathf.Clamp(angleDelta, -maxAngleDegree, maxAngleDegree);
        var rot = currentRot * Quaternion.Euler(0, 0, moveAngleDegree);
        trans.localRotation = rot;
        // Set speed
        if (distance > 10.0f) {
          rigidbody_.velocity = rot * Vector2.left * velocity;
        } else {
          rigidbody_.velocity = Vector2.zero;
        }
      } else if (currentHash == State.Fighting) {
        ref var timeToFire = ref timeToFire_.Mut;
        timeToFire -= deltaTime;
        if (timeToFire <= 0.0f) {
          var direction = trans.localRotation * Vector3.left;
          var b = Instantiate(bullet, trans.parent);
          b.transform.localPosition = trans.localPosition + direction * 1.3f;
          var aim = b.GetComponent<FixedSpeedBullet>();
          aim.Direction = direction * 15.0f;
          timeToFire = 0.3f;
        }
        rigidbody_.velocity = Vector2.zero;
      } else if (currentHash == State.Escaping) {
        var direction = currentRot * Vector3.left;
        rigidbody_.velocity = direction * 15.0f;
      }

      // Think Next Action!
      if (distance >= 10.0f || angleDelta >= 3.0f) {
        animator_.SetInteger(Param.NextAction, 0);
      } else if (fireCount_.Ref >= 3) {
        animator_.SetInteger(Param.NextAction, 2);
      } else {
        animator_.SetInteger(Param.NextAction, 1);
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

    public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      if (stateInfo.shortNameHash == State.Fighting) {
        fireCount_.Mut++;
      }
    }
  }
}
