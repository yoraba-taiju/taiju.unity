using UnityEngine;

namespace Witch.Bullet {
  public class SoraBullet : Reversible.Unity.ReversibleBehaviour {
    private int terrainLayer_;

    private new void Start() {
      var self = this as Reversible.Unity.ReversibleBehaviour;
      self.Start();
      terrainLayer_ = LayerMask.NameToLayer("Terrain");
    }

    protected override void OnStart() {
      
    }

    protected override void OnForward() {
    }

    private void OnCollisionEnter2D(Collision2D other) {
      Destroy();
    }

    private void OnCollisionStay2D(Collision2D other) {
      Destroy();
    }
    
    private void OnTriggerEnter2D(Collider2D other) {
      OnTrigger2D(other);
    }
    
    private void OnTriggerStay2D(Collider2D other) {
      OnTrigger2D(other);
    }

    private void OnTrigger2D(Collider2D other) {
      if (clockHolder.IsLeaping) {
        return;
      }
      if (other.gameObject.layer == terrainLayer_) {
        Destroy();
      }
    }
  }
}
