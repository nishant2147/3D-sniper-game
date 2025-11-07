using System.Collections;
using UnityEngine;

public class CamerafollowBUlleu : MonoBehaviour
{
    [Header("Camera References")]
    public Camera mainCamera;
    public Camera bulletCamera;

    [Header("Follow Settings")]
    public float followDistance;
    public float followHeight;
    public float smoothSpeed;
    public float slowMotionScale;

    private Transform bulletToFollow;
    private bool isFollowing = false;

    void Start()
    {
        mainCamera.enabled = true;
        bulletCamera.enabled = false;
    }

    public void FollowBullet(Transform bullet)
    {
        if (isFollowing) return;

        bulletToFollow = bullet;
        StartCoroutine(FollowRoutine());
    }

    private IEnumerator FollowRoutine()
    {
        isFollowing = true;

        mainCamera.enabled = false;
        bulletCamera.enabled = true;
        Time.timeScale = slowMotionScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale; 

        while (bulletToFollow != null)
        {
            Vector3 desiredPos = bulletToFollow.position - bulletToFollow.forward * followDistance + Vector3.up * followHeight;
            bulletCamera.transform.position = Vector3.Lerp(bulletCamera.transform.position, desiredPos, Time.deltaTime * smoothSpeed);
            bulletCamera.transform.LookAt(bulletToFollow);
            yield return null;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
        bulletCamera.enabled = false;
        mainCamera.enabled = true;

        isFollowing = false;
    }
}
