namespace Tests.Resources
{
  using Core.Resources;
  using NSubstitute;
  using NUnit.Framework;

  [TestFixture]
  public class NeoQueueTest
  {
    private NeoQueue _subject;
    private IWork wo1, wo2, wo3;

    private const int ID1 = 1;
    private const int ID2 = 2;
    private const int ID3 = 3;

    [SetUp]
    protected void SetUp()
    {
      _subject = new NeoQueue();
      wo1 = Substitute.For<IWork>();
      wo1.Id.Returns(ID1);
      wo2 = Substitute.For<IWork>();
      wo2.Id.Returns(ID2);
      wo3 = Substitute.For<IWork>();
      wo3.Id.Returns(ID3);
    }
// TODO - Implement NeoQueue Tests.
    [Test]
    public void Dequeue_WhenEmpty_ThrowsError()
    {
      Assert.Throws<System.IndexOutOfRangeException>(
        delegate { _subject.Dequeue(); }
      );
    }

    [Test]
    public void Dequeue_WhenHas1_ReturnsIt()
    {
      _subject.Enqueue(wo1);
      IWork wo = _subject.Dequeue();
      Assert.AreEqual(wo.Id, wo1.Id);
    }

    [Test]
    public void Dequeue_WhenHas2_ReturnsFirst()
    {
      _subject.Enqueue(wo2);
      _subject.Enqueue(wo1);
      IWork wo = _subject.Dequeue();
      Assert.AreEqual(wo.Id, wo2.Id);
    }

    [Test]
    public void Dequeue_WhenHas2AfterRemoval_ReturnsMin()
    {
      _subject.Enqueue(wo1);
      _subject.Enqueue(wo2);
      _subject.Enqueue(wo3);
      _subject.Remove(wo1.Id);
      IWork wo = _subject.Dequeue();
      Assert.AreEqual(wo.Id, wo2.Id);
    }

    [Test]
    public void Enqueue_WhenEmpty_AddsWo()
    {
      _subject.Enqueue(wo1);
      Assert.AreEqual(wo1.Id, _subject.FirstId());
    }

    [Test]
    public void Enqueue_Always_AddsWo()
    {
      _subject.Enqueue(wo1);
      _subject.Enqueue(wo2);
      _subject.Remove(wo1.Id);
      _subject.Enqueue(wo3);

      var values = _subject.Values;

      Assert.Contains(wo2, values);
      Assert.Contains(wo3, values);
    }

    [Test]
    public void Remove_WhenEmpty_ThrowsError()
    {
      Assert.Throws<System.Collections.Generic.KeyNotFoundException>(
        delegate { _subject.Remove(ID1); }
      );
    }

    [Test]
    public void Remove_WhenNotThere_ThrowsError()
    {
      _subject.Enqueue(wo1);
      Assert.Throws<System.Collections.Generic.KeyNotFoundException>(
        delegate { _subject.Remove(ID2); }
      );
    }

    [Test]
    public void Remove_WhenThere_ReturnsWo()
    {
      _subject.Enqueue(wo1);
      _subject.Enqueue(wo2);
      IWork wo = _subject.Remove(wo2.Id);
      Assert.AreEqual(wo.Id, ID2);
    }
  }
}