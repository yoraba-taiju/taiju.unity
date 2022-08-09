using System.Security.Cryptography;
using Enemy;
using Reversible.Unity;
using Reversible.Value;
using UnityEngine;
using Utility;

namespace Witch.Bullet {
  public class ArrowOfLight: ReversibleBehaviour {
    [SerializeField] public Color color = Color.white;
    [SerializeField] private float bodyRotationSpeed = 360.0f;
    [SerializeField] private float period = 1.0f;
    [SerializeField] public Vector3 initialVelocity;

    // Position management
    private Dense<Vector3> velocity_;
    private Sparse<bool> isTracking_;
    private Sparse<bool> hit_;

    // Body
    private Transform body_;
    private Dense<float> bodyRotation_;

    // target
    private Transform target_;
    private Rigidbody2D targetRigidbody_;
    private EnemyBehaviour targetBehaviour_;

    // Management
    private Dense<float> totalTime_;
    private float Duration { get; set; }

    private new void Start() {
      var self = this as ReversibleBehaviour;
      self.Start();
    }

    protected override void OnStart() {
      var trans = transform;
      bodyRotation_ = new Dense<float>(clock, 0.0f);
      totalTime_ = new Dense<float>(clock, 0.0f);
      velocity_ = new Dense<Vector3>(clock, initialVelocity);
      isTracking_ = new Sparse<bool>(clock, true);
      body_ = trans.Find("Body")!;
      {
        var material = body_.GetComponent<MeshRenderer>().material;
        material.color = color;
      }
      var trailRenderer = GetComponent<TrailRenderer>();
      {
        var colorGradient = trailRenderer.colorGradient;
        var colorKeys = colorGradient.colorKeys;
        colorKeys[1].color = color;
        colorGradient.colorKeys = colorKeys;
        trailRenderer.colorGradient = colorGradient;
      }
      Duration = period + trailRenderer.time;
    }

    private void Track(GameObject obj) {
      target_ = obj.transform;
      targetRigidbody_ = obj.GetComponent<Rigidbody2D>();
      targetBehaviour_ = obj.GetComponent<EnemyBehaviour>();
    }

    protected override void OnForward() {
      var dt = Time.deltaTime;
      ref var bodyRotation = ref bodyRotation_.Mut;
      ref var velocity = ref velocity_.Mut;
      var trans = transform;
      ref var totalTime = ref totalTime_.Mut;
      totalTime += dt;
      if (totalTime <= period) {
        var isTracking = isTracking_.Ref;
        if (isTracking && (target_ == null || !target_.gameObject.activeSelf)) {
          isTracking = FindNextTarget();
        }
        if (isTracking) {
          var force = Mover.TrackingForce(
            trans.localPosition,
            velocity,
            target_.localPosition,
            targetRigidbody_.velocity,
            period - totalTime
          );
          velocity += force * dt;
        }
        bodyRotation += bodyRotationSpeed * dt;
        trans.localPosition += velocity * dt;
        trans.localRotation = Quaternion.FromToRotation(Vector3.right, velocity) * Quaternion.Euler(bodyRotation, 0, 0);
      } else if (totalTime <= Duration) {
        var scale = (Duration - totalTime) / (Duration - period);
        if (isTracking_.Ref) {
          if (!hit_.Ref) {
            targetBehaviour_.OnCollide(gameObject);
            hit_.Mut = true;
          } else {
            trans.localScale = Vector3.zero;
          }
        } else {
          trans.localScale = new Vector3(scale, scale, scale);
        }
      } else {
        Destroy();
      }
    }

    private bool FindNextTarget() {
      var minDistance = float.MaxValue;
      var trans = transform;
      GameObject nextTarget = null;
      bool found = false;
      foreach (var other in world.LivingObjects) {
        if (other.GetComponent<EnemyBehaviour>() == null) {
          continue;
        }
        var diff = other.localPosition - trans.localPosition;
        var distance = diff.magnitude;
        if (distance < minDistance) {
          minDistance = distance;
          nextTarget = other.gameObject;
          found = true;
        }
      }
      if (found) {
        Track(nextTarget);
      } else {
        isTracking_.Mut = false;
      }

      return found;
    }
    protected override void OnReverse() {
      ref readonly var bodyRotation = ref bodyRotation_.Ref;
      body_.localRotation = Quaternion.Euler(bodyRotation, 0, 0);
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
      if (clockHolder.IsLeaping) {
        return;
      }
    }
  }
  
}
