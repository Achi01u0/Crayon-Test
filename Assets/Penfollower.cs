using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penfollower : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 确保Z轴位置在相机平面上
        transform.position = mousePos;
    }
}
