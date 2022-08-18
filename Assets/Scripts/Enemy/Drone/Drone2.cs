using Effect;
using Enemy.Bullet;
using Enemy.StateMachine;
using Lib;
using Reversible.Value;
using UnityEngine;

namespace Enemy.Drone {
  /**
   * Drone2:
   * 目的：近距離攻撃用小型ドローン。
   * 行動：魔女に向かい、魔女を見つけたら発砲。何度か撃ち、最後は特攻する。
   */
  public class Drone2 : EnemyBehaviour, IAnimatorEventSubscriber {
    private struct State {
      public static readonly int Seeking = Animator.StringToHash("Seeking");
      public static readonly int Fighting = Animator.StringToHash("Fighting");
      public static readonly int Sleeping = Animator.StringToHash("Sleeping");
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
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private GameObject magicElementEmitterPrefab;
    [SerializeField] private GameObject bulletPrefab;

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

      var targetDirection = (Vector2) (sora_.transform.localPosition - trans.localPosition);
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
          rigidbody_.velocity = currentRot * Vector2.left *
                                (seekVelocity * Mathf.Exp(Mathf.Clamp(targetDistance - 10.0f, 0.0f, 0.5f)));
        } else {
          rigidbody_.velocity = currentRot * Vector2.left * (rigidbody_.velocity.magnitude * Mathf.Exp(-dt));
        }
      } else if (currentHash == State.Fighting) {
        // Rotate to the target
        RotateToTarget();
        // Fire
        ref var timeToFire = ref timeToFire_.Mut;
        timeToFire -= dt;
        if (timeToFire <= 0.0f) {
          var direction = trans.localRotation * Vector3.left;
          var b = Instantiate(bulletPrefab, trans.parent);
          b.transform.localPosition = trans.localPosition + direction * 1.3f;
          var aim = b.GetComponent<FixedSpeedBullet>();
          aim.Velocity = direction * 15.0f;
          timeToFire = 0.3f;
        }

        rigidbody_.velocity *= Mathf.Exp(-dt);
      } else if (currentHash == State.Sleeping) {
        // Just sleep.
      } else if (currentHash == State.Escaping) {
        if (rigidbody_.velocity.magnitude < escapeVelocity) {
          rigidbody_.velocity = currentRot * Vector2.left * escapeVelocity;
        } else {
          rigidbody_.velocity *= Mathf.Exp(dt);
        }
      }

      // Think Next Action!
      if (fireCount_.Ref >= 5) {
        animator_.SetInteger(Param.NextAction, NextState.Escaping);
      } else if (targetDistance >= 10.0f || Mathf.Abs(deltaAngle) >= 1f) {
        animator_.SetInteger(Param.NextAction, NextState.Seeking);
      } else {
        animator_.SetInteger(Param.NextAction, NextState.Fighting);
      }
    }

    public override void OnCollision2D(GameObject other) {
      if (!gameObject.activeSelf) {
        return;
      }
      ref var shield = ref shield_.Mut;
      shield -= 1.0f;
      if (shield > 0.0f) {
        return;
      }

      var trans = transform;
      var parent = trans.parent;
      var localPosition = trans.localPosition;
        
      Instantiate(explosionEffectPrefab, localPosition, Quaternion.identity, parent);
      var emitter = Instantiate(magicElementEmitterPrefab, localPosition, Quaternion.identity, parent).GetComponent<MagicElementEmitter>();
      emitter.numMagicElements = System.Math.Max(1, Mathf.RoundToInt( initialShield / 3f));

      Deactivate();
    }

    public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      if (stateInfo.shortNameHash == State.Fighting) {
        fireCount_.Mut++;
      }
    }
  }
}