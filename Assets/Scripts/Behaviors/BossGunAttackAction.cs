using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Boss gun Attack", story: "Boss [Gun] Shoot", category: "Action/Boss", id: "a23bae2ab0c4b11efe0712fe4da5f300")]
public partial class BossGunAttackAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Gun;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

