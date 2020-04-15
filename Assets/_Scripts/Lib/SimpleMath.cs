using System;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMath : MonoBehaviour
{
    public static Vector2 Normalize2D(Vector2 vector)
    {
        return vector / vector.magnitude;
    }

    internal static Vector3 PointwiseDivide(Vector3 vector, Vector3 dividedBy)
    {
        return new Vector3(
            vector.x / dividedBy.x,
            vector.y / dividedBy.y,
            vector.z / dividedBy.z
        );
    }

    internal static Vector3 PointwiseMultiply(Vector3 vector, Vector3 multiplyBy)
    {
        return new Vector3(
            vector.x * multiplyBy.x,
            vector.y * multiplyBy.y,
            vector.z * multiplyBy.z
        );
    }

    internal static Vector2 AngleSplit(Vector2 position1, Vector2 position2)
    {
        return Vector2.zero;
    }

    internal static Vector3 RotateTowards(Vector3 current, Vector3 around, float theta)
    {
        return current * Mathf.Cos(theta) + (Vector3.Cross(around, current) * Mathf.Sin(theta) + around * (Vector3.Dot(around, current)) * (1F - Mathf.Cos(theta)));
    }   

    internal static int StringToInt(string s)
    {
        int result = 0;
        for (int i = s.Length; i > 0; i--)
        {
            int value = (int) Char.GetNumericValue(s.ToCharArray()[i-1]);
            result += value * (int) Mathf.Pow(10F, s.Length - i);
        }
        return result;
    }

    internal static Vector2 NormalVector2D(Vector2 vector)
    {
        return new Vector2(-vector.y, vector.x);
    }

    internal static T[] RemoveArrayElement<T>(T[] array, T elementToRemove)
    {
        T[] result = new T[array.Length - 1];
        bool offset = false;
        for(int i = 0; i < array.Length - 1; i++)
        {
            if (array[i] == null || array[i].Equals(elementToRemove))
            {
                Debug.Log("Removed Array Element " + i);
                offset = true;
            } else
            {
                result[i] = array[offset ? i + 1 : i];
            }
        }
        return result;
    }

    internal static T[] AddArrayElement<T>(T[] array, T elementToAdd)
    {
        T[] result = new T[array.Length + 1];
        for (int i = 0; i < array.Length; i++)
           result[i] = array[i];

        result[array.Length] = elementToAdd;
        return result;
    }
}
