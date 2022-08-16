using UnityEngine;

namespace Reversible.Unity.Specialized {
  public sealed class MakeAnimatorReversible : SpecializedReversibleComponentBehaviour {
    private Animator animator_;
    private Companion.Animator companion_;

    protected override void OnStart() {
      animator_ = GetComponent<Animator>();
      companion_ = new Companion.Animator(player, animator_);
    }

    protected override void OnTick() {
      companion_.OnTick();
    }

    protected override void OnBack() {
      companion_.OnBack();
    }

    protected override void OnLeap() {
      companion_.OnLeap();
    }

    private void OnParticleSystemStopped() {
      world.Destroy(gameObject);
    }
  }
}