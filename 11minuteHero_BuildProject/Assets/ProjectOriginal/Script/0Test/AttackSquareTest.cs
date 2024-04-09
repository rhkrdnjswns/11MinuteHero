using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSquareTest : MonoBehaviour
{
    [SerializeField] private Vector3 size;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] overlap = Physics.OverlapBox(transform.position, size);
        foreach(var item in overlap)
        {
            if(item.GetComponent<Character>())
            {
                Debug.Log("¹üÀ§ ¾È");
            }
        }
    }
}
