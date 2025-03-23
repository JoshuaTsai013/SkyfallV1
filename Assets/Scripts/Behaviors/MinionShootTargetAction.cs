using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Minion shoot target", story: "Minion [Gun] shoot [Target]", category: "Action/MyActions", id: "4861e21d53c42c2293e5b91769176b6c")]
public partial class MinionShootTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<MinionGun> Gun;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    protected override Status OnStart()
    {
        Gun.Value.Shoot(Target.Value.transform);
        return Status.Running;
    }

    // protected override Status OnUpdate()
    // {
    //     return Status.Success;
    // }

    protected override void OnEnd()
    {
    }
}

