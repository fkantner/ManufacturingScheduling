namespace Tests.Enterprise
{
    using Core;
    using Core.Enterprise;
    using Core.Resources;
    using Core.Plant;
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
        private IWork _wo;

        private const string PLANT1_NAME = "Plant1", PLANT2_NAME = "Plant2";

        [SetUp]
        protected void SetUp()
        {
            _plant1 = Substitute.For<IPlant>();
            _plant1.Name.Returns(PLANT1_NAME);

            _plant2 = Substitute.For<IPlant>();
            _plant2.Name.Returns(PLANT2_NAME);

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
            _wo = Substitute.For<IWork>();
            _plant1.ShipToOtherPlants().Returns(new Dictionary<IWork, string>() { { _wo, PLANT2_NAME } });
            Dictionary<int, string> expected = new Dictionary<int, string>();
            expected.Add(_wo.Id, PLANT2_NAME);

            DayTime newTime = _first.CreateTimestamp(10);
            _subject.Work(newTime);

            Assert.AreEqual(PLANT1_NAME, _subject.CurrentLocation);
            Assert.IsNotEmpty(_subject.Inventory());
            CollectionAssert.AreEquivalent(_subject.Inventory(), expected);
        }
    }
}