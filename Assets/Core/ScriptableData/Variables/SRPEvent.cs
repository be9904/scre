using System;
using UnityEngine.Rendering.Universal;

[Serializable]
public class SRPEvent
{
    public bool isConstant = true;
    public RenderPassEvent ConstantValue;
    public SRPEventVariable Variable;

    public RenderPassEvent Value
    {
        get { return isConstant ? ConstantValue : Variable.Value; }
    }
}
