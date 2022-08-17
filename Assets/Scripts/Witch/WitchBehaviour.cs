using Reversible.Unity;
using UnityEngine;

namespace Witch {
  public abstract class WitchBehaviour : ReversibleBehaviour {
    private int collisionLayers_;
    private static int layerMask_;

    private new void Start() {
      base.Start();
      if (layerMask_ == 0) {
        layerMask_ = LayerMask.GetMask("Enemy", "EnemyBullet");
      }

      collisionLayers_ = layerMask_;
    }

    private void OnCollisionEnter2D(Collision2D col) {
      OnCollisionAll2D(col.gameObject);
    }

    private void OnCollisionStay2D(Collision2D col) {
      OnCollisionAll2D(col.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col) {
      OnCollisionAll2D(col.gameObject);
    }

    private void OnTriggerStay2D(Collider2D col) {
      OnCollisionAll2D(col.gameObject);
    }

    private void OnCollisionAll2D(GameObject other) {
      if (!player.IsForwarding) {
        return;
      }

      if (((1 << other.layer) & collisionLayers_) != 0) {
        OnCollision2D(other);
      }
    }

    protected abstract void OnCollision2D(GameObject other);
  }
}