using Enemy.StateMachine;
using Lib;
using Reversible.Value;
using UnityEngine;
using Utility;

namespace Enemy.Drone {
  /**
   * Drone1:
   * 目的：調査用ドローン
   * 行動：魔女に向かい、魔女を見つけたら情報を収集し、持ち帰る。
   */
  public class Drone1 : EnemyBehaviour, IAnimatorEventSubscriber {
    private struct State {
      public static readonly int Seeking = Animator.StringToHash("Seeking");
      public static readonly int Watching = Animator.StringToHash("Watching");
      public static readonly int Returning = Animator.StringToHash("Returning");
    }

    private struct Trigger {
      public static readonly int ToWatching = Animator.StringToHash("ToWatching");
    }

    private GameObject sora_;
    private Animator animator_;
    private Rigidbody2D rigidbody_;
    [SerializeField] private float initialShield = 10.0f;
    [SerializeField] private float maxRotateDegreePerSecond = 180.0f;
    private Sparse<float> shield_;
    private Sparse<Quaternion> originalRotation_;
    [SerializeField] private GameObject explosionEffect;

    protected override void OnStart() {
      sora_ = GameObject.FindWithTag("Player");
      animator_ = GetComponent<Animator>();
      rigidbody_ = GetComponent<Rigidbody2D>();
      rigidbody_.velocity = Vector2.left * 7.0f;
      shield_ = new Sparse<float>(clock, initialShield);
    }

    protected override void OnForward() {
      var trans = transform;
      var stateInfo = animator_.GetCurrentAnimatorStateInfo(1);
      var currentHash = stateInfo.shortNameHash;
      var soraPosition = (Vector2) sora_.transform.localPosition;
      var currentPosition = (Vector2) trans.localPosition;
      var targetDirection = soraPosition - currentPosition;
      var targetDistance = targetDirection.magnitude;
      var currentRot = trans.localRotation;
      var dt = Time.deltaTime;

      var deltaAngle = VecMath.DeltaAngle(currentRot.eulerAngles.z - 180.0f, targetDirection);

      void RotateToTarget() {
        var maxAngleDegree = maxRotateDegreePerSecond * dt;
        var moveAngleDegree = Mathf.Clamp(deltaAngle, -maxAngleDegree, maxAngleDegree);
        var rot = Quaternion.Euler(0, 0, moveAngleDegree) * currentRot;
        trans.localRotation = rot;
        currentRot = rot;
      }

      if (currentHash == State.Seeking) {
        RotateToTarget();
        if (targetDistance > 10.0f) {
          rigidbody_.velocity = currentRot * Vector2.left *
                                (7.0f * Mathf.Exp(Mathf.Clamp(targetDistance - 10.0f, 0.0f, 0.5f)));
        } else {
          rigidbody_.velocity = currentRot * Vector2.left * (rigidbody_.velocity.magnitude * Mathf.Exp(-dt));
        }
      }

      trans.localRotation =
        Quaternion.RotateTowards(currentRot, 
          Quaternion.FromToRotation(Vector3.left, rigidbody_.velocity), 30.0f * dt);
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
      var explosion = Instantiate(explosionEffect, transform.parent);
      explosion.transform.localPosition = transform.localPosition;
      Deactivate();
    }

    public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    }

    public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    }
  }
}