// ZooERP.Logic/Contracts.cs
namespace ZooERP.Logic;

/// <summary>итоговый мед. статус для решения</summary>
public enum HealthStatus
{
    Healthy = 1,
    ConditionallyHealthy = 2,
    Sick = 3
}

/// <summary>решение клиники: статус + можно ли принять условно здорового</summary>
public sealed record VetDecision(HealthStatus Status, bool AcceptIfConditional = false);

/// <summary>результат попытки принять животное</summary>
public sealed record AdmissionResult(bool Accepted, string Reason);

/// <summary>отчёт по корму: сколько животных и суммарный расход</summary>
public sealed record FoodReport(int AnimalCount, int TotalFoodKgPerDay);