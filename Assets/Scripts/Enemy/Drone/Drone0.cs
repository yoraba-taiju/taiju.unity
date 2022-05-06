using System;
using Reversible.Value;
using UnityEngine;
using Utility;

namespace Enemy.Drone {
  public class Drone0: EnemyBehaviour {
    private static readonly int Seeking = Animator.StringToHash("Seeking");
    private static readonly int Watching = Animator.StringToHash("Watching");
    private static readonly int Return = Animator.StringToHash("Return");
    private static readonly int ToWatching = Animator.StringToHash("ToWatching");

    private GameObject sora_;
    private Animator animator_;
    private Rigidbody2D rigidbody_;
    [SerializeField] public float initialShield = 1.0f;
    [SerializeField] public float maxRotateDegreePerSecond = 10.0f;
    private Sparse<float> shield_;
    [SerializeField] public GameObject explosionEffect;


    protected override void OnStart() {
      sora_ = GameObject.FindWithTag("Player");
      animator_ = GetComponent<Animator>();
      rigidbody_ = GetComponent<Rigidbody2D>();
      rigidbody_.velocity = Vector2.left * 7.0f;
      shield_ = new Sparse<float>(clock, initialShield);
    }

    protected override void OnForward() {
      var currentHash = animator_.GetCurrentAnimatorStateInfo(1).shortNameHash;
      var delta = (Vector2)(sora_.transform.position - transform.position);
      if (currentHash == Seeking) {
        if (delta.magnitude <= 6.0f) {
          animator_.SetTrigger(ToWatching);
        } else {
          var current = rigidbody_.velocity;
          var angleDelta = VecUtil.DeltaDegreeToTarget(current, delta, Time.deltaTime * maxRotateDegreePerSecond);
          rigidbody_.velocity =  VecUtil.RotateByAngleDegree(current, angleDelta).normalized * 7.0f;

        }
      } else if (currentHash == Watching) {
        var d = Mathf.Clamp(delta.magnitude - 4.0f, -2.0f, 2.0f);
        var current = rigidbody_.velocity;
        var angleDelta = VecUtil.DeltaDegreeToTarget(current, delta, Time.deltaTime * maxRotateDegreePerSecond);
        rigidbody_.velocity =  VecUtil.RotateByAngleDegree(current, angleDelta).normalized * d;
      } else if (currentHash == Return) {
        rigidbody_.velocity = Vector2.right * 3.0f;
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
