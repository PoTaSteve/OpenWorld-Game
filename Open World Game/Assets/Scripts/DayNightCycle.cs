using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0, 1f)]
    public float time;
    public float fullDayLength;
    public float startTime;
    private float timeRate;
    public Vector3 noon;

    public bool canTimeFlow;

    public Material defaultSkybox;
    public Material nightSkybox;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Settings")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    private void Start()
    {
        timeRate = 1f / fullDayLength;
        time = startTime;
    }

    private void Update()
    {
        // Increment time
        if (canTimeFlow)
        {
            time += timeRate * Time.deltaTime;

            if (time >= 1f)
            {
                time = 0f;
            }
        }        

        // Light rotation
        sun.transform.eulerAngles = (time - 0.25f) * noon * 4f;
        moon.transform.eulerAngles = (time - 0.75f) * noon * 4f;

        // Light intensity
        sun.intensity = sunIntensity.Evaluate(time);
        moon.intensity = moonIntensity.Evaluate(time);

        // Change color
        sun.color = sunColor.Evaluate(time);
        moon.color = moonColor.Evaluate(time);

        // Enable - Disable sun
        if (sun.intensity == 0 && sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(false);
            //RenderSettings.skybox = nightSkybox;
            RenderSettings.sun = moon;
        }
        else if (sun.intensity > 0 && !sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(true);
            //RenderSettings.skybox = defaultSkybox;
            RenderSettings.sun = sun;
        }

        // Enable - Disable moon
        if (moon.intensity == 0 && moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(false);
            //RenderSettings.skybox = defaultSkybox;
            RenderSettings.sun = sun;
        }
        else if (moon.intensity > 0 && !moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(true);
            //RenderSettings.skybox = nightSkybox;
            RenderSettings.sun = moon;
        }

        // Lighting and reflection intensity
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }
}
