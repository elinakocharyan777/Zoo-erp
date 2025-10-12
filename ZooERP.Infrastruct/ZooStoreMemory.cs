// ZooERP.Infrastruct/ZooStoreMemory.cs
using System.Collections.Generic;
using System.Threading; // interlocked
using ZooERP.Logic;

namespace ZooERP.Infrastruct;

/// <summary>простое in-memory хранилище + генератор номеров</summary>
public sealed class ZooStoreMemory : IZooStore, IInventoryNumberGenerator
{
    private readonly List<Animal> _animals = new();
    private readonly List<Thing> _things = new();
    private readonly object _sync = new();

    // счётчик номеров (стартуем с 0, чтобы первым выдать 1)
    private int _currentNumber = 0;

    /// <summary>выдать новый инвентарный номер (потокобезопасно)</summary>
    public int Next() => Interlocked.Increment(ref _currentNumber);

    /// <summary>добавить животное</summary>
    public void AddAnimal(Animal animal)
    {
        lock (_sync) _animals.Add(animal);
    }

    /// <summary>получить все животные (копия массива)</summary>
    public IReadOnlyCollection<Animal> Animals()
    {
        lock (_sync) return _animals.ToArray();
    }

    /// <summary>добавить предмет</summary>
    public void AddThing(Thing thing)
    {
        lock (_sync) _things.Add(thing);
    }

    /// <summary>получить все предметы (копия массива)</summary>
    public IReadOnlyCollection<Thing> Things()
    {
        lock (_sync) return _things.ToArray();
    }
}