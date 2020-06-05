namespace Tests.Enterprise
{
    using Core.Enterprise;
    using Core.Plant;
    using Core.Resources;
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
            _plant2 = Substitute.For<IPlant>();
            _plant2.Name.Returns(PLANT2_NAME);
            List<IPlant> list = new List<IPlant>() { _plant1, _plant2 };

            _subject = new Erp(ERP_NAME, list);
        }

        [Test]
        public void Initially_Has0Workorders()
        {
            Assert.IsEmpty(_subject.Workorders);
        }

        public void AddWorkorders_AddsAWorkorder()
        {
            IWork wo = Substitute.For<IWork>();
            wo.Id.Returns(1);
            wo.CurrentOpIndex.Returns(0);
            
            _subject.AddWorkorder(PLANT1_NAME, wo);

            Assert.IsNotEmpty(_subject.Workorders);
            Assert.IsNotEmpty(_subject.LocationInventories[PLANT1_NAME]);
            Assert.IsEmpty(_subject.LocationInventories[PLANT2_NAME]);
        }

    }
}