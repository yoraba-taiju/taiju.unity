using Reversible.Unity;
using UnityEngine;

namespace Witch.Bullet {
  public class SoraBullet : ReversibleBehaviour {
    private static int terrainLayer_;
    public Vector2 velocity;

    private new void Start() {
      var self = this as ReversibleBehaviour;
      self.Start();
      if (terrainLayer_ == 0) {
        terrainLayer_ = LayerMask.NameToLayer("Terrain");
      }
    }

    protected override void OnStart() {
    }

    protected override void OnForward() {
      var trans = transform;
      var localPosition = trans.localPosition;
      var delta = velocity * Time.deltaTime;
      localPosition.x += delta.x;
      localPosition.y += delta.y;
      trans.localPosition = localPosition;
    }

    /****************************************************************
     * Collision
     ****************************************************************/

    private void OnCollisionEnter2D(Collision2D other) {
      OnTrigger2D(other.gameObject);
    }

    private void OnCollisionStay2D(Collision2D other) {
      OnTrigger2D(other.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
      OnTrigger2D(other.gameObject);
    }

    private void OnTriggerStay2D(Collider2D other) {
      OnTrigger2D(other.gameObject);
    }

    private void OnTrigger2D(GameObject other) {
      if (!clockController.IsForwarding) {
        return;
      }

      if (other.layer == terrainLayer_) {
        Destroy();
      }
    }
  }
}