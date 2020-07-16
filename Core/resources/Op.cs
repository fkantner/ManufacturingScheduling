namespace Core.Resources
{
  using System;

  public class Op : ICloneable
  {
    public enum OpTypes {
      ShippingOp,
      StageOp,
      DrillOpType1,
      DrillOpType2,
      DrillOpType3,
      LatheOpType1,
      LatheOpType2,
      CncOpType1,
      CncOpType2,
      CncOpType3,
      CncOpType4,
      PressOpType1,
      PressOpType2
    }

    public int EstTimeToComplete { get; }
    public OpTypes Type { get; }
    public int SetupTime { get; }

    public Op(OpTypes type)
    {
      Type = type;
      EstTimeToComplete = GetCompleteTime(type);
      SetupTime = GetSetupTime(type);
    }

    public object Clone()
    {
      return new Op(Type);
    }

    private int GetCompleteTime(OpTypes type)
    {
      return type switch 
      {
        OpTypes.ShippingOp   =>  0,
        OpTypes.StageOp      =>  0,
        OpTypes.DrillOpType1 =>  4,
        OpTypes.DrillOpType2 =>  6,
        OpTypes.DrillOpType3 =>  5,
        OpTypes.LatheOpType1 => 15,
        OpTypes.LatheOpType2 => 12,
        OpTypes.CncOpType1   => 30,
        OpTypes.CncOpType2   => 35,
        OpTypes.CncOpType3   => 40,
        OpTypes.CncOpType4   => 45,
        OpTypes.PressOpType1 => 70,
        OpTypes.PressOpType2 => 90,
        _                    =>  0
      };
    }

    private int GetSetupTime(OpTypes type)
    {
      return type switch 
      {
        OpTypes.ShippingOp   =>  0,
        OpTypes.StageOp      =>  0,
        OpTypes.DrillOpType1 =>  2,
        OpTypes.DrillOpType2 =>  3,
        OpTypes.DrillOpType3 =>  4,
        OpTypes.LatheOpType1 =>  5,
        OpTypes.LatheOpType2 =>  7,
        OpTypes.CncOpType1   => 10,
        OpTypes.CncOpType2   => 15,
        OpTypes.CncOpType3   => 12,
        OpTypes.CncOpType4   => 10,
        OpTypes.PressOpType1 => 40,
        OpTypes.PressOpType2 => 55,
        _                    =>  0
      };
    }
  }
}