using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooERP.Logic;
using ZooERP.Infrastruct;

namespace ZooERP.Tests;

[TestClass]
public class VetClinicTests
{
    [TestMethod]
    public void Decide_LowHealth_ReturnsSick()
    {
        var clinic = new VetClinic();
        var decision = clinic.Decide(new Animal("А", AnimalKind.Wolf, 2, 5), 3);
        Assert.AreEqual(HealthStatus.Sick, decision.Status);
    }

    [TestMethod]
    public void Decide_MediumHealth_ConditionalAccept()
    {
        var clinic = new VetClinic();
        var decision = clinic.Decide(new Animal("А", AnimalKind.Wolf, 2, 5), 6);
        Assert.AreEqual(HealthStatus.ConditionallyHealthy, decision.Status);
        Assert.IsTrue(decision.AcceptIfConditional);
    }

    [TestMethod]
    public void Decide_HighHealth_Healthy()
    {
        var clinic = new VetClinic();
        var decision = clinic.Decide(new Animal("А", AnimalKind.Wolf, 2, 5), 9);
        Assert.AreEqual(HealthStatus.Healthy, decision.Status);
    }
}