using Boom.Values;
using Candid.World.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ActionTriggerDefault : MonoBehaviour
{
    [SerializeField] string actionName;
    [SerializeField] List<Field> args;
    [field: SerializeField] public UnityEvent<UResult<ProcessedActionResponse, ActionErrType.Base>> OnActionResponse { get; private set; }
    public void Trigger()
    {
        _Trigger();
    }
    private async void _Trigger()
    {
        var result = await ActionUtil.ProcessAction(actionName, args);

        OnActionResponse.Invoke(result);
    }
}
