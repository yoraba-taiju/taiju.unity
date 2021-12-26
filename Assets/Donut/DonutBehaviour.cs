using System;
using Lib;
using UnityEngine;

namespace Donut {
  public abstract class DonutBehaviour: MonoBehaviour {
    private Clock clock_;
    protected PlayerInput PlayerInput => clock_.PlayerInput;
    protected abstract void OnStart();
    private void Start() {
      var obj = GameObject.FindGameObjectWithTag("Clock");
      clock_ = obj.GetComponent<Clock>();
      OnStart();
    }
    protected abstract void OnUpdate();
    private void Update() {
      OnUpdate();
    }
  }
}