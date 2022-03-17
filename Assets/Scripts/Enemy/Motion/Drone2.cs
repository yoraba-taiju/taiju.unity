using System;
using Donut.Unity;
using UnityEngine;

namespace Enemy.Motion {
  public class Drone2: MonoBehaviour {

    public GameObject sora;

    private void Start() {
      sora = GameObject.FindWithTag("Player");
    }
  }
}