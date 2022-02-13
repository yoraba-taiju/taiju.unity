using System;

namespace Donut.Values {
  public struct Dense<T> {
    private readonly Clock clock_;
    private readonly T[] entries_;
    private uint historyBegin_;
    private uint lastTouchedLeap_;
    private uint lastTouchedTick_;

    public Dense(Clock clock, T initial) {
      clock_ = clock;
      entries_ = new T[Clock.HISTORY_LENGTH];
      entries_[0] = initial;
      historyBegin_ = clock.CurrentTick;
      lastTouchedLeap_ = clock.CurrentLeap;
      lastTouchedTick_ = clock.CurrentTick;
    }

    public ref T Value {
      get {
        var currentTick = clock_.CurrentTick;
        if (currentTick < historyBegin_) {
          throw new InvalidOperationException("Can't access before value born.");
        }
        if (clock_.CurrentLeap != lastTouchedLeap_) {
          var branch = clock_.BranchTick(lastTouchedLeap_);
          var v = entries_[branch % Clock.HISTORY_LENGTH];
          for (var i = branch + 1; i <= currentTick; i++) {
            entries_[i % Clock.HISTORY_LENGTH] = v;
          }
          lastTouchedLeap_ = clock_.CurrentLeap;
          lastTouchedTick_ = currentTick;
          historyBegin_ = Math.Max(historyBegin_, (currentTick >= Clock.HISTORY_LENGTH) ? currentTick - Clock.HISTORY_LENGTH + 1 : 0);
        } else if (lastTouchedTick_ != currentTick) {
          var v = entries_[lastTouchedTick_ % Clock.HISTORY_LENGTH];
          for (var i = lastTouchedTick_; i <= currentTick; i++) {
            entries_[i % Clock.HISTORY_LENGTH] = v;
          }
          lastTouchedTick_ = currentTick;
          historyBegin_ = Math.Max(historyBegin_, (currentTick >= Clock.HISTORY_LENGTH) ? currentTick - Clock.HISTORY_LENGTH + 1 : 0);
        }
        return ref entries_[currentTick % Clock.HISTORY_LENGTH];
      }
    }
    public void Debug() {
      var vs = "[";
      for (var i = historyBegin_; i <= lastTouchedTick_; i++) {
        vs += $"[{i}]{entries_[i % Clock.HISTORY_LENGTH]}, ";
      }
      vs += "]";
      UnityEngine.Debug.Log($"Beg: {historyBegin_}, last: ({lastTouchedLeap_}, {lastTouchedTick_}), rec: {vs}");
    }
  }
}
