using System;
using UnityEngine;
using Reversible.Unity;

namespace Witch.Bullet {
  public class SoraBullet : ReversibleBehaviour {

    protected override void OnStart() {
    }

    protected override void OnForward() {
    }

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
      if (!clock.IsTicking) {
        return;
      }
      if (other.gameObject.CompareTag("Terrain")) {
        Destroy();
      }
    }
  }
}
