namespace Donut.Values {
  public struct Dense<T> {
    private readonly Clock clock_;
    private readonly T[] entries_;
    private uint lastTouchedLeap_;
    private uint lastTouchedTick_;

    public Dense(Clock clock) {
      clock_ = clock;
      entries_ = new T[Clock.HISTORY_LENGTH];
      lastTouchedLeap_ = clock.CurrentLeap;
      lastTouchedTick_ = clock.CurrentTick;
    }

    public ref T Value {
      get {
        var currentTick = clock_.CurrentTick;
        if (clock_.CurrentLeap != lastTouchedLeap_) {
          var branch = clock_.BranchTick(lastTouchedLeap_);
          var v = entries_[branch % Clock.HISTORY_LENGTH];
          for (var i = branch + 1; i <= currentTick; i++) {
            entries_[i % Clock.HISTORY_LENGTH] = v;
          }
          lastTouchedLeap_ = clock_.CurrentLeap;
          lastTouchedTick_ = currentTick;
        } else if (lastTouchedTick_ != currentTick) {
          var v = entries_[lastTouchedTick_ % Clock.HISTORY_LENGTH];
          for (var i = lastTouchedTick_; i <= currentTick; i++) {
            entries_[i % Clock.HISTORY_LENGTH] = v;
          }
          lastTouchedTick_ = currentTick;
        }
        return ref entries_[currentTick % Clock.HISTORY_LENGTH];
      }
    }
  }
}
