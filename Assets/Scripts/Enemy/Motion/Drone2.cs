using UnityEngine;

namespace Enemy.Motion {
  public class Drone2: MonoBehaviour {

    [HideInInspector] public GameObject sora;

    private void Start() {
      sora = GameObject.FindWithTag("Player");
    }
  }
}
