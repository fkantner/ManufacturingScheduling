namespace Core.Plant
{
  using System.Collections.Generic;
  using Core.Workcenters;
  using Core.Resources;

  public class Stage : IAcceptWorkorders
  {
// Properties
    public string Name { get; }
    public ICustomQueue OutputBuffer { get; }
    public ICustomQueue Queue { get; }
    
// Constructor
    public Stage()
    {
      OutputBuffer = new NeoQueue();
      Queue = new NeoQueue();
      Name = "Stage";
      _mes = null;
      _stage_counter = COUNTER_RESET_VALUE;
    }

// Pure Methods
    public List<Op.OpTypes> ListOfValidTypes()
    {
      return new List<Op.OpTypes>(){ VALID_TYPE };
    }

    public bool ReceivesType(Op.OpTypes type)
    {
      return type == VALID_TYPE;
    }

// Impure Methods
    public void AddPlant(IPlant plant)
    {
      return; // Noop.
    }
    
    public void AddToQueue(IWork workorder)
    {
      _mes?.StopTransit(workorder.Id, Name);
      Queue.Enqueue(workorder);
    }

    public void SetMes(IMes mes)
    {
      if(_mes != null) { return; }
      _mes = mes;
    }

    public void Work(DayTime dayTime)
    {
      if(!Queue.Any())
      {
        return;
      }

      if (_stage_counter == 0)
      {
        IWork process = Queue.Dequeue();
        process.SetNextOp();
        OutputBuffer.Enqueue(process);
        _stage_counter = COUNTER_RESET_VALUE;
      }

      _stage_counter--;
    }

// Private
    private IMes _mes;
    private int _stage_counter;
    private const int COUNTER_RESET_VALUE = 10;
    private const Op.OpTypes VALID_TYPE = Op.OpTypes.StageOp;
  }
}