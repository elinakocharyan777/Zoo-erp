using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ZooERP.Logic;

namespace ZooERP.Tests;

[TestClass]
public class AnimalTests
{
    [TestMethod]
    public void Constructor_AssignsValuesCorrectly()
    {
        var a = new Animal("Борис", AnimalKind.Rabbit, 3, 8);

        Assert.AreEqual("Борис", a.Name);
        Assert.AreEqual(AnimalKind.Rabbit, a.Kind);
        Assert.AreEqual(DietType.Herbivore, a.Diet);
        Assert.AreEqual(3, a.FoodKgPerDay);
        Assert.AreEqual(8, a.Kindness);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Constructor_EmptyName_Throws()
    {
        _ = new Animal("", AnimalKind.Wolf, 1, 5);
    }

    [TestMethod]
    public void Diet_ForTiger_IsPredator()
    {
        var a = new Animal("Тигра", AnimalKind.Tiger, 6, 5);
        Assert.AreEqual(DietType.Predator, a.Diet);
    }
    [TestMethod]
    public void ToString_should_include_name_and_number()
    {
        var a = new Animal("Кроша", AnimalKind.Rabbit, 2, 9);
        a.AssignInventoryNumber(1);

        var s = a.ToString();

        Assert.IsTrue(s.Contains("Кроша"));
        Assert.IsTrue(s.Contains("1"));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AssignInventoryNumber_twice_should_throw()
    {
        var a = new Animal("Рекс", AnimalKind.Wolf, 3, 5);
        a.AssignInventoryNumber(1);
        a.AssignInventoryNumber(2); // должно упасть
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_negative_food_should_throw()
    {
        _ = new Animal("А", AnimalKind.Rabbit, -1, 5);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void Constructor_kindness_out_of_range_should_throw()
    {
        _ = new Animal("А", AnimalKind.Rabbit, 1, 0);
    }

    
}