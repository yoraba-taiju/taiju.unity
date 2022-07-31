using Reversible.Unity;
using UnityEngine;

namespace Witch {
  public abstract class WitchBehaviour: ReversibleBehaviour {
    private int damageLayerMask_;
    private new void Start() {
      var self = this as ReversibleBehaviour;
      self.Start();
      damageLayerMask_ = LayerMask.GetMask("Enemy", "EnemyBullet");
    }

    private void OnCollisionEnter2D(Collision2D col) {
      OnCollision2D(col.gameObject);
    }

    private void OnCollisionStay2D(Collision2D col) {
      OnCollision2D(col.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col) {
      OnCollision2D(col.gameObject);
    }

    private void OnTriggerStay2D(Collider2D col) {
      OnCollision2D(col.gameObject);
    }

    private void OnCollision2D(GameObject other) {
      if (clockHolder.IsLeaping) {
        return;
      }
      if (((1 << other.layer) & damageLayerMask_) != 0) {
        OnCollide(other);
      }
    }

    protected abstract void OnCollide(GameObject other);
  }
}