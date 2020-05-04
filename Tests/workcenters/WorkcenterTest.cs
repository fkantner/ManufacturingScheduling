namespace Tests.Workcenters
{
  using Core;
  using Core.Plant;
  using Core.Resources;
  using Core.Workcenters;
  using NSubstitute;
  using NUnit.Framework;
  using System.Collections.Generic;

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

      CollectionAssert.IsEmpty(subject.OutputBuffer);
      CollectionAssert.IsEmpty(subject.Inspection.Buffer);
      Assert.IsNull(subject.Inspection.CurrentWo);
      _emptyTestMachine.Received().Work(Arg.Any<DayTime>());
    }

    [Test]
    public void Work_WhenWoInBuffer_StartsWork()
    {
      _testMachine.Work(Arg.Any<DayTime>()).Returns(_workorder);

      _subject.Work(_dayTime);

      Assert.IsEmpty(_subject.OutputBuffer);
      _testMachine.Received().Work(_dayTime);
      Assert.IsNull(_subject.Inspection.CurrentWo);
      Assert.IsNotEmpty(_subject.Inspection.Buffer);
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

      Assert.IsNotEmpty(_subject.OutputBuffer);
      Assert.IsNull(_subject.Inspection.CurrentWo);
      Assert.IsEmpty(_subject.Inspection.Buffer);
    }
  }
}