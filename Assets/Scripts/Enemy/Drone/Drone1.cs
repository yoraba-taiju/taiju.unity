using Reversible.Unity;
using UnityEngine;

namespace Enemy.Drone {
  public class Drone1: ReversibleBehaviour {

    [HideInInspector] public GameObject sora;

    protected override void OnStart() {
      sora = GameObject.FindWithTag("Player");
    }

    protected override void OnForward() {
    }

    private void OnCollisionEnter2D(Collision2D col) {
      
    }

    private void OnCollisionStay2D(Collision2D collision) {
      
    }
  }
}
