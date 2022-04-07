using Reversible.Unity;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Enemy {
  public abstract class EnemyBehaviour: ReversibleBehaviour {
    private int damageLayerMask_;
    private new void Start() {
      var self = this as ReversibleBehaviour;
      self.Start();
      damageLayerMask_ = LayerMask.GetMask("Witch", "WitchBullet");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      var layer = collision.gameObject.layer;
      if (((1 << layer) & damageLayerMask_) != 0) {
        OnCollide(collision);
      }
    }

    private void OnCollisionStay2D(Collision2D collision) {
      var layer = collision.gameObject.layer;
      if (((1 << layer) & damageLayerMask_) != 0) {
        OnCollide(collision);
      }
    }

    protected abstract void OnCollide(Collision2D collision);
  }
}