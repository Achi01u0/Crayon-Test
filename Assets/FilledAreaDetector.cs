using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilledAreaDetector : MonoBehaviour
{
    public Color targetColor; // Ŀ����ɫ
    public float requiredArea = 1f; // �����¼�����С���

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Line"))
        {
            // �����������Ƿ������Ŀ����ɫ�����������׼
            if (IsFilledWithColor(other.gameObject) && GetFilledArea(other.gameObject) >= requiredArea)
            {
                TriggerEvent();
            }
        }
    }

    bool IsFilledWithColor(GameObject line)
    {
        // ʵ�ּ�������Ƿ����Ŀ����ɫ���߼�
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        return lineRenderer.startColor == targetColor && lineRenderer.endColor == targetColor;
    }

    float GetFilledArea(GameObject line)
    {
        // ����������
        PolygonCollider2D polygonCollider = line.GetComponent<PolygonCollider2D>();
        if (polygonCollider == null) return 0f;

        Vector2[] points = polygonCollider.points;
        return CalculatePolygonArea(points);
    }

    float CalculatePolygonArea(Vector2[] points)
    {
        // ʹ�ö���������ʽ�������
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
        // ʵ�ִ��������¼����߼�
        Debug.Log("�����¼���");
    }
}
