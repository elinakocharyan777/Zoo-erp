// ZooERP.Logic/ZooService.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZooERP.Logic;

/// <summary>сервис-оркестратор: приёмка, вещи, отчёты</summary>
public sealed class ZooService
{
    private readonly IZooStore _store;
    private readonly IVetClinic _clinic;
    private readonly IInventoryNumberGenerator _numbers;

    /// <summary>внедряем зависимости через конструктор (dip)</summary>
    public ZooService(IZooStore store, IVetClinic clinic, IInventoryNumberGenerator numbers)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
        _clinic = clinic ?? throw new ArgumentNullException(nameof(clinic));
        _numbers = numbers ?? throw new ArgumentNullException(nameof(numbers));
    }

    /// <summary>попытка принять животное по шкале здоровья 0..10</summary>
    public AdmissionResult TryAdmitAnimal(Animal animal, int healthScore)
    {
        if (animal is null) throw new ArgumentNullException(nameof(animal));
        if (healthScore is < 0 or > 10) throw new ArgumentOutOfRangeException(nameof(healthScore), "0..10");

        // запрашиваем решение у клиники (правило инкапсулировано в инфраструктуре)
        var decision = _clinic.Decide(animal, healthScore);

        // переводим решение в результат приёмки
        return decision.Status switch
        {
            HealthStatus.Sick => new AdmissionResult(false, "Отказ: животное больно."),
            HealthStatus.Healthy => Accept(animal, "Принято: отличное здоровье."),
            HealthStatus.ConditionallyHealthy when decision.AcceptIfConditional
                => Accept(animal, "Принято: возьмём и подлечим."),
            HealthStatus.ConditionallyHealthy
                => new AdmissionResult(false, "Отказ: состояние нестабильное, лечение необходимо до приёма."),
            _ => new AdmissionResult(false, "Отказ: неизвестный статус.")
        };
    }

    /// <summary>добавить предмет: присвоить номер и сохранить</summary>
    public void AddThing(Thing thing)
    {
        if (thing is null) throw new ArgumentNullException(nameof(thing));
        thing.AssignInventoryNumber(_numbers.Next()); // берём номер из генератора
        _store.AddThing(thing);
    }

    /// <summary>сводный отчёт по корму (кол-во + общий расход)</summary>
    public FoodReport GetFoodReport()
    {
        var animals = _store.Animals();
        var total = animals.Sum(a => a.FoodKgPerDay); // суммируем расход
        return new FoodReport(animals.Count, total);
    }

    /// <summary>контактный зоопарк: только травоядные и доброта выше порога</summary>
    public IReadOnlyCollection<Animal> GetContactAnimals(int threshold = 6) =>
        _store.Animals()
              .Where(a => a.Diet == DietType.Herbivore && a.Kindness > threshold)
              .OrderByDescending(a => a.Kindness)
              .ThenBy(a => a.Name)
              .ToArray();

    /// <summary>все предметы (как есть из хранилища)</summary>
    public IReadOnlyCollection<Thing> GetThings() => _store.Things();

    /// <summary>все животные (как есть из хранилища)</summary>
    public IReadOnlyCollection<Animal> GetAnimals() => _store.Animals();

    /// <summary>вспомогательный приёмник: присвоить номер и сохранить</summary>
    private AdmissionResult Accept(Animal animal, string reason)
    {
        animal.AssignInventoryNumber(_numbers.Next());
        _store.AddAnimal(animal);
        return new AdmissionResult(true, reason);
    }
}