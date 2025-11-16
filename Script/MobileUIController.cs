using UnityEngine;

public class MobileUIController : MonoBehaviour
{
    public GameObject fireButton;
    public GameObject scopeButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        bool isMobile =
            Application.platform == RuntimePlatform.Android ||
            Application.platform == RuntimePlatform.IPhonePlayer;

        fireButton.SetActive(isMobile);
        scopeButton.SetActive(isMobile);
    }
}
