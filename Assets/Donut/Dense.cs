namespace Donut {
  public struct Dense<T> {
    private const int HISTORY_LENGTH = 600;

    private Clock clock_;
    private readonly T[] values_;
    private readonly uint[] leaps_;

    public Dense(Clock clock) {
      clock_ = clock;
      values_ = new T[HISTORY_LENGTH];
      leaps_ = new uint[HISTORY_LENGTH];
    }

    public ref T Value {
      get {
        var index = 0;
        return ref values_[index];
      }
    }
  }
}
