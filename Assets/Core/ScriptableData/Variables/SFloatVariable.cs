using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Data/Variables/Float")]
public class SFloatVariable : ScriptableObject
{
    public float Value;

    public void SetValue(float value)
    {
        Value = value;
    }

    public void SetValue(SFloatVariable value)
    {
        Value = value.Value;
    }
}
