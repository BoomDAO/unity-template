using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class CustomTagAttribute : PropertyAttribute
{
#if UNITY_EDITOR
    public string[] tags = new string[0];
#endif
    public CustomTagAttribute(string[] tags)
    {
#if UNITY_EDITOR
        this.tags = tags;
#endif
    }
}