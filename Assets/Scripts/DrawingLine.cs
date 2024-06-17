using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingLine : MonoBehaviour
{
    public float speed = 5f;
    public Color penColor = Color.blue;
    public int penSize = 4; // 笔尖的粗细（单位：像素）
    public int textureWidth = 256; // 绘制层的宽度（单位：像素）
    public int textureHeight = 256; // 绘制层的高度（单位：像素）

    private Texture2D drawTexture;
    private SpriteRenderer spriteRenderer;
    private Vector2 previousPosition;

    void Start()
    {
        // 创建一个新的 Texture2D
        drawTexture = new Texture2D(textureWidth, textureHeight);
        drawTexture.filterMode = FilterMode.Point;

        // 将 Texture2D 应用到一个 SpriteRenderer 上
        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(drawTexture, new Rect(0, 0, textureWidth, textureHeight), new Vector2(0.5f, 0.5f));

        // 初始化绘制层为透明
        ClearTexture();
        previousPosition = transform.position;
    }

    void Update()
    {
        Move();
        Draw();
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
        Vector2 currentPosition = transform.position;
        if (Vector2.Distance(currentPosition, previousPosition) > 0.1f)
        {
            DrawLine(previousPosition, currentPosition);
            previousPosition = currentPosition;
        }
    }

    private void ClearTexture()
    {
        Color[] clearColorArray = drawTexture.GetPixels();
        for (int i = 0; i < clearColorArray.Length; i++)
        {
            clearColorArray[i] = Color.clear;
        }
        drawTexture.SetPixels(clearColorArray);
        drawTexture.Apply();
    }

    private void DrawLine(Vector2 start, Vector2 end)
    {
        int x0 = Mathf.RoundToInt(start.x);
        int y0 = Mathf.RoundToInt(start.y);
        int x1 = Mathf.RoundToInt(end.x);
        int y1 = Mathf.RoundToInt(end.y);

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            DrawPixel(x0, y0);
            if (x0 == x1 && y0 == y1) break;
            int e2 = err * 2;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
        drawTexture.Apply();
    }

    private void DrawPixel(int x, int y)
    {
        for (int i = -penSize / 2; i < penSize / 2; i++)
        {
            for (int j = -penSize / 2; j < penSize / 2; j++)
            {
                int px = x + i;
                int py = y + j;
                if (px >= 0 && px < drawTexture.width && py >= 0 && py < drawTexture.height)
                {
                    drawTexture.SetPixel(px, py, penColor);
                }
            }
        }
    }

}
