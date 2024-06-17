using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawwithPen : MonoBehaviour
{
    public float speed = 5f;
    public LineRenderer lineRenderer;
    private PolygonCollider2D polyCollider;
    private Vector3 previousPosition;

    void Start()
    {
        //lineRenderer = GetComponent<LineRenderer>();
        //if (lineRenderer == null) lineRenderer = gameObject.AddComponent<LineRenderer>();

        polyCollider = gameObject.AddComponent<PolygonCollider2D>();
        polyCollider.isTrigger = true;

        lineRenderer.positionCount = 0;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.25f;
        lineRenderer.endWidth = 0.25f;
        lineRenderer.startColor = Color.blue;
        lineRenderer.endColor = Color.blue;

        previousPosition = transform.position;
    }

    void Update()
    {
        Move();
        Draw();
        CheckCoverage();
    }

    private void CheckCoverage()
    {
        GameObject[] shapes = GameObject.FindGameObjectsWithTag("Shape");
        foreach (GameObject shape in shapes)
        {
            if (IsFullyCovered(shape))
            {
                TriggerStory(shape);
                break;
            }
        }
    }

    private bool IsFullyCovered(GameObject shape)
    {
        PolygonCollider2D shapeCollider = shape.GetComponent<PolygonCollider2D>();
        if (shapeCollider == null)
        {
            Debug.LogWarning("PolygonCollider2D not found on " + shape.name);
            return false;
        }

        // 增加边缘点的检测
        Vector2[] shapePoints = shapeCollider.GetPath(0);
        int additionalPoints = 10; // 每条边增加的检测点数
        List<Vector2> pointsToCheck = new List<Vector2>(shapePoints);

        for (int i = 0; i < shapePoints.Length; i++)
        {
            Vector2 start = shape.transform.TransformPoint(shapePoints[i]);
            Vector2 end = shape.transform.TransformPoint(shapePoints[(i + 1) % shapePoints.Length]);
            for (int j = 1; j <= additionalPoints; j++)
            {
                Vector2 point = Vector2.Lerp(start, end, (float)j / (additionalPoints + 1));
                pointsToCheck.Add(point);
            }
        }

        foreach (Vector2 point in pointsToCheck)
        {
            Vector2 worldPoint = shape.transform.TransformPoint(point);
            if (!polyCollider.OverlapPoint(worldPoint))
            {
                Debug.Log("Point not covered: " + worldPoint);
                return false;
            }
        }

        Debug.Log("Shape fully covered: " + shape.name);
        return true;
    }

    private void TriggerStory(GameObject shape)
    {
        Debug.Log("Shape fully covered! Story triggered for: " + shape.name);
        // 触发剧情代码
    }

    private void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, vertical, 0f) * speed * Time.deltaTime;
        transform.position += direction;
    }

    private void Draw()
    {
        if (Vector3.Distance(transform.position, previousPosition) > 0.1f)
        {
            lineRenderer.positionCount += 1;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position);
            previousPosition = transform.position;
            UpdateColliderPath();
        }
    }

    private void UpdateColliderPath()
    {
        Vector2[] newPoints = new Vector2[lineRenderer.positionCount];
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            newPoints[i] = new Vector2(lineRenderer.GetPosition(i).x, lineRenderer.GetPosition(i).y);
        }

        if (newPoints.Length > 1)
        {
            polyCollider.SetPath(0, newPoints);
        }
    }

}
