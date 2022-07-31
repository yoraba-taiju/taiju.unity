﻿using Reversible.Unity;
using Reversible.Value;
using UnityEngine;
using Utility;

namespace Witch.Bullet {
  public class ArrowOfLight: ReversibleBehaviour {
    private Rigidbody2D rigidbody_;
    private Transform body_;
    [SerializeField] private float bodyRotationSpeed = 360.0f;
    [SerializeField] private float period = 3.0f;
    private Transform target_;
    private Rigidbody2D targetRigidbody_;
    private Dense<float> bodyRotation_;
    private Dense<float> leftPeriod_;
    protected override void OnStart() {
      bodyRotation_ = new Dense<float>(clock, 0.0f);
      leftPeriod_ = new Dense<float>(clock, period);
      rigidbody_ = GetComponent<Rigidbody2D>();
      rigidbody_.AddForce(Vector2.right * 7.0f, ForceMode2D.Impulse);
      body_ = transform.Find("Body");
    }

    public void Track(GameObject obj) {
      target_ = obj.transform;
      targetRigidbody_ = obj.GetComponent<Rigidbody2D>();
    }

    protected override void OnForward() {
      var dt = Time.deltaTime;
      ref var bodyRotation = ref bodyRotation_.Mut;
      ref var leftPeriod = ref leftPeriod_.Mut;
      bodyRotation += bodyRotationSpeed * dt;
      var force = Mover.TrackingForce(
        transform.localPosition,
        rigidbody_.velocity,
        target_.localPosition,
        targetRigidbody_.velocity,
        leftPeriod
      );
      rigidbody_.AddForce(force * (rigidbody_.mass * dt), ForceMode2D.Impulse);
      body_.localRotation = Quaternion.FromToRotation(Vector3.right, rigidbody_.velocity) * Quaternion.Euler(bodyRotation, 0, 0);
      leftPeriod -= dt;
      if (leftPeriod < 0.0f) {
        Destroy();
      }
    }
    protected override void OnReverse() {
      ref readonly var bodyRotation = ref bodyRotation_.Ref;
      body_.rotation = Quaternion.Euler(bodyRotation, 0, 0);
    }
  }
}
