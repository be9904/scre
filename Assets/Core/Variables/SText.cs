using System;

[Serializable]
public class SText
{
    public bool isConstant = true;
    public string ConstantValue;
    public STextVariable Variable;

    public SText()
    { }

    public SText(string value)
    {
        isConstant = true;
        ConstantValue = value;
    }

    public string Value
    {
        get { return isConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator string(SText reference)
    {
        return reference.Value;
    }
}
