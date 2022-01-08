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
  }
}
