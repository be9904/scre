using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Data/Variables/Text")]
public class STextVariable : ScriptableObject
{
    [TextArea(minLines: 7, maxLines: 15)]
    public string Value;

    public void SetValue(string value)
    {
        Value = value;
    }

    public void SetValue(STextVariable value)
    {
        Value = value.Value;
    }
}