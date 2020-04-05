namespace Tests.Workcenters
{
  using Core;
  using Core.Resources;
  using Core.Workcenters;
  using Core.Schedulers;
  using NUnit.Framework;
  using System.Collections.Generic;
  using NSubstitute;

  [TestFixture]
  public class MachineTest
  {
    private Machine _subject;
    private IScheduleMachines machineScheduler;
    private DayTime _dayTime;

    [SetUp]
    protected void SetUp()
    {
      machineScheduler = Substitute.For<IScheduleMachines>();
      machineScheduler.Sort(Arg.Any<Queue<IWork>>());

      List<string> types = new List<string>(){"type1", "type2"};
      _subject = new Machine("test subject", machineScheduler, types);
      _dayTime = new DayTime();
    }

    [Test]
    public void AddToQueue_AddsWOAndCallsScheduler()
    {
      Workorder wo = GenerateWorkorder();
      _subject.AddToQueue(wo);

      machineScheduler.Received().Sort(Arg.Any<Queue<IWork>>());
    }

    [Test]
    public void Work_WhenEmpty_ReturnsNull()
    {
      var answer = _subject.Work(_dayTime);
      Assert.IsNull(answer);
    }

    [Test]
    public void Work_WhenNoCurrent_SetsCurrent()
    {
      SetupSubject(0);

      var answer = _subject.Work(_dayTime);

      Assert.IsNull(answer);
      Assert.IsEmpty(_subject.InputBuffer);
      Assert.AreEqual(1, _subject.CurrentWorkorder.Id);
      Assert.AreEqual(1, _subject.SetupTime);
      Assert.AreEqual(1, _subject.EstTimeToComplete);
    }

    [Test]
    public void Work_WhenSetup_ReduceSetupTime()
    {
      SetupSubject(1);

      var answer = _subject.Work(_dayTime);

      Assert.IsNull(answer);
      Assert.IsEmpty(_subject.InputBuffer);
      Assert.AreEqual(1, _subject.CurrentWorkorder.Id);
      Assert.AreEqual(0, _subject.SetupTime);
      Assert.AreEqual(1, _subject.EstTimeToComplete);
    }

    [Test]
    public void Work_WhenRunning_ReduceEstTimeToComplete()
    {
      SetupSubject(2);

      var answer = _subject.Work(_dayTime);

      Assert.AreEqual(1, answer.Id);
      Assert.AreEqual("type1", _subject.LastType);
      Assert.IsNull(_subject.CurrentWorkorder);
      Assert.IsEmpty(_subject.InputBuffer);
      Assert.AreEqual(0, _subject.SetupTime);
      Assert.AreEqual(0, _subject.EstTimeToComplete);
    }

    // Generators
    public Workorder GenerateWorkorder()
    {
      List<Op> ops = new List<Op>{
        new Op("type1", 1, 1),
        new Op("type2", 1, 1)
      };
      Workorder wo = new Workorder(1, ops);
      return wo;
    }

    //Prep Helpers
    public void SetupSubject(int workIterations)
    {
      Workorder wo = GenerateWorkorder();
      _subject.AddToQueue(wo);

      for(int i = 0; i < workIterations; i++)
      {
        _subject.Work(_dayTime);
      }
    }
  }
}