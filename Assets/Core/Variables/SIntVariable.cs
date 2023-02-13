using UnityEngine;

[CreateAssetMenu(menuName = "Scripted Variables/Int")]
public class SIntVariable : ScriptableObject
{
    public int Value;

    public void SetValue(int value)
    {
        Value = value;
    }

    public void SetValue(SIntVariable value)
    {
        Value = value.Value;
    }
}
