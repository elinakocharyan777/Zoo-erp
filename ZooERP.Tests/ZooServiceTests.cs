using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooERP.Logic;
using System;
using System.Linq;
using ZooERP.Infrastruct;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZooERP.Tests;

[TestClass]
public class ZooServiceTests
{
    private ZooService _service;

    [TestInitialize]
    public void Setup()
    {
        var store = new ZooStoreMemory();
        var clinic = new VetClinic();
        _service = new ZooService(store, clinic, store);
    }

    [TestMethod]
    public void TryAdmitAnimal_Healthy_Accepted()
    {
        var a = new Animal("Рыжик", AnimalKind.Rabbit, 1, 8);
        var result = _service.TryAdmitAnimal(a, 9);

        Assert.IsTrue(result.Accepted);
    }

    [TestMethod]
    public void TryAdmitAnimal_Sick_Rejected()
    {
        var a = new Animal("Больной", AnimalKind.Tiger, 5, 2);
        var result = _service.TryAdmitAnimal(a, 2);

        Assert.IsFalse(result.Accepted);
    }

    [TestMethod]
    public void GetContactAnimals_ReturnsOnlyHerbivores()
    {
        _service.TryAdmitAnimal(new Animal("Кролик", AnimalKind.Rabbit, 2, 9), 8);
        _service.TryAdmitAnimal(new Animal("Тигр", AnimalKind.Tiger, 5, 9), 8);

        var list = _service.GetContactAnimals(6);
        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(AnimalKind.Rabbit, list.First().Kind);
    }

    [TestMethod]
    public void FoodReport_CalculatesSumCorrectly()
    {
        _service.TryAdmitAnimal(new Animal("А", AnimalKind.Rabbit, 2, 5), 8);
        _service.TryAdmitAnimal(new Animal("Б", AnimalKind.Monkey, 3, 6), 8);

        var report = _service.GetFoodReport();
        Assert.AreEqual(5, report.TotalFoodKgPerDay);
        Assert.AreEqual(2, report.AnimalCount);
    }
    // fake-клиника для ветки "условно здоров, но НЕ принимаем"
    private sealed class FakeClinicReject : IVetClinic
    {
        public VetDecision Decide(Animal c, int score) => new VetDecision(HealthStatus.ConditionallyHealthy, AcceptIfConditional: false);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AddThing_null_should_throw()
    {
#pragma warning disable CS8625
        _service.AddThing(null);
#pragma warning restore CS8625
    }

  
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void Ctor_null_numbers_should_throw()
    {
        var store = new ZooStoreMemory(); var clinic = new VetClinic();
#pragma warning disable CS8625
        _ = new ZooService(store, clinic, null);
#pragma warning restore CS8625
    }

    // опционально: граничный случай порога (== threshold не попадает)
    [TestMethod]
    public void ContactZoo_excludes_equal_threshold()
    {
        _service.TryAdmitAnimal(new Animal("Добряк", AnimalKind.Rabbit, 1, 6), 9);
        var list = _service.GetContactAnimals(6);
        Assert.AreEqual(0, list.Count);
    }
    // клиника, которая условно-здоровых НЕ принимает

    [TestMethod]
    public void AddThing_should_add_and_be_listed_in_GetThings()
    {
        var thing = new Thing(ThingType.Table);

        _service.AddThing(thing);
        var list = _service.GetThings();

        Assert.AreEqual(1, list.Count);
        Assert.AreEqual(ThingType.Table, list.First().Type);
        Assert.IsTrue(list.First().InventoryNumber > 0);
    }

    [TestMethod]
    public void GetAnimals_should_return_animals_after_successful_admission()
    {
        var a = new Animal("Боня", AnimalKind.Monkey, 3, 8);

        var res = _service.TryAdmitAnimal(a, 9);
        var animals = _service.GetAnimals();

        Assert.IsTrue(res.Accepted);
        Assert.AreEqual(1, animals.Count);
        Assert.AreEqual("Боня", animals.First().Name);
        Assert.IsTrue(animals.First().InventoryNumber > 0);
    }



    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TryAdmitAnimal_health_below_0_should_throw()
    {
        var a = new Animal("А", AnimalKind.Rabbit, 1, 7);
        _service.TryAdmitAnimal(a, -1);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void TryAdmitAnimal_health_above_10_should_throw()
    {
        var a = new Animal("А", AnimalKind.Rabbit, 1, 7);
        _service.TryAdmitAnimal(a, 11);
    }

    [TestMethod]
    public void FoodReport_when_no_animals_should_be_zero()
    {
        var r = _service.GetFoodReport();
        Assert.AreEqual(0, r.AnimalCount);
        Assert.AreEqual(0, r.TotalFoodKgPerDay);
    }
}