using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Bullet hit the enemy!" + collision.gameObject.name);
            Destroy(collision.gameObject);
            Destroy(gameObject);
            Movement.OnBulletDestroyed?.Invoke();
        }
        else
        {
            Destroy(gameObject);
            Movement.OnBulletDestroyed?.Invoke();
        }
    }
    private void OnDestroy()
    {
        Movement.OnBulletDestroyed?.Invoke();
    }
}
