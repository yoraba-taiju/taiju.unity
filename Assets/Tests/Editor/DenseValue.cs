using NUnit.Framework;
using Donut;
using Donut.Values;

public class DenseValue {
  [Test]
  public void DenseValueSimplePasses() {
    var clock = new Clock();
    var v = new Dense<int>(clock) {
      Value = 0
    };
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
