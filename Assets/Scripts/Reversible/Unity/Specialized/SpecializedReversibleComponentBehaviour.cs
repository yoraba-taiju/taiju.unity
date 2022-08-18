namespace Reversible.Unity.Specialized {
  public abstract class SpecializedReversibleComponentBehaviour : ReversibleBase {
    public new void Start() {
      base.Start();
      OnStart();
    }
    protected override void OnUpdate() {
      if (player.Ticked) {
        OnTick();
      } else if (player.Backed) {
        OnBack();
      } else if (player.Leaped) {
        OnLeap();
      }
    }

    protected abstract void OnStart();

    protected abstract void OnTick();

    protected abstract void OnBack();

    protected abstract void OnLeap();
    
    public override void OnDeactivated() {
    }

    public override void OnReactivated() {
    }
  }
}