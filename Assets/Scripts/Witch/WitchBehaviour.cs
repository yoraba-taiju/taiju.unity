using Reversible.Unity;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Witch {
  public abstract class WitchBehaviour: ReversibleBehaviour {
    private int damageLayerMask_;
    private new void Start() {
      var self = this as ReversibleBehaviour;
      self.Start();
      damageLayerMask_ = LayerMask.GetMask("Enemy", "EnemyBullet");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      if (!clock.IsTicking) {
        return;
      }
      var layer = collision.gameObject.layer;
      if (((1 << layer) & damageLayerMask_) != 0) {
        OnCollide(collision);
      }
    }

    private void OnCollisionStay2D(Collision2D collision) {
      if (!clock.IsTicking) {
        return;
      }
      var layer = collision.gameObject.layer;
      if (((1 << layer) & damageLayerMask_) != 0) {
        OnCollide(collision);
      }
    }

    protected abstract void OnCollide(Collision2D collision);
  }
}