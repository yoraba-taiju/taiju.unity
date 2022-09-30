using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Background.Stage01 {
  public class ForestScroll : ReversibleBehaviour {
    private Dense<Vector2> offset_;
    private Renderer renderer_;

    protected override void OnStart() {
      renderer_ = GetComponent<Renderer>();
      offset_ = new Dense<Vector2>(clock, renderer_.material.mainTextureOffset);
    }

    protected override void OnForward() {
      ref var offset = ref offset_.Mut;
      offset += Vector2.left * (Time.deltaTime * 0.1f);
      renderer_.material.mainTextureOffset = offset;
    }

    protected override void OnReverse() {
      renderer_.material.mainTextureOffset = offset_.Ref;
    }
  }
}