using System;
using NUnit.Framework;
using Utility;

namespace Tests.Editor {
  [TestFixture]
  public class RingBufferTest {
    [Test]
    public void BasicTest() {
      var buff = new RingBuffer<int>(8192);
      Assert.IsTrue(buff.IsEmpty);
      Assert.IsFalse(buff.IsFull);
      buff.Add(1);
      Assert.IsFalse(buff.IsEmpty);
      Assert.IsFalse(buff.IsFull);
      Assert.AreEqual(1, buff.First);
      Assert.AreEqual(1, buff.Last);
      buff.Add(2);
      Assert.AreEqual(1, buff.First);
      Assert.AreEqual(2, buff.Last);
    }
    [Test]
    public void EmptyTest() {
      var buff = new RingBuffer<int>(1);
      Assert.IsFalse(buff.IsFull);
      buff.Add(1);
      Assert.IsTrue(buff.IsFull);
      Assert.Throws<InvalidOperationException>(() => {
        buff.Add(2);
      });
    }
    [Test]
    public void LongTest() {
      var buff = new RingBuffer<int>(256);
      Assert.IsFalse(buff.IsFull);
      for (var i = 0; i < buff.Capacity; ++i) {
        buff.Add(i);
      }
      Assert.IsTrue(buff.IsFull);
      for (var i = 256; i < 8192; ++i) {
        var last = buff.RemoveFirst();
        Assert.IsFalse(buff.IsFull);
        Assert.AreEqual(i - 256, last);
        buff.Add(i);
        Assert.IsTrue(buff.IsFull);
      }
      Assert.AreEqual(8191, buff.Last);
    }
  }
}