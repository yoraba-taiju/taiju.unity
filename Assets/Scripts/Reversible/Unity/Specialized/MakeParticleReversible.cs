
using UnityEngine;

namespace Reversible.Unity.Specialized {
  public sealed class MakeParticleReversible : SpecializedReversibleComponentBehaviour {
    private ParticleSystem particle_;
    private Companion.ParticleSystem companion_;
    protected override void OnStart() {
      particle_ = GetComponent<ParticleSystem>();
      var main = particle_.main;
      main.stopAction = ParticleSystemStopAction.Callback;
      companion_ = new Companion.ParticleSystem(clockHolder, particle_);
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
