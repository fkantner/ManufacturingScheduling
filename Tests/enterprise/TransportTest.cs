namespace Tests.Enterprise
{
    using Core;
    using Core.Enterprise;
    using Core.Resources;
    using Core.Plant;
    using Core.Workcenters;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class TransportTest
    {
        private Transport _subject;
        private IEnterprise _enterprise;
        private Dictionary<DayTime, string> _routes;
        private IPlant _plant1, _plant2;
        private DayTime _first;
        private IWork _wo1, _wo2;

        private const string PLANT1_NAME = "Plant1", PLANT2_NAME = "Plant2";

        [SetUp]
        protected void SetUp()
        {
            _plant1 = Substitute.For<IPlant>();
            _plant1.Name.Returns(PLANT1_NAME);

            _wo1 = Substitute.For<IWork>();
            _plant1.ShipToOtherPlants().Returns(new Dictionary<IWork, string>() { { _wo1, PLANT2_NAME } });

            _plant2 = Substitute.For<IPlant>();
            _plant2.Name.Returns(PLANT2_NAME);

            _wo2 = Substitute.For<IWork>();
            _plant2.ShipToOtherPlants().Returns(new Dictionary<IWork, string>() { { _wo2, PLANT1_NAME } });

            var dock = Substitute.For<IReceive>();
            dock.Name.Returns("Shipping Dock");
            _plant1.Dock().Returns(dock);
            _plant2.Dock().Returns(dock);

            _enterprise = Substitute.For<IEnterprise>();
            _enterprise.Plants.Returns(new List<IPlant>(){_plant1, _plant2});

            _first = new DayTime();
            _routes = new Dictionary<DayTime, string>()
            {
                {_first.CreateTimestamp(10), PLANT1_NAME},
                {_first.CreateTimestamp(20), PLANT2_NAME},
                {_first.CreateTimestamp(30), PLANT1_NAME},
                {_first.CreateTimestamp(40), PLANT2_NAME}
            };

            _subject = new Transport(_enterprise, _routes);
        }

        [Test]
        public void Transport_Starts_WithoutLocationNorCargo()
        {
            Assert.IsNull(_subject.CurrentLocation);
            Assert.IsEmpty(_subject.Inventory());

            _subject.Work(_first);

            //Should remain the same.
            Assert.IsNull(_subject.CurrentLocation);
            Assert.IsEmpty(_subject.Inventory());
        }

        [Test]
        public void Transport_AtFirstStop_UpdatesLocationAndReceivesCargo()
        {
            
            Dictionary<int, string> expected = new Dictionary<int, string>();
            expected.Add(_wo1.Id, PLANT2_NAME);

            DayTime newTime = _first.CreateTimestamp(10);
            _subject.Work(newTime);

            Assert.AreEqual(PLANT1_NAME, _subject.CurrentLocation);
            Assert.IsNotEmpty(_subject.Inventory());
            CollectionAssert.AreEquivalent(_subject.Inventory(), expected);
        }

        [Test]
        public void Transport_AtSecondStop_UpdatesLocationDropsOffCargoReceivesNewCargo()
        {
            
            Dictionary<int, string> expected = new Dictionary<int, string>();
            expected.Add(_wo1.Id, PLANT2_NAME);

            DayTime timestamp = _first.CreateTimestamp(10);
            _subject.Work(timestamp);

            // Test successful pickup
            CollectionAssert.AreEquivalent(_subject.Inventory(), expected);

            // Updating test parameters for second stop
            expected.Clear();
            expected.Add(_wo2.Id, PLANT1_NAME);
            timestamp = _first.CreateTimestamp(20);
            
            _subject.Work(timestamp);
            
            Assert.AreEqual(PLANT2_NAME, _subject.CurrentLocation);
            Assert.IsNotEmpty(_subject.Inventory());
            CollectionAssert.AreEquivalent(_subject.Inventory(), expected);
        }
    }
}