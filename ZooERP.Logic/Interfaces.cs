// ZooERP.Logic/Interfaces.cs
using System.Collections.Generic;

namespace ZooERP.Logic;

/// <summary>маркер «на балансе» — у сущности есть инвентарный номер</summary>
public interface IInventory
{
    /// <summary>инвентарный номер (только чтение)</summary>
    int InventoryNumber { get; }

    /// <summary>присваиваем номер один раз</summary>
    void AssignInventoryNumber(int number);
}

/// <summary>маркер «живое» — минимум свойств для животных</summary>
public interface IAlive
{
    /// <summary>сколько ест в кг/сутки</summary>
    int FoodKgPerDay { get; }

    /// <summary>уровень доброты 1..10</summary>
    int Kindness { get; }
}

/// <summary>общее хранилище животных и предметов</summary>
public interface IZooStore
{
    /// <summary>добавить животное в хранилище</summary>
    void AddAnimal(Animal animal);

    /// <summary>получить все животные (снимок)</summary>
    IReadOnlyCollection<Animal> Animals();

    /// <summary>добавить предмет в хранилище</summary>
    void AddThing(Thing thing);

    /// <summary>получить все предметы (снимок)</summary>
    IReadOnlyCollection<Thing> Things();
}

/// <summary>ветклиника решает, принимать ли животное по шкале 0..10</summary>
public interface IVetClinic
{
    /// <summary>принять решение по животному</summary>
    VetDecision Decide(Animal candidate, int healthScore);
}

/// <summary>генератор инвентарных номеров</summary>
public interface IInventoryNumberGenerator
{
    /// <summary>выдать следующий номер (монотонно возрастает)</summary>
    int Next();
}