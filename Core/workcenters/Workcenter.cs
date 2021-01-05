namespace Core.Workcenters
{
  using Resources;
  using Core.Plant;
  using System.Collections.Generic;

  public class Workcenter : IAcceptWorkorders
  {
    private IMes _mes;
    private Enterprise.IHandleBigData _bigData;

    public Workcenter(string name, Machine.Types machineType)
    {
      Machine = new Machine("Machine " + name, machineType);
      Name = name;
      OutputBuffer = new NeoQueue();
      Inspection = new Quality();
      _mes = null;
      _bigData = null;
    }

    public Quality Inspection { get; }
    public IDoWork Machine { get; }
    public string Name { get; }
    public ICustomQueue OutputBuffer { get; }

    public void AddPlant(IPlant plant)
    {
      Machine.AddPlant(plant);
    }

    public void AddBigData(Enterprise.IHandleBigData bigData)
    {
      if (_bigData != null) { return; }
      _bigData = bigData;
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
      IWork wo = Inspection.Work(dayTime, _bigData, Name);

      if(wo != null)
      {
        if (wo.NonConformance)
        {
          Machine.AddToQueue(wo);
          _mes.NonConformance(wo.Id);
        }
        else
        {
          OutputBuffer.Enqueue(wo);
          _mes.Complete(wo.Id);
        }
      }

      wo = Machine.Work(_bigData.IsBreakdown(this.Name, dayTime));

      if(wo != null)
      {
        wo.NonConformance = false;
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