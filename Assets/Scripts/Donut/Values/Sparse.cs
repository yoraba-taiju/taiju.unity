using System;

namespace Donut.Values {
  public struct Sparse<T> {
    private struct Entry {
      public uint tick;
      public T value;
    }
    private readonly Clock clock_;
    private uint lastTouchedLeap_;
    private uint lastTouchedTick_;

    private readonly Entry[] entries_;
    private uint entriesBeg_;
    private uint entriesEnd_;

    public Sparse(Clock clock, T initial) {
      clock_ = clock;
      entries_ = new Entry[Clock.HISTORY_LENGTH];
      entries_[0] = new Entry {
        tick = clock.CurrentTick,
        value = initial,
      };
      lastTouchedLeap_ = clock.CurrentLeap;
      lastTouchedTick_ = clock.CurrentTick;
      entriesBeg_ = 0;
      entriesEnd_ = 0;
    }

    private uint AllocateEntry(uint beg, uint end, uint currentTick) {
      if (beg == entriesEnd_) {
        var idx = (entriesEnd_ + 1) % Clock.HISTORY_LENGTH;
        if (idx == entriesBeg_) {
          entriesBeg_ = (entriesBeg_ + 1) % Clock.HISTORY_LENGTH;
        }
        entriesEnd_ = idx;
        entries_[idx].tick = currentTick;
        entries_[idx].value = entries_[(idx + Clock.HISTORY_LENGTH - 1) % Clock.HISTORY_LENGTH].value;
        return idx;
      }
      if (end == entriesBeg_) {
        throw new InvalidOperationException("Can't access before value born or forgotten.");
      }
      var mid = (beg + ((end - beg) / 2)) % Clock.HISTORY_LENGTH;
      var midTick = entries_[mid].tick;
      if (midTick < currentTick) {
        return AllocateEntry(beg + 1, end, currentTick);
      } else if (currentTick < midTick) {
        return AllocateEntry(beg, end - 1, currentTick);
      } else {
        return mid;
      }
    }

    public ref T Value {
      get {
        var currentTick = clock_.CurrentTick;
        var currentLeap = clock_.CurrentLeap;
        var lastTouchedTick = entries_[entriesEnd_].tick;
        if (currentLeap == lastTouchedLeap_ && currentTick == lastTouchedTick) {
          return ref entries_[entriesEnd_].value;
        }
        var idx = AllocateEntry(
          entriesBeg_,
          (entriesBeg_ <= entriesEnd_) ? entriesEnd_ : entriesEnd_ + Clock.HISTORY_LENGTH,
          currentTick
        );
        lastTouchedLeap_ = currentLeap;
        lastTouchedTick_ = currentTick;
        return ref entries_[idx].value;
      }
    }
  }
} 
