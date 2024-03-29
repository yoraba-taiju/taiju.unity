﻿using Enemy.StateMachine;
using Lib;
using Lib.Unity;
using Reversible.Value;
using UnityEngine;

namespace Enemy.Drone {
  /**
   * Drone1:
   * 目的：調査用ドローン
   * 行動：魔女に向かい、魔女を見つけたら情報を収集し、持ち帰る。
   */
  public class Drone1 : EnemyBehaviour, IAnimatorEventSubscriber {
    private struct State {
      public static readonly int Seeking = Animator.StringToHash("Seeking");
      public static readonly int Returning = Animator.StringToHash("Returning");
    }

    private struct Params {
      public static readonly int SeekCount = Animator.StringToHash("SeekCount");
    }

    private GameObject sora_;
    private Animator animator_;
    private Rigidbody2D rigidbody_;
    [SerializeField] private float maxRotateDegreePerSecond = 180.0f;
    private Sparse<Quaternion> originalRotation_;

    protected override void OnStart() {
      sora_ = GameObject.FindWithTag("Player");
      animator_ = GetComponent<Animator>();
      rigidbody_ = GetComponent<Rigidbody2D>();
      rigidbody_.velocity = Vector2.left * 7.0f;
    }
    protected override void OnFixedForward() {
      transform.localRotation = Quaternion.FromToRotation(Vector3.left, rigidbody_.velocity);
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

      var deltaAngle = MathVec.DeltaAngle(currentRot.eulerAngles.z - 180.0f, targetDirection);

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
      } else if (currentHash == State.Returning) {
        rigidbody_.velocity = Mover.Follow(Vector2.right, 7.0f * Vector2.right, 30.0f * dt);
      }
    }

    public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      if (stateInfo.shortNameHash == State.Seeking) {
        animator.SetInteger(Params.SeekCount, animator.GetInteger(Params.SeekCount) + 1);
      }
    }
  }
}
