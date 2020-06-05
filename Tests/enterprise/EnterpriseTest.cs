namespace Tests.Enterprise
{
    using Core;
    using Core.Enterprise;
    using Core.Plant;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class EnterpriseTest
    {
        private DayTime dayTime;
        private Enterprise _subject;
        private IPlant _plant1, _plant2;

        [SetUp]
        protected void Setup()
        {
            dayTime = new DayTime();
            _plant1 = Substitute.For<IPlant>();
            _plant2 = Substitute.For<IPlant>();
            List<IPlant> list = new List<IPlant>() { _plant1, _plant2 };

            _subject = new Enterprise(dayTime, list);
        }

        [Test]
        public void Work_CallsBothPlants()
        {
            _subject.Work(dayTime);

            _plant1.Received().Work(dayTime);
            _plant2.Received().Work(dayTime);
        }
    }
}