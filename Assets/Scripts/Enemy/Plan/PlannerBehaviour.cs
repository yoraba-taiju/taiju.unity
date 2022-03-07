using Donut;
using Donut.Unity;
using Donut.Values;

namespace Enemy.Plan {
  public abstract class PlannerBehaviour<T>: DonutBehaviour where T: struct {
    private Dense<T> current_;
    
    protected override void OnStart() {
      OnStart(out var first);
      current_ = new Dense<T>(clock, first);
    }

    protected override void OnUpdate() {
      if (clockHolder.Ticked) {
        OnDispatch(ref current_.Mut);
      }
    }

    protected abstract void OnStart(out T first);
    protected abstract void OnDispatch(ref T self);
  }
}