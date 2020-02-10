namespace Tests.Workcenters
{
  using Core;
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

    private Workorder _workorder;

    [SetUp]
    protected void SetUp()
    {
      _testMachine = Substitute.For<IDoWork>();
      _workorder = Substitute.For<Workorder>(1, new List<Op>());
      _dayTime = new DayTime();
      _subject = new Workcenter("TestWC", _testMachine);
    }

    [Test]
    public void Work_WhenEmpty_DoesNothing()
    {
      _testMachine.Work(Arg.Any<DayTime>()).Returns((Workorder)null);
      
      _subject.Work(_dayTime);
      
      Assert.IsEmpty(_subject.OutputBuffer);
      Assert.IsEmpty(_subject.Inspection.Buffer);
      Assert.IsNull(_subject.Inspection.CurrentWo);
      _testMachine.Received().Work(_dayTime);
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
      Workorder returnValue = _workorder;
      _testMachine.Work(Arg.Any<DayTime>()).Returns(
        _workorder,
        (Workorder) null
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