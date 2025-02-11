using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Sanity : MonoBehaviour
{
    public static Sanity Instance;

    public float sanity = 100f;
    public float decreasePerMinute = 40f;
    private Light2D[] lights;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        lights = Object.FindObjectsOfType<Light2D>();
    }

    private void Update()
    {
        float intensity = GetTotalLightIntensity(transform.position);
        if (intensity > 1)
            intensity = 1;
        sanity -= (1-intensity) * (decreasePerMinute * GameManager.ghost.stats.sanityDrainModifier / 60f) * Time.deltaTime;

        if (GameManager.player.currentRoom == RoomManager.Instance.ghostRoom)
            sanity -= GameManager.ghost.stats.roomSanityDrain * Time.deltaTime;
        if (GameManager.player.currentRoom == GameManager.ghost.currentRoom)
            sanity -= GameManager.ghost.stats.proximitySanityDrain * Time.deltaTime;

        if (sanity < 0)
            sanity = 0;
        if (sanity > 100)
            sanity = 100;
    }

    public float GetTotalLightIntensity(Vector2 point)
    {
        float totalIntensity = 0f;

        foreach (Light2D light in lights)
        {
            if (!light.gameObject.activeSelf) continue;
            if (light.lightType == Light2D.LightType.Global) continue;

            // Check if the light affects the point
            float distance = Vector2.Distance(light.transform.position, point);
            if (distance > light.pointLightOuterRadius) continue;

            // Calculate intensity based on distance and falloff
            float normalizedDistance = Mathf.Clamp01(distance / light.pointLightOuterRadius);
            float falloff = Mathf.Lerp(1f, 0f, normalizedDistance); // Linear falloff
            float intensityAtPoint = light.intensity * falloff;

            totalIntensity += intensityAtPoint;
        }

        return totalIntensity;
    }
}
