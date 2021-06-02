using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class Vector2Extensions
    {
        public static void Rotate(this ref Vector2 v, float angle)
        {
            angle *= Mathf.Deg2Rad;

            v = new Vector2(
                v.x * Mathf.Cos(angle) - v.y * Mathf.Sin(angle),
                v.x * Mathf.Sin(angle) + v.y * Mathf.Cos(angle)
            );
        }
    }
}
