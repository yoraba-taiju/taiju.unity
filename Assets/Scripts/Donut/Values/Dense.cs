﻿using System;

namespace Donut.Values {
  public struct Dense<T> where T: struct {
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
      entries_[0] = initial;
      historyBegin_ = clock.CurrentTick;
      lastTouchedLeap_ = clock.CurrentLeap;
      lastTouchedTick_ = clock.CurrentTick;
      cloner_ = clonerImpl;
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
