using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjectController : MonoBehaviour
{
    [SerializeField] private Transform leftPivot;
    [SerializeField] private Transform rightPivot;
    [SerializeField] private Transform forwardPivot;
    [SerializeField] private Transform backPivot;

    private float absX;
    private float absZ;

    private void Awake()
    {
        absX = Mathf.Abs(leftPivot.localPosition.x) + Mathf.Abs(rightPivot.localPosition.x);
        absZ = Mathf.Abs(forwardPivot.localPosition.z) + Mathf.Abs(backPivot.localPosition.z);
    }
    private void FixedUpdate()
    {
        if(InGameManager.Instance.Player.transform.position.x < leftPivot.position.x)
        {
            MoveLevel(Vector3.left, absX);
            return;
        }
        if(InGameManager.Instance.Player.transform.position.x > rightPivot.position.x)
        {
            MoveLevel(Vector3.right, absX);
            return;
        }
        if (InGameManager.Instance.Player.transform.position.z > forwardPivot.position.z)
        {
            MoveLevel(Vector3.forward, absZ);
            return;
        }
        if (InGameManager.Instance.Player.transform.position.z < backPivot.position.z)
        {
            MoveLevel(Vector3.back, absZ);
            return;
        }
    }
    private void MoveLevel(Vector3 direction, float value)
    {
        transform.position += direction * value; 
    }
}
