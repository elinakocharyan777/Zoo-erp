// ZooERP.Logic/Animal.cs
using System;

namespace ZooERP.Logic;

/// <summary>внутренний перечень видов животных</summary>
public enum AnimalKind { Monkey, Rabbit, Tiger, Wolf }

/// <summary>тип питания</summary>
public enum DietType { Herbivore, Predator }

/// <summary>животное, состоящее на балансе</summary>
public sealed class Animal : IInventory, IAlive
{
    private int _inventoryNumber;

    /// <summary>кличка</summary>
    public string Name { get; }

    /// <summary>вид</summary>
    public AnimalKind Kind { get; }

    /// <summary>диета определяется от вида</summary>
    public DietType Diet { get; }

    /// <summary>потребление корма кг/сутки</summary>
    public int FoodKgPerDay { get; }

    /// <summary>доброта 1..10</summary>
    public int Kindness { get; }

    /// <summary>инвентарный №, присваивается при приёмке</summary>
    public int InventoryNumber => _inventoryNumber;

    /// <summary>конструктор с базовой валидацией</summary>
    public Animal(string name, AnimalKind kind, int foodKgPerDay, int kindness)
    {
        // простая проверка входных параметров
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name is required", nameof(name)) : name.Trim();
        Kind = kind;
        Diet = GetDietFor(kind);

        if (foodKgPerDay < 0) throw new ArgumentOutOfRangeException(nameof(foodKgPerDay), "Food must be >= 0");
        if (kindness is < 1 or > 10) throw new ArgumentOutOfRangeException(nameof(kindness), "Kindness must be 1..10");

        FoodKgPerDay = foodKgPerDay;
        Kindness = kindness;
    }

    /// <summary>простое соответствие вида и диеты</summary>
    private static DietType GetDietFor(AnimalKind kind) => kind switch
    {
        AnimalKind.Rabbit => DietType.Herbivore,
        AnimalKind.Monkey => DietType.Herbivore,   // для учебной задачи считаем травоядным
        AnimalKind.Tiger => DietType.Predator,
        AnimalKind.Wolf => DietType.Predator,
        _ => DietType.Predator
    };

    /// <summary>присваиваем инвентарный номер 1 раз</summary>
    public void AssignInventoryNumber(int number)
    {
        if (number <= 0) throw new ArgumentOutOfRangeException(nameof(number));
        if (_inventoryNumber != 0) throw new InvalidOperationException("Inventory number already assigned.");
        _inventoryNumber = number;
    }

    /// <summary>техническое представление, используется при отладке</summary>
    public override string ToString() =>
        $"{Kind} \"{Name}\" (Diet={Diet}, Food={FoodKgPerDay} kg/day, Kindness={Kindness}, No={(InventoryNumber == 0 ? "—" : InventoryNumber.ToString())})";
}