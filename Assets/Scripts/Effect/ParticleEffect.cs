using UnityEngine;
using ParticleSystem = Reversible.Companion.ParticleSystem;

namespace Effect {
  public class ParticleEffect : EffectBehaviour {
    private UnityEngine.ParticleSystem particle_;
    private ParticleSystem companion_;
    protected override void OnStart() {
      particle_ = GetComponent<UnityEngine.ParticleSystem>();
      var main = particle_.main;
      main.stopAction = ParticleSystemStopAction.Callback;
      companion_ = new ParticleSystem(clockHolder, particle_);
    }

    protected override void OnUpdate() {
      if (clockHolder.Ticked) {
        if (clockHolder.Leaped) {
          companion_.OnLeap();
        }
        companion_.OnTick();
      } else if (clockHolder.Backed) {
        companion_.OnBack();
      }
    }

    private void OnParticleSystemStopped() {
      graveyard.Destroy(gameObject);
    }
  }
}