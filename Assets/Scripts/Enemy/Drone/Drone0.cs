using Reversible.Value;
using UnityEngine;
using Utility;

namespace Enemy.Drone {
  /**
   * Drone0:
   * 目的：監視用ドローン。
   * 行動：魔女に向かい、魔女を見つけたら逃げる
   */
  public class Drone0 : EnemyBehaviour {
    private struct State {
      public static readonly int Seeking = Animator.StringToHash("Seeking");
      public static readonly int Escaping = Animator.StringToHash("Escaping");
    }

    private struct Trigger {
      public static readonly int ToEscaping = Animator.StringToHash("ToEscaping");
    }

    private GameObject sora_;
    private Animator animator_;
    private Rigidbody2D rigidbody_;
    [SerializeField] private float initialShield = 1.0f;
    [SerializeField] private float maxRotateDegreePerSecond = 60.0f;
    private Sparse<float> shield_;
    private Sparse<Quaternion> originalRotation_;
    [SerializeField] private GameObject explosionEffect;

    protected override void OnStart() {
      sora_ = GameObject.FindWithTag("Player");
      animator_ = GetComponent<Animator>();
      rigidbody_ = GetComponent<Rigidbody2D>();
      rigidbody_.velocity = Vector2.left * 10.0f;
      shield_ = new Sparse<float>(clock, initialShield);
    }

    protected override void OnForward() {
      var stateInfo = animator_.GetCurrentAnimatorStateInfo(1);
      var currentHash = stateInfo.shortNameHash;
      var soraPosition = (Vector2) sora_.transform.localPosition;
      var currentPosition = (Vector2) transform.localPosition;
      var dt = Time.deltaTime;
      var maxAngle = dt * maxRotateDegreePerSecond;

      if (currentHash == State.Seeking) {
        var delta = soraPosition - currentPosition;
        if (Mathf.Abs(delta.x) > 10.0f) {
          rigidbody_.velocity = Mover.Follow(delta, rigidbody_.velocity, maxAngle);
        } else {
          animator_.SetTrigger(Trigger.ToEscaping);
        }
      } else if (currentHash == State.Escaping) {
        var delta = soraPosition - currentPosition;
        if (delta.magnitude < 10.0f) {
          rigidbody_.velocity = VecMath.Rotate(rigidbody_.velocity, Mathf.Sign(delta.y) * maxAngle) * Mathf.Exp(dt / 2);
        }
      }

      transform.localRotation = Quaternion.RotateTowards(transform.localRotation,
        Quaternion.FromToRotation(Vector3.left, rigidbody_.velocity), maxAngle * 0.75f);
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
  }
}