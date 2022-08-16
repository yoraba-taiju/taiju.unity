using UnityEngine;

namespace Reversible.Unity.Specialized {
  public sealed class MakeTransformReversible : SpecializedReversibleComponentBehaviour {
    private Companion.Transform companion_;

    protected override void OnStart() {
      companion_ = new Companion.Transform(player, transform);
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