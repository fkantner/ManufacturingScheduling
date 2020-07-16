namespace Tests.Workcenters
{
  using Core;
  using Core.Plant;
  using Core.Resources;
  using Core.Workcenters;
  using NSubstitute;
  using NUnit.Framework;

  [TestFixture]
  public class WorkcenterTest
  {
    private Workcenter _subject;
    private DayTime _dayTime;
    private IWork _workorder;
    private IMes _mes;

    [SetUp]
    protected void SetUp()
    {
      _workorder = Substitute.For<IWork>();
      _workorder.CurrentOpType.Returns(Op.OpTypes.DrillOpType1);
      _workorder.CurrentOpSetupTime.Returns(0);
      _workorder.CurrentOpEstTimeToComplete.Returns(1);
          
      _mes = Substitute.For<IMes>();
      _dayTime = new DayTime();
      _subject = new Workcenter("TestWC", Machine.Types.BigDrill);
      _subject.SetMes(_mes);
    }

    [Test]
    public void Work_WhenEmpty_DoesNothing()
    {
      _dayTime = new DayTime();

      _subject.Work(_dayTime);

      Assert.IsFalse(_subject.OutputBuffer.Any());
      Assert.IsTrue(_subject.Inspection.Buffer.Empty());
      Assert.IsNull(_subject.Inspection.CurrentWo);
    }

    [Test]
    public void Work_WhenWoInBuffer_StartsWork()
    {
      _subject.AddToQueue(_workorder);

      _subject.Work(_dayTime);

      Assert.IsFalse(_subject.OutputBuffer.Any());
      Assert.IsNull(_subject.Inspection.CurrentWo);
      Assert.IsFalse(_subject.Inspection.Buffer.Empty());
    }

    [Test]
    public void Work_WhenWoIsInspected_PutsWoInBuffer()
    {
      _subject.AddToQueue(_workorder);

      for(int i = 0; i < 5; i++)
      {
        _subject.Work(_dayTime);
      }

      Assert.IsTrue(_subject.OutputBuffer.Any());
      Assert.IsNull(_subject.Inspection.CurrentWo);
      Assert.IsTrue(_subject.Inspection.Buffer.Empty());
    }
  }
}