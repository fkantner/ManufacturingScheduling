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
    private IDoWork _testMachine;
    private DayTime _dayTime;
    private IWork _workorder;
    private IMes _mes;

    [SetUp]
    protected void SetUp()
    {
      _testMachine = Substitute.For<IDoWork>();
      _workorder = Substitute.For<IWork>();
      _mes = Substitute.For<IMes>();
      _dayTime = new DayTime();
      _subject = new Workcenter("TestWC", _testMachine);
      _subject.SetMes(_mes);
    }

    [Test]
    public void Work_WhenEmpty_DoesNothing()
    {
      _dayTime = new DayTime();
      IDoWork _emptyTestMachine = Substitute.For<IDoWork>();
      _emptyTestMachine.Work(_dayTime).ReturnsForAnyArgs((IWork) null);
      _emptyTestMachine.Work(null).Returns((IWork) null);

      Workcenter subject = new Workcenter("TestWC", _emptyTestMachine);
      subject.SetMes(_mes);

      subject.Work(_dayTime);

      Assert.IsFalse(subject.OutputBuffer.Any());
      Assert.IsTrue(subject.Inspection.Buffer.Empty());
      Assert.IsNull(subject.Inspection.CurrentWo);
      _emptyTestMachine.Received().Work(Arg.Any<DayTime>());
    }

    [Test]
    public void Work_WhenWoInBuffer_StartsWork()
    {
      _testMachine.Work(Arg.Any<DayTime>()).Returns(_workorder);

      _subject.Work(_dayTime);

      Assert.IsFalse(_subject.OutputBuffer.Any());
      _testMachine.Received().Work(_dayTime);
      Assert.IsNull(_subject.Inspection.CurrentWo);
      Assert.IsFalse(_subject.Inspection.Buffer.Empty());
    }

    [Test]
    public void Work_WhenWoIsInspected_PutsWoInBuffer()
    {
      //IWork returnValue = _workorder;
      _testMachine.Work(Arg.Any<DayTime>()).Returns(
        _workorder,
        (IWork) null
      );

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