using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Boss Rotate based on RightGun", story: "[Self] rotate to [target] with [offset] in [time] sec and check if align", category: "Action/Boss", id: "94678221d449b33306ad9ace90f84cfc")]
public partial class BossRotateBasedOnRightGunAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<Vector3> Offset;
    [SerializeReference] public BlackboardVariable<float> Time;
    private float _elapsedTime = 0;

    protected override Status OnStart()
    {
        _elapsedTime = Time.Value;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self.Value == null || Target.Value == null || Time.Value <= 0)
        {
            return Status.Failure;
        }
        _elapsedTime -= UnityEngine.Time.deltaTime;

        // Correct the Offset logic to ensure it adjusts based on the right arm
        Vector3 adjustedOffset = Self.Value.transform.right * Offset.Value.x +
                                 Self.Value.transform.up * Offset.Value.y +
                                 Self.Value.transform.forward * Offset.Value.z;

        Debug.DrawLine(Self.Value.transform.position + adjustedOffset, Target.Value.transform.position, Color.yellow);

        Vector3 direction = (Target.Value.transform.position - (Self.Value.transform.position + adjustedOffset)).normalized;
        direction.y = 0; // Keep only the horizontal direction

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Self.Value.transform.rotation = Quaternion.RotateTowards(
            Self.Value.transform.rotation,
            targetRotation,
            UnityEngine.Time.deltaTime * 360 / Time.Value
        );

        if (Quaternion.Angle(Self.Value.transform.rotation, targetRotation) < 0.1f && _elapsedTime <= 0)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

