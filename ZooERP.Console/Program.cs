using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ZooERP.Logic;
using ZooERP.Infrastruct;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ZooERP.Console;

// исключаем этот файл/класс из покрытия тестами (ui не тестируем юнитами)
[ExcludeFromCodeCoverage]
internal static class Program
{
    private static void Main(string[] args)
    {
        var services = new ServiceCollection();

        // регистрация зависимостей (di)
        services.AddSingleton<ZooStoreMemory>(); // один синглтон на всё
        services.AddSingleton<IZooStore>(sp => sp.GetRequiredService<ZooStoreMemory>());
        services.AddSingleton<IInventoryNumberGenerator>(sp => sp.GetRequiredService<ZooStoreMemory>());
        services.AddSingleton<IVetClinic, VetClinic>();
        services.AddSingleton<ZooService>();

        var provider = services.BuildServiceProvider();
        var zoo = provider.GetRequiredService<ZooService>();

        RunMenu(zoo);
    }

    // главный цикл меню
    private static void RunMenu(ZooService zoo)
    {
        while (true)
        {
            System.Console.WriteLine("\n    Московский зоопарк:");
            System.Console.WriteLine("1) Принять животное");
            System.Console.WriteLine("2) Добавить предмет");
            System.Console.WriteLine("3) Отчёт по корму");
            System.Console.WriteLine("4) Список животных для контактного зоопарка");
            System.Console.WriteLine("5) Показать баланс (животные + предметы)");
            System.Console.WriteLine("0) Выход");
            System.Console.Write("Выбор: ");

            var choice = System.Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1": AdmitAnimal(zoo); break;
                    case "2": AddThing(zoo); break;
                    case "3": PrintFoodReport(zoo); break;
                    case "4": PrintContactAnimals(zoo); break;
                    case "5": PrintBalance(zoo); break;
                    case "0": return;
                    default: System.Console.WriteLine("Неверный пункт меню."); break;
                }
            }
            catch (Exception ex) // ловим любые ошибки, чтобы не падать на вводе
            {
                System.Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }

    // Операции

    /// <summary>
    /// меню: принять животное через ветклинику
    /// </summary>
    /// <param name="zoo"></param>
    private static void AdmitAnimal(ZooService zoo)
    {
        System.Console.WriteLine("\n    Принять животное:");

        var kind = ReadAnimalKind("Вид (волк, тигр, кролик, обезьяна): ");
        var name = ReadNonEmpty("Кличка: ");
        var food = ReadInt("Корм (кг/сутки): ", min: 0);
        var kindness = ReadInt("Доброта (от 1 до 10): ", min: 1, max: 10);
        var health = ReadInt("Оценка здоровья (от 0 до 10): ", min: 0, max: 10);

        var animal = new Animal(name, kind, food, kindness);
        var result = zoo.TryAdmitAnimal(animal, health);

        System.Console.WriteLine(result.Accepted
            ? $"{result.Reason} Присвоен номер {animal.InventoryNumber}"
            : result.Reason);
    }
    /// <summary>
    /// меню: добавить предмет на баланс
    /// </summary>
    /// <param name="zoo"></param>
    private static void AddThing(ZooService zoo)
    {
        System.Console.WriteLine("\n    Добавить предмет:");
        var type = ReadThingTypeRus("Тип (стол, компьютер, кормушка, клетка): ");

        var thing = new Thing(type);
        zoo.AddThing(thing);

        System.Console.WriteLine($"Предмет добавлён. Присвоен номер {thing.InventoryNumber}.");
    }
    /// <summary>
    /// меню: отчёт по корму построчно + общий итог
    /// </summary>
    /// <param name="zoo"></param>
    private static void PrintFoodReport(ZooService zoo)
    {
        System.Console.WriteLine("\n    Отчёт по корму:");
        var animals = zoo.GetAnimals();

        if (animals.Count == 0)
        {
            System.Console.WriteLine("Животных пока нет");
        }
        else
        {
            foreach (var a in animals.OrderBy(a => a.InventoryNumber))
                System.Console.WriteLine($"Животное: {a.Kind.ToRus()} \"{a.Name}\" — {a.FoodKgPerDay} кг/сутки");
        }
        var r = zoo.GetFoodReport();
        System.Console.WriteLine($"Общая потребность: {r.TotalFoodKgPerDay} кг");
    }

    /// <summary>
    /// меню: кандидаты в контактный зоопарк (фильтр по доброте)
    /// </summary>
    /// <param name="zoo"></param>
    private static void PrintContactAnimals(ZooService zoo)
    {
        System.Console.WriteLine("\n    Контактный зоопарк:");
        var maybeThreshold = ReadIntOptional("Введите порог доброты (по умолчанию 6): ");
        var threshold = maybeThreshold ?? 6;

        var list = zoo.GetContactAnimals(threshold);
        if (list.Count == 0)
        {
            System.Console.WriteLine("Подходящих животных нет");
            return;
        }

        foreach (var a in list)
            System.Console.WriteLine($"{a.Kind.ToRus()} \"{a.Name}\" — доброта {a.Kindness}, номер {a.InventoryNumber}");
    }

    /// <summary>
    /// меню: общий баланс — сначала животные, затем предметы
    /// </summary>
    /// <param name="zoo"></param>
    private static void PrintBalance(ZooService zoo)
    {
        System.Console.WriteLine("\n    Баланс (животные → предметы):");

        var animals = zoo.GetAnimals().OrderBy(a => a.InventoryNumber).ToArray();
        if (animals.Length == 0) System.Console.WriteLine("Животных пока нет");
        foreach (var a in animals)
            System.Console.WriteLine($"{a.InventoryNumber}. {a.Kind.ToRus()} \"{a.Name}\" — доброта {a.Kindness}");

        System.Console.WriteLine(); // пустая строка-разделитель

        var things = zoo.GetThings().OrderBy(t => t.InventoryNumber).ToArray();
        if (things.Length == 0) System.Console.WriteLine("Предметов пока нет");
        foreach (var t in things)
            System.Console.WriteLine($"{t.InventoryNumber}. {t.Type.ToRus()} — номер {t.InventoryNumber}");
    }

    // для ввода 

    /// <summary>
    /// считывает непустую строку (гоняем, пока не введут)
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    private static string ReadNonEmpty(string prompt)
    {
        while (true)
        {
            System.Console.Write(prompt);
            var s = System.Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(s)) return s.Trim();
            System.Console.WriteLine("Поле не может быть пустым.");
        }
    }

    /// <summary>
    /// считывает целое число с проверками min/max
    /// </summary>
    /// <param name="prompt"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private static int ReadInt(string prompt, int? min = null, int? max = null)
    {
        while (true)
        {
            System.Console.Write(prompt);
            var s = System.Console.ReadLine();

            if (int.TryParse(s, out var val))
            {
                if (min.HasValue && val < min.Value) { System.Console.WriteLine($"Минимум: {min}."); continue; }
                if (max.HasValue && val > max.Value) { System.Console.WriteLine($"Максимум: {max}."); continue; }
                return val;
            }
            System.Console.WriteLine("Введите целое число.");
        }
    }

    /// <summary>
    /// считывает целое число или пусто (возвращает null)
    /// </summary>
    /// <param name="prompt"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private static int? ReadIntOptional(string prompt, int? min = null, int? max = null)
    {
        while (true)
        {
            System.Console.Write(prompt);
            var s = System.Console.ReadLine();

            if (string.IsNullOrWhiteSpace(s)) return null;

            if (int.TryParse(s, out var val))
            {
                if (min.HasValue && val < min.Value) { System.Console.WriteLine($"Минимум: {min}."); continue; }
                if (max.HasValue && val > max.Value) { System.Console.WriteLine($"Максимум: {max}."); continue; }
                return val;
            }
            System.Console.WriteLine("Введите целое число или оставьте пустым.");
        }
    }
    /// <summary>
    /// прочтение вида животного
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    private static AnimalKind ReadAnimalKind(string prompt)
    {
        while (true)
        {
            System.Console.Write(prompt);
            var s = (System.Console.ReadLine() ?? "").Trim();

            try
            {
                return LocalizationHelper.ParseAnimalKind(s);
            }
            catch
            {
                System.Console.WriteLine("Неверный вид. Допустимо: волк, тигр, кролик, обезьяна.");
            }
        }
    }
    /// <summary>
    /// прочтение тип вещей
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    private static ThingType ReadThingTypeRus(string prompt)
    {
        while (true)
        {
            System.Console.Write(prompt);
            var s = (System.Console.ReadLine() ?? "").Trim();

            try
            {
                return LocalizationHelper.ParseThingType(s);
            }
            catch
            {
                System.Console.WriteLine("Неверный тип. Допустимо: стол, компьютер, кормушка, клетка.");
            }
        }
    }
}