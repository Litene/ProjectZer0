Wondering how the snow terrain works?

Here's a brief outline of the process:
    Cumulatively recording historic snow prints:
        1)  A top-down orthographic camera ("Snow Prints Camera") culls all but a single layer ("SnowPrintsMarker").
        This single layer is used to identify where the snow is immediately being condensed.
        For instance, a single quad rendering a basic particle may be fixed at the base of the player to create a simple continuous path.
        Particle systems may alternatively be used to create more sophisticated trails of singular footprints.
        The render resulting from this camera is output to a single-channel render texture ("CurrentSnowPrints"), representing the current height-map.

        2)  The render texture representing the current height-map is then passed though a shader ("FactoredLighten").
        The shader applies the lighten blend, equivalent to a max function, on the current height-map and a texture which displays a cumulative collection of all previous height-maps.

        3)  A script ("MaterialDiffuseToRenderTexture") takes the diffuse texture output of the material instance of the shader and copies it to a render texture ("AccumulatedSnowPrints").
        This render texture is the collage of all previous height-maps, which is fed back into the shader as mentioned in step (2).

            Further notes:
                a)  The render texture used for storing the cumulative snow prints is cleared in the Start method.
                
                b)  The render texture used for storing the cumulative snow prints is only amended to in the Update method, and thus only in Play mode.
                
                c)  If these behaviours are not as desired, then the appropriate methods need to be called from elsewhere.

    Interpreting the snow prints texture:
        4)  A plane ("Tessellated Plane") possesses a material which uses a shader supporting tesselation ("Snow").
        The shader first samples the cumulative snow prints texture.

        5)  An appropriate color is then determined based on the sampled texture.

        6)  The appropriate height of the snow is also calculated and applied to the tessellated plane, and normals re-approximated.

------------------------------------------------------------------------------------------------------------------------

Experiencing performance issues?

a)  Try reducing the size of the textures ("AccumulatedSnowPrints" and "CurrentSnowPrints") whilst playing with the filter mode.

b)  Resolve TO DO note left in "MaterialDiffuseToRenderTexture.cs", as this may halve(?) the processing time from material to render texture.
    i)  'Understand why "Graphics.Blit(null, _cumulativeRenderTexture, _material, 0);" alone does not work.'.
