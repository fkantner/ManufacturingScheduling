namespace Core
{
  public sealed class Configuration
  {
    // Program Settings
    public static readonly string ResultFileName = "../ui-api/public/data/test.json";
    public static readonly string ResultFolder = "../ui-api/public/data/";
    public static readonly int MinutesForProgramToTest = 24*60*7;

    public static readonly int InitialNumberOfWorkorders = 40;

    // DayTime Settings
    public static readonly int MinutesInDay = 24*60; // 1440 minutes

    private static Configuration instance = null;
    public static Configuration Instance 
    {
      get { return instance; }
    }

    public static Configuration Initialize(string test, int eS, int mS, int pS, int tS, int bdS)
    {
      if (instance == null) { 
        instance = new Configuration(test, eS, mS, pS, tS, bdS);
      }

      return instance;
    }

    private Configuration(string test, int eS, int mS, int pS, int tS, int bdS)
    {
      EnterpriseSchedule = eS;
      MachineSchedule = mS;
      PlantSchedule = pS;
      TransportationSchedule = tS;
      BigDataSchedule = bdS;
      TestFilename = test;
    }

    // Schedule Settings
    public readonly int EnterpriseSchedule;
    public readonly int MachineSchedule;
    public readonly int PlantSchedule;
    public readonly int TransportationSchedule;
    public readonly int BigDataSchedule;
    public readonly string TestFilename;

  }
}