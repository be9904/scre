using UnityEngine;

public class STypeVariable<T> : ScriptableObject
{
    public T Value;

    public void SetValue(T value)
    {
        Value = value;
    }

    public void SetValue(STypeVariable<T> value)
    {
        Value = value.Value;
    }
}