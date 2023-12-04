using UnityEngine;

public class FieldLimit : MonoBehaviour
{
    private BoxCollider2D _boxCollider;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        if (_boxCollider == null)
        {
            _boxCollider = gameObject.AddComponent<BoxCollider2D>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Summon�� �� ������
        if (collision.gameObject.CompareTag("Summon"))
        {

        }
        // Summon�� ������ ��� �ֵ��� ����
        else
        {
            Destroy(collision.gameObject);
        }
    }
}