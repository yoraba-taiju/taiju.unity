using System;
using Lib;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Donut {
  public class Clock {
    public const uint HISTORY_LENGTH = 512;

    /* State management */
    private enum State {
      Normal,
      Leaping,
    }

    private State state_ = State.Normal;

    /* current leaps */
    private uint currentLeaps_ = 0;
    private uint currentTicks_ = 0;

    /* History management */
    private readonly uint[] historyBranches_ = new uint[HISTORY_LENGTH];
    private uint historyBegin_ = 0;
    private uint historyEnd_ = 0;

    public Clock() {
      historyBranches_[0] = 0;
    }

    public void Tick() {
      if (state_ != State.Normal) {
        DecideToLeap();
        return;
      }
      currentTicks_++;
      historyBegin_ = (currentTicks_ >= HISTORY_LENGTH) ? (currentTicks_ - HISTORY_LENGTH) : 0;
      historyEnd_ = currentTicks_;
    }

    public void Back() {
      if (state_ == State.Normal) {
        state_ = State.Leaping;
      } else {
        currentTicks_--;
        historyEnd_ = currentTicks_;
      }
    }

    private void DecideToLeap() {
      if (state_ != State.Leaping) {
        throw new InvalidOperationException("Clock can't leap before using magick.");
      }

      for (var i = (currentLeaps_ >= HISTORY_LENGTH) ? (currentLeaps_ - HISTORY_LENGTH) : 0; i < currentLeaps_; ++i) {
        var idx = i % HISTORY_LENGTH;
        historyBranches_[idx] = Math.Min(historyBranches_[idx], currentTicks_);
      }

      currentLeaps_++;
      historyBranches_[currentLeaps_ % HISTORY_LENGTH] = uint.MaxValue;
      historyEnd_ = currentTicks_;
      state_ = State.Normal;
    }

    public uint AdjustTime(uint lastTouchLeap, uint time) {
      return Math.Min(historyBranches_[lastTouchLeap % HISTORY_LENGTH], time);
    }
    
    public uint CurrentTicks => currentTicks_;
    public uint CurrentLeaps => currentLeaps_;

    public uint HistoryBegin => historyBegin_;
    public uint HistoryEnd => historyEnd_;
  }
}
