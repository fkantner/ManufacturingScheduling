namespace Tests.Workcenters
{
  using System.Collections.Generic;
  using Core;
  using Core.Workcenters;
  using Core.Resources;
  using NUnit.Framework;
  using NSubstitute;
  

  [TestFixture]
  public class QualityTest
  {
    private Quality _subject;
    private DayTime _dayTime;
    private IWork _wo1;

    [SetUp]
    protected void SetUp()
    {
      _subject = new Quality();
      _dayTime = new DayTime();

      List<Op> ops = new List<Op>()
      {
        new Op(Op.OpTypes.DrillOpType1),
        new Op(Op.OpTypes.DrillOpType2)
      };


      _wo1 = Substitute.For<IWork>();
      _wo1.Id.Returns(1);
      _wo1.Operations.Returns(ops);
    }

    [Test]
    public void Work_WhenNoWorkOrder_ReturnsNothing()
    {
      var answer = _subject.Work(_dayTime);

      Assert.IsNull(answer);
      Assert.IsNull(_subject.CurrentWo);
    }

    [Test]
    public void Work_WhenWoInQueue_SetsWoAndReturnsNull()
    {
      _subject.AddToQueue(_wo1);
      var answer = _subject.Work(_dayTime);

      Assert.IsNull(answer);
      Assert.AreEqual(_wo1, _subject.CurrentWo);
      Assert.AreEqual(3, _subject.CurrentInspectionTime);
    }

    [Test]
    public void Work_WhenProcessing_ReducesCurrentInspectionTime()
    {
      _subject.AddToQueue(_wo1);
      _subject.Work(_dayTime);
      var answer = _subject.Work(_dayTime);

      Assert.IsNull(answer);
      Assert.AreEqual(_wo1, _subject.CurrentWo);
      Assert.AreEqual(2, _subject.CurrentInspectionTime);
    }

    [Test]
    public void Work_WhenDoneProcessing_SetsNextOpAndReturns()
    {
      _subject.AddToQueue(_wo1);
      IWork answer = null;

      for(int i=0; i<4; i++)
      {
        answer = _subject.Work(_dayTime);
      }

      Assert.IsNotNull(answer);
      Assert.AreEqual(1, answer.Id);
      Assert.IsNull(_subject.CurrentWo);
      _wo1.Received().SetNextOp();
    }
  }
}