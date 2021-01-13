namespace Tests.Plant
{
  using Core;
  using Core.Resources;
  using Core.Plant;
  using Core.Schedulers;
  using Core.Workcenters;
  using NSubstitute;
  using NUnit.Framework;
  using System.Collections.Generic;

  [TestFixture]
  public class TransportationTest
  {
    private Transportation _subject;
    private IScheduleTransport _scheduler;
    private IAcceptWorkorders _start;
    private IAcceptWorkorders _destination;
    private IMes _mes;
    private DayTime _dayTime;
    private IWork _workorder;
    private ICustomQueue _nQueue1;
    private ICustomQueue _nQueue2;
    private IPlant _plant;

    private const int WORKORDER_ID = 1;
    private const int EMPTY_WO_ID = 0;

    [SetUp]
    protected void SetUp()
    {
      Test test = new Test("Default", 0, 0, 0, 0, 0);
      Configuration.Initialize(test);
      
      _mes = Substitute.For<IMes>();
      
      _start = Substitute.For<IAcceptWorkorders>();
      _start.Name.Returns("Start Location");
      
      _destination = Substitute.For<IAcceptWorkorders>();
      _destination.Name.Returns("Destination");
      _destination.ReceivesType(Arg.Any<Core.Resources.Op.OpTypes>()).Returns(true);
      
      _plant = Substitute.For<IPlant>();
      _plant.Mes.Returns(_mes);
      _plant.Workcenters.Returns(new List<IAcceptWorkorders>(){_destination, _start});
      
      ISchedulePlants isp = Substitute.For<ISchedulePlants>();
      isp.ValidateDestinationForTransport(Arg.Any<int?>(), Arg.Any<string>(), Arg.Any<string>()).Returns(x => (string)x[2]);
      isp.ValidateWoForTransport(Arg.Any<int?>(), Arg.Any<string>()).Returns(x => x[0]);
      
      _plant.PlantScheduler.Returns(isp);

      _dayTime = new DayTime();
      _workorder = Substitute.For<IWork>();
      _workorder.Id.Returns(WORKORDER_ID);

      _nQueue1 = Substitute.For<ICustomQueue>();
      _nQueue1.Remove(WORKORDER_ID).Returns(_workorder);
      _nQueue1.Any().Returns(true);

      _nQueue2 = Substitute.For<ICustomQueue>();
      _nQueue2.Remove(WORKORDER_ID).Returns(_workorder);
      _nQueue2.Any().Returns(true);

      _start.OutputBuffer.Returns(_nQueue1);
      _destination.OutputBuffer.Returns(_nQueue2);

      _subject = new Transportation(_start, _plant);
    }

    [Test]
    public void Work_InitialWithoutDestination_ChecksTheSchedule()
    {
      const int TRANSPORT_TIME = 5;

      _subject.Work(_dayTime);

      Assert.AreEqual(_start.Name, _subject.CurrentLocation);
      Assert.AreEqual(_destination.Name, _subject.Destination);
      Assert.AreEqual(TRANSPORT_TIME, _subject.TransportTime);
      Assert.AreEqual(EMPTY_WO_ID, _subject.CargoNumber);
      _destination.DidNotReceive().AddToQueue(Arg.Any<Workorder>());
    }

    [Test]
    public void Work_WhenEmptyButAssignedADestination_GoesThereAndPicksUpCargo()
    {
      const int TRANSPORT_TIME = 5;
      _nQueue1.Any().Returns(false);
      _nQueue2.FirstId().Returns(1);
      _destination.ReceivesType(Arg.Any<Core.Resources.Op.OpTypes>()).Returns(false);
      _start.ReceivesType(Arg.Any<Core.Resources.Op.OpTypes>()).Returns(true);

      _subject.Work(_dayTime); // To Set Destination
      for(int i=0; i<5; i++)
      {
        _subject.Work(_dayTime); // To Go There
      }
      _subject.Work(_dayTime); // To Arrive

      Assert.AreEqual(_destination.Name, _subject.CurrentLocation); // Made it to destination
      Assert.AreEqual(WORKORDER_ID, _subject.CargoNumber); // Picked up Cargo -- if any
      Assert.AreEqual(_start.Name, _subject.Destination); // Is now assigned to new destination
      Assert.AreEqual(TRANSPORT_TIME, _subject.TransportTime);
      _destination.DidNotReceive().AddToQueue(Arg.Any<IWork>());
      _mes.Received().StartTransit(WORKORDER_ID, _subject.CurrentLocation);
    }

    [Test]
    public void Work_WhenHasCargo_GoesToDestinationAndDropsOff()
    {
      _nQueue2.Any().Returns(false);
      _nQueue1.FirstId().Returns(1);
      _start.ReceivesType(Arg.Any<Core.Resources.Op.OpTypes>()).Returns(false);
      _destination.ReceivesType(Arg.Any<Core.Resources.Op.OpTypes>()).Returns(true);

      _subject.Work(_dayTime); // To Set Destination
      for(int i=0; i<5; i++)
      {
        _subject.Work(_dayTime); // To Go There
      }
      _subject.Work(_dayTime); // To Arrive

      Assert.AreEqual(_destination.Name, _subject.CurrentLocation); // Made it to destination
      Assert.AreEqual(EMPTY_WO_ID, _subject.CargoNumber); // Picked up Cargo -- if any
      Assert.AreEqual("None", _subject.Destination); // destination is consumed
      Assert.AreEqual(0, _subject.TransportTime); // Transport is no longer going anywhere
      _destination.Received().AddToQueue(_workorder);
    }
  }
}