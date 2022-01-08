namespace Donut.Values {
  public struct Dense<T> {
    private Clock clock_;
    private T[] entries_;
    private uint lastTouched_;

    public Dense(Clock clock) {
      clock_ = clock;
      entries_ = new T[Clock.HISTORY_LENGTH];
      lastTouched_ = clock.CurrentLeaps;
    }

    public ref T Value {
      get {
        var currentTick = clock_.CurrentTicks;
        if (clock_.CurrentLeaps != lastTouched_) {
          var branch = clock_.BranchTick(lastTouched_);
          var v = entries_[branch % Clock.HISTORY_LENGTH];
          for (var i = branch + 1; i <= currentTick; i++) {
            entries_[i % Clock.HISTORY_LENGTH] = v;
          }
          lastTouched_ = clock_.CurrentLeaps;
        }
        return ref entries_[currentTick % Clock.HISTORY_LENGTH];
      }
    }
  }
}
