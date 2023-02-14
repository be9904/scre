using System;

[Serializable]
public class SBool
{
    public bool isConstant = true;
    public bool ConstantValue;
    public SBoolVariable Variable;

    public SBool()
    { }

    public SBool(bool value)
    {
        isConstant = true;
        ConstantValue = value;
    }

    public bool Value
    {
        get { return isConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator bool(SBool reference)
    {
        return reference.Value;
    }
}