namespace Core.Workcenters
{
  using Resources;
  using System.Collections.Generic;
  using Core.Plant;

  public class Workcenter : IAcceptWorkorders
  {
    private IMes _mes;
    public Workcenter(string name, IDoWork machine)
    {
      Machine = machine;
      Name = name;
      _output_buffer = new Queue<IWork>();
      Inspection = new Quality();
      _mes = null;
    }

    public IDoWork Machine { get; }
    public string Name { get; }
    private readonly Queue<IWork> _output_buffer;
    public IEnumerable<IWork> OutputBuffer
    {
      get { return _output_buffer; }
    }
    public Quality Inspection { get; }

    public string ListOfValidTypes()
    {
      return Machine.ListOfValidTypes();
    }

    public void AddToQueue(IWork wo)
    {
      //Check for _mes null for adding WO's before adding to MES.
      _mes?.StopTransit(wo.Id, Name);
      Machine.AddToQueue(wo);
      return;
    }

    public bool ReceivesType(string type)
    {
      return Machine.ReceivesType(type);
    }

    public void SetMes(IMes mes)
    {
      if(_mes != null){ return; }
      _mes = mes;
    }

    public void Work(DayTime dayTime)
    {
      IWork wo = Inspection.Work(dayTime);

      if(wo != null)
      {
        _output_buffer.Enqueue(wo);
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
  }
}