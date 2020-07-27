namespace Tests.Workcenters
{
  using Core;
  using Core.Plant;
  using Core.Resources;
  using Core.Schedulers;
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
    private IPlant _plant;

    [SetUp]
    protected void SetUp()
    {
      _workorder = Substitute.For<IWork>();
      _workorder.CurrentOpType.Returns(Op.OpTypes.DrillOpType1);
      _workorder.CurrentOpSetupTime.Returns(0);
      _workorder.CurrentOpEstTimeToComplete.Returns(1);
      _workorder.Id.Returns(1);
      
      ISchedulePlants ps = Substitute.For<ISchedulePlants>();
      ps.ValidateWoForMachines(Arg.Any<int>(), Arg.Any<string>()).Returns(x => x[0]);
      _plant = Substitute.For<IPlant>();
      _plant.PlantScheduler.Returns(ps);

      _mes = Substitute.For<IMes>();
      _dayTime = new DayTime();
      _subject = new Workcenter("TestWC", Machine.Types.BigDrill);
      _subject.SetMes(_mes);

      _subject.AddPlant(_plant);
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
      Assert.IsTrue(_subject.Inspection.Buffer.Empty());
      _mes.Received().StartProgress(_workorder.Id);
    }

    [Test]
    public void Work_WhenWoIsComplete_StartsInspection()
    {
      _subject.AddToQueue(_workorder);

      for(int i = 0; i < 3; i++)
      {
        _subject.Work(_dayTime);
      }

      Assert.IsFalse(_subject.OutputBuffer.Any());
      Assert.IsNotNull(_subject.Inspection.CurrentWo);
      Assert.IsTrue(_subject.Inspection.Buffer.Empty());
      _mes.Received().StartProgress(_workorder.Id);
    }

    [Test]
    public void Work_WhenWoIsInspected_PutsWoInBuffer()
    {
      _subject.AddToQueue(_workorder);

      for(int i = 0; i < 6; i++)
      {
        _subject.Work(_dayTime);
      }

      Assert.IsTrue(_subject.OutputBuffer.Any());
      Assert.IsNull(_subject.Inspection.CurrentWo);
      Assert.IsTrue(_subject.Inspection.Buffer.Empty());
    }
  }
}