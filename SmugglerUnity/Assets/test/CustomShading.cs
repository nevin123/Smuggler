using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CustomShading : MonoBehaviour {

    public Material ShadingMaterial;

    private void OnRenderImage(RenderTexture sourceImage, RenderTexture destinationImage)
    {
        Graphics.Blit(sourceImage, destinationImage, ShadingMaterial);
    }
}
