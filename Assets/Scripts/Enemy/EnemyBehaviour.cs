using System;
using Reversible.Unity;
using UnityEngine;
using UnityEngine.Assertions;

namespace Enemy {
  public abstract class EnemyBehaviour : ReversibleBehaviour {
    private static int layerMask_;

    private new void Start() {
      if (layerMask_ == 0) {
        layerMask_ = LayerMask.GetMask("WitchBullet");
      }
      base.Start();
      world.RegisterEnemy(this);
    }

    public override void OnDeactivated() {
      world.UnregisterEnemy(this);
    }

    public override void OnReactivated() {
      world.RegisterEnemy(this);
    }

    #region Collision

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

    private bool CanCollide() {
      var pos = transform.localPosition;
      return Mathf.Abs(pos.x) <= 18.0f && Mathf.Abs(pos.y) <= 10.0f;
    }

    public bool CanTrack() {
      var pos = transform.localPosition;
      return Mathf.Abs(pos.x) <= 20.0f && Mathf.Abs(pos.y) <= 12.0f;
    }

    private void OnCollisionAll2D(GameObject other) {
      if (!player.IsForwarding || !CanCollide()) {
        return;
      }

      var obj = other.gameObject;
      var layer = obj.layer;
      if (((1 << layer) & layerMask_) != 0) {
        OnCollision2D(obj);
      }
    }

    public abstract void OnCollision2D(GameObject other);

    #endregion
  }
}