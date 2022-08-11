using Reversible.Unity;
using UnityEngine;

namespace Witch.Bullet {
  public class SoraBullet : ReversibleBehaviour {
    private int terrainLayer_;

    private new void Start() {
      var self = this as ReversibleBehaviour;
      self.Start();
      terrainLayer_ = LayerMask.NameToLayer("Terrain");
    }

    protected override void OnStart() {
      
    }

    protected override void OnForward() {
    }

    /****************************************************************
     * Collision
     ****************************************************************/

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
      if (!clockController.IsForwarding) {
        return;
      }
      if (other.gameObject.layer == terrainLayer_) {
        Destroy();
      }
    }
  }
}
