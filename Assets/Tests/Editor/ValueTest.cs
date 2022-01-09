using System;
using NUnit.Framework;
using Donut;
using Donut.Values;

namespace Tests.Editor {
  public class ValueTest {
    [Test]
    public void DenseValueBasicTest() {
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
    public void DenseValueCantBeAccessedBefore() {
      var clock = new Clock();
      clock.Tick();
      var v = new Dense<int>(clock, 0);
      clock.Back();
      Assert.Throws<InvalidOperationException>(() => {
        v.Value = 10;
      });
    }

    [Test]
    public void SparseValueBasicTest() {
      var clock = new Clock();
      var v = new Sparse<int>(clock, 0);

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
  }
}
