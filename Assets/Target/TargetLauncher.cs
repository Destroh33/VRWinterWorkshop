using UnityEngine;

public class TargetLauncher : MonoBehaviour
{
    [Header("Target")]
    public Rigidbody targetPrefab;
    public Transform launchPoint;

    [Header("Launch Settings")]
    public float launchForce = 12f;
    public Vector2 randomUpAngleRange = new Vector2(5f, 25f);
    public Vector2 randomFireIntervalRange = new Vector2(0.5f, 2f);
    public bool autoFire = true;
    public float lifetime = 5f;
    float _nextFire;

    void Start()
    {
        ScheduleNextFire();
    }

    void Update()
    {
        if (autoFire && Time.time >= _nextFire)
        {
            Launch();
            ScheduleNextFire();
        }
    }

    void ScheduleNextFire()
    {
        float randomInterval = Random.Range(randomFireIntervalRange.x, randomFireIntervalRange.y);
        _nextFire = Time.time + randomInterval;
    }

    public void Launch()
    {
        if (!targetPrefab || !launchPoint) return;

        float upAngle = Random.Range(randomUpAngleRange.x, randomUpAngleRange.y);
        Quaternion randomTilt = Quaternion.AngleAxis(-upAngle, launchPoint.right);
        Quaternion shotRotation = randomTilt * launchPoint.rotation;
        Rigidbody proj = Instantiate(targetPrefab, launchPoint.position, shotRotation);
        Destroy(proj.gameObject, lifetime);
        proj.linearVelocity = shotRotation * Vector3.forward * launchForce;
    }
}