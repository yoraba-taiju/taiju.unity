namespace Reversible.Unity {
  public abstract class ReversibleBehaviour : ReversibleBase {
    protected new void Start() {
      base.Start();
      OnStart();
    }

    protected override void OnFixedUpdate() {
      if (player.IsForwarding) {
        OnFixedForward();
      }
    }
    protected override void OnUpdate() {
      if (player.Leaped) {
        OnLeap();
        return;
      }
      if (player.IsForwarding) {
        OnForward();
      } else {
        OnReverse();
      }
    }
    
    protected abstract void OnStart();

    protected virtual void OnFixedForward() {
      
    }
    protected abstract void OnForward();

    protected virtual void OnReverse() {
    }

    protected virtual void OnLeap() {
    }
    
    public override void OnDeactivated() {
    }

    public override void OnReactivated() {
    }
  }
}