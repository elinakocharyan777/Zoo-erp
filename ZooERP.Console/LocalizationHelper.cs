// ZooERP.Console/LocalizationHelper.cs
using System;
using ZooERP.Logic;

public static class LocalizationHelper
{
    /// <summary>
    /// перевод AnimalKind в русский текст
    /// </summary>
    /// <param name="kind"></param>
    /// <returns></returns>
    public static string ToRus(this AnimalKind kind) => kind switch
    {
        AnimalKind.Wolf => "волк",
        AnimalKind.Tiger => "тигр",
        AnimalKind.Rabbit => "кролик",
        AnimalKind.Monkey => "обезьяна",
        _ => kind.ToString()
    };

    /// <summary>
    /// перевод ThingType в русский текст
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string ToRus(this ThingType type) => type switch
    {
        ThingType.Table => "стол",
        ThingType.Computer => "компьютер",
        ThingType.Feeder => "кормушка",
        ThingType.Cage => "клетка",
        _ => type.ToString()
    };

    /// <summary>
    /// парсинг русского ввода вида животного
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static AnimalKind ParseAnimalKind(string input)
    {
        var s = (input ?? "").Trim().ToLowerInvariant();
        return s switch
        {
            "волк" => AnimalKind.Wolf,
            "тигр" => AnimalKind.Tiger,
            "кролик" or "заяц" => AnimalKind.Rabbit,
            "обезьяна" or "макака" => AnimalKind.Monkey,
            _ => throw new ArgumentException("Неверный вид. Допустимо: волк, тигр, кролик, обезьяна.")
        };
    }

    /// <summary>
    /// парсинг русского ввода типа предмета
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static ThingType ParseThingType(string input)
    {
        var s = (input ?? "").Trim().ToLowerInvariant();
        return s switch
        {
            "стол" => ThingType.Table,
            "компьютер" => ThingType.Computer,
            "кормушка" => ThingType.Feeder,
            "клетка" => ThingType.Cage,
            _ => throw new ArgumentException("Неверный тип. Допустимо: стол, компьютер, кормушка, клетка.")
        };
    }
}