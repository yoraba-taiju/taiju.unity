namespace Donut.Values {
  public struct Sparse<T> {
    private struct Entry {
      public uint leap;
      public T value;
    }
    private Clock clock_;
    private readonly Entry[] entries_;

    public Sparse(Clock clock) {
      clock_ = clock;
      entries_ = new Entry[Clock.HISTORY_LENGTH];
    }
    
  }
} 
