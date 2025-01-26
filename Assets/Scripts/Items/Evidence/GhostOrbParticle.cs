using UnityEngine;

public class GhostOrbParticle : MonoBehaviour
{
    public float lifetime = 3f; // Lifetime of the particle
    public float movementSpeed = 1f; // Speed of the random movement
    public float angleChangeSpeed = 30f; // Speed at which the angle changes (degrees per second)

    private float currentAngle; // Current angle of movement
    private float targetAngle; // Target angle for smooth adjustment
    private float timeAlive = 0f;

    void Start()
    {
        // Initialize with a random starting angle
        currentAngle = Random.Range(0f, 360f);
        GenerateNewTargetAngle();
    }

    void Update()
    {
        // Update the lifetime
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifetime || GhostOrbs.inUse == false)
        {
            Destroy(gameObject); // Destroy particle after lifetime ends
            return;
        }

        // Smoothly adjust the current angle toward the target angle
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, angleChangeSpeed * Time.deltaTime);

        // Calculate the direction from the current angle
        Vector3 direction = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad), 0f);

        // Move the particle in the calculated direction
        transform.position += direction * movementSpeed * Time.deltaTime;

        // If the current angle is close to the target angle, generate a new target angle
        if (Mathf.Approximately(currentAngle, targetAngle))
        {
            GenerateNewTargetAngle();
        }
    }

    private void GenerateNewTargetAngle()
    {
        // Generate a new random angle as the target (0 to 360 degrees)
        targetAngle = Random.Range(0f, 360f);
    }
}
