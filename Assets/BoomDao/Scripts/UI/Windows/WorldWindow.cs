using Boom.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldWindow : Window
{
    public override bool RequireUnlockCursor()
    {
        return true;
    }

    public override void Setup(object data)
    {
        
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link );
    }
}
