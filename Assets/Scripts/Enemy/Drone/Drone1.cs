using System;
using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Enemy.Drone {
  public class Drone1: EnemyBehaviour {
    private static readonly int Seeking = Animator.StringToHash("Seeking");
    private static readonly int Fighting = Animator.StringToHash("Fighting");
    private static readonly int ToFighting = Animator.StringToHash("ToFighting");
    private static readonly int ToSeeking = Animator.StringToHash("ToSeeking");

    private GameObject sora_;
    private Animator animator_;
    private Rigidbody2D rigidbody_;

    [SerializeField] public float initialShield = 40.0f;
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
      var currentHash = animator_.GetCurrentAnimatorStateInfo(1).shortNameHash;
      var delta = (Vector2)(sora_.transform.position - transform.position);
      if (currentHash == Seeking) {
        if (delta.magnitude <= 15.0f) {
          animator_.SetTrigger(ToFighting);
          timeToFire_.Mut = 0.1f;
        } else {
          rigidbody_.velocity = delta.normalized * 5.0f;
        }
      } else if (currentHash == Fighting) {
        ref var timeToFire = ref timeToFire_.Mut;
        timeToFire -= Time.deltaTime;
        if (timeToFire <= 0.0f) {
          var e = Instantiate(bullet, transform.parent);
          e.transform.localPosition = transform.localPosition + Vector3.left * 2.5f;
          timeToFire += 0.3f;
        }
        if (delta.magnitude >= 20.0f) {
          animator_.SetTrigger(ToSeeking);
        }
        var d = Math.Clamp(delta.magnitude - 15.0f, -2.0f, 2.0f);
        rigidbody_.velocity = delta.normalized * d;
      }
    }

    protected override void OnCollide(Collision2D collision) {
      ref var shield = ref shield_.Mut;
      shield -= 1.0f;
      if (shield <= 0) {
        Destroy();
        var e = Instantiate(explosionEffect, transform.parent);
        e.transform.localPosition = transform.localPosition;
      }
    }
  }
}
