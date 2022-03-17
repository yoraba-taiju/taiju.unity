using System.Linq;
using Donut.Unity;
using Donut.Values;
using UnityEngine;

namespace Donut.Reversible {
  public class ReversibleTransform: MonoBehaviour {
    private Graveyard graveyard_;

    // Clock
    private ClockHolder holder_;
    private Clock clock_;
    private uint bornAt_;

    // Visibility
    [SerializeField] public bool destroyWhenInvisible = true;
    private Renderer[] renderers_;
    private bool wasVisible_;

    // Transform records
    private struct Record {
      public Vector3 position;
      public Vector3 scale;
      public Quaternion rot;
    }
    private Dense<Record> record_;
    private void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      holder_ = clockObj.GetComponent<ClockHolder>();
      clock_ = holder_.Clock;
      graveyard_ = clockObj.GetComponent<Graveyard>();
      bornAt_ = clock_.CurrentTick;
      if (destroyWhenInvisible) {
        renderers_ = GetComponentsInChildren<Renderer>();
        wasVisible_ = renderers_.All(it => it.isVisible);
      }
      var trans = transform;
      record_ = new Dense<Record>(clock_, new Record() {
        position = trans.localPosition,
        scale = trans.localScale,
        rot = trans.localRotation,
      });
    }

    private void Update() {
      if (bornAt_ > clock_.CurrentTick) {
        Destroy(gameObject);
        return;
      }
      var ticked = holder_.Ticked;
      var backed = holder_.Backed;
      if (destroyWhenInvisible) {
        if (ticked) {
          var visible = renderers_.Any(it => it.isVisible);
          if (wasVisible_) {
            if (!visible) {
              graveyard_.Destroy(gameObject);
            }
          } else {
            wasVisible_ = visible;
          }
        } else if (backed) {
          wasVisible_ = false;
        }
      }
      { // Manage transforms
        var trans = transform;
        if (ticked) {
          ref var record = ref record_.Mut;
          record.position = trans.localPosition;
          record.scale = trans.localScale;
          record.rot = trans.localRotation;
        } else if (backed) {
          ref readonly var record = ref record_.Ref;
          trans.localPosition = record.position;
          trans.localScale = record.scale;
          trans.localRotation = record.rot;
        }
      }
    }
  }
}
