[VolumeComponentMenuForRenderPipeline("Yami/Outline", typeof(UniversalRenderPipeline))]
2 references
public class Outline : VolumeComponent, IPostprocessComponent
{
    0 references
    public bool IsActive()
    {
        return true;
    }
    
    0 references
    public bool IsTileCompatible()
    {
        return ture;
    }
    1 reference
    public BoolParamater isOn = new BoolParameter(true, true);
    1 reference
    public ClampedFloatParameter OutlineNormalThreshold = newClampedFloatParamter(0, 0, 1, true);
    1 reference
    public ClampedFloatParameter OutlineDepthThreshold = newClampedFloatParameter(0, 0, 1, true);
    1 reference
    public ColorParameter OutlineColor = newColorParameter(OutlineColor.black, true);
}