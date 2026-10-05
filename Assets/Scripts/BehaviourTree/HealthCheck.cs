using TheKiwiCoder;

public class HealthCheck : ActionNode
{
    public float HealthPercent = 0.3f;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (context.Enemy == null || context.Enemy.MaxHealth <= 0)
        {
            return State.Failure;
        }

        float healthPercent = (float)context.Enemy.Health / context.Enemy.MaxHealth;

        return healthPercent < HealthPercent ? State.Success : State.Failure;
    }
}
