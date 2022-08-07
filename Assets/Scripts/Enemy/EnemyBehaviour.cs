using Reversible.Unity;
using UnityEngine;

namespace Enemy {
  public abstract class EnemyBehaviour: ReversibleBehaviour {
    private int collisionLayers_;
    private new void Start() {
      var self = this as ReversibleBehaviour;
      self.Start();
      collisionLayers_ = LayerMask.GetMask("Witch", "WitchBullet");
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      OnCollision2D(collision);
    }

    private void OnCollisionStay2D(Collision2D collision) {
      OnCollision2D(collision);
    }

    private void OnCollision2D(Collision2D collision) {
      if (clockHolder.IsLeaping) {
        return;
      }
      var pos = transform.localPosition;
      if (Mathf.Abs(pos.x) >= 18.0f || Mathf.Abs(pos.y) >= 10.0f) {
        return;
      }

      var obj = collision.gameObject;
      var layer = obj.layer;
      if (((1 << layer) & collisionLayers_) != 0) {
        OnCollide(obj);
      }
    }

    public abstract void OnCollide(GameObject other);
  }
}
