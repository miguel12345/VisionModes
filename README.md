# Vision modes

Note : All of the scripts/shader/etc developed by me can be found under the `Assets/Miguel Ferreira` folder

## Night Vision

I started the implementation of the night vision mode by using a post-processing effect that would calculate the luminance for each pixel, and remove the red and blue components.

This solution worked fine but it didn't give good results for very dark places, where the luminance was 0 or close to 0. In order to make up for this lack of color information from the final image, I've used the diffuse texture from the g-buffer (__CameraGBufferTexture0_). The _Strength_ param controls the effect of the g-buffer color in the final color value.

### Problems and possible improvements
While simple and effective, this current solution shows some signs of color banding for very dark places, where dealing with very small luminance values. This would improve If I applied some kind of dithering.

Also, there is no way to control the night vision strength for a particular object in the world. This could be implemented by creating a "extra luminance" texture where we could draw a given set of elements. This texture could then be used to add more luminance to the formula.





