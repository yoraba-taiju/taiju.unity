using System.Linq;
using Donut.Unity;
using Donut.Values;
using UnityEngine;

namespace Donut.Reversible {
  public class ReversibleRigidbody2D: MonoBehaviour {
    private Graveyard graveyard_;
    private Rigidbody2D body_;

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
      public Vector2 position;
      public Vector2 velocity;
      public float rotation;
      public float angularVelocity;
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
      body_ = GetComponent<Rigidbody2D>();
      record_ = new Dense<Record>(clock_, new Record {
        position = body_.position,
        velocity = body_.velocity,
        rotation = body_.rotation,
        angularVelocity = body_.angularVelocity,
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
        if (ticked) {
          ref var record = ref record_.Mut;
          record = new Record {
            position = body_.position,
            velocity = body_.velocity,
            rotation = body_.rotation,
            angularVelocity = body_.angularVelocity,
          };
        } else if (backed) {
          ref readonly var record = ref record_.Ref;
          body_.position = record.position;
          body_.velocity = record.velocity;
          body_.rotation = record.rotation;
          body_.angularVelocity = record.angularVelocity;
        }
      }
    }
  }
}
