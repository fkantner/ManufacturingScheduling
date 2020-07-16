namespace Core
{
  public static class Configuration
  {
    // Program Settings
    public static readonly string ResultFileName = "../simulation-ui/src/data/test.json";
    public static readonly string ResultFolder = "../simulation-ui/src/data/";
    public static readonly int MinutesForProgramToTest = 1000;

    public static readonly int InitialNumberOfWorkorders = 40;

    // DayTime Settings
    public static readonly int MinutesInDay = 24*60; // 1440 minutes

    // Schedule Settings
    public static readonly int EnterpriseSchedule = 0;
    public static readonly int MachineSchedule = 0;
    public static readonly int PlantSchedule = 0;
    public static readonly int TransportationSchedule = 0;

  }
}