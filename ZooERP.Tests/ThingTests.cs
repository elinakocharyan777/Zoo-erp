using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ZooERP.Infrastruct;
using ZooERP.Logic;

namespace ZooERP.Tests;

[TestClass]
public class ThingTests
{
    [TestMethod]
    public void AssignInventoryNumber_WorksOnce()
    {
        var t = new Thing(ThingType.Table);
        t.AssignInventoryNumber(1);
        Assert.AreEqual(1, t.InventoryNumber);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AssignInventoryNumber_Twice_Throws()
    {
        var t = new Thing(ThingType.Table);
        t.AssignInventoryNumber(1);
        t.AssignInventoryNumber(2);
    }
    [TestMethod]
    public void ToString_should_contain_word_number_and_value()
    {
        var t = new Thing(ThingType.Feeder);
        t.AssignInventoryNumber(5);

        var s = t.ToString();

        Assert.IsTrue(s.Contains("номер"));
        Assert.IsTrue(s.Contains("5"));
    }

    [TestMethod]
    public void Type_getter_should_return_passed_value()
    {
        var t = new Thing(ThingType.Cage);
        var type = t.Type; 
        Assert.AreEqual(ThingType.Cage, type);
    }
    [TestMethod]
    public void AddThing_and_Things_should_work()
    {
        var store = new ZooStoreMemory();
        store.AddThing(new Thing(ThingType.Computer));

        var things = store.Things();

        Assert.AreEqual(1, things.Count);
        Assert.AreEqual(ThingType.Computer, things.First().Type);
    }
}