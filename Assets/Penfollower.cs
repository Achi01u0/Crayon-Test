using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penfollower : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // ȷ��Z��λ�������ƽ����
        transform.position = mousePos;
    }
}
