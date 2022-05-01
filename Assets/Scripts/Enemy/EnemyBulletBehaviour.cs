using UnityEngine;

namespace Enemy {
  public abstract class EnemyBulletBehaviour: Reversible.Unity.ReversibleBehaviour {
    private int witchLayer_;
    private int terrainLayer_;
    private new void Start() {
      var self = this as Reversible.Unity.ReversibleBehaviour;
      self.Start();
      witchLayer_ = LayerMask.NameToLayer("Witch");
      terrainLayer_ = LayerMask.NameToLayer("Terrain");
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
      var layer = other.gameObject.layer;
      if (layer == terrainLayer_) {
        Destroy();
        return;
      }
      if (layer == witchLayer_) {
        OnCollideWithWitch(other);
      }
    }

    protected abstract void OnCollideWithWitch(GameObject other);

    protected static Vector2 CalcSpeed(Vector2 direction, float speed, float angle) {
      if (direction.magnitude <= float.Epsilon) {
        return Vector2.left * speed;
      }
      return Quaternion.AngleAxis(angle, Vector3.forward) * direction.normalized * speed;
    }
  }
}
