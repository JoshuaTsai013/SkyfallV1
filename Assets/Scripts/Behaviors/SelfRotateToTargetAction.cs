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
    [SerializeReference] public BlackboardVariable<bool> RotationLockOnYaxis;

    private float _elapsedTime = 0;

    protected override Status OnStart()
    {
        _elapsedTime = Time.Value;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Agent.Value == null || Target.Value == null || Time.Value <= 0)
        {
            return Status.Failure;
        }
        _elapsedTime -= UnityEngine.Time.deltaTime;

        Vector3 direction = Target.Value.transform.position - Agent.Value.transform.position;
        if (RotationLockOnYaxis.Value)
        {
            // Keep only the horizontal direction
            direction.y = 0;
        }
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Agent.Value.transform.rotation = Quaternion.RotateTowards(
            Agent.Value.transform.rotation,
            targetRotation,
            UnityEngine.Time.deltaTime * 360 / Time.Value
        );

        if (_elapsedTime <= 0)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

