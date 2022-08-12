using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using Transform = Reversible.Unity.Companion.Transform;

namespace Reversible.Unity {
  public sealed class ReversibleComponents : MonoBehaviour {
    // Clock
    private ClockController controller_;
    private Clock clock_;
    private uint bornAt_;

    // Visibility
    [SerializeField] public bool destroyWhenInvisible = true;
    private World world_;
    private Renderer[] renderers_;
    private bool wasVisible_;

    // Components
    public enum Component {
      None,
      Transform,
      Rigidbody2D,
      ParticleSystem,
      Animator,
      PlayableDirector,
    }

    [SerializeField] public Component[] targetComponents = {Component.Transform};
    private ICompanion[] companions_;

    private void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      controller_ = clockObj.GetComponent<ClockController>();
      clock_ = controller_.Clock;
      bornAt_ = clock_.CurrentTick;
      if (destroyWhenInvisible) {
        renderers_ = GetComponentsInChildren<Renderer>();
        if (renderers_.Length == 0) {
          Debug.LogWarning("No renderers attached.");
          destroyWhenInvisible = false;
        }

        world_ = clockObj.GetComponent<World>();
        wasVisible_ = false;
      }

      if (targetComponents.Length <= 0) {
        return;
      }

      companions_ = new ICompanion[targetComponents.Length];
      var i = -1;
      foreach (var target in targetComponents) {
        i++;
        companions_[i] = target switch {
          Component.None => throw new InvalidEnumArgumentException("Please set some target"),
          Component.Transform => new Transform(controller_, transform),
          Component.Rigidbody2D => new Companion.Rigidbody2D(controller_, GetComponent<Rigidbody2D>()),
          Component.ParticleSystem => new Companion.ParticleSystem(controller_, GetComponent<ParticleSystem>()),
          Component.Animator => new Companion.Animator(controller_, GetComponent<Animator>()),
          Component.PlayableDirector => new Companion.PlayableDirector(controller_, GetComponent<PlayableDirector>()),
          _ => throw new InvalidEnumArgumentException($"Unknown target: {target}"),
        };
      }
    }

    private void Update() {
      if (bornAt_ > clock_.CurrentTick) {
        Destroy(gameObject);
        return;
      }

      var ticked = controller_.Ticked;
      var backed = controller_.Backed;
      var leaped = controller_.Leaped;
      if (destroyWhenInvisible) {
        if (ticked) {
          var visible = false;
          foreach (var r in renderers_) {
            visible |= r.isVisible;
          }

          if (wasVisible_) {
            if (!visible) {
              world_.Destroy(gameObject);
            }
          } else {
            wasVisible_ = visible;
          }
        } else if (backed) {
          wasVisible_ = false;
        }
      }

      if (companions_ == null) {
        return;
      }

      if (ticked) {
        foreach (var companion in companions_) {
          companion.OnTick();
        }
      } else if (backed) {
        foreach (var companion in companions_) {
          companion.OnBack();
        }
      } else if (leaped) {
        foreach (var companion in companions_) {
          companion.OnLeap();
        }
      }
    }
  }
}