using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooERP.Logic;
using ZooERP.Infrastruct;

namespace ZooERP.Tests;

[TestClass]
public class ZooStoreMemoryTests
{
    [TestMethod]
    public void AddAnimal_StoresAnimal()
    {
        var store = new ZooStoreMemory();
        var a = new Animal("Лола", AnimalKind.Monkey, 2, 7);
        store.AddAnimal(a);

        Assert.AreEqual(1, store.Animals().Count);
    }

    [TestMethod]
    public void Next_GeneratesSequentialNumbers()
    {
        var store = new ZooStoreMemory();
        var n1 = store.Next();
        var n2 = store.Next();
        Assert.AreEqual(1, n1);
        Assert.AreEqual(2, n2);
    }
    [TestMethod]
    public void AddThing_should_store_and_Things_should_return_snapshot()
    {
        var store = new ZooStoreMemory();

        store.AddThing(new Thing(ThingType.Computer));
        var things = store.Things();

        Assert.AreEqual(1, things.Count);
        Assert.AreEqual(ThingType.Computer, things.First().Type);
    }
}