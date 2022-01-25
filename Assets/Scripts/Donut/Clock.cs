using System;

namespace Donut {
  public class Clock {
    public const uint HISTORY_LENGTH = 512;

    /* State management */
    private enum State {
      Ticking,
      Leaping,
    }

    private State state_ = State.Ticking;
    
    public bool IsTicking => state_ == State.Ticking;
    public bool IsLeaping => state_ == State.Leaping;

    /* current leaps */
    private uint currentLeap_ = 0;
    private uint currentTick_ = 0;

    /* History management */
    private readonly uint[] historyBranches_ = new uint[HISTORY_LENGTH];
    private uint historyBegin_ = 0;
    private uint historyEnd_ = 0;

    public Clock() {
      historyBranches_[0] = 0;
    }

    public void Tick() {
      if (state_ != State.Ticking) {
        DecideToLeap();
        return;
      }
      currentTick_++;
      historyBegin_ = Math.Max(historyBegin_, (currentTick_ >= HISTORY_LENGTH) ? (currentTick_ - HISTORY_LENGTH + 1) : 0);
      historyEnd_ = currentTick_;
    }

    public void Back() {
      if (state_ == State.Ticking) {
        state_ = State.Leaping;
      }
      if (currentTick_ > historyBegin_) {
        currentTick_--;
        historyEnd_ = currentTick_;
      }
    }

    private void DecideToLeap() {
      if (state_ != State.Leaping) {
        throw new InvalidOperationException("Clock can't leap before using magick.");
      }

      for (var i = (currentLeap_ >= HISTORY_LENGTH) ? (currentLeap_ - HISTORY_LENGTH) : 0; i <= currentLeap_; ++i) {
        var idx = i % HISTORY_LENGTH;
        historyBranches_[idx] = Math.Min(historyBranches_[idx], currentTick_);
      }

      currentLeap_++;
      historyBranches_[currentLeap_ % HISTORY_LENGTH] = uint.MaxValue;
      historyEnd_ = currentTick_;
      state_ = State.Ticking;
    }

    public uint AdjustTick(uint lastTouchLeap, uint time) {
      return Math.Min(historyBranches_[lastTouchLeap % HISTORY_LENGTH], time);
    }
    public uint BranchTick(uint leap) {
      return historyBranches_[leap % HISTORY_LENGTH];
    }
    
    public uint CurrentTick => currentTick_;
    public uint CurrentLeap => currentLeap_;

    public uint HistoryBegin => historyBegin_;
    public uint HistoryEnd => historyEnd_;
  }
}
