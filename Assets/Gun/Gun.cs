using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Gun : MonoBehaviour
{
    public enum FireMode
    {
        Semi,
        Auto
    }

    public Transform muzzle;
    public GameObject projectilePrefab;
    public float muzzleVelocity = 30f;
    public FireMode fireMode = FireMode.Semi;
    public float fireRate = 8f;
    [Range(0f, 1f)] public float hapticStrength = 0.6f;
    public float hapticDuration = 0.06f;

    XRGrabInteractable grabInteractable;
    HandControllerVisual handVisual;
    XRBaseInputInteractor inputInteractorForHaptics;

    bool isHeld;
    bool triggerHeld;
    float nextTimeCanShoot;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
        grabInteractable.activated.AddListener(OnTriggerPressed);
        grabInteractable.deactivated.AddListener(OnTriggerReleased);
    }

    void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
        grabInteractable.activated.RemoveListener(OnTriggerPressed);
        grabInteractable.deactivated.RemoveListener(OnTriggerReleased);
    }

    void Update()
    {
        if (fireMode == FireMode.Auto && isHeld && triggerHeld)
        {
            TryShoot();
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
        inputInteractorForHaptics = args.interactorObject as XRBaseInputInteractor;
        handVisual = args.interactorObject.transform.GetComponentInParent<HandControllerVisual>();
        if (handVisual != null)
        {
            handVisual.SetHasGun(true);
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        triggerHeld = false;

        inputInteractorForHaptics = null;

        if (handVisual != null)
        {
            handVisual.SetHasGun(false);
            handVisual = null;
        }
    }

    void OnTriggerPressed(ActivateEventArgs args)
    {
        triggerHeld = true;

        if (fireMode == FireMode.Semi)
        {
            TryShoot();
        }
    }

    void OnTriggerReleased(DeactivateEventArgs args)
    {
        triggerHeld = false;
    }

    void TryShoot()
    {
        if (Time.time < nextTimeCanShoot) return;
        if (muzzle == null || projectilePrefab == null) return;

        GameObject projectile = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(muzzle.forward * muzzleVelocity, ForceMode.VelocityChange);
        }

        SendHaptics();

        nextTimeCanShoot = Time.time + (1f / fireRate);
    }

    void SendHaptics()
    {
        if (inputInteractorForHaptics == null) return;
        if (hapticStrength <= 0f || hapticDuration <= 0f) return;

        inputInteractorForHaptics.SendHapticImpulse(hapticStrength, hapticDuration);
    }
}
