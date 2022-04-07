using Reversible.Unity;
using UnityEngine;

namespace Enemy.Drone {
  public class Drone2: EnemyBehaviour {

    [HideInInspector] public GameObject sora;
    [SerializeField] public float shield = 1.0f;

    protected override void OnStart() {
      sora = GameObject.FindWithTag("Player");
    }

    protected override void OnForward() {
    }

    protected override void OnCollide(Collision2D collision) {
      shield -= 1.0f;
      Debug.Log($"Left: {shield}");
      if (shield <= 0) {
        Destroy();
      }
    }
  }
}
