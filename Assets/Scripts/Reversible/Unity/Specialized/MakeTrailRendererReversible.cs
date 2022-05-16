
namespace Reversible.Unity.Specialized {
  public class MakeTrailRendererReversible : SpecializedReversibleComponentBehaviour {
    private UnityEngine.TrailRenderer trailRenderer_;
    private Companion.TrailRenderer companion_;
    protected override void OnStart() {
      trailRenderer_ = GetComponent<UnityEngine.TrailRenderer>();
      companion_ = new Companion.TrailRenderer(clockHolder, trailRenderer_);
    }

    protected override void OnUpdate() {
      UpdateCompanion(ref companion_);
    }

    private void OnParticleSystemStopped() {
      graveyard.Destroy(gameObject);
    }
  }
}
