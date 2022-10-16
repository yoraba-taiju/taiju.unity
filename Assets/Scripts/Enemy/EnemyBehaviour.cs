using System;
using Effect;
using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Enemy {
  public abstract class EnemyBehaviour : ReversibleBehaviour {
    private static int layerMask_;
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private MagicElementEmitter magicElementEmitterPrefab;
    [SerializeField] private float initialShield;
    private Sparse<float> shield_;

    private new void Start() {
      if (layerMask_ == 0) {
        layerMask_ = LayerMask.GetMask("WitchBullet");
      }
      shield_ = new Sparse<float>(clock, initialShield);
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
    
    public virtual void OnCollision2D(GameObject other) {
      if (!gameObject.activeSelf) {
        return;
      }
      ref var shield = ref shield_.Mut;
      shield -= 1.0f;
      if (shield > 0.0f) {
        return;
      }
      var trans = transform;
      var parent = trans.parent;
      var localPosition = trans.localPosition;
        
      Instantiate(explosionEffectPrefab, localPosition, Quaternion.identity, parent);
      var emitter = Instantiate(magicElementEmitterPrefab, localPosition, Quaternion.identity, parent);
      emitter.numMagicElements = Math.Max(1, Mathf.RoundToInt( initialShield / 3f));

      Deactivate();
    }

    #endregion
  }
}