using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Background.Stage01 {
  public class ForestScroll : ReversibleBehaviour {
    private struct Params {
      public static readonly int Offset = Shader.PropertyToID("_Offset");
    }
    [SerializeField] private float speed = 0.5f;
    private Dense<float> offset_;
    private Renderer renderer_;
    private Material material_;

    protected override void OnStart() {
      renderer_ = GetComponent<Renderer>();
      material_ = renderer_.material;
      offset_ = new Dense<float>(clock, 0.0f);
    }

    protected override void OnForward() {
      ref var offset = ref offset_.Mut;
      offset += (Time.deltaTime * speed);
      material_.SetFloat(Params.Offset, offset);
    }

    protected override void OnReverse() {
      material_.SetFloat(Params.Offset, offset_.Ref);
    }
  }
}