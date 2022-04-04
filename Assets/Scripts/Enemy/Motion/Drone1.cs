using UnityEngine;

namespace Enemy.Motion {
  public class Drone1: MonoBehaviour {

    public GameObject sora;

    private void Start() {
      sora = GameObject.FindWithTag("Player");
    }
  }
}