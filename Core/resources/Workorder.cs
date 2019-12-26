namespace Core.Resources
{
  using System.Collections.Generic;

  public class Workorder
  {
    private readonly List<Op> operations;

    private int current_op_index;

    private readonly int id;

    public Workorder(int number, List<Op> ops)
    {
      operations = ops;
      current_op_index = 0;
      id = number;
    }

    public Op CurrentOp()
    {
      return operations[current_op_index];
    }

    public void SetNextOp()
    {
      current_op_index++;
      return;
    }

    public int ID() { return id; }

    public override string ToString()
    {
      string answer = "WorkOrder: " + ID() + "\n";
      for(int i = 0; i<operations.Count; i++)
      {
        string part = "\t" + operations[i].ToString();
        if (i == current_op_index)
        {
          part = part + " <= CURRENT";
        }
        part = part + "\n";
        answer = answer + part;
      }
      return answer;
    }
  }
}