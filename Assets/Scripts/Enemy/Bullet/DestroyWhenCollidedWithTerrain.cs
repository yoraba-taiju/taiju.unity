using System;
using Donut;
using Donut.Unity;
using UnityEngine;

namespace Enemy.Bullet {
  public class DestroyWhenCollidedWithTerrain : MonoBehaviour {
    private Graveyard graveyard_;
    private Clock clock_;
    private void Start() {
      var obj = GameObject.FindGameObjectWithTag("Clock");
      graveyard_ = obj.GetComponent<Graveyard>();
      clock_ = obj.GetComponent<ClockComponent>().Clock;
    }

    private void OnTriggerEnter2D(Collider2D col) {
      if (col.gameObject.CompareTag("Terrain") && clock_.IsTicking) {
        graveyard_.Destroy(gameObject);
      }
    }
  }
}