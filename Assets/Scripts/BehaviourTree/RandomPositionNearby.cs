using UnityEngine;
using UnityEngine.AI;
using TheKiwiCoder;

//Bira nasumicnu tocku na NavMeshu oko neprijatelja (za lutanje)
public class RandomPositionNearby : ActionNode
{
    public float Radius = 6f;

    protected override void OnStart()
    {
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        Vector2 offset = Random.insideUnitCircle * Radius;
        Vector3 candidate = context.transform.position + new Vector3(offset.x, 0, offset.y);
        if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            blackboard.moveToPosition = hit.position;
            return State.Success;
        }
        return State.Failure;
    }
}
