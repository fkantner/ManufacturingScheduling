namespace Core
{
  using System.Collections.Generic;
  using Core.Schedulers;

  public class Test
  {
    public enum Schedule { DEFAULT=0, MATCH=1, SCHEDULED=2 };
    public string Name { get; }
    public int EnterpriseSchedule { get; }
    public int MachineSchedule { get; }
    public int PlantSchedule { get; }
    public int TransportationSchedule { get; }
    public int BigDataSchedule { get; }
    
    public Dictionary<string, int> configOptions;
    
    public Test(string name, Schedule schedule, Dictionary<string, int> options)
    {
      Name = name;
      EnterpriseSchedule = 0;
      PlantSchedule = 0;
      BigDataSchedule = 0;
      configOptions = new Dictionary<string, int>();

      if(schedule == Schedule.DEFAULT)
      {
        MachineSchedule = 0;
        TransportationSchedule = 0;
      }
      else if (schedule == Schedule.MATCH)
      {
        MachineSchedule = 1;
        TransportationSchedule = 0;
      }
      else
      {
        MachineSchedule = (int) MachineScheduler.Schedule.BASIC_SCHEDULE;
        TransportationSchedule = (int) TransportationScheduler.Schedule.BASIC;
        configOptions = options;
      }
    }

    public Test(string name, int enterprise_schedule, int machine_schedule, int plant_schedule, int transportation_schedule, int big_data_schedule)
    {
      Name = name;
      EnterpriseSchedule = enterprise_schedule;
      MachineSchedule = machine_schedule;
      PlantSchedule = plant_schedule;
      TransportationSchedule = transportation_schedule;
      BigDataSchedule = big_data_schedule;
    }

    public bool Equals(Test other)
    {
      return Name == other.Name;
    }
  }
}