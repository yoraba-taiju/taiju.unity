using System;
using Reversible.Unity;
using UnityEngine;

namespace Enemy {
  public abstract class EnemyBehaviour : ReversibleBehaviour {
    private static int layerMask_;

    private new void Start() {
      base.Start();
      world.RegisterEnemy(transform);
      if (layerMask_ == 0) {
        layerMask_ = LayerMask.GetMask("WitchBullet");
      }
    }

    private void OnCollisionEnter2D(Collision2D other) {
      OnCollisionAll2D(other.gameObject);
    }

    private void OnCollisionStay2D(Collision2D other) {
      OnCollisionAll2D(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
      OnCollisionAll2D(other.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other) {
      OnCollisionAll2D(other.gameObject);
    }

    private void OnCollisionAll2D(GameObject other) {
      if (!player.IsForwarding) {
        return;
      }

      var pos = transform.localPosition;
      if (Mathf.Abs(pos.x) >= 18.0f || Mathf.Abs(pos.y) >= 10.0f) {
        return;
      }

      var obj = other.gameObject;
      var layer = obj.layer;
      if (((1 << layer) & layerMask_) != 0) {
        OnCollision2D(obj);
      }
    }

    public abstract void OnCollision2D(GameObject other);
  }
}