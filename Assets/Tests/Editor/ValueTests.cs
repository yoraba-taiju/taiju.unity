using System;
using NUnit.Framework;
using Reversible;
using Reversible.Value;

namespace Tests.Editor {
  public abstract class ValueTest<T>
    where T: IValue<int> {
    protected abstract T Create(Clock clock, int initial);
    [Test]
    public void BasicTest() {
      var clock = new Clock();
      var v = Create(clock, 0);

      Assert.AreEqual(0, v.Ref);
      v.Mut = 1;
      Assert.AreEqual(1, v.Ref);
      clock.Tick();
      Assert.AreEqual(1, v.Ref);
      v.Mut = 2;
      Assert.AreEqual(2, v.Ref);
      clock.Back();
      Assert.AreEqual(1, v.Ref);
      clock.Leap();
      clock.Tick();
      Assert.AreEqual(1, v.Ref);
      v.Mut = 3;
      Assert.AreEqual(3, v.Ref);
    }

    [Test]
    public void CantBeAccessedBefore() {
      var clock = new Clock();
      clock.Tick();
      var v = Create(clock, 0);
      clock.Back();
      clock.Leap();
      Assert.Throws<InvalidOperationException>(() => {
        v.Mut = 10;
      });
    }

    [Test]
    public void LongTest() {
      var clock = new Clock();
      clock.Tick();
      clock.Tick();
      clock.Tick();
      var v = Create(clock, 0);
      for (var i = 0; i < Clock.HISTORY_LENGTH * 2; ++i) {
        clock.Tick();
        v.Mut = i;
      }
      var backCount = 0;
      for (var i = Clock.HISTORY_LENGTH * 2 - 1; i >= Clock.HISTORY_LENGTH; --i) {
        Assert.AreEqual(i, v.Ref);
        Assert.AreEqual(clock.CurrentTick, v.Ref + 4);
        clock.Back();
        backCount++;
      }
      Assert.AreEqual(Clock.HISTORY_LENGTH, backCount);
    }
    [Test]
    public void InvalidOperation() {
      var clock = new Clock();
      clock.Tick();
      var w = Create(clock, 1);
      clock.Back();
      Assert.Throws<InvalidOperationException>(() => {
        var unused = w.Ref;
      });
    }

    [Test]
    public void BackAndRef() {
      var clock = new Clock();
      var v = Create(clock, 0);
      // tick = 0
      clock.Tick();
      // tick = 1
      v.Mut = 1;
      clock.Tick();
      // tick = 2
      clock.Tick();
      // tick = 3
      v.Mut = 3;
      clock.Back();
      // tick = 2
      clock.Leap();
      Assert.AreEqual(1, v.Ref);
    }
  }

  public class DenseValueTest : ValueTest<Dense<int>> {
    protected override Dense<int> Create(Clock clock, int initial) {
      return new Dense<int>(clock, initial);
    }
  }
  public class SparseValueTest : ValueTest<Sparse<int>> {
    protected override Sparse<int> Create(Clock clock, int initial) {
      return new Sparse<int>(clock, initial);
    }
  }
}
