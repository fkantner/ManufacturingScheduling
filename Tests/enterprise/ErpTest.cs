namespace Tests.Enterprise
{
    using Core;
    using Core.Enterprise;
    using Core.Plant;
    using Core.Resources;
    using Core.Schedulers;
    using Core.Workcenters;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;
    
    [TestFixture]
    public class ErpTest
    {
        private IPlant _plant1, _plant2;
        private Erp _subject;
        private const string ERP_NAME = "Name";
        private const string PLANT1_NAME = "Plant1", PLANT2_NAME = "Plant2";

        [SetUp]
        protected void SetUp()
        {
            _plant1 = Substitute.For<IPlant>();
            _plant1.Name.Returns(PLANT1_NAME);
            IMes mes1 = Substitute.For<IMes>();
            ISchedulePlants isp1 = Substitute.For<ISchedulePlants>();
            _plant1.Mes.Returns(mes1);
            _plant1.PlantScheduler.Returns(isp1);
            _plant1.Workcenters.Returns(new List<IAcceptWorkorders>());
            
            _plant2 = Substitute.For<IPlant>();
            _plant2.Name.Returns(PLANT2_NAME);
            IMes mes2 = Substitute.For<IMes>();
            ISchedulePlants isp2 = Substitute.For<ISchedulePlants>();
            _plant2.Mes.Returns(mes2);
            _plant2.PlantScheduler.Returns(isp2);
            _plant2.Workcenters.Returns(new List<IAcceptWorkorders>());

            List<IPlant> list = new List<IPlant>() { _plant1, _plant2 };

            _subject = new Erp(ERP_NAME);
            _subject.Add(_plant1);
            _subject.Add(_plant2);
        }

        [Test]
        public void Initially_Has0Workorders()
        {
            Assert.IsEmpty(_subject.Workorders);
        }

        [Test]
        public void AddWorkorders_AddsAWorkorder()
        {
            IWork wo = Substitute.For<IWork>();
            wo.Id.Returns(1);
            wo.ProductType.Returns(Workorder.PoType.p1);
            wo.CurrentOpIndex.Returns(0);
            wo.Operations.Returns(new List<Op>() { new Op(Op.OpTypes.DrillOpType2) });
            
            _subject.AddWorkorder(PLANT1_NAME, wo);

            Assert.IsNotEmpty(_subject.Workorders);
            Assert.IsNotEmpty(_subject.LocationInventories[PLANT1_NAME]);
            Assert.IsEmpty(_subject.LocationInventories[PLANT2_NAME]);
        }

        [Test]
        public void Work_SendsWorkordersToPlant()
        {
            Workorder.PoType productType = Workorder.PoType.p1;
            DayTime due = new DayTime().CreateTimestamp(50);
            _plant1.CanWorkOnType(Arg.Any<Op.OpTypes>()).Returns(false);
            _plant2.CanWorkOnType(Arg.Any<Op.OpTypes>()).Returns(true);

            _subject.CreateWorkorder(productType, due);
            _subject.Work(new DayTime());

            _plant1.DidNotReceive().Add(Arg.Any<IWork>());
            _plant2.Received().Add(Arg.Any<IWork>());
        }
    }
}