using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "Scriptable Data/Variables/Render Pass Event")]
public class SRPEventVariable : STypeVariable<RenderPassEvent>
{
    public new RenderPassEvent Value;
}
