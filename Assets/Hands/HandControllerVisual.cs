using UnityEngine;
using UnityEngine.InputSystem;

public class HandControllerVisual : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private InputActionProperty gripValue;
    [SerializeField] private InputActionProperty triggerValue;
    [SerializeField] private float grabThreshold = 0.5f;
    [SerializeField] private float shootThreshold = 0.5f;

    public bool hasGun;

    private bool grabbing;
    private bool shooting;

    void OnEnable()
    {
        if (gripValue.action != null) gripValue.action.Enable();
        if (triggerValue.action != null) triggerValue.action.Enable();
    }

    void OnDisable()
    {
        if (gripValue.action != null) gripValue.action.Disable();
        if (triggerValue.action != null) triggerValue.action.Disable();
    }

    void Update()
    {
        float grip = gripValue.action != null
            ? gripValue.action.ReadValue<float>()
            : 0f;

        float trigger = triggerValue.action != null
            ? triggerValue.action.ReadValue<float>()
            : 0f;

        grabbing = grip > grabThreshold;
        shooting = hasGun && trigger > shootThreshold;

        if (!animator) return;

        animator.SetBool("grabbing", grabbing);
        animator.SetBool("hasGun", hasGun);
        animator.SetBool("shooting", shooting);
    }

    public void SetHasGun(bool value)
    {
        hasGun = value;
    }
}
