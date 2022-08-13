using System;

namespace Utility {
  public struct RingBuffer<T> {
    private T[] buff_;
    private int beg_;
    private int mask_;
    public int Count { get; private set; }
    public int Capacity { get; }
    public readonly bool IsEmpty => Count == 0;
    public readonly bool IsFull => Count >= Capacity;

    public RingBuffer(int capacity) {
      capacity = Pow2(capacity);
      buff_ = new T[capacity];
      beg_ = 0;
      Count = 0;
      Capacity = capacity;
      mask_ = capacity - 1;
    }

    private static int Pow2(int v) {
      var r = 1;
      while (r < v) {
        r <<= 1;
      }
      return r;
    }
    
    public void Add(T item) {
      if (IsFull) {
        throw new InvalidOperationException("Full");
      }
      buff_[(beg_ + Count) & mask_] = item;
      Count++;
    }
    public T RemoveFirst() {
      var v = First;
      beg_ = (beg_ + 1) & mask_;
      Count--;
      return v;
    }
    public T RemoveLast() {
      var v = Last;
      Count--;
      return v;
    }
    
    public readonly ref T Last {
      get {
        if (IsEmpty) {
          throw new InvalidOperationException("Empty");
        }
        return ref buff_[(beg_ + Count - 1) & mask_];
      }
    }
    public readonly ref T First {
      get {
        if (IsEmpty) {
          throw new InvalidOperationException("Empty");
        }
        return ref buff_[beg_];
      }
    }
  }
}