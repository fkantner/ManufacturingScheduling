namespace Core.Resources
{
  using System.Collections.Generic;

  public class Workorder : IWork
  {
    public enum PoType 
    {
      p0,p1,p2,p3,p4,p5,p6,p7,p8,p9,p10,p11,p12
    }

    // PROPERTIES //
    public int CountCompletedOps { get => CurrentOpIndex; }
    public int CountTotalOps { get => Operations.Count; }
    public Op CurrentOp { get => Operations[CurrentOpIndex]; }
    public int CurrentOpEstTimeToComplete { get => CurrentOp.EstTimeToComplete; }
    public int CurrentOpIndex { get; private set;}
    public int CurrentOpSetupTime { get => CurrentOp.SetupTime; }
    public Op.OpTypes CurrentOpType { get => CurrentOp.Type; }
    public int Id { get; }
    public List<Op> Operations { get; }
    public PoType ProductType { get; }
    public bool NonConformance { get; set; }

    public Workorder(int number, PoType type, int initialOpIndex=0)
    {
      Id = number;
      ProductType = type;
      CurrentOpIndex = initialOpIndex;
      Operations = GetOperations(type);
      NonConformance = false;
    }

    // PUBLIC METHODS //
    public static int GetMaxOps(PoType type)
    {
      return GetMiddleOperations(type).Count + 2;
    }

    public void SetNextOp()
    {
      if(CurrentOpIndex < Operations.Count - 1)
        CurrentOpIndex++;
      return;
    }

    private List<Op> GetOperations(PoType type)
    {
      List<Op> middle = GetMiddleOperations(type);
      List<Op> answer = new List<Op>();
      answer.Add(new Op(Op.OpTypes.StageOp));
      answer.AddRange(middle);
      answer.Add(new Op(Op.OpTypes.ShippingOp));
      return answer;
    }

    private static List<Op> GetMiddleOperations(PoType type)
    {
      return type switch 
      {
        PoType.p0 => new List<Op>() { 
          new Op(Op.OpTypes.DrillOpType1),
          new Op(Op.OpTypes.DrillOpType2),
          new Op(Op.OpTypes.LatheOpType1)
        },
        PoType.p1 => new List<Op>() {
          new Op(Op.OpTypes.DrillOpType2),
          new Op(Op.OpTypes.DrillOpType2),
          new Op(Op.OpTypes.DrillOpType1),
          new Op(Op.OpTypes.LatheOpType2)
        },
        PoType.p2 => new List<Op>() {
          new Op(Op.OpTypes.LatheOpType1),
          new Op(Op.OpTypes.LatheOpType1)
        },
        PoType.p3 => new List<Op>() {
          new Op(Op.OpTypes.LatheOpType2),
          new Op(Op.OpTypes.LatheOpType2),
          new Op(Op.OpTypes.DrillOpType1)
        },
        PoType.p4 => new List<Op>() {
          new Op(Op.OpTypes.CncOpType1),
          new Op(Op.OpTypes.DrillOpType1),
          new Op(Op.OpTypes.DrillOpType2)
        },
        PoType.p5 => new List<Op>() {
          new Op(Op.OpTypes.CncOpType2),
          new Op(Op.OpTypes.DrillOpType3),
          new Op(Op.OpTypes.DrillOpType2),
          new Op(Op.OpTypes.DrillOpType1)
        },
        PoType.p6 => new List<Op>() {
          new Op(Op.OpTypes.PressOpType1),
          new Op(Op.OpTypes.CncOpType4),
          new Op(Op.OpTypes.DrillOpType2)
        },
        PoType.p7 => new List<Op>() {
          new Op(Op.OpTypes.PressOpType2),
          new Op(Op.OpTypes.CncOpType3),
          new Op(Op.OpTypes.DrillOpType2),
          new Op(Op.OpTypes.DrillOpType3)
        },
        PoType.p8 => new List<Op>() {
          new Op(Op.OpTypes.CncOpType3),
          new Op(Op.OpTypes.DrillOpType1),
          new Op(Op.OpTypes.DrillOpType2)
        },
        PoType.p9 => new List<Op>() {
          new Op(Op.OpTypes.LatheOpType2),
          new Op(Op.OpTypes.DrillOpType3),
          new Op(Op.OpTypes.DrillOpType1)
        },
        PoType.p10 => new List<Op>() {
          new Op(Op.OpTypes.PressOpType1),
          new Op(Op.OpTypes.LatheOpType1),
          new Op(Op.OpTypes.DrillOpType1)
        },
        PoType.p11 => new List<Op>() {
          new Op(Op.OpTypes.CncOpType4),
          new Op(Op.OpTypes.DrillOpType2),
          new Op(Op.OpTypes.DrillOpType3),
          new Op(Op.OpTypes.LatheOpType1),
          new Op(Op.OpTypes.DrillOpType1),
          new Op(Op.OpTypes.LatheOpType1)
        },
        PoType.p12 => new List<Op>() {
          new Op(Op.OpTypes.CncOpType2),
          new Op(Op.OpTypes.DrillOpType1),
          new Op(Op.OpTypes.CncOpType3),
          new Op(Op.OpTypes.DrillOpType1),
          new Op(Op.OpTypes.CncOpType2),
          new Op(Op.OpTypes.DrillOpType2),
          new Op(Op.OpTypes.DrillOpType1),
          new Op(Op.OpTypes.LatheOpType2)
        },
        _ => new List<Op>()
      };
    }
  }
}