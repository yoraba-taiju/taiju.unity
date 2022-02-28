using System;

namespace Donut.Values {
  public struct Sparse<T> where T: struct {
    private struct Entry {
      public uint tick;
      public T value;
    }
    private readonly Clock clock_;
    private uint lastTouchedLeap_;
    private uint lastTouchedTick_;

    private readonly Entry[] entries_;
    private uint entriesBeg_;
    private uint entriesLen_;

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
      entriesLen_ = 1;
    }
    
    private uint LowerBound(uint beg, uint end, uint tick) {
      while (beg < end) {
        var midIdx = beg + (end - beg) / 2;
        var midTick = entries_[midIdx % Clock.HISTORY_LENGTH].tick;
        if (tick <= midTick) {
          end = midIdx;
        } else {
          beg = midIdx + 1;
        }
      }
      return beg;
    }

    public ref T Value {
      get {
        var currentTick = clock_.CurrentTick;
        var currentLeap = clock_.CurrentLeap;
        if (currentLeap == lastTouchedLeap_ && currentTick == lastTouchedTick_) {
          return ref entries_[(entriesBeg_ + entriesLen_ - 1) % Clock.HISTORY_LENGTH].value;
        }
        var rawIdx = LowerBound(
          entriesBeg_,
          entriesBeg_ + entriesLen_,
          currentTick
        );
        var oldEntriesLen = entriesLen_;
        var idx = rawIdx % Clock.HISTORY_LENGTH;
        if (idx == entriesBeg_) {
          if (entriesLen_ == Clock.HISTORY_LENGTH) {
            entriesBeg_ = (entriesBeg_ + 1) % Clock.HISTORY_LENGTH;
          } else {
            if (currentTick < entries_[idx].tick) {
              throw new InvalidOperationException("Can't access before value born.");
            }
            entriesLen_ = (rawIdx - entriesBeg_) + 1;
          }
        } else {
          entriesLen_ = (rawIdx - entriesBeg_) + 1;
        }
        entries_[idx].tick = currentTick;
        if (oldEntriesLen < entriesLen_) {
          entries_[idx].value = entries_[(idx + Clock.HISTORY_LENGTH - 1) % Clock.HISTORY_LENGTH].value;
        }
        lastTouchedLeap_ = currentLeap;
        lastTouchedTick_ = currentTick;
        return ref entries_[idx].value;
      }
    }
  }
} 
