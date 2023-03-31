using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Background.Stage01 {
  public class ForestScroll : ReversibleBehaviour {
    private struct Params {
      public static readonly int Offset = Shader.PropertyToID("_Offset");
    }
    [SerializeField] private float speed = 2.0f;
    private Renderer renderer_;
    private Material material_;

    protected override void OnStart() {
      renderer_ = GetComponent<Renderer>();
      material_ = renderer_.material;
    }

    protected override void OnForward() {
      material_.SetFloat(Params.Offset, IntegrationTime * speed);
    }

    protected override void OnReverse() {
      material_.SetFloat(Params.Offset, IntegrationTime * speed);
    }
  }
}
