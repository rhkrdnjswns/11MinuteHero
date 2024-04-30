using UnityEngine;

public class FireStormObject : WispObject
{
    public override void IncreaseSize(float value)
    {
        foreach (var child in GetComponentsInChildren<Transform>())
        {
            child.localScale += Vector3.one * value;
            child.localPosition += Vector3.up * (value / 2);
        }
    }
}
