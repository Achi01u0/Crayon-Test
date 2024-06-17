using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenController : MonoBehaviour
{
    public GameObject linePrefab; // 线条预制件
    private LineRenderer currentLineRenderer;
    private PolygonCollider2D currentPolygonCollider;
    private List<Vector2> points;

    public Color[] colors; // 颜色数组
    private int currentColorIndex = 0;

    void Update()
    {
        HandleDrawing();
        SwitchColor();
    }

    void HandleDrawing()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(mousePos, points[points.Count - 1]) > .1f)
            {
                UpdateLine(mousePos);
            }
        }
    }

    void CreateLine()
    {
        GameObject line = Instantiate(linePrefab);
        currentLineRenderer = line.GetComponent<LineRenderer>();
        currentPolygonCollider = line.AddComponent<PolygonCollider2D>();
        currentLineRenderer.startColor = colors[currentColorIndex];
        currentLineRenderer.endColor = colors[currentColorIndex];
        Debug.Log("Current Color: " + colors[currentColorIndex]); // 添加调试信息
        points = new List<Vector2>();
        points.Add(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        currentLineRenderer.positionCount = 1;
        currentLineRenderer.SetPosition(0, points[0]);
        currentPolygonCollider.points = points.ToArray();
    }

    void UpdateLine(Vector2 newPoint)
    {
        points.Add(newPoint);
        currentLineRenderer.positionCount++;
        currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, newPoint);
        currentPolygonCollider.points = points.ToArray();
    }

    void SwitchColor()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            currentColorIndex = (currentColorIndex + 1) % colors.Length;
        }
    }
}
