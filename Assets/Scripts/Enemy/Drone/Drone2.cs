using Enemy.Bullet;
using Reversible.Value;
using UnityEngine;
using Utility;

namespace Enemy.Drone {
  public class Drone2: EnemyBehaviour, IAnimatorEventSubscriber {
    private static readonly int ParamNextAction = Animator.StringToHash("NextAction");
    private static readonly int HashSeeking = Animator.StringToHash("Seeking");
    private static readonly int HashFighting = Animator.StringToHash("Fighting");
    private static readonly int HashEscaping = Animator.StringToHash("Escaping");
    
    private GameObject sora_;
    private Animator animator_;
    private Rigidbody2D rigidbody_;

    [SerializeField] public float initialShield = 10.0f;
    [SerializeField] public float maxRotateDegreePerSecond = 120.0f;
    private Sparse<float> shield_;
    private Sparse<int> fireCount_;
    private Dense<float> timeToFire_;
    [SerializeField] public GameObject explosionEffect;
    [SerializeField] public GameObject bullet;
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
      var currentHash = animator_.GetCurrentAnimatorStateInfo(0).shortNameHash;
      var delta = (Vector2)(sora_.transform.position - trans.position);
      var distance = delta.magnitude;
      var currentRot = trans.localRotation;
      var angleDelta = VecUtil.DeltaDegreeToTarget(
        currentRot.eulerAngles.z + 180.0f,
        delta);

      if (currentHash == HashSeeking) {
        // Rotate to the target
        var maxAngleDegree = maxRotateDegreePerSecond * Time.deltaTime;
        var moveAngleDegree = Mathf.Clamp(angleDelta, -maxAngleDegree, maxAngleDegree);
        var rot = currentRot * Quaternion.Euler(0, 0, moveAngleDegree);
        trans.localRotation = rot;
        // Set speed
        if (distance > 10.0f) {
          rigidbody_.velocity = rot * Vector2.left * 5.0f;
        } else {
          rigidbody_.velocity = Vector2.zero;
        }
      } else if (currentHash == HashFighting) {
        ref var timeToFire = ref timeToFire_.Mut;
        timeToFire -= Time.deltaTime;
        if (timeToFire <= 0.0f) {
          var direction = trans.localRotation * Vector3.left;
          var b = Instantiate(bullet, trans.parent);
          b.transform.localPosition = trans.localPosition + direction * 1.3f;
          var aim = b.GetComponent<FixedSpeedBullet>();
          aim.Direction = direction * 15.0f;
          timeToFire = 0.3f;
        }
        rigidbody_.velocity = Vector2.zero;
      } else if (currentHash == HashEscaping) {
        var direction = trans.localRotation * Vector3.left;
        rigidbody_.velocity = direction * 15.0f;
      }
      if (distance >= 10.0f || angleDelta > 3.0f) {
        animator_.SetInteger(ParamNextAction, 0);
      } else if (fireCount_.Ref >= 3) {
        animator_.SetInteger(ParamNextAction, 2);
      } else {
        animator_.SetInteger(ParamNextAction, 1);
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

    public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
      if (stateInfo.shortNameHash == HashFighting) {
        fireCount_.Mut++;
      }
    }
  }
}
