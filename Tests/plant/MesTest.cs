namespace Tests.Mes
{
  //TODO Create and Fill out tests for MES.
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
      IWork wo = Substitute.For<IWork>();
      wo.Id.Returns(1);
      wo.CurrentOpType.Returns(TYPE1);
      wo.Operations.Returns(new List<Op>());

      _subject.AddWorkorder(WC1, wo);

      Assert.Contains(wo.Id, _subject.GetLocationWoIds(WC1));
    }

    [Test]
    public void Move_MovesWo()
    {

    }

    [Test]
    public void Complete_CompletesOp()
    {

    }

    [Test]
    public void Location_ReturnsListOfWos()
    {

    }

  }
}