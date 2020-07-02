namespace Core.Enterprise
{
  using Core;
  using Plant;
  using Resources;
  using Schedulers;
  using System.Collections.Generic;

  public interface IEnterprise
  {
    DayTime DayTime { get; }
    IEnumerable<IPlant> Plants { get; }
    IRequestWork Customer { get; }
    IErp Erp { get; }
    EnterpriseScheduler Scheduler { get; }
    ITransportWorkBetweenPlants Transport { get; }
    void AddTransport(ITransportWorkBetweenPlants transport);
    void AddCustomer(IRequestWork customer);
    void CreateOrder(string type, DayTime due);
    void Work(DayTime dayTime);
  }

  public class Enterprise : IEnterprise
  {
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

    public IRequestWork Customer { get; private set;}
    public DayTime DayTime { get; }
    public IEnumerable<IPlant> Plants { get; }
    public IErp Erp { get; }
    public EnterpriseScheduler Scheduler { get; }
    public ITransportWorkBetweenPlants Transport { get; private set; }

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

    public void CreateOrder(string type, DayTime due)
    {
      Erp.CreateWorkorder(type, due);
    }

    public void Work(DayTime dayTime)
    {
      foreach(IPlant plant in Plants)
      {
        plant.Work(dayTime);
      }

      Transport.Work(dayTime);
      Erp.Work(dayTime);
    }
  }
}