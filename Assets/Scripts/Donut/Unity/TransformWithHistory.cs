using System.Linq;
using Donut.Values;
using UnityEngine;

namespace Donut.Unity {
  public class TransformWithHistory: MonoBehaviour {
    // Clock
    private ClockComponent clockComponent_;
    private Clock clock_;
    private uint bornAt_;

    // Visibility
    [SerializeField] public bool destroyWhenInvisible = true;
    private Renderer[] renderers_;
    private bool wasVisible_;

    // Transform records
    private Dense<Vector3> position_;
    private Dense<Vector3> scale_;
    private Dense<Quaternion> rot_;
    private void Start() {
      var obj = GameObject.FindGameObjectWithTag("Clock");
      clockComponent_ = obj.GetComponent<ClockComponent>();
      clock_ = clockComponent_.Clock;
      bornAt_ = clock_.CurrentTick;
      if (destroyWhenInvisible) {
        renderers_ = GetComponentsInChildren<Renderer>();
        wasVisible_ = renderers_.All(it => it.isVisible);
      }
      var trans = transform;
      position_ = new Dense<Vector3>(clock_, trans.localPosition);
      scale_ = new Dense<Vector3>(clock_, trans.localScale);
      rot_ = new Dense<Quaternion>(clock_, trans.localRotation);
    }

    private void Update() {
      if (bornAt_ > clock_.CurrentTick) {
        Destroy(gameObject);
        return;
      }
      if(destroyWhenInvisible) { // Visibility Management
        var visible = renderers_.Any(it => it.isVisible);
        if (wasVisible_) {
          if (!visible) {
            // TODO
            Destroy(gameObject);
          }
        } else {
          wasVisible_ = visible;
        }
      }
      { // Manage transforms
        var trans = transform;
        if (clock_.IsTicking) {
          position_.Value = trans.localPosition;
          scale_.Value = trans.localScale;
          rot_.Value = trans.localRotation;
        } else {
          trans.localPosition = position_.Value;
          trans.localScale = scale_.Value;
          trans.localRotation = rot_.Value;
        }
      }
    }
  }
}
