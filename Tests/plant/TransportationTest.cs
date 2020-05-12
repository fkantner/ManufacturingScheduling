namespace Tests.Plant
{
  using Core;
  using Core.Resources;
  using Core.Plant;
  using Core.Schedulers;
  using Core.Workcenters;
  using NSubstitute;
  using NUnit.Framework;

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
    private ICustomQueue _nQueue;

    private const int WORKORDER_ID = 1;
    private const int EMPTY_WO_ID = 0;

    [SetUp]
    protected void SetUp()
    {
      _scheduler = Substitute.For<IScheduleTransport>();
      _mes = Substitute.For<IMes>();
      _scheduler.Mes.Returns(_mes);
      _start = Substitute.For<IAcceptWorkorders>();
      _start.Name.Returns("Start Location");
      _destination = Substitute.For<IAcceptWorkorders>();
      _destination.Name.Returns("Destination");
      _subject = new Transportation(_start, _scheduler);
      _dayTime = new DayTime();
      _workorder = Substitute.For<IWork>();
      _workorder.Id.Returns(WORKORDER_ID);

      _nQueue = Substitute.For<ICustomQueue>();
      _nQueue.Remove(WORKORDER_ID).Returns(_workorder);

      _start.OutputBuffer.Returns(_nQueue);
      _destination.OutputBuffer.Returns(_nQueue);
    }

    [Test]
    public void Work_InitialWithoutDestination_ChecksTheSchedule()
    {
      const int TRANSPORT_TIME = 5;
      _scheduler.ChooseNextCargo(Arg.Any<IAcceptWorkorders>());
      _scheduler.GetCargoID().Returns((int?)null);
      _scheduler.Destination.Returns(_destination);
      _scheduler.TransportTime.Returns(TRANSPORT_TIME);

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
      const int TRANSPORT_TIME = 1;
      _scheduler.ChooseNextCargo(Arg.Any<IAcceptWorkorders>());
      _scheduler.GetCargoID().Returns(null, WORKORDER_ID);
      _scheduler.Destination.Returns(_destination, _start);
      _scheduler.TransportTime.Returns(TRANSPORT_TIME);

      _subject.Work(_dayTime); // To Set Destination
      _subject.Work(_dayTime); // To Go There
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
      const int TRANSPORT_TIME = 1;
      _scheduler.ChooseNextCargo(Arg.Any<IAcceptWorkorders>());
      _scheduler.GetCargoID().Returns(WORKORDER_ID, null);
      _scheduler.Destination.Returns(_destination, _start);
      _scheduler.TransportTime.Returns(TRANSPORT_TIME);
      _destination.AddToQueue(Arg.Any<IWork>());

      _subject.Work(_dayTime); // To Set Destination
      _subject.Work(_dayTime); // To Go There
      _subject.Work(_dayTime); // To Arrive

      Assert.AreEqual(_destination.Name, _subject.CurrentLocation); // Made it to destination
      Assert.AreEqual(EMPTY_WO_ID, _subject.CargoNumber); // Picked up Cargo -- if any
      Assert.AreEqual("None", _subject.Destination); // destination is consumed
      Assert.AreEqual(0, _subject.TransportTime); // Transport is no longer going anywhere
      _destination.Received().AddToQueue(_workorder);
    }
  }
}