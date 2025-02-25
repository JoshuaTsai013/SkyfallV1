using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Self rotate to target", story: "[Agent] rotate toward [Target] over [time] sec", category: "Action/MyActions", id: "07bda80912d97a85fb2e90438689cf6c")]
public partial class SelfRotateToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Time;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null || Target.Value == null || Time.Value <= 0)
        {
            return Status.Failure;
        }

        Vector3 direction = Target.Value.transform.position - Agent.Value.transform.position;
        direction.y = 0; // Keep only the horizontal direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Agent.Value.transform.rotation = Quaternion.RotateTowards(
            Agent.Value.transform.rotation,
            targetRotation,
            UnityEngine.Time.deltaTime * 360 / Time.Value
        );

        if (Quaternion.Angle(Agent.Value.transform.rotation, targetRotation) < 0.1f)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

