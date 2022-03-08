using System;

namespace Donut.Values {
  public struct Dense<T> : IValue<T> where T: struct {
    private readonly Clock clock_;
    private readonly T[] entries_;
    private uint historyBegin_;
    private uint lastTouchedLeap_;
    private uint lastTouchedTick_;
    public delegate void Cloner(ref T dst, in T src);
    private readonly Cloner cloner_;

    public Dense(Clock clock, T initial): this(clock, null, initial)
    {
    }

    public Dense(Clock clock, Cloner clonerImpl, T initial) {
      clock_ = clock;
      entries_ = new T[Clock.HISTORY_LENGTH];
      historyBegin_ = clock.CurrentTick;
      entries_[historyBegin_ % Clock.HISTORY_LENGTH] = initial;
      lastTouchedLeap_ = clock.CurrentLeap;
      lastTouchedTick_ = clock.CurrentTick;
      cloner_ = clonerImpl;
    }
    
    public void Debug() {
      var vs = "[";
      for (var i = historyBegin_; i <= lastTouchedTick_; i++) {
        vs += $"[{i}]{entries_[i % Clock.HISTORY_LENGTH]}, ";
      }
      vs += "]";
      UnityEngine.Debug.Log($"Beg: {historyBegin_}, last: ({lastTouchedLeap_}, {lastTouchedTick_}), rec: {vs}");
    }

    public ref readonly T Ref {
      get {
        var currentTick = clock_.CurrentTick;
        if (currentTick < historyBegin_) {
          throw new InvalidOperationException("Can't access before value born.");
        }

        if (clock_.CurrentLeap != lastTouchedLeap_) {
          var branch = clock_.BranchTick(lastTouchedLeap_);
          lastTouchedTick_ = Math.Min(currentTick, branch);
          lastTouchedLeap_ = clock_.CurrentLeap;
          return ref entries_[lastTouchedTick_ % Clock.HISTORY_LENGTH];
        }

        if (currentTick != lastTouchedTick_) {
          lastTouchedTick_ = Math.Min(currentTick, lastTouchedTick_);
        }
        return ref entries_[lastTouchedTick_ % Clock.HISTORY_LENGTH];
      }
    }

    public ref T Mut {
      get {
        var currentTick = clock_.CurrentTick;
        if (currentTick < historyBegin_) {
          throw new InvalidOperationException("Can't access before value born.");
        }
        if (clock_.CurrentLeap != lastTouchedLeap_) {
          var branch = clock_.BranchTick(lastTouchedLeap_);
          var v = entries_[branch % Clock.HISTORY_LENGTH];
          for (var i = branch + 1; i <= currentTick; i++) {
            if (cloner_ == null) {
              entries_[i % Clock.HISTORY_LENGTH] = v;
            } else {
              cloner_(ref entries_[i % Clock.HISTORY_LENGTH], in v);
            }
          }
          lastTouchedLeap_ = clock_.CurrentLeap;
          lastTouchedTick_ = currentTick;
          historyBegin_ = Math.Max(historyBegin_, (currentTick >= Clock.HISTORY_LENGTH) ? currentTick - Clock.HISTORY_LENGTH + 1 : 0);
        } else if (lastTouchedTick_ != currentTick) {
          var v = entries_[lastTouchedTick_ % Clock.HISTORY_LENGTH];
          for (var i = lastTouchedTick_; i <= currentTick; i++) {
            if (cloner_ == null) {
              entries_[i % Clock.HISTORY_LENGTH] = v;
            } else {
              cloner_(ref entries_[i % Clock.HISTORY_LENGTH], in v);
            }
          }
          lastTouchedTick_ = currentTick;
          historyBegin_ = Math.Max(historyBegin_, (currentTick >= Clock.HISTORY_LENGTH) ? currentTick - Clock.HISTORY_LENGTH + 1 : 0);
        }
        return ref entries_[currentTick % Clock.HISTORY_LENGTH];
      }
    }
  }
}
