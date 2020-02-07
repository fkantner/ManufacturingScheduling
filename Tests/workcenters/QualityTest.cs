namespace Tests.Workcenters
{
  using Core;
  using Core.Workcenters;
  using Core.Resources;
  using NUnit.Framework;
  using System.Collections.Generic;

  [TestFixture]
  public class QualityTest
  {
    private Quality _subject;
    private DayTime _dayTime;
    private Workorder _wo1;

    //TODO - Test Quality Work
    [SetUp]
    protected void SetUp()
    {
      _subject = new Quality();
      _dayTime = new DayTime();

      List<Op> ops = new List<Op>()
      {
        new Op("type1", 1, 1), 
        new Op("type2", 1, 1)
      };

      _wo1 = new Workorder(1, ops);
    }

    [Test]
    public void Work_WhenNoWorkOrder_ReturnsNothing()
    {
      Workorder answer = _subject.Work(_dayTime);

      Assert.IsNull(answer);
      Assert.IsNull(_subject.CurrentWo);
    }

    [Test]
    public void Work_WhenWoInQueue_SetsWoAndReturnsNull()
    {
      _subject.AddToQueue(_wo1);
      Workorder answer = _subject.Work(_dayTime);

      Assert.IsNull(answer);
      Assert.AreEqual(_subject.CurrentWo, _wo1);
    }

  }
}