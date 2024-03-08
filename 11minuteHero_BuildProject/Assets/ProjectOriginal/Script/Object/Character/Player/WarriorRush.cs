using UnityEngine;

public class WarriorRush : MonoBehaviour
{
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }

    public void Activate(bool isActive)
    {
        boxCollider.enabled = isActive;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(ConstDefine.TAG_MONSTER))
        {
            other.GetComponent<Monster>().KnockBack(1.5f, 1f);
            //∏ÛΩ∫≈Õ ≥ÀπÈ
        }
    }
}
