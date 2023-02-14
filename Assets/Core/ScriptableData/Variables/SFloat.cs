using System;

[Serializable]
public class SFloat
{
    public bool isConstant = true;
    public float ConstantValue;
    public SFloatVariable Variable;

    public SFloat()
    { }

    public SFloat(float value)
    {
        isConstant = true;
        ConstantValue = value;
    }

    public float Value
    {
        get { return isConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator float(SFloat reference)
    {
        return reference.Value;
    }
}
