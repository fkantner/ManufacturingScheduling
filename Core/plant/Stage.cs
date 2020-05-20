namespace Core.Plant
{
  using Core.Workcenters;
  using Core.Resources;

  public class Stage : IAcceptWorkorders
  {
    private IMes _mes;
    private int _stage_counter;

    public Stage()
    {
      OutputBuffer = new NeoQueue();
      Queue = new NeoQueue();
      Name = "Stage";
      _mes = null;
      ResetCounter();
    }

    public ICustomQueue OutputBuffer { get; }
    public ICustomQueue Queue { get; }
    public string Name { get; }

    public void AddPlant(Plant plant)
    {
      // TODO Implement Stage scheduling
      return;
    }

    public void AddToQueue(IWork workorder)
    {
      _mes?.StopTransit(workorder.Id, Name);
      Queue.Enqueue(workorder);
    }

    public bool ReceivesType(string type)
    {
      return type == "stageOp";
    }

    public string ListOfValidTypes()
    {
      return "stageOp";
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
        ResetCounter();
      }

      _stage_counter--;
    }

    private void ResetCounter()
    {
      _stage_counter = 10;
    }
  }
}