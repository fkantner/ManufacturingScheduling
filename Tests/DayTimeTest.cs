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
      int expectedDay = 6;
      int expectedTime = 300;
      DayTime custom = new DayTime(6, 300);

      Assert.AreEqual(custom.Day, expectedDay);
      Assert.AreEqual(custom.Time, expectedTime);
    }

    [Test]
    public void InitialAtWed0()
    {
      int expectedDay = 3; //Wed
      int expectedTime = 0;

      Assert.AreEqual(_dayTime.Day, expectedDay);
      Assert.AreEqual(_dayTime.Time, expectedTime);
    }
    #endregion

    #region Methods
    [Test]
    public void CreateTimeStamp_FromInitial_CreatesATimestampAMinuteInFuture()
    {
      int expectedDay = 3;
      int expectedTime = 1;
      int expectedOriginalTime = 0;
      
      DayTime next = _dayTime.CreateTimestamp(1);

      Assert.AreEqual(_dayTime.Day, expectedDay);
      Assert.AreEqual(_dayTime.Time, expectedOriginalTime);
      Assert.AreEqual(next.Day, expectedDay);
      Assert.AreEqual(next.Time, expectedTime);
    }

    public void CreateTimeStamp_FromInitial_CreateATimestampADayInFuture()
    {
      int expectedDay = 3;
      int expectedOriginalDay = 4;
      int expectedTime = 0;
      int expectedOriginalTime = 0;
      
      DayTime next = _dayTime.CreateTimestamp(1440); //Should be right at 24 hours.

      Assert.AreEqual(_dayTime.Day, expectedOriginalDay);
      Assert.AreEqual(_dayTime.Time, expectedOriginalTime);
      Assert.AreEqual(next.Day, expectedDay);
      Assert.AreEqual(next.Time, expectedTime);
    }

    //TODO - Add Tests for Equals and LessThan

    public void Next_FromInitial_Increments1Minute()
    {
      int expectedDay = 3;
      int expectedTime = 1;

      _dayTime.Next();

      Assert.AreEqual(_dayTime.Day, expectedDay);
      Assert.AreEqual(_dayTime.Time, expectedTime);
    }

    //TODO - Add DayTime Test for Next Day

    #endregion
    
  }
}