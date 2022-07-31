using Enemy.Bullet;
using Reversible.Value;
using UnityEngine;
using Utility;

namespace Enemy.Drone {
  /**
   * Drone1:
   * 目的：監視用ドローン。
   * 行動：魔女に向かい、魔女を見つけたら情報を収集し、持ち帰る。
   */
  public class Drone1: EnemyBehaviour, IAnimatorEventSubscriber {
    private struct State {
      public static readonly int Seeking = Animator.StringToHash("Seeking");
      public static readonly int Fighting = Animator.StringToHash("Fighting");
      public static readonly int Escaping = Animator.StringToHash("Escaping");
    }
    private struct NextState {
      public const int Seeking = 0;
      public const int Fighting = 1;
      public const int Escaping = 2;
    }
    
    private struct Param {
      public static readonly int NextAction = Animator.StringToHash("NextAction");
    }
    
    private GameObject sora_;
    private Animator animator_;
    private Rigidbody2D rigidbody_;

    [SerializeField] private float initialShield = 10.0f;
    [SerializeField] private float maxRotateDegreePerSecond = 180.0f;
    [SerializeField] private float seekVelocity = 7.0f;
    [SerializeField] private float escapeVelocity = 15.0f;
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
      var dt = Time.deltaTime;
      var currentHash = animator_.GetCurrentAnimatorStateInfo(0).shortNameHash;

      var targetDirection = (Vector2)(sora_.transform.position - trans.position);
      var targetDistance = targetDirection.magnitude;
      var currentRot = trans.localRotation;
      var deltaAngle = VecMath.DeltaAngle(currentRot.eulerAngles.z - 180.0f, targetDirection);

      void RotateToTarget() {
        var maxAngleDegree = maxRotateDegreePerSecond * dt;
        var moveAngleDegree = Mathf.Clamp(deltaAngle, -maxAngleDegree, maxAngleDegree);
        var rot = Quaternion.Euler(0, 0, moveAngleDegree) * currentRot;
        trans.localRotation = rot;
        currentRot = rot;
      }

      if (currentHash == State.Seeking) {
        // Rotate to the target
        RotateToTarget();
        // Set speed
        if (targetDistance > 10.0f) {
          rigidbody_.velocity = currentRot * Vector2.left * (seekVelocity * Mathf.Exp(Mathf.Clamp(targetDistance - 10.0f, 0.0f, 0.5f)));
        } else {
          rigidbody_.velocity = currentRot * Vector2.left * (rigidbody_.velocity.magnitude * Mathf.Exp(-dt)) ;
        }
      } else if (currentHash == State.Fighting) {
        // Rotate to the target
        RotateToTarget();
        // Fire
        ref var timeToFire = ref timeToFire_.Mut;
        timeToFire -= dt;
        if (timeToFire <= 0.0f) {
          var direction = trans.localRotation * Vector3.left;
          var b = Instantiate(bullet, trans.parent);
          b.transform.localPosition = trans.localPosition + direction * 1.3f;
          var aim = b.GetComponent<FixedSpeedBullet>();
          aim.Velocity = direction * 15.0f;
          timeToFire = 0.3f;
        }
        rigidbody_.velocity *= Mathf.Exp(-dt);
      } else if (currentHash == State.Escaping) {
        if (rigidbody_.velocity.magnitude < escapeVelocity) {
          rigidbody_.velocity = currentRot * Vector2.left * escapeVelocity;
        } else {
          rigidbody_.velocity *= Mathf.Exp(dt);
        }
      }

      // Think Next Action!
      if (fireCount_.Ref >= 3) {
        animator_.SetInteger(Param.NextAction, NextState.Escaping);
      } else if (targetDistance >= 10.0f || Mathf.Abs(deltaAngle) >= 1f) {
        animator_.SetInteger(Param.NextAction, NextState.Seeking);
      } else {
        animator_.SetInteger(Param.NextAction, NextState.Fighting);
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
