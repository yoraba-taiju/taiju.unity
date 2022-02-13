using System;
using UnityEngine;

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
    public uint CurrentLeap { get; private set; } = 0;
    public uint CurrentTick { get; private set; } = 0;

    /* History management */
    private readonly uint[] historyBranches_ = new uint[HISTORY_LENGTH];
    private uint HistoryBegin { get; set; }
    private uint HistoryEnd => CurrentTick;

    public Clock() {
      historyBranches_[0] = uint.MaxValue;
    }

    public void Tick() {
      if (state_ != State.Ticking) {
        DecideToLeap();
        return;
      }
      CurrentTick++;
      HistoryBegin = Math.Max(HistoryBegin, (CurrentTick >= HISTORY_LENGTH) ? (CurrentTick - HISTORY_LENGTH + 1) : 0);
    }

    public void Back() {
      if (state_ == State.Ticking) {
        state_ = State.Leaping;
      }
      if (CurrentTick > HistoryBegin) {
        CurrentTick--;
      }
    }

    private void DecideToLeap() {
      if (state_ != State.Leaping) {
        throw new InvalidOperationException("Clock can't leap before using magick.");
      }

      for (var i = (CurrentLeap >= HISTORY_LENGTH) ? (CurrentLeap - HISTORY_LENGTH) : 0; i <= CurrentLeap; ++i) {
        var idx = i % HISTORY_LENGTH;
        historyBranches_[idx] = Math.Min(historyBranches_[idx], CurrentTick);
      }

      CurrentLeap++;
      historyBranches_[CurrentLeap % HISTORY_LENGTH] = uint.MaxValue;
      state_ = State.Ticking;
      // {
      //   var branches = "";
      //   for (var i = (CurrentLeap >= HISTORY_LENGTH) ? (CurrentLeap - HISTORY_LENGTH) : 0; i <= CurrentLeap; ++i) {
      //     branches += $"(i={historyBranches_[i % HISTORY_LENGTH]}), ";
      //   }
      //   UnityEngine.Debug.Log($"Leaping {CurrentLeap} at {CurrentTick}. branches: [{branches}]");
      // }
    }

    public uint AdjustTick(uint lastTouchLeap, uint time) {
      return Math.Min(historyBranches_[lastTouchLeap % HISTORY_LENGTH], time);
    }
    public uint BranchTick(uint leap) {
      return historyBranches_[leap % HISTORY_LENGTH];
    }
  }
}
