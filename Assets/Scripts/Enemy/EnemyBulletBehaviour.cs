using Reversible.Unity;
using UnityEngine;

namespace Enemy {
  public abstract class EnemyBulletBehaviour : ReversibleBehaviour {
    private static int witchLayer_;
    private static int terrainLayer_;

    private new void Start() {
      var self = this as ReversibleBehaviour;
      self.Start();
      if (witchLayer_ == 0) {
        witchLayer_ = LayerMask.NameToLayer("Witch");
      }

      if (terrainLayer_ == 0) {
        terrainLayer_ = LayerMask.NameToLayer("Terrain");
      }
    }

    private void OnCollisionEnter2D(Collision2D other) {
      OnCollision2D(other.gameObject);
    }

    private void OnCollisionStay2D(Collision2D other) {
      OnCollision2D(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
      OnCollision2D(other.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other) {
      OnCollision2D(other.gameObject);
    }

    private void OnCollision2D(GameObject other) {
      if (!clockController.IsForwarding) {
        return;
      }

      var layer = other.layer;
      if (layer == terrainLayer_) {
        Destroy();
        return;
      }

      if (layer == witchLayer_) {
        OnCollideWithWitch(other);
      }
    }

    protected abstract void OnCollideWithWitch(GameObject other);
  }
}