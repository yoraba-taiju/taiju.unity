using PlayableDirector = UnityEngine.Playables.PlayableDirector;

namespace Reversible.Unity.Specialized {
  public sealed class MakePlayableDirectorReversible : SpecializedReversibleComponentBehaviour {
    private PlayableDirector playableDirector_;
    private Companion.PlayableDirector companion_;

    protected override void OnStart() {
      playableDirector_ = GetComponent<PlayableDirector>();
      companion_ = new Companion.PlayableDirector(player, playableDirector_);
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
    
  }
}