using Donut.Unity;
using Donut.Values;
using UnityEngine;

namespace Donut.Reversible {
  public class ReversibleRenderer: MonoBehaviour {
    // Clock
    private Clock clock_;
    private uint bornAt_;

    // Visibility
    private Renderer renderer_;

    // Transform records
    private Dense<Vector2> offset_;
    private void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      clock_ = clockObj.GetComponent<ClockHolder>().Clock;
      bornAt_ = clock_.CurrentTick;
      renderer_ = gameObject.GetComponent<Renderer>();
      offset_ = new Dense<Vector2>(clock_, renderer_.material.mainTextureOffset);
    }

    private void Update() {
      if (bornAt_ > clock_.CurrentTick) {
        Destroy(gameObject);
        return;
      }

      if (clock_.IsTicking) {
        offset_.Value = renderer_.material.mainTextureOffset;
      } else {
        renderer_.material.mainTextureOffset = offset_.Value;
      }
    }
  }
}
