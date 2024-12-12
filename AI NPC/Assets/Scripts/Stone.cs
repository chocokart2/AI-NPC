using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f; // ���� ���� �ֱ� (��)

    private void OnEnable()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

    }
}
