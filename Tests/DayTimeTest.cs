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

    #region ConstructorTests
    [Test]
    public void CustomInitialStartsSometimeElse()
    {
      const int expectedDay = 6;
      const int expectedTime = 300;
      DayTime custom = new DayTime(6, 300);

      Assert.AreEqual(custom.Day, expectedDay);
      Assert.AreEqual(custom.Time, expectedTime);
    }

    [Test]
    public void InitialAtWed0()
    {
      const int expectedDay = 0; //Sun
      const int expectedTime = 0;

      Assert.AreEqual(_dayTime.Day, expectedDay);
      Assert.AreEqual(_dayTime.Time, expectedTime);
    }
    #endregion

    #region Methods
    [Test]
    public void CreateTimeStamp_FromInitial_CreatesATimestampAMinuteInFuture()
    {
      const int expectedDay = 0;
      const int expectedTime = 1;
      const int expectedOriginalTime = 0;

      DayTime next = _dayTime.CreateTimestamp(1);

      Assert.AreEqual(_dayTime.Day, expectedDay);
      Assert.AreEqual(_dayTime.Time, expectedOriginalTime);
      Assert.AreEqual(next.Day, expectedDay);
      Assert.AreEqual(next.Time, expectedTime);
    }

    [Test]
    public void CreateTimeStamp_FromInitial_CreateATimestampADayInFuture()
    {
      const int expectedDay = 1;
      const int expectedOriginalDay = 0;
      const int expectedTime = 0;
      const int expectedOriginalTime = 0;

      DayTime next = _dayTime.CreateTimestamp(1440); //Should be right at 24 hours.

      Assert.Multiple( () =>
      {
        Assert.AreEqual(expectedOriginalDay, _dayTime.Day);
        Assert.AreEqual(expectedOriginalTime, _dayTime.Time);
        Assert.AreEqual(next.Day, expectedDay);
        Assert.AreEqual(next.Time, expectedTime);
      });
    }

    [Test]
    public void Next_FromInitial_Increments1Minute()
    {
      const int expectedDay = 0;
      const int expectedTime = 1;

      _dayTime.Next();

      Assert.AreEqual(_dayTime.Day, expectedDay);
      Assert.AreEqual(_dayTime.Time, expectedTime);
    }

    [Test]
    public void LessThan_WhenLess_ReturnsTrue()
    {
      _dayTime = _dayTime.CreateTimestamp(1440);
      _dayTime.Next();
      DayTime lessDay = new DayTime((int) DayTime.Days.Sun, 0);
      DayTime lessTime = new DayTime((int) DayTime.Days.Mon, 0);

      Assert.IsTrue(lessDay.LessThan(_dayTime));
      Assert.IsFalse(_dayTime.LessThan(lessDay));
      Assert.IsTrue(lessTime.LessThan(_dayTime));
      Assert.IsFalse(_dayTime.LessThan(lessTime));
      Assert.IsFalse(_dayTime.Equals(lessDay));
      Assert.IsFalse(_dayTime.Equals(lessTime));
    }

    [Test]
    public void Equals_WhenEqual_ReturnsTrue()
    {
      DayTime other = new DayTime();

      Assert.IsTrue(_dayTime.Equals(other));
      Assert.IsFalse(_dayTime.LessThan(other));
    }

    #endregion

  }
}