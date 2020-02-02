namespace Tests
{
  using NUnit.Framework;
  using Core;

  [TestFixture]
  public class DayTimeTest
  {
    private DayTime _dayTime;

    [SetUp]
    protected void SetUp()
    {
      _dayTime = new DayTime();
    }

    [Test]
    public void InitialAtWed0()
    {
      int expectedDay = 3; //Wed
      int expectedTime = 0;

      Assert.AreEqual(_dayTime.Day, expectedDay);
      Assert.AreEqual(_dayTime.Time, expectedTime);
    }
  }

}