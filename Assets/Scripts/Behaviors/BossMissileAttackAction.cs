using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Boss Missile Attack", story: "Boss [Missile] Launch", category: "Action/Boss", id: "f0c4303e816b01927314e0a77d903c89")]
public partial class BossMissileAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<BossMissiles> Missile;

    protected override Status OnStart()
    {
        Missile.Value.SpawnMissile(); // Call the SpawnMissile method on the BossMissiles instance
        return Status.Running;
    }
}

