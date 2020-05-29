namespace Core.Resources.Virtual
{
  using Core.Plant;
  using Core.Schedulers;
  using Core.Workcenters;
  using System.Collections.Generic;

  public class VirtualPlant : IPlant
  {
    public VirtualPlant(string name, IPlant original)
    {
      Name = name;
      Mes = original.Mes;
      PlantScheduler = original.PlantScheduler;
      InternalTransportation = null;
      Workcenters = new List<IAcceptWorkorders>();
    }

    public IMes Mes { get; }
    public string Name { get; }
    public ISchedulePlants PlantScheduler { get; }
    public IEnumerable<IAcceptWorkorders> Workcenters { get; }
    public ITransportWork InternalTransportation { get; set; }

    public void Work(DayTime dt)
    {
      return;
    }
  }
}