using Boom;
using Boom.Patterns.Broadcasts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonDependencyHandler : MonoBehaviour
{
    [SerializeField] string[] actionIdDependencies;
    [SerializeField] bool entityDependency;

    Button button;

    private void OnEnable()
    {
        if (!button) button = GetComponent<Button>();

        if (button)
        {
            if (actionIdDependencies.Length > 0) Broadcast.Register<OnActionInProcessCountChange>(_Update);
            if (entityDependency) UserUtil.AddListenerDataChangeSelf<DataTypes.Entity>(_Update, true);
        }
    }

    private void OnDisable()
    {
        if (button)
        {
            if (actionIdDependencies.Length > 0) Broadcast.Unregister<OnActionInProcessCountChange>(_Update);
            if (entityDependency) UserUtil.RemoveListenerDataChangeSelf<DataTypes.Entity>(_Update);
        }
    }


    private void _Update(Data<DataTypes.Entity> state)
    {
        _UpdateButton();
    }

    private void _Update(OnActionInProcessCountChange change)
    {
        _UpdateButton();
    }

    private void _UpdateButton()
    {
        if (actionIdDependencies.Length > 0)
        {
            foreach (var actionDependency in actionIdDependencies)
            {
                if (ActionUtil.GetActionProcessingCount(actionDependency) > 0)
                {
                    button.interactable = false;
                    return;
                }
            }
        }


        if (entityDependency)
        {
            if (UserUtil.IsDataValidSelf<DataTypes.Entity>() == false)
            {
                button.interactable = false;
                return;
            }
        }


        button.interactable = true;
    }
}
