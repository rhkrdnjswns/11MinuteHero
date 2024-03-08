using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour
{
    public Transform targetTr;
    public float rotSpeed;
    public float speed;
    private void Start()
    {
        
        StartCoroutine(Rand());
    }
    private IEnumerator Rand()
    {
        while (true)
        {
            Vector3 dir = targetTr.position - transform.position;
            Quaternion targetRot = Quaternion.LookRotation(dir.normalized, transform.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);
            transform.position += transform.forward * speed * Time.deltaTime;
            yield return InGameManager.Instance.FrameDelay;
        }
    }
    void Update()
    {
        
    }
}
