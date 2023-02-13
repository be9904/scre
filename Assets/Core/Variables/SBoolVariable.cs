using UnityEngine;

[CreateAssetMenu(menuName = "Scripted Variables/Bool")]
public class SBoolVariable : ScriptableObject
{
    public bool Value;

    public void SetValue(bool value)
    {
        Value = value;
    }

    public void SetValue(SBoolVariable value)
    {
        Value = value.Value;
    }
}