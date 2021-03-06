namespace Tests.Enterprise
{
    using Core;
    using Core.Enterprise;
    using Core.Resources;
    using NSubstitute;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class CustomerTest
    {
        private DayTime _dayTime;
        private Customer _subject;
        private IEnterprise _enterprise;
        private readonly Workorder.PoType test_type = Workorder.PoType.p0;

        [SetUp]
        protected void Setup()
        {
            _subject = new Customer();
            _enterprise = Substitute.For<IEnterprise>();

            _subject.AddEnterprise(_enterprise);

            _dayTime = new DayTime();
        }

        [Test]
        public void CreateOrder_AddsAnOrder()
        {
            List<string> expected_answer = new List<string>(){
                test_type + " ; " + ConvertTime(_dayTime)
            };
            
            _subject.CreateOrder(test_type, _dayTime);

            CollectionAssert.AreEquivalent(expected_answer, _subject.ActiveOrders);
            CollectionAssert.IsEmpty(_subject.CompleteOrders);
        }

        [Test]
        public void ReceiveProduct_CompletesOrder()
        {
            DayTime completedDate = _dayTime.CreateTimestamp(20);
            List<string>expected_complete = new List<string>() {
                test_type + " ; " + ConvertTime(_dayTime) + " ; " + ConvertTime(completedDate)
            };

            _subject.CreateOrder(test_type, _dayTime);
            _subject.ReceiveProduct(test_type, completedDate);

            CollectionAssert.IsEmpty(_subject.ActiveOrders);
            CollectionAssert.AreEquivalent(expected_complete, _subject.CompleteOrders);
        }

        [Test]
        public void ReceiveProduct_ShouldKeepCompletedOrderDate()
        {
            DayTime day2 = _dayTime.CreateTimestamp(1440);
            DayTime day3 = day2.CreateTimestamp(1440);
            DayTime day4 = day3.CreateTimestamp(1440);

             List<string> expected = new List<string>();

             _subject.CreateOrder(test_type, _dayTime);
             _subject.CreateOrder(test_type, day2);
             _subject.CreateOrder(test_type, day3);
             _subject.CreateOrder(test_type, day4);

            CollectionAssert.AreEquivalent(expected, _subject.CompleteOrders);

            _subject.ReceiveProduct(test_type, _dayTime);

            expected.Add(test_type + " ; 0 ; 0");
            CollectionAssert.AreEquivalent(expected, _subject.CompleteOrders);
            
            _subject.ReceiveProduct(test_type, day2);

            expected.Add(test_type + " ; 1 ; 1");
            CollectionAssert.AreEquivalent(expected, _subject.CompleteOrders);

            _subject.ReceiveProduct(test_type, day2);

            expected.Add(test_type + " ; 2 ; 1");
            CollectionAssert.AreEquivalent(expected, _subject.CompleteOrders);

            _subject.ReceiveProduct(test_type, day4);

            expected.Add(test_type + " ; 3 ; 3");
            CollectionAssert.AreEquivalent(expected, _subject.CompleteOrders);
        }

        [Test]
        public void Work_SendsToEnterpriseOnce()
        {
            DayTime nextMinute = _dayTime.CreateTimestamp(1);
            _subject.CreateOrder(test_type, _dayTime);

            _subject.Work(_dayTime);
            
            _subject.Work(nextMinute);
            _enterprise.Received(1).StartOrder(test_type, Arg.Any<DayTime>(), 0);
        }

        private string ConvertTime(DayTime time)
        {
            return time.Day.ToString(); // + ":" + time.Time;
        }
    }
}