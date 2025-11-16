using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static Action OnBulletFired;
    public static Action OnBulletDestroyed;

    public float mouseSensitivity = 100f;
    public Transform playerBody;

    [Header("Rotation Limits")]
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    [Header("Bullet Fire")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed;
    public float fireRate;

    private float nextFireTime = 0f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    private bool isMobile;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }

    void Update()
    {
        // --- FIRE ---
        if (!isMobile)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireRate;
            }
        }

        // --- CAMERA ROTATION ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation += mouseX;

        xRotation = Mathf.Clamp(xRotation, minX, maxX);
        yRotation = Mathf.Clamp(yRotation, minY, maxY);

        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    public void OnMobileFireButton() // call from UI button
    {
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Fire()
    {
        GunScope scope = FindAnyObjectByType<GunScope>();
        if (scope != null && scope.isScoped)
        {
            scope.DisableScope();
        }

        OnBulletFired?.Invoke();

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * bulletSpeed;

        BulletDestroy bulletController = bullet.AddComponent<BulletDestroy>();

        CamerafollowBUlleu cameraSwitcher = FindAnyObjectByType<CamerafollowBUlleu>();
        if (cameraSwitcher != null)
        {
            cameraSwitcher.FollowBullet(bullet.transform);
        }
    }
}
