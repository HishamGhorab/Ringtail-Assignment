using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_Utility
{
    /// <summary>
    /// Converts values from 0-1 to 0-255
    /// </summary>
    /// <param name="valueIn01Range"></param>
    /// <returns></returns>
    public static int ConvertColorComponentTo255(float valueIn01Range)
    {
        return Mathf.RoundToInt(valueIn01Range * 255f);
    }
    
    /// <summary>
    /// Remaps a given value from one range to another.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="fromMin"></param>
    /// <param name="fromMax"></param>
    /// <param name="toMin"></param>
    /// <param name="toMax"></param>
    /// <returns></returns>
    public static float RemapValue(float value, float fromMin, float fromMax, float toMin, float toMax) 
    {
        //Ensure the value is within the original range
        value = Mathf.Clamp(value, fromMin, fromMax);
    
        //Map the value from the original range to the new range
        float normalizedValue = (value - fromMin) / (fromMax - fromMin);
        float remappedValue = Mathf.Lerp(toMin, toMax, normalizedValue);
    
        return remappedValue;
    }
}
