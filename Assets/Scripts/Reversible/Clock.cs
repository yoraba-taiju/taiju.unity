using System;
using UnityEngine;

namespace Reversible {
  public class Clock {
    public const uint HISTORY_LENGTH = 512;

    /* current leaps */
    public uint CurrentLeap { get; private set; }
    public uint CurrentTick { get; private set; }

    /* History management */
    private readonly uint[] historyBranches_ = new uint[HISTORY_LENGTH];
    private uint HistoryBegin { get; set; }
    private uint HistoryEnd => CurrentTick;

    public Clock() {
      historyBranches_[0] = uint.MaxValue;
    }

    public void Tick() {
      CurrentTick++;
      HistoryBegin = Math.Max(HistoryBegin, (CurrentTick >= HISTORY_LENGTH) ? (CurrentTick - HISTORY_LENGTH + 1) : 0);
    }

    public void Back() {
      if (CurrentTick > HistoryBegin) {
        CurrentTick--;
      }
    }

    public void DecideToLeap() {
      for (var i = (CurrentLeap >= HISTORY_LENGTH) ? (CurrentLeap - HISTORY_LENGTH) : 0; i <= CurrentLeap; ++i) {
        var idx = i % HISTORY_LENGTH;
        historyBranches_[idx] = Math.Min(historyBranches_[idx], CurrentTick);
      }

      CurrentLeap++;
      historyBranches_[CurrentLeap % HISTORY_LENGTH] = uint.MaxValue;
      // DebugBranch();
    }

    private void DebugBranch() {
      var branches = "";
      for (var i = (CurrentLeap >= HISTORY_LENGTH) ? (CurrentLeap - HISTORY_LENGTH) : 0; i <= CurrentLeap; ++i) {
        branches += $"(i={historyBranches_[i % HISTORY_LENGTH]}), ";
      }
      Debug.Log($"Leaping {CurrentLeap} at {CurrentTick}. branches: [{branches}]");
    }

    public uint AdjustTick(uint lastTouchLeap, uint tick) {
      return Math.Min(historyBranches_[lastTouchLeap % HISTORY_LENGTH], tick);
    }
    public uint BranchTickOfLeap(uint leap) {
      return historyBranches_[leap % HISTORY_LENGTH];
    }
  }
}
