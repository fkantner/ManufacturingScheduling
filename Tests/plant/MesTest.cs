namespace Tests.Mes
{
  using Core.Resources;
  using Core.Plant;
  using Core.Workcenters;
  using NSubstitute;
  using NUnit.Framework;
  using System.Collections.Generic;

  [TestFixture]
  public class MesTest
  {
    private Mes _subject;
    private IAcceptWorkorders _wc1;
    private IAcceptWorkorders _wc2;

    private const string WC1 = "wc1", WC2 = "wc2";
    private const string TYPE1 = "type1", TYPE2 = "type2";

    [SetUp]
    protected void SetUp()
    {
      _wc1 = Substitute.For<IAcceptWorkorders>();
      _wc1.Name.Returns(WC1);
      _wc1.ReceivesType(TYPE1).Returns(true);
      _wc1.ReceivesType(TYPE2).Returns(false);
      _wc2 = Substitute.For<IAcceptWorkorders>();
      _wc2.Name.Returns(WC2);
      _wc2.ReceivesType(TYPE1).Returns(false);
      _wc2.ReceivesType(TYPE2).Returns(true);

      Dictionary<string, IAcceptWorkorders> list = new Dictionary<string, IAcceptWorkorders>()
      {
        { WC1, _wc1 },
        { WC2, _wc2 }
      };

      _subject = new Mes("mes1", list);
    }

    [Test]
    public void AddWorkorder_AddsWo()
    {
      IWork wo = CreateSubstituteWo(1, TYPE1, new List<Op>());

      _subject.AddWorkorder(WC1, wo);

      Assert.Contains(wo.Id, _subject.GetLocationWoIds(WC1));
    }

    [Test]
    public void Move_MovesWo()
    {
      IWork wo1 = CreateSubstituteWo(1, TYPE1, new List<Op>());
      IWork wo2 = CreateSubstituteWo(2, TYPE1, new List<Op>());

      _subject.AddWorkorder(WC1, wo1);
      _subject.AddWorkorder(WC1, wo2);

      _subject.Move(1, WC1, WC2);

      List<int> wc1_list = _subject.GetLocationWoIds(WC1);
      List<int> wc2_list = _subject.GetLocationWoIds(WC2);

      Assert.Contains(1, wc2_list);
      Assert.Contains(2, wc1_list);
      Assert.That(wc1_list, Has.No.Member(1));
      Assert.That(wc2_list, Has.No.Member(2));
    }

    [Test]
    public void Complete_CompletesOp()
    {
      Op op1 = new Op(TYPE1, 1, 1);
      Op op2 = new Op(TYPE2, 1, 1);
      List<Op> lo = new List<Op>(){op1, op2};

      IWork wo = CreateSubstituteWo(1, TYPE1, lo);
      _subject.AddWorkorder(WC1, wo);

      _subject.Complete(1);

      IWork answer = _subject.GetWorkorder(1);
      Assert.AreEqual(answer.CurrentOpIndex, 1);
    }

    [Test]
    public void GetLocationWoIds_ReturnsListOfWoIds()
    {
      IWork wo1 = CreateSubstituteWo(1, TYPE1, new List<Op>());
      IWork wo2 = CreateSubstituteWo(2, TYPE1, new List<Op>());
      IWork wrongWo = CreateSubstituteWo(3, TYPE1, new List<Op>());

      _subject.AddWorkorder(WC1, wo1);
      _subject.AddWorkorder(WC1, wo2);
      _subject.AddWorkorder(WC2, wrongWo);

      List<int> answer = _subject.GetLocationWoIds(WC1);

      Assert.Contains(1, answer);
      Assert.Contains(2, answer);
      Assert.That( answer, Has.No.Member(3));
    }

    private IWork CreateSubstituteWo(int id, string type, List<Op> ops)
    {
      IWork wo = Substitute.For<IWork>();
      wo.Id.Returns(id);
      wo.CurrentOpType.Returns(type);
      wo.Operations.Returns(ops);

      return wo;
    }
  }
}