// ZooERP.Logic/Thing.cs
using System;

namespace ZooERP.Logic;

/// <summary>перечень типов предметов на балансе</summary>
public enum ThingType
{
    Table,      // стол
    Computer,   // компьютер
    Feeder,     // кормушка
    Cage        // клетка
}

/// <summary>предмет на балансе (только тип и номер)</summary>
public sealed class Thing : IInventory
{
    private int _inventoryNumber;

    /// <summary>тип предмета</summary>
    public ThingType Type { get; }

    /// <summary>инвентарный номер</summary>
    public int InventoryNumber => _inventoryNumber;

    /// <summary>создание предмета по типу</summary>
    public Thing(ThingType type) => Type = type;

    /// <summary>присвоение номера 1 раз</summary>
    public void AssignInventoryNumber(int number)
    {
        if (number <= 0) throw new ArgumentOutOfRangeException(nameof(number));
        if (_inventoryNumber != 0) throw new InvalidOperationException("Inventory number already assigned.");
        _inventoryNumber = number;
    }

    /// <summary>техническая строка (для отладки)</summary>
    public override string ToString() =>
        $"{Type} — номер {(InventoryNumber == 0 ? "—" : InventoryNumber.ToString())}";
}