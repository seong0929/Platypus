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
        // Summon이 못 나가게
        if (collision.gameObject.CompareTag("Summon"))
        {

        }
        // Summon을 제외한 모든 애들은 삭제
        else
        {
            Destroy(collision.gameObject);
        }
    }
}