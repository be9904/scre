using System;

[Serializable]
public class SType<T>
{
    public bool isConstant = true;
    public T ConstantValue;
    public STypeVariable<T> Variable;

    public SType()
    { }

    public SType(T value)
    {
        isConstant = true;
        ConstantValue = value;
    }

    public T Value
    {
        get { return isConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator T(SType<T> reference)
    {
        return reference.Value;
    }
}