namespace Donut {
  public struct History<T> {
    private const int HistoryLength = 600;
    private Clock clock_;
    private int lastRead_;
    private readonly T[] values_;

    public History(Clock clock) {
      clock_ = clock;
      this.lastRead_ = 0;
      values_ = new T[HistoryLength];
    }

    public ref T Value {
      get {
        var index = 0;
        return ref values_[index];
      }
    }
  }
}
