using UnityEngine;
using System.Collections;
using Framework;

public class Tools : SingletonFramework<Tools>
{
    private Tools()
    {

    }

    //获取投影的长度
    public float Projection(Vector3 v3a, Vector3 v3b)
    {
        Vector2 v2a = new Vector2(v3a.x, v3a.z);
        Vector2 v2b = new Vector2(v3b.x, v3b.z);

        return Vector2.Distance(v2a, v2b);
    }

    public Quaternion ClampRotationAroundYAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.y);

        angleY = Mathf.Clamp(angleY, -90.0f, 90.0f);

        q.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

        return q;
    }
}

