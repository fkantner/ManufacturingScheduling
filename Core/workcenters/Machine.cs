namespace Core.Workcenters
{
  using Core.Resources;
  using Core.Schedulers;
  using System.Collections.Generic;

  public class Machine
  {
    private List<Workorder> queue;
    private MachineScheduler scheduler;
    private readonly List<string> TYPE;

    private readonly string name;

    public Machine(string name, MachineScheduler ms, List<string> type)
    {
      this.name = name;
      queue = new List<Workorder>();
      scheduler = ms;
      TYPE = type;
    }

    public void Add(Workorder wc)
    {
      queue.Add(wc);
      return;
    }

    public override string ToString()
    {
      string answer = "Name: " + name + " Types: {";
      foreach (string type in TYPE)
      {
        answer += " " + type + " ";
      }
      answer += "}";

      if(queue.Count > 0)
      {
        answer += "\n\tQueue:\n";
        foreach(Workorder wc in queue)
        {
          answer += "\t" + wc.ToString() + "\n";
        }
      }

      return answer;
    }
    
  }
}
