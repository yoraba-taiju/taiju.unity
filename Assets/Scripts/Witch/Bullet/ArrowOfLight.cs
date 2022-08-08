﻿using Enemy;
using Reversible.Unity;
using Reversible.Value;
using UnityEngine;
using Utility;

namespace Witch.Bullet {
  public class ArrowOfLight: ReversibleBehaviour {
    [SerializeField] private Color color = Color.white;
    [SerializeField] private float bodyRotationSpeed = 360.0f;
    [SerializeField] private float period = 1.0f;

    // Position management
    private Dense<Vector3> velocity_;

    // Body
    private Transform body_;
    private Dense<float> bodyRotation_;

    // target
    private Transform target_;
    private Rigidbody2D targetRigidbody_;
    private EnemyBehaviour targetBehaviour_;

    // Management
    private Dense<float> leftPeriod_;

    private new void Start() {
      var self = this as ReversibleBehaviour;
      self.Start();
    }

    protected override void OnStart() {
      var trans = transform;
      bodyRotation_ = new Dense<float>(clock, 0.0f);
      leftPeriod_ = new Dense<float>(clock, period);
      velocity_ = new Dense<Vector3>(clock, Vector3.zero);
      body_ = trans.Find("Body")!;
      {
        var material = body_.GetComponent<MeshRenderer>().material;
        material.color = color;
      }
      {
        var trailRenderer = body_.GetComponent<TrailRenderer>();
        var colorGradient = trailRenderer.colorGradient;
        var colorKeys = colorGradient.colorKeys;
        colorKeys[1].color = color;
        colorGradient.colorKeys = colorKeys;
        trailRenderer.colorGradient = colorGradient;
      }
    }

    private void Track(GameObject obj, Vector3 initialVelocity) {
      if (obj == null) {
        return;
      }
      target_ = obj.transform;
      targetRigidbody_ = obj.GetComponent<Rigidbody2D>();
      targetBehaviour_ = obj.GetComponent<EnemyBehaviour>();
      velocity_.Mut = initialVelocity;
    }

    protected override void OnForward() {
      var dt = Time.deltaTime;
      ref var bodyRotation = ref bodyRotation_.Mut;
      ref var velocity = ref velocity_.Mut;
      var trans = transform;
      ref var leftPeriod = ref leftPeriod_.Mut;
      leftPeriod -= dt;
      if (target_ != null) {
        if (leftPeriod < 0.0f) {
          targetBehaviour_.OnCollide(gameObject);
          Destroy();
          return;
        }
        var force = Mover.TrackingForce(
          trans.localPosition,
          velocity,
          target_.localPosition,
          targetRigidbody_.velocity,
          leftPeriod
        );
        velocity += force * dt;
      }
      bodyRotation += bodyRotationSpeed * dt;
      trans.localPosition += velocity * dt;
      trans.localRotation = Quaternion.FromToRotation(Vector3.right, velocity);
      body_.localRotation = Quaternion.Euler(bodyRotation, 0, 0);
    }
    protected override void OnReverse() {
      ref readonly var bodyRotation = ref bodyRotation_.Ref;
      body_.rotation = Quaternion.Euler(bodyRotation, 0, 0);
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
