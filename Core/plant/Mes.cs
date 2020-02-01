namespace Core.Plant
{
  using System.Collections.Generic;
  using Core.Resources;
  using Core.Workcenters;

  public class Mes
  {
    // TODO - Connect MES to ERP
    // TODO - Connect MES to Plant Scheduler
    
    private Dictionary<int, Workorder> _workorders;
    private Dictionary<string, Workcenter> _workcenters;
    
    public Mes(Dictionary<string, Workcenter> workcenters)
    {
      _workorders = new Dictionary<int, Workorder>();
      _workcenters = workcenters;
    }

    public Workorder Workorder(int key) { return _workorders[key]; }
    public Workcenter Workcenter(string key) { return _workcenters[key]; }

    public void AddWorkorder(int number, List<Op> ops)
    {
      var workorder = new Workorder(number, ops);
      _workorders.Add(number, workorder);
    }

    public void CompleteOp(int wo_number)
    {
      _workorders[wo_number].SetNextOp();
    }

    public void RemoveOp(int wo_number)
    {
      // TODO - Defend workorders that aren't supposed to be removed from MES.
      _workorders.Remove(wo_number);
    }

  }
}