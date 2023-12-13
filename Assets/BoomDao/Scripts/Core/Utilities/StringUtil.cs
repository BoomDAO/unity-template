using Boom.Utility;
using Boom.Values;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class StringUtil
{
    public static LinkedList<string> PeekBraketsValues(this string value)
    {
        LinkedList<string> returnValue = new();
        StringBuilder subExpr = new StringBuilder();
        bool isOpen = false;
        var index = 0;

        while (index < value.Length)
        {
            string token = $"{value[index]}";


            if (isOpen)
            {
                subExpr.Append(token);
            }

            if (token == "{")
            {
                isOpen = true;
            }
            else if (index < value.Length - 1)
            {
                string nextToken = $"{value[index + 1]}";

                if (nextToken == "}")
                {
                    isOpen = false;
                }
            }

            if (!isOpen && subExpr.Length > 0)
            {
                returnValue.AddLast(subExpr.ToString());
                subExpr.Length = 0;

            }
            index += 1;
        }

        return returnValue;
    }

}
