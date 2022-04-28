using UnityEngine;
using ParticleSystem = Reversible.Companion.ParticleSystem;

namespace Reversible.Unity.Specialized {
  public class MakeParticleReversible : SpecializedReversibleComponentBehaviour {
    private UnityEngine.ParticleSystem particle_;
    private ParticleSystem companion_;
    protected override void OnStart() {
      particle_ = GetComponent<UnityEngine.ParticleSystem>();
      var main = particle_.main;
      main.stopAction = ParticleSystemStopAction.Callback;
      companion_ = new ParticleSystem(clockHolder, particle_);
    }

    protected override void OnUpdate() {
      UpdateCompanion(ref companion_);
    }

    private void OnParticleSystemStopped() {
      graveyard.Destroy(gameObject);
    }
  }
}
