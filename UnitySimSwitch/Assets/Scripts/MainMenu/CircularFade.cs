using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

/// <summary>
/// Custom UI Image that applies a circular fade effect by modifying the stencil buffer comparison.
/// Overrides the default material rendering to enable unique visual effects.
/// </summary>
public class CircularFade : Image
{
    /// <summary>
    /// Overrides the material used for rendering the image, modifying the stencil comparison function.
    /// Changes the comparison to "Not Equal" for a circular fade effect.
    /// </summary>
    public override Material materialForRendering
    {
        get
        {
            // Create a new material instance to avoid modifying the base material globally.
            Material mat = new Material(base.materialForRendering);

            // Modify the stencil comparison function to "NotEqual" to create a circular fade effect.
            mat.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            
            // Return the modified material for rendering.
            return mat;
        }
    }
}