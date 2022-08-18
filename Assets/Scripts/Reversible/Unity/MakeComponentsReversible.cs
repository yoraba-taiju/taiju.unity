using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using Transform = Reversible.Unity.Companion.Transform;

namespace Reversible.Unity {
  public sealed class MakeComponentsReversible : ReversibleBase {
    // Visibility
    [SerializeField] public bool deactivateWhenInvisible = true;
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

    private new void Start() {
      base.Start();
      if (deactivateWhenInvisible) {
        var renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0) {
          renderers_ = renderers;
          wasVisible_ = false;
        } else {
          Debug.LogWarning("No renderers attached.");
          deactivateWhenInvisible = false;
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
          Component.Transform => new Transform(player, transform),
          Component.Rigidbody2D => new Companion.Rigidbody2D(player, GetComponent<Rigidbody2D>()),
          Component.ParticleSystem => new Companion.ParticleSystem(player, GetComponent<ParticleSystem>()),
          Component.Animator => new Companion.Animator(player, GetComponent<Animator>()),
          Component.PlayableDirector => new Companion.PlayableDirector(player, GetComponent<PlayableDirector>()),
          _ => throw new InvalidEnumArgumentException($"Unknown target: {target}"),
        };
        ++i;
      }
    }

    private new void Update() {
      base.Update();

      var ticked = player.Ticked;
      var backed = player.Backed;
      var leaped = player.Leaped;
      if (deactivateWhenInvisible) {
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
              Deactivate();
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

    public override void OnDeactivated() {
    }

    public override void OnReactivated() {
    }
  }
}