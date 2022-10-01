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

    private Dense<float> timeToFire_;
    [SerializeField] public GameObject bulletPrefab;

    protected override void OnStart() {
      animator_ = GetComponent<Animator>();
      rigidbody_ = GetComponent<Rigidbody2D>();
      timeToFire_ = new Dense<float>(clock, 0.3f);
    }

    protected override void OnFixedForward() {
      transform.localRotation = Quaternion.FromToRotation(Vector3.left, rigidbody_.velocity);
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

          var obj = Instantiate(bulletPrefab, transform.parent);
          obj.transform.localPosition = transform.localPosition + Vector3.left * 2.5f;

          var aim = obj.GetComponent<FixedSpeedBullet>();
          aim.Velocity = Vector2.left * 15.0f;
        }

        rigidbody_.velocity *= Mathf.Exp(-Time.deltaTime);
      }
    }
  }
}