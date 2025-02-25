using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "minionAttack", story: "Minion [gun] Shoot", category: "Action", id: "05fc8d2646ece7ca2aa37923f0b0ac39")]
public partial class MinionAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<MinionGun> Gun;

    protected override Status OnStart()
    {
        Gun.Value.Shoot();
        return Status.Running;
    }

}

