﻿namespace Donut.Values {
  public interface IValue<T> where T: struct {
    public ref readonly T Ref {
      get;
    }

    public ref T Mut {
      get;
    }
  }
}