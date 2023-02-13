using System;
using UnityEngine.Rendering.Universal;

[Serializable]
public class SRPEvent : SType<RenderPassEvent>
{
    public new bool isConstant = true;
    public new RenderPassEvent ConstantValue;
    public new SRPEventVariable Variable;

    public new RenderPassEvent Value
    {
        get { return isConstant ? ConstantValue : Variable.Value; }
    }
}
