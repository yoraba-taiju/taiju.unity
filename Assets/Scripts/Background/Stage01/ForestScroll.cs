using Donut.Unity;
using UnityEngine;

namespace Background.Stage01 {
  public class ForestScroll : DonutBehaviour {
    private Renderer renderer_;
    protected override void OnStart() {
      renderer_ = GetComponent<Renderer>();
    }

    protected override void OnUpdate() {
      renderer_.material.mainTextureOffset -= Vector2.one * Time.deltaTime * 0.1f;
    }
  }
}