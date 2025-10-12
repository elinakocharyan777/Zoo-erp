using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZooERP.Logic;

namespace ZooERP.Tests;

[TestClass]
public class LocalizationHelperTests
{
    [TestMethod]
    public void ParseAnimalKind_ReturnsCorrectKind()
    {
        var kind = LocalizationHelper.ParseAnimalKind("волк");
        Assert.AreEqual(AnimalKind.Wolf, kind);
    }

    [TestMethod]
    public void ParseThingType_ReturnsCorrectType()
    {
        var type = LocalizationHelper.ParseThingType("стол");
        Assert.AreEqual(ThingType.Table, type);
    }

    [TestMethod]
    public void ToRus_AnimalKind_Works()
    {
        Assert.AreEqual("кролик", AnimalKind.Rabbit.ToRus());
    }
    [TestMethod]
    public void ToRus_should_cover_animal_kinds()
    {
        Assert.AreEqual("волк", AnimalKind.Wolf.ToRus());
        Assert.AreEqual("тигр", AnimalKind.Tiger.ToRus());
        Assert.AreEqual("кролик", AnimalKind.Rabbit.ToRus());
        Assert.AreEqual("обезьяна", AnimalKind.Monkey.ToRus());
    }

    [TestMethod]
    public void ToRus_should_cover_thing_types()
    {
        Assert.AreEqual("стол", ThingType.Table.ToRus());
        Assert.AreEqual("компьютер", ThingType.Computer.ToRus());
        Assert.AreEqual("кормушка", ThingType.Feeder.ToRus());
        Assert.AreEqual("клетка", ThingType.Cage.ToRus());
    }

    [TestMethod]
    public void ParseAnimalKind_should_support_synonyms_and_trim()
    {
        Assert.AreEqual(AnimalKind.Rabbit, LocalizationHelper.ParseAnimalKind("заяц"));
        Assert.AreEqual(AnimalKind.Monkey, LocalizationHelper.ParseAnimalKind("  ОБЕЗЬЯНА  "));
        Assert.AreEqual(AnimalKind.Tiger, LocalizationHelper.ParseAnimalKind("тигр"));
        Assert.AreEqual(AnimalKind.Wolf, LocalizationHelper.ParseAnimalKind("волк"));
    }

    [TestMethod]
    public void ParseThingType_should_parse_all_types()
    {
        Assert.AreEqual(ThingType.Table, LocalizationHelper.ParseThingType("стол"));
        Assert.AreEqual(ThingType.Computer, LocalizationHelper.ParseThingType("компьютер"));
        Assert.AreEqual(ThingType.Feeder, LocalizationHelper.ParseThingType("кормушка"));
        Assert.AreEqual(ThingType.Cage, LocalizationHelper.ParseThingType("клетка"));
    }
}