using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilledAreaDetector : MonoBehaviour
{
    public Color targetColor; // 目标颜色
    public float requiredArea = 1f; // 触发事件的最小面积

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Line"))
        {
            // 检查绘制区域是否填充了目标颜色并满足面积标准
            if (IsFilledWithColor(other.gameObject) && GetFilledArea(other.gameObject) >= requiredArea)
            {
                TriggerEvent();
            }
        }
    }

    bool IsFilledWithColor(GameObject line)
    {
        // 实现检查区域是否填充目标颜色的逻辑
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        return lineRenderer.startColor == targetColor && lineRenderer.endColor == targetColor;
    }

    float GetFilledArea(GameObject line)
    {
        // 计算填充面积
        PolygonCollider2D polygonCollider = line.GetComponent<PolygonCollider2D>();
        if (polygonCollider == null) return 0f;

        Vector2[] points = polygonCollider.points;
        return CalculatePolygonArea(points);
    }

    float CalculatePolygonArea(Vector2[] points)
    {
        // 使用多边形面积公式计算面积
        float area = 0f;
        int j = points.Length - 1;
        for (int i = 0; i < points.Length; i++)
        {
            area += (points[j].x + points[i].x) * (points[j].y - points[i].y);
            j = i;
        }
        return Mathf.Abs(area / 2f);
    }

    void TriggerEvent()
    {
        // 实现触发剧情事件的逻辑
        Debug.Log("触发事件！");
    }
}
