using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LookAt : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    private Transform target;

    void Start()
    {
        target = _mainCamera.transform;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;

        direction.y = 0;

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(-direction);
        }
    }
}
