using Reversible.Unity;
using UnityEngine;

namespace Enemy.Drone {
  public class Drone1: EnemyBehaviour {

    [HideInInspector] public GameObject sora;

    protected override void OnStart() {
      sora = GameObject.FindWithTag("Player");
    }

    protected override void OnForward() {
    }

    protected override void OnCollide(Collision2D collision) {
      
    }
  }
}
