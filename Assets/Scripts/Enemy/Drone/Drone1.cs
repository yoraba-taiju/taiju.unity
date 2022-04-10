using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Enemy.Drone {
  public class Drone1: EnemyBehaviour {

    [HideInInspector] public GameObject sora;
    [SerializeField] public float initialShield = 10.0f;
    private Sparse<float> shield_;
    [SerializeField] public GameObject explosionEffect;
    [SerializeField] public GameObject bullet;

    protected override void OnStart() {
      sora = GameObject.FindWithTag("Player");
      shield_ = new Sparse<float>(clock, initialShield);
    }

    protected override void OnForward() {
    }

    protected override void OnCollide(Collision2D collision) {
      ref var shield = ref shield_.Mut;
      shield -= 1.0f;
      if (shield <= 0) {
        Destroy();
        var e = Instantiate(explosionEffect, transform.parent);
        e.transform.localPosition = transform.localPosition;
      }
    }
  }
}
