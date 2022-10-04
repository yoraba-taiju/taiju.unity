using Reversible.Unity;
using UnityEngine;

namespace Background {
  public class CloudLayer : ReversibleBase {
    private Renderer renderer_;
    private Material material_;

    private struct ShaderKey {
      public static readonly int CurrentTime = Shader.PropertyToID("_CurrentTime");
    }

    protected new void Start() {
      base.Start();
      renderer_ = GetComponent<MeshRenderer>();
      material_ = renderer_.material;
    }

    protected override void OnUpdate() {
      material_.SetFloat(ShaderKey.CurrentTime, IntegrationTime);
    }

    public override void OnDeactivated() {
    }

    public override void OnReactivated() {
    }
  }
}