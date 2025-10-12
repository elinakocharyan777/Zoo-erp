// ZooERP.Infrastruct/VetClinic.cs
using ZooERP.Logic;

namespace ZooERP.Infrastruct;

/// <summary>простая политика клиники по шкале 0..10</summary>
public sealed class VetClinic : IVetClinic
{
    /// <summary>0–4 — отказ, 5–7 — берём и подлечим, 8–10 — берём</summary>
    public VetDecision Decide(Animal candidate, int healthScore)
    {
        // тут никаких данных не сохраняем — только правило
        if (healthScore <= 4) return new VetDecision(HealthStatus.Sick);
        if (healthScore <= 7) return new VetDecision(HealthStatus.ConditionallyHealthy, AcceptIfConditional: true);
        return new VetDecision(HealthStatus.Healthy); // 8..10
    }
}