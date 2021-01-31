public class CMath
{
    public static float ScaleRange(float value, float sourceMin, float sourceMax, float targetMin, float targetMax)
    {
        return (((targetMax - targetMin) * (value - sourceMin)) / (sourceMax - sourceMin)) + targetMin;
    }

}
