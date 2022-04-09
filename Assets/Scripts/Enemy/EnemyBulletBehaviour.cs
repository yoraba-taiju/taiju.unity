using UnityEngine;

namespace Enemy {
  public abstract class EnemyBulletBehaviour: Reversible.Unity.ReversibleBehaviour {
    private int witchLayerMask_;
    private new void Start() {
      var self = this as Reversible.Unity.ReversibleBehaviour;
      self.Start();
      witchLayerMask_ = LayerMask.NameToLayer("Witch");
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
      OnCollision2D(other);
    }

    private void OnCollisionStay2D(Collision2D other) {
      OnCollision2D(other);
    }

    private void OnCollision2D(Collision2D other) {
      if (!clock.IsTicking) {
        return;
      }
      if (other.gameObject.layer == witchLayerMask_) {
        OnCollide(other);
      }
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
    protected abstract void OnCollide(Collision2D collision);
  }
}
