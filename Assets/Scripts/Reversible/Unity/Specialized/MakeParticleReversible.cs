
using UnityEngine;

namespace Reversible.Unity.Specialized {
  public class MakeParticleReversible : SpecializedReversibleComponentBehaviour {
    private ParticleSystem particle_;
    private Companion.ParticleSystem companion_;
    protected override void OnStart() {
      particle_ = GetComponent<ParticleSystem>();
      var main = particle_.main;
      main.stopAction = ParticleSystemStopAction.Callback;
      companion_ = new Companion.ParticleSystem(clockHolder, particle_);
    }

    protected override void OnUpdate() {
      UpdateCompanion(ref companion_);
    }

    private void OnParticleSystemStopped() {
      graveyard.Destroy(gameObject);
    }
  }
}
