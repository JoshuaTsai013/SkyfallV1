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
    [SerializeReference] public BlackboardVariable<bool> IsAddOffset;
    private float _elapsedTime = 0;
    private Vector3 _adjustedOffset = Vector3.zero;
    
    // Cache this constant rotation
    private readonly Quaternion _xAxisAdjustment = Quaternion.Euler(0, -90, 0);

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
        
        // Cache transform references
        Transform selfTransform = Self.Value.transform;
        Transform targetTransform = Target.Value.transform;
        
        _elapsedTime -= UnityEngine.Time.deltaTime;
        
        // Only reset adjustedOffset if needed
        if (IsAddOffset.Value)
        {
            _adjustedOffset = selfTransform.right * Offset.Value.x +
                             selfTransform.up * Offset.Value.y +
                             selfTransform.forward * Offset.Value.z;

            Debug.DrawLine(selfTransform.position + _adjustedOffset, targetTransform.position, Color.yellow);
        }
        else
        {
            _adjustedOffset = Vector3.zero;
        }
        
        Vector3 direction = (targetTransform.position - (selfTransform.position + _adjustedOffset)).normalized;
        direction.y = 0; // Keep only the horizontal direction

        // Create base target rotation
        Quaternion targetRotation = Quaternion.LookRotation(direction) * _xAxisAdjustment;
        
        // Cache rotation speed
        float rotationSpeed = UnityEngine.Time.deltaTime * 360f / Time.Value;

        selfTransform.rotation = Quaternion.RotateTowards(
            selfTransform.rotation,
            targetRotation,
            rotationSpeed
        );
        
        // Check time first (cheaper than angle calculation)
        if (_elapsedTime <= 0)
        {
            if (Quaternion.Angle(selfTransform.rotation, targetRotation) < 0.1f)
            {
                return Status.Success;
            }
        }

        return Status.Running;
    }
}

