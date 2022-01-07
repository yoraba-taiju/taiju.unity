namespace Donut.Values {
  public struct Dense<T> {
    private struct Entry {
      public uint leap;
      public T value;
    }

    private Clock clock_;
    private readonly Entry[] entries_;

    public Dense(Clock clock) {
      clock_ = clock;
      entries_ = new Entry[Clock.HISTORY_LENGTH];
    }

    public ref T Value {
      get {
        var index = 0;
        return ref entries_[index].value;
      }
    }
  }
}
