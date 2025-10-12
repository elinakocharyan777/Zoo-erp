using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using ZooERP.Infrastruct;
using ZooERP.Logic;
namespace ZooERP.Tests
{
    [TestClass]
    public class InventoryTests
    {
        private ZooService _zoo = null!;

        [TestInitialize]
        public void Init()
        {
            // ZooStoreMemory реализует и IZooStore, и IInventoryNumberGenerator
            var store = new ZooStoreMemory();
            var vet = new VetClinic();
            _zoo = new ZooService(store, vet, store);
        }

        [TestMethod]
        public void AddThing_should_add_and_be_listed_in_GetThings()
        {
            var thing = new Thing(ThingType.Table);

            _zoo.AddThing(thing);
            var list = _zoo.GetThings();

            Assert.AreEqual(1, list.Count);
            Assert.AreEqual(ThingType.Table, list.First().Type);
            Assert.IsTrue(list.First().InventoryNumber > 0);
        }

        [TestMethod]
        public void GetAnimals_should_return_animals_after_successful_admission()
        {
            var a = new Animal("Боня", AnimalKind.Monkey, 3, 8);

            var res = _zoo.TryAdmitAnimal(a, healthScore: 9);
            var animals = _zoo.GetAnimals();

            Assert.IsTrue(res.Accepted);
            Assert.AreEqual(1, animals.Count);
            Assert.AreEqual("Боня", animals.First().Name);
            Assert.IsTrue(animals.First().InventoryNumber > 0);
        }
    }
}
