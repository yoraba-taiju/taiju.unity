using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using Transform = Reversible.Unity.Companion.Transform;

namespace Reversible.Unity {
  public sealed class ReversibleComponents: MonoBehaviour {
    // Clock
    private ClockHolder holder_;
    private Clock clock_;
    private uint bornAt_;

    // Visibility
    [SerializeField] public bool destroyWhenInvisible = true;
    private Graveyard graveyard_;
    private Renderer[] renderers_;
    private bool wasVisible_;

    // Components
    public enum Component {
      None,
      Transform,
      Rigidbody2D,
      ParticleSystem,
      TrailRenderer,
      Animator,
      PlayableDirector,
    }
    [SerializeField] public Component[] targetComponents = {Component.Transform};
    private ICompanion[] companions_;

    private void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      holder_ = clockObj.GetComponent<ClockHolder>();
      clock_ = holder_.Clock;
      bornAt_ = clock_.CurrentTick;
      if (destroyWhenInvisible) {
        renderers_ = GetComponentsInChildren<Renderer>();
        if (renderers_.Length == 0) {
          Debug.LogWarning("No renderers attached.");
          destroyWhenInvisible = false;
        }
        graveyard_ = clockObj.GetComponent<Graveyard>();
        wasVisible_ = false;
      }
      companions_ = new ICompanion[targetComponents.Length];
      var i = -1;
      foreach (var target in targetComponents) {
        i++;
        companions_[i] = target switch {
          Component.None => throw new InvalidEnumArgumentException("Please set some target"),
          Component.Transform => new Transform(holder_, transform),
          Component.Rigidbody2D => new Companion.Rigidbody2D(holder_, GetComponent<Rigidbody2D>()),
          Component.TrailRenderer => new Companion.TrailRenderer(holder_, GetComponent<TrailRenderer>()),
          Component.ParticleSystem => new Companion.ParticleSystem(holder_, GetComponent<ParticleSystem>()),
          Component.Animator => new Companion.Animator(holder_, GetComponent<Animator>()),
          Component.PlayableDirector => new Companion.PlayableDirector(holder_, GetComponent<PlayableDirector>()),
          _ => throw new InvalidEnumArgumentException($"Unknown target: {target}"),
        };
      }
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
          var visible = false;
          foreach (var r in renderers_) {
            visible |= r.isVisible;
          }
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

      if (ticked) {
        if (holder_.Leaped) {
          foreach (var companion in companions_) {
            companion.OnLeap();
          }
        }
        foreach (var companion in companions_) {
          companion.OnTick();
        }
      }else if (backed) {
        foreach (var companion in companions_) {
          companion.OnBack();
        }
      }
    }
  }
}
