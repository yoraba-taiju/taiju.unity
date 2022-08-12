using Enemy.Bullet;
using Reversible.Value;
using UnityEngine;

namespace Enemy.Drone {
  public class Drone3 : EnemyBehaviour {
    private struct State {
      public static readonly int Seeking = Animator.StringToHash("Seeking");
      public static readonly int GivingUp = Animator.StringToHash("GivingUp");
      public static readonly int Fighting = Animator.StringToHash("Fighting");
    }

    private Animator animator_;
    private Rigidbody2D rigidbody_;

    [SerializeField] public float initialShield = 40.0f;
    private Sparse<float> shield_;
    private Dense<float> timeToFire_;
    [SerializeField] public GameObject explosionEffect;
    [SerializeField] public GameObject bullet;

    protected override void OnStart() {
      animator_ = GetComponent<Animator>();
      rigidbody_ = GetComponent<Rigidbody2D>();
      shield_ = new Sparse<float>(clock, initialShield);
      timeToFire_ = new Dense<float>(clock, 0.3f);
    }

    protected override void OnForward() {
      var currentHash = animator_.GetCurrentAnimatorStateInfo(1).shortNameHash;
      if (currentHash == State.Seeking || currentHash == State.GivingUp) {
        rigidbody_.velocity = Vector2.left * 5.0f;
      } else if (currentHash == State.Fighting) {
        ref var timeToFire = ref timeToFire_.Mut;
        timeToFire -= Time.deltaTime;
        if (timeToFire <= 0.0f) {
          timeToFire += 0.3f;

          var obj = Instantiate(bullet, transform.parent);
          obj.transform.localPosition = transform.localPosition + Vector3.left * 2.5f;

          var aim = obj.GetComponent<FixedSpeedBullet>();
          aim.Velocity = Vector2.left * 15.0f;
        }

        rigidbody_.velocity *= Mathf.Exp(-Time.deltaTime);
      }
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