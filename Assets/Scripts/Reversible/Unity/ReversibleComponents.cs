using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using Transform = Reversible.Unity.Companion.Transform;

namespace Reversible.Unity {
  public sealed class ReversibleComponents : MonoBehaviour {
    // Clock
    private Player player_;
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

    [SerializeField] public Component[] targetComponents = {
       Component.Transform,
    };
    private ICompanion[] companions_;

    private void Start() {
      var backstage = GameObject.FindGameObjectWithTag("Backstage");
      player_ = backstage.GetComponent<Player>();
      clock_ = player_.Clock;
      bornAt_ = clock_.CurrentTick;
      if (destroyWhenInvisible) {
        var renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0) {
          renderers_ = renderers;
          world_ = backstage.GetComponent<World>();
          wasVisible_ = false;
        } else {
          Debug.LogWarning("No renderers attached.");
          destroyWhenInvisible = false;
        }
      }

      if (targetComponents.Length <= 0) {
        return;
      }

      companions_ = new ICompanion[targetComponents.Length];
      var i = 0;
      foreach (var target in targetComponents) {
        companions_[i] = target switch {
          Component.None => throw new InvalidEnumArgumentException("Please set some target"),
          Component.Transform => new Transform(player_, transform),
          Component.Rigidbody2D => new Companion.Rigidbody2D(player_, GetComponent<Rigidbody2D>()),
          Component.ParticleSystem => new Companion.ParticleSystem(player_, GetComponent<ParticleSystem>()),
          Component.Animator => new Companion.Animator(player_, GetComponent<Animator>()),
          Component.PlayableDirector => new Companion.PlayableDirector(player_, GetComponent<PlayableDirector>()),
          _ => throw new InvalidEnumArgumentException($"Unknown target: {target}"),
        };
        ++i;
      }
    }

    private void Update() {
      if (bornAt_ > clock_.CurrentTick) {
        Destroy(gameObject);
        return;
      }

      var ticked = player_.Ticked;
      var backed = player_.Backed;
      var leaped = player_.Leaped;
      if (destroyWhenInvisible) {
        if (ticked) {
          var visible = false;
          foreach (var r in renderers_) {
            visible |= r.isVisible;
            if (visible) {
              break;
            }
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