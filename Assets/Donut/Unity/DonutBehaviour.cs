using System;
using Lib;
using UnityEngine;

namespace Donut.Unity {
  public abstract class DonutBehaviour: MonoBehaviour {
    private ClockComponent clock_;
    protected PlayerInput PlayerInput => clock_.PlayerInput;
    protected abstract void OnStart();
    private void Start() {
      var obj = GameObject.FindGameObjectWithTag("Clock");
      clock_ = obj.GetComponent<ClockComponent>();
      OnStart();
    }
    protected abstract void OnUpdate();
    private void Update() {
      OnUpdate();
    }
  }
}