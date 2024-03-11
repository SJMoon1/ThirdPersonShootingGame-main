using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmos : MonoBehaviour
{
    [SerializeField] private Color _color = Color.red;
    [SerializeField] private float _radius = 0.3f;
    void Start()
    {
        
    }
    void OnDrawGizmos() //선이나 색상 그려주는 유니티 지원 함수 콜백함수 
    {
        Gizmos.color = _color; //컬러 
        Gizmos.DrawSphere(transform.position, _radius); // 모양이나 도형(위치, 반경)

    }

}
