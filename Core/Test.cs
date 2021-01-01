namespace Core
{
  public class Test
  {
    public string Name { get; }
    public int EnterpriseSchedule { get; }
    public int MachineSchedule { get; }
    public int PlantSchedule { get; }
    public int TransportationSchedule { get; }
    public int BigDataSchedule { get; }
    
    
    public Test(string name, int enterprise_schedule, int machine_schedule, int plant_schedule, int transportation_schedule, int big_data_schedule)
    {
      Name = name;
      EnterpriseSchedule = enterprise_schedule;
      MachineSchedule = machine_schedule;
      PlantSchedule = plant_schedule;
      TransportationSchedule = transportation_schedule;
      BigDataSchedule = big_data_schedule;
    }
  }
}