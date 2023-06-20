using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ItsJackAnton.Utility;
using ItsJackAnton.Patterns.Broadcasts;
using Candid;

public static class Utilities
{

    public static string AddressToShort(this string value)
    {
        string newString = "";

        int index = 0;
        char runner = value[index];
        while (runner != '-')
        {
            newString += runner;
            ++index;

            if (index == value.Length)
            {
                return newString;
            }
            runner = value[index];
        }

        newString += "...";

        index = value.Length - 1;
        runner = value[index];
        while (runner != '-')
        {
            newString += runner;
            --index;
            runner = value[index];
        }

        return newString;
    }

}
