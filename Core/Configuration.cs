namespace Core
{
  public sealed class Configuration
  {
    // Program Settings
    public static readonly string ResultFolder = "../ui-api/public/data/";
    public static readonly int MinutesForProgramToTest = 24*60*7;

    public static readonly int InitialNumberOfWorkorders = 40;

    // DayTime Settings
    public static readonly int MinutesInDay = 24*60; // 1440 minutes
    public static readonly int MinutesBetweenNewOrders = 60;

    // Scheduler Settings
    public static int EnterpriseDueDateVariable = 10;
    public static int EnterpriseTravelVariable = 10;
    public static int PlantOperationTimeVariable = 10;
    public static int PlantOperationCountVariable = 10;
    public static int MachineOpTypeVariable = 10;
    public static int MachineWaitTimeVariable = 10;
    public static int MachineDowntimeVariable = 10;
    public static int TransportJobStayVariable = 10;
    public static int TransportJobTransportStayVariable = 10;
    public static int TransportWCWaitVariable = 10;
    public static int TransportWCJobCountVariable = 10;
    public static int TransportWCAtCurrentPlantVariable = 10;

    private static Configuration instance = null;
    public static Configuration Instance 
    {
      get { return instance; }
    }

    public static Configuration Initialize(Core.Test test)
    {
      instance = new Configuration(test.Name, test.EnterpriseSchedule, test.MachineSchedule, test.PlantSchedule, test.TransportationSchedule, test.BigDataSchedule);
      if(test.configOptions.Count > 0)
      {
        EnterpriseDueDateVariable = test.configOptions["EnterpriseDueDateVariable"];

        EnterpriseTravelVariable = test.configOptions["EnterpriseTravelVariable"];
        PlantOperationTimeVariable = test.configOptions["PlantOperationTimeVariable"];
        PlantOperationCountVariable = test.configOptions["PlantOperationCountVariable"];
        MachineOpTypeVariable = test.configOptions["MachineOpTypeVariable"];
        MachineWaitTimeVariable = test.configOptions["MachineWaitTimeVariable"];
        MachineDowntimeVariable = test.configOptions["MachineDowntimeVariable"];       
        TransportJobStayVariable = test.configOptions["TransportJobStayVariable"];     
        TransportJobTransportStayVariable = test.configOptions["TransportJobTransportStayVariable"];
        TransportWCWaitVariable = test.configOptions["TransportWCWaitVariable"];
        TransportWCJobCountVariable = test.configOptions["TransportWCJobCountVariable"];
        TransportWCAtCurrentPlantVariable = test.configOptions["TransportWCAtCurrentPlantVariable"];
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