﻿using System;
using Reversible;
using Reversible.Unity;
using UnityEngine;

namespace Effect {
  public abstract class EffectBehaviour : MonoBehaviour {
    protected ClockHolder clockHolder;
    protected Graveyard graveyard;
    protected Clock clock;
    private uint bornAt_;

    protected abstract void OnStart();
    protected abstract void OnUpdate();
    public void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      clockHolder = clockObj.GetComponent<ClockHolder>();
      graveyard = clockObj.GetComponent<Graveyard>();
      clock = clockHolder.Clock;
      bornAt_ = clock.CurrentTick;
      OnStart();
    }
    public void Update() {
      if (bornAt_ > clock.CurrentTick) {
        Destroy(gameObject);
        return;
      }
      OnUpdate();
    }
  }
}