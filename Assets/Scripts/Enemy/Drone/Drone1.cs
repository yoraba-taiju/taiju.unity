using Enemy.StateMachine;
using Reversible.Value;
using UnityEngine;
using Utility;

namespace Enemy.Drone {
  /**
   * Drone1:
   * 目的：調査用ドローン
   * 行動：魔女に向かい、魔女を見つけたら情報を収集し、持ち帰る。
   */
  public class Drone1: EnemyBehaviour, IAnimatorEventSubscriber {
    private struct State {
      public static readonly int Seeking = Animator.StringToHash("Seeking");
      public static readonly int Watching = Animator.StringToHash("Watching");
      public static readonly int Rotating = Animator.StringToHash("Rotating");
      public static readonly int Returning = Animator.StringToHash("Returning");
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
      var stateInfo = animator_.GetCurrentAnimatorStateInfo(1);
      var currentHash = stateInfo.shortNameHash;
      var soraPosition = (Vector2)sora_.transform.localPosition;
      var currentPosition = (Vector2)transform.localPosition;
      var dt = Time.deltaTime;

      if (currentHash == State.Seeking) {
        var targetDirection = soraPosition - currentPosition;
        if (targetDirection.magnitude <= 6.0f) {
          animator_.SetTrigger(Trigger.ToWatching);
          rigidbody_.velocity = rigidbody_.velocity.normalized * 3.0f;
        } else {
          rigidbody_.velocity =  Mover.Follow(targetDirection, rigidbody_.velocity, dt * maxRotateDegreePerSecond);
        }
      } else if (currentHash == State.Watching) {
        var targetDirection = soraPosition + Vector2.right * 5.0f - currentPosition;
        rigidbody_.velocity = Mover.Follow(targetDirection, rigidbody_.velocity, dt * maxRotateDegreePerSecond);
      } else if (currentHash == State.Returning) {
        var vel = rigidbody_.velocity;
        if (vel.magnitude < 6.0f) {
          rigidbody_.velocity = transform.localRotation * Vector3.left * 7.0f;
        } else {
          //rigidbody_.velocity =  Mover.Follow(Vector2.right, rigidbody_.velocity, dt * maxRotateDegreePerSecond);
        }
      }
      transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.FromToRotation(Vector3.left, rigidbody_.velocity), 30.0f * dt);
    }

    public override void OnCollide(GameObject other) {
      ref var shield = ref shield_.Mut;
      shield -= 1.0f;
      if (shield <= 0) {
        Destroy();
        var explosion = Instantiate(explosionEffect, transform.parent);
        explosion.transform.localPosition = transform.localPosition;
      }
    }

    public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      
    }

    public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      
    }
  }
}
