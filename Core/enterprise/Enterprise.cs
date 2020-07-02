namespace Core.Enterprise
{
  using System.Collections.Generic;
  using Core;
  using Plant;
  using Resources;
  using Schedulers;

  public interface IEnterprise
  {
    IRequestWork Customer { get; }
    DayTime DayTime { get; }
    IErp Erp { get; }
    IEnumerable<IPlant> Plants { get; }
    EnterpriseScheduler Scheduler { get; }
    ITransportWorkBetweenPlants Transport { get; }
    void AddCustomer(IRequestWork customer);
    void AddTransport(ITransportWorkBetweenPlants transport);
    void StartOrder(string type, DayTime due);
    void Work(DayTime dayTime);
  }

  public class Enterprise : IEnterprise
  {
// Properties
    public IRequestWork Customer { get; private set;}
    public IErp Erp { get; }
    public DayTime DayTime { get; }
    public IEnumerable<IPlant> Plants { get; }
    public EnterpriseScheduler Scheduler { get; }
    public ITransportWorkBetweenPlants Transport { get; private set; }

// Constructor
    public Enterprise(DayTime dayTime, IEnumerable<IPlant> plants)
    {
      DayTime = dayTime;
      Plants = plants;
      Erp = (IErp) new Erp("ERP", Plants);
      Scheduler = new EnterpriseScheduler(Erp);
      Transport = null;
      Customer = null;

      foreach(IPlant plant in Plants)
      {
        plant.Mes.AddErp(Erp);
        plant.AddEnterprise(this);

        foreach(IWork wo in plant.Mes.Workorders.Values)
        {
          Erp.AddWorkorder(plant.Name, wo);
        }
      }
    }

// Pure Methods

// Impure Methods
    public void AddCustomer(IRequestWork customer)
    {
      if(Customer != null) { return; }
      Customer = customer;
    }

    public void AddTransport(ITransportWorkBetweenPlants transport)
    {
      if(Transport != null) { return; }
      Transport = transport;
    }

    public void StartOrder(string type, DayTime due)
    {
      Erp.CreateWorkorder(type, due);
    }

    public void Work(DayTime dayTime)
    {
      foreach(IPlant plant in Plants)
      {
        plant.Work(dayTime);
      }

      Transport?.Work(dayTime);
      Erp.Work(dayTime);
    }

// Private Methods

  }
}