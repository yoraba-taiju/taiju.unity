using Donut.Unity;
using Donut.Values;
using UnityEngine;

namespace Background.Stage01 {
  public class ForestScroll : DonutBehaviour {
    private Dense<Vector2> offset_;
    private Renderer renderer_;
    protected override void OnStart() {
      renderer_ = GetComponent<Renderer>();
      offset_ = new Dense<Vector2>(clock, renderer_.material.mainTextureOffset);
    }

    protected override void OnUpdate() {
      ref var offset = ref offset_.Mut;
      offset -= Vector2.one * Time.deltaTime * 0.1f;
      renderer_.material.mainTextureOffset = offset;
    }

    protected override void OnReverse() {
      renderer_.material.mainTextureOffset = offset_.Ref;
    }
  }
}