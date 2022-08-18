using Reversible.Unity.Companion;

namespace Reversible.Unity.Specialized {
  public sealed class MakeTransformReversible : SpecializedReversibleComponentBehaviour {
    private Transform companion_;

    protected override void OnStart() {
      companion_ = new Transform(player, transform);
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