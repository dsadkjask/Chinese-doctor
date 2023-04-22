using UnityEngine;

public class DayNightController : MonoBehaviour
{
    public Light directionalLight;
    public Gradient lightColorGradient;
    public float dayCycleDuration = 60.0f;
    [Range(0, 1)]
    public float currentTimeOfDay = 0.5f;

    private float lightIntensity;
    private Color lightColor;
    private float timeOfDay;

    void Start()
    {
        lightIntensity = directionalLight.intensity;
        lightColor = directionalLight.color;
    }

    void Update()
    {
        // 根据时间计算光线的强度和颜色
        timeOfDay += Time.deltaTime / dayCycleDuration;

        if (timeOfDay >= 1.0f)
        {
            timeOfDay = 0.0f;
        }

        float alpha = Mathf.Clamp01(1.0f - Mathf.Abs(currentTimeOfDay - timeOfDay) * 2.0f);
        directionalLight.intensity = lightIntensity * alpha;
        directionalLight.color = lightColorGradient.Evaluate(timeOfDay);

        // 调整环境光
        RenderSettings.ambientIntensity = directionalLight.intensity;
        RenderSettings.ambientSkyColor = directionalLight.color;
        RenderSettings.ambientGroundColor = directionalLight.color;
    }
}


