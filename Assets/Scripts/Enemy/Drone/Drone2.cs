using Reversible.Unity;
using UnityEngine;

namespace Enemy.Drone {
  public class Drone2: ReversibleBehaviour {

    [HideInInspector] public GameObject sora;
    [SerializeField] public float shield = 1.0f;
    private int damageLayerMask_;

    protected override void OnStart() {
      sora = GameObject.FindWithTag("Player");
      damageLayerMask_ = LayerMask.GetMask("Witch", "WitchBullet");
    }

    protected override void OnForward() {
    }

    private void OnCollisionEnter2D(Collision2D collision) {
      var layer = collision.gameObject.layer;
      if (((1 << layer) & damageLayerMask_) != 0) {
        OnDamage();
      }
    }

    private void OnCollisionStay2D(Collision2D collision) {
      var layer = collision.gameObject.layer;
      if (((1 << layer) & damageLayerMask_) != 0) {
        OnDamage();
      }
    }

    private void OnDamage() {
      shield -= 1.0f;
      Debug.Log($"Left: {shield}");
      if (shield <= 0) {
        Destroy();
      }
    }
  }
}
