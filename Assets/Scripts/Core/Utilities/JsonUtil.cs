using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonUtil
{
    public static bool IsJsonObject(this string json)
    {
        return string.IsNullOrEmpty(json) == false && json.Length >= 2 && json[0] == '{' && json[json.Length - 1] == '}';
    }
    public static (string value, bool found) AccessJsonValue(this string json, string key)
    {
        if (json.IsJsonObject() == false) return ("", false);

        var data = JObject.Parse(json);

        if (data == null)
        {
            Debug.LogWarning($"> > > GetMetadataValueBy, data is null, metaData: {json}");
            return ("", false);
        }

        var value = data[key];

        if (value == null)
        {
            Debug.LogWarning($"> > > GetMetadataValueBy, value is null, key: {key}");
            return ("", false);
        }

        return (value.Value<string>(), true);
    }
    public static (string value, bool found) AccessJsonValue(this (string value, bool found) jsonData, string key)
    {
        if (jsonData.found) return AccessJsonValue(jsonData.value, key);
        return ("", false);
    }
    public static bool TryParseValue<T>(this string str, out T outValue)
    {
        outValue = default;
        if (string.IsNullOrEmpty(str)) return false;

        try
        {
            Type type = typeof(T);
            object value = Convert.ChangeType(str, type);
            outValue = (T)value;
            return true;
        }
        catch
        {
            return false;
        }
    }
    public static bool As<T>(this string json, out T convertedValue)
    {
        if (json.IsJsonObject())
        {
            convertedValue = JsonConvert.DeserializeObject<T>(json);
            return convertedValue != null;
        }
        else
        {
            return json.TryParseValue<T>(out convertedValue);
        }
    }
    public static bool As<T>(this (string value, bool found) jsonData, out T outValue)
    {
        outValue = default;

        if (jsonData.found == false) return false;

        return jsonData.value.As<T>(out outValue);
    }

    //public static string SetupJson()
    //{
    //    return"{}";
    //}
    //public static string SetJsonField(this string json, string name, string value)
    //{
    //    if (json.IsJsonObject())
    //    {
    //        json = json.TrimStart('{').TrimEnd('}');
    //        json
    //    }
    //    else return json;
    //}
}
