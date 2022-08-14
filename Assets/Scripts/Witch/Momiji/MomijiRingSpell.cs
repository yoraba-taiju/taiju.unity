﻿using Reversible.Unity;
using UnityEngine;

namespace Witch.Momiji {
  public class MomijiRingSpell: ReversibleBehaviour {
    private float startAt_;
    private Vector3 pole_;
    [SerializeField] private float angularVelocity = 230f;
    [SerializeField] private float poleAngularVelocity = 230f;
    protected override void OnStart() {
      startAt_ = IntegrationTime;
      pole_ = Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180)) * Vector3.right;
    }

    protected override void OnForward() {
      var totalTime = IntegrationTime - startAt_;
      var trans = transform;
      var scale = Mathf.Min(Mathf.Pow(totalTime ,2.0f) * 3.0f, 10.0f);
      trans.localScale  = new Vector3(scale, scale, 1.0f);
      trans.localRotation = Quaternion.AngleAxis(totalTime * poleAngularVelocity, pole_) *
                            Quaternion.AngleAxis(totalTime * angularVelocity, Vector3.up);
    }
  }
}