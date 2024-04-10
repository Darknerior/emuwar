using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tools
{


    public static class Vector3Tools
    {

        /// <summary>
        /// Returns a Random Vector between an upper and lower limit
        /// </summary>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <returns></returns>
        public static Vector3 RandomVector(Vector3 lower, Vector3 upper) =>
            new Vector3(Random.Range(lower.x, upper.x), Random.Range(lower.y, upper.y),
                Random.Range(lower.z, upper.z));

        /// <summary>
        /// Returns a Random Vector between the positive and negative of the value given
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Vector3 RandomVector(Vector3 value) =>
            new Vector3(Random.Range(-value.x, value.x), Random.Range(-value.y, value.y),
                Random.Range(-value.z, value.z));

        public static Vector3 Multiply(this Vector3 vector, Vector3 multi) => new(vector.x * multi.x, vector.y * multi.y, vector.z * multi.z);

        public static void AddVector2(this ref Vector3 vector3, Vector2 vector2) => 
                vector3 = new(vector3.x + vector2.x, vector3.y + vector2.y, vector3.z);
    }

    public static class FloatTools
    {
        /// <summary>
        /// Rounds value to the closest multiple of the given floating point number.
        /// </summary>
        /// <param name="value">The number this method is used on</param>
        /// <param name="amount">The number to round the value to</param>
        public static void RoundToNearest(this ref float value,float amount)
        {
            float a = Mathf.Abs(amount); //make sure value is positive
            float valueDivByAmount = value / amount; //find raw result of value divided by amount
            int intValue = (int)valueDivByAmount; //cast an int vale for comparisons
            //if the remainder of float - int is less than 0.5, the value needs rounding down to the closest multiple, 
            //else it should be rounded up
            value = valueDivByAmount - intValue < 0.5 ? intValue * a : (intValue + 1) * a;
        }
    }
}
