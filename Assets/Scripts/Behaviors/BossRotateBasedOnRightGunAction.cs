using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Boss Rotate based on RightGun", story: "[Self] rotate to [target] in [time] sec at [speedMultiplier] multiplier and check [isAlign] align", category: "Action/Boss", id: "94678221d449b33306ad9ace90f84cfc")]
public partial class BossRotateBasedOnRightGunAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<Vector3> Offset;
    [SerializeReference] public BlackboardVariable<float> Time;
    [SerializeReference] public BlackboardVariable<bool> IsAddOffset;
    [SerializeReference] public BlackboardVariable<float> SpeedMultiplier;
    [SerializeReference] public BlackboardVariable<bool> IsAlign;
    private float _elapsedTime = 0;
    private Vector3 _adjustedOffset = Vector3.zero;
    
    // Cache this constant rotation
    private readonly Quaternion _xAxisAdjustment = Quaternion.Euler(0, -90, 0);

    protected override Status OnStart()
    {
        _elapsedTime = Time.Value;
        
        // Use default value if not set
        if (SpeedMultiplier == null || SpeedMultiplier.Value <= 0)
        {
            if (SpeedMultiplier != null)
                SpeedMultiplier.Value = 1f;
        }
        
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
        
        // Cache rotation speed with multiplier
        float multiplier = (SpeedMultiplier != null) ? SpeedMultiplier.Value : 1f;
        float rotationSpeed = UnityEngine.Time.deltaTime * 360f / Time.Value * multiplier;

        selfTransform.rotation = Quaternion.RotateTowards(
            selfTransform.rotation,
            targetRotation,
            rotationSpeed
        );
        
        // Check if elapsed time has run out
        if (_elapsedTime <= 0)
        {
            // If alignment is required, check both time and rotation alignment
            if (IsAlign.Value)
            {
            if (Quaternion.Angle(selfTransform.rotation, targetRotation) < 0.1f)
            {
                return Status.Success;
            }
            }
            else
            {
            // If alignment is not required, success is based on time alone
            return Status.Success;
            }
        }

        return Status.Running;
    }
}

