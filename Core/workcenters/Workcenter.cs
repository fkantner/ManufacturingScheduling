namespace Core.Workcenters
{
  using Resources;
  using Core.Plant;
  using System.Collections.Generic;

  public class Workcenter : IAcceptWorkorders
  {
    private IMes _mes;

    public Workcenter(string name, Machine.Types machineType)
    {
      Machine = new Machine("Machine " + name, machineType);
      Name = name;
      OutputBuffer = new NeoQueue();
      Inspection = new Quality();
      _mes = null;
    }

    public Quality Inspection { get; }
    public IDoWork Machine { get; }
    public string Name { get; }
    public ICustomQueue OutputBuffer { get; }

    public void AddPlant(Plant plant)
    {
      Machine.AddPlant(plant);
    }

    public void AddToQueue(IWork wo)
    {
      //Check for _mes null for adding WO's before adding to MES.
      _mes?.StopTransit(wo.Id, Name);
      Machine.AddToQueue(wo);
      return;
    }

    public List<Op.OpTypes> ListOfValidTypes()
    {
      return Machine.ListOfValidTypes();
    }

    public bool ReceivesType(Op.OpTypes type)
    {
      return Machine.ReceivesType(type);
    }

    public void Work(DayTime dayTime)
    {
      IWork wo = Inspection.Work(dayTime);

      if(wo != null)
      {
        OutputBuffer.Enqueue(wo);
        _mes.Complete(wo.Id);
        // TODO - Implement Notify Scheduler when WC done
      }

      wo = Machine.Work(dayTime);

      if(wo != null)
      {
        Inspection.AddToQueue(wo);
      }
      else
      {
        wo = Machine.CurrentWorkorder;
        if( wo != null)
        {
          _mes.StartProgress(wo.Id);
        }
      }
    }

    public void SetMes(IMes mes)
    {
      if(_mes != null){ return; }
      _mes = mes;
    }
  }
}