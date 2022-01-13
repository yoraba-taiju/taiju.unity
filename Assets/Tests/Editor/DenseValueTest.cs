using System;
using NUnit.Framework;
using Donut;
using Donut.Values;

namespace Tests.Editor {
  public class DenseValueTest {
    [Test]
    public void BasicTest() {
      var clock = new Clock();
      var v = new Dense<int>(clock, 0);

      Assert.AreEqual(0, v.Value);
      v.Value = 1;
      Assert.AreEqual(1, v.Value);
      clock.Tick();
      Assert.AreEqual(1, v.Value);
      v.Value = 2;
      Assert.AreEqual(2, v.Value);
      clock.Back();
      Assert.AreEqual(1, v.Value);
      clock.Tick();
      Assert.AreEqual(1, v.Value);
      v.Value = 3;
      Assert.AreEqual(3, v.Value);
    }

    [Test]
    public void CantBeAccessedBefore() {
      var clock = new Clock();
      clock.Tick();
      var v = new Dense<int>(clock, 0);
      clock.Back();
      Assert.Throws<InvalidOperationException>(() => {
        v.Value = 10;
      });
    }

    [Test]
    public void LongTest() {
      var clock = new Clock();
      clock.Tick();
      clock.Tick();
      clock.Tick();
      var v = new Dense<int>(clock, 0);
      for (var i = 0; i < Clock.HISTORY_LENGTH * 2; ++i) {
        clock.Tick();
        v.Value = i;
      }
      for (var i = Clock.HISTORY_LENGTH * 2 - 1; i >= Clock.HISTORY_LENGTH; --i) {
        Assert.AreEqual(i, v.Value);
        clock.Back();
      }
      Assert.Throws<InvalidOperationException>(() => {
        var unused = v.Value;
      });
    }
  }
}