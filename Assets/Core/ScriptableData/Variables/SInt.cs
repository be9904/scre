using System;

[Serializable]
public class SInt
{
    public bool isConstant = true;
    public int ConstantValue;
    public SIntVariable Variable;

    public SInt()
    { }

    public SInt(int value)
    {
        isConstant = true;
        ConstantValue = value;
    }

    public int Value
    {
        get { return isConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator int(SInt reference)
    {
        return reference.Value;
    }
}
