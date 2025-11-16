using UnityEngine;

public class GunScope : MonoBehaviour
{
    public Animator animator;
    public GameObject scopeOverlay;
    public GameObject gunModel;
    public Camera mainCamera;

    [Header("UI Elements")]
    public GameObject scopeEyepiece;

    [Header("Zoom Settings")]
    public float minFOV = 15f;
    public float maxFOV = 60f;
    public float zoomSpeed = 10f;
    public float scrollSensitivity = 10f;

    public bool isScoped = false;
    private float targetFOV;

    private bool isMobile;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        targetFOV = mainCamera.fieldOfView;
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }

    void Update()
    {
        if (!isMobile)
        {
            if (Input.GetButtonDown("Fire2"))
                ToggleScope();

            if (isScoped)
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll != 0f)
                {
                    targetFOV -= scroll * scrollSensitivity;
                    targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);
                }
            }
            else
            {
                targetFOV = maxFOV;
            }
        }
        else
        {
            // Mobile pinch zoom
            if (isScoped && Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                targetFOV -= difference * 0.05f; // adjust sensitivity
                targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);
            }
        }

        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
    }

    public void OnScopeButton() // for mobile UI button
    {
        ToggleScope();
    }

    public void ToggleScope()
    {
        isScoped = !isScoped;
        animator?.SetBool("Scoped", isScoped);
        gunModel.SetActive(!isScoped);
        scopeOverlay?.SetActive(isScoped);
        scopeEyepiece?.SetActive(isScoped);
    }

    public void DisableScope()
    {
        if (isScoped)
        {
            isScoped = false;
            animator?.SetBool("Scoped", false);
            gunModel.SetActive(true);
            scopeOverlay?.SetActive(false);
            scopeEyepiece?.SetActive(false);
            targetFOV = maxFOV;
        }
    }
}
