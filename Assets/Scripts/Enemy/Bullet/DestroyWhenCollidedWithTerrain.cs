using System;
using Donut;
using Donut.Unity;
using UnityEngine;

namespace Enemy.Bullet {
  public class DestroyWhenCollidedWithTerrain : MonoBehaviour {
    private Graveyard graveyard_;
    private Clock clock_;
    private void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      graveyard_ = clockObj.GetComponent<Graveyard>();
      clock_ = clockObj.GetComponent<ClockHolder>().Clock;
    }

    private void OnTriggerStay2D(Collider2D col) {
      if (col.gameObject.CompareTag("Terrain") && clock_.IsTicking) {
        graveyard_.Destroy(gameObject);
      }
    }
  }
}