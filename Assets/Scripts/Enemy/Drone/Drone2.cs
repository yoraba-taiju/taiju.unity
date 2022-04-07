using Reversible.Unity;
using UnityEngine;

namespace Enemy.Drone {
  public class Drone2: EnemyBehaviour {

    [HideInInspector] public GameObject sora;
    [SerializeField] public float shield = 1.0f;
    [SerializeField] public GameObject explosion;

    protected override void OnStart() {
      sora = GameObject.FindWithTag("Player");
    }

    protected override void OnForward() {
    }

    protected override void OnCollide(Collision2D collision) {
      shield -= 1.0f;
      if (shield <= 0) {
        Destroy();
        var e = Instantiate(explosion, transform.parent);
        e.transform.localPosition = transform.localPosition;
      }
    }
  }
}
