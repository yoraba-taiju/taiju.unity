using System;
using Reversible.Value;
using UnityEngine;
using Utility;

namespace Enemy.Drone {
  /**
   * Drone0:
   * 目的：監視用ドローン。
   * 行動：魔女に向かい、魔女を見つけたら情報を収集し、持ち帰る。
   */
  public class Drone0: EnemyBehaviour {
    private struct State {
      public static readonly int Seeking = Animator.StringToHash("Seeking");
      public static readonly int Watching = Animator.StringToHash("Watching");
      public static readonly int Return = Animator.StringToHash("Return");
    }
    private struct Trigger {
      public static readonly int ToWatching = Animator.StringToHash("ToWatching");
    }

    private GameObject sora_;
    private Animator animator_;
    private Rigidbody2D rigidbody_;
    [SerializeField] private float initialShield = 1.0f;
    [SerializeField] private float maxRotateDegreePerSecond = 30.0f;
    private Sparse<float> shield_;
    [SerializeField] private GameObject explosionEffect;
    
    protected override void OnStart() {
      sora_ = GameObject.FindWithTag("Player");
      animator_ = GetComponent<Animator>();
      rigidbody_ = GetComponent<Rigidbody2D>();
      rigidbody_.velocity = Vector2.left * 7.0f;
      shield_ = new Sparse<float>(clock, initialShield);
    }

    protected override void OnForward() {
      var currentHash = animator_.GetCurrentAnimatorStateInfo(1).shortNameHash;
      var soraPosition = (Vector2)sora_.transform.position;
      var currentPosition = (Vector2)transform.position;
      var diff = soraPosition - currentPosition;
      var dt = Time.deltaTime;

      if (currentHash == State.Seeking) {
        if (diff.magnitude <= 6.0f) {
          animator_.SetTrigger(Trigger.ToWatching);
        } else {
          rigidbody_.velocity =  VecUtil.Follow(rigidbody_.velocity, diff, dt * maxRotateDegreePerSecond);
        }
      } else if (currentHash == State.Watching) {
        var targetPosition = soraPosition + Vector2.right * 5.0f;
        var d = Mathf.Clamp(diff.magnitude - 4.0f, -2.0f, 2.0f);
        var current = rigidbody_.velocity;
        var angleDelta = VecUtil.DeltaAngle(current, diff, dt * maxRotateDegreePerSecond);
        rigidbody_.velocity =  VecUtil.Rotate(current, angleDelta).normalized * d;
      } else if (currentHash == State.Return) {
        rigidbody_.velocity = Vector2.right * 7.0f;
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
