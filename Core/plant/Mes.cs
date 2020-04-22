namespace Core.Plant
{
  using System.Collections.Generic;
  using Core.Resources;
  using Core.Workcenters;

  public class Mes
  {
    // TODO - Design MES and API.
    // TODO - Create Initialization for MES
    // Should handle Workcenters and Workorders coming in.
    // TODO - Create API for Workcenters to interact with
    // Start wo work
    // End wo work
    // Non conformance
    // 
    // TODO - Create API for Schedulers to interact with
    // List workcenters with wo's in them
    // Give recommendations??
    // TODO - Connect MES to Plant Scheduler
    // TODO - Connect MES to Transportation Scheduler
    // TODO - Create API for MES to ERP
    // Send process data up? Similar to Workcenter data?
    // Anything else needed?
    // TODO - Connect MES to ERP
    // TODO - API for Machine Breakdowns

    public Mes(string name, Dictionary<string, IAcceptWorkorders> locations)
    {
      Name = name;
      Workorders = new Dictionary<int, VirtualWorkorder>();
      LocationInventories = new Dictionary<string, List<VirtualWorkorder>>();
      Locations = new Dictionary<string, VirtualWorkcenter>();

      foreach(var location in locations)
      {
        var value = location.Value;
        //TODO - Fix Machine Type Issue for creating a Virtual Workcenter... 
        Locations.Add(location.Key, new VirtualWorkcenter(value.Name, value.ListOfValidTypes()));
      }

      foreach(var location in Locations)
      {
        LocationInventories.Add(location.Key, new List<VirtualWorkorder>());
      }
    }

    public string Name { get; }
    private Dictionary<int, VirtualWorkorder> Workorders { get; }
    private Dictionary<string, VirtualWorkcenter> Locations { get; }
    private Dictionary<string, List<VirtualWorkorder>> LocationInventories { get; }

    public void AddWorkorder(string location, IWork wo)
    {
      if(Workorders.ContainsKey(wo.Id))
      {
        throw new System.ArgumentException("Workorder already exists in MES");
      }
      VirtualWorkorder newWo = new VirtualWorkorder(wo.CurrentOpIndex, wo);
      Workorders.Add(wo.Id, newWo);

      LocationInventories[location].Add(newWo);
    }

    public List<int> GetLocationWoIds(string location)
    {
      return LocationInventories[location].ConvertAll<int>(x => x.Id);
    }
/*
    public void CompleteOp(int wo_number)
    {
      _workorders[wo_number].SetNextOp();
    }

    public void RemoveOp(int wo_number)
    {
      // TODO - Defend workorders that aren't supposed to be removed from MES.
      _workorders.Remove(wo_number);
    }
*/
    private class VirtualWorkcenter : IAcceptWorkorders
    {
      private readonly List<IWork> _output_buffer;
      public VirtualWorkcenter(string name, string type)
      {
        Name = name;
        Type = type;
        _output_buffer = new List<IWork>();
        InputBuffer = new List<IWork>();
      }

      public void AddToQueue(IWork wo)
      {
        InputBuffer.Add(wo);
      }

      public bool ReceivesType(string type)
      {
        return Type.IndexOf("," + type + ",") > 0;
      }

      public string ListOfValidTypes() { return Type; }

      public void Work(DayTime dayTime) {}
      public string Name { get; }
      public IEnumerable<IWork> OutputBuffer
      {
        get { return _output_buffer as IEnumerable<IWork>; }
      }
      public List<IWork> InputBuffer { get; }
      private string Type { get; }
    }

    private class VirtualWorkorder : IWork
    {
      public VirtualWorkorder(int currentOp, IWork woToClone)
      {
        Id = woToClone.Id;
        Operations = new List<Op>();
        woToClone.Operations.ForEach((operation) => Operations.Add((Op) operation.Clone()));
        CurrentOpIndex = currentOp;
      }

      public int CurrentOpIndex { get; private set; }
      public Op CurrentOp
      {
        get => Operations[CurrentOpIndex];
      }
      public List<Op> Operations { get; }
      public int CurrentOpEstTimeToComplete { get => CurrentOp.EstTimeToComplete; }
      public int CurrentOpSetupTime { get => CurrentOp.SetupTime; }
      public string CurrentOpType { get => CurrentOp.Type; }
      public int Id { get; }
      public void SetNextOp(){ CurrentOpIndex++; }
    }
  }
}