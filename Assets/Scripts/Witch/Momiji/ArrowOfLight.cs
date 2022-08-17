using Enemy;
using Reversible.Unity;
using Reversible.Unity.ReversibleGameObject;
using Reversible.Value;
using UnityEngine;
using Utility;

namespace Witch.Momiji {
  public class ArrowOfLight : ReversibleBehaviour {
    [SerializeField] public Color color = Color.white;
    [SerializeField] private float period = 1.0f;
    [SerializeField] public Vector3 initialVelocity;

    // Position management
    private Dense<Vector3> velocity_;
    private Sparse<bool> isTracking_;
    private Sparse<bool> alreadyHit_;

    // target
    private EnemyBehaviour target_;
    private Rigidbody2D targetRigidbody_;

    // Management
    private Dense<float> totalTime_;
    private float Duration { get; set; }

    private new void Start() {
      base.Start();
    }

    protected override void OnStart() {
      totalTime_ = new Dense<float>(clock, 0.0f);
      velocity_ = new Dense<Vector3>(clock, initialVelocity);
      isTracking_ = new Sparse<bool>(clock, true);
      alreadyHit_ = new Sparse<bool>(clock, false);
      var lineRenderer = GetComponent<LineRenderer>();
      lineRenderer.startColor = color;
      GetComponent<Light>().color = color;
      Duration = period + GetComponent<ReversibleTrail>().lifeTime;
    }

    private void Track(EnemyBehaviour obj) {
      target_ = obj;
      targetRigidbody_ = obj.GetComponent<Rigidbody2D>();
    }

    protected override void OnForward() {
      var dt = Time.deltaTime;
      ref var velocity = ref velocity_.Mut;
      var trans = transform;
      ref var totalTime = ref totalTime_.Mut;
      totalTime += dt;
      if (totalTime <= period) {
        var isTracking = isTracking_.Ref;
        if (isTracking && !world.LivingEnemies.Contains(target_)) {
          isTracking = FindNextTarget();
        }

        if (isTracking) {
          var force = Mover.TrackingForce(
            trans.localPosition,
            velocity,
            target_.transform.localPosition,
            targetRigidbody_.velocity,
            period - totalTime
          );
          velocity += force * dt;
        }

        trans.localPosition += velocity * dt;
      } else if (totalTime <= Duration) {
        if (!isTracking_.Ref || alreadyHit_.Ref) {
          return;
        }

        target_.OnCollision2D(gameObject);
        alreadyHit_.Mut = true;
        isTracking_.Mut = false;
      } else {
        Destroy();
      }
    }

    private bool FindNextTarget() {
      var minDistance = float.MaxValue;
      var trans = transform;
      EnemyBehaviour nextTarget = null;
      var found = false;
      foreach (var other in world.LivingEnemies) {
        if (!other.CanCollide()) {
          continue;
        }
        var diff = other.transform.localPosition - trans.localPosition;
        var distance = diff.magnitude;
        if (distance >= minDistance) {
          continue;
        }

        minDistance = distance;
        nextTarget = other;
        found = true;
      }

      if (found) {
        Track(nextTarget);
      } else {
        isTracking_.Mut = false;
      }

      return found;
    }

    /****************************************************************
     * Collision
     ****************************************************************/

    private void OnCollisionEnter2D(Collision2D other) {
      Destroy();
    }

    private void OnCollisionStay2D(Collision2D other) {
      Destroy();
    }

    private void OnTriggerEnter2D(Collider2D other) {
      OnTrigger2D(other);
    }

    private void OnTriggerStay2D(Collider2D other) {
      OnTrigger2D(other);
    }

    private void OnTrigger2D(Collider2D other) {
      if (!player.IsForwarding) {
        return;
      }
    }
  }
}