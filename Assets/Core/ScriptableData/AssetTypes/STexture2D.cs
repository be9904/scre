using System;
using UnityEngine;

[Serializable]
public class STexture2D
{
    public STexture2DAsset Variable;

    public static implicit operator Texture2D(STexture2D reference)
    {
        return reference.Variable.texture;
    }
}