using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Self rotate to target and check if align", story: "[Agent] rotate toward [Target] over [time] sec and check if align", category: "Action/MyActions", id: "803d0296ef9c6420f20aa09ed271d4c7")]
public partial class SelfRotateToTargetAndCheckIfAlignAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> Time;

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
        direction.y = 0; 
        // Keep only the horizontal direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Agent.Value.transform.rotation = Quaternion.RotateTowards(
            Agent.Value.transform.rotation,
            targetRotation,
            UnityEngine.Time.deltaTime * 360 / Time.Value
        );

        if (Quaternion.Angle(Agent.Value.transform.rotation, targetRotation) < 0.1f && _elapsedTime <= 0)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

