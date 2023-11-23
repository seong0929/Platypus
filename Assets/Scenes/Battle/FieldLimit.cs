using UnityEngine;

public class FieldLimit : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    private BoxCollider2D _boxCollider;
    private Vector3 initialPosition;

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        if (_boxCollider == null)
        {
            _boxCollider = gameObject.AddComponent<BoxCollider2D>();
        }

        AdjustColliderToCanvasSize();

        initialPosition = new Vector3(0f, 0f, 0f);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Summon"))
        {
            BounceOffField(collision.transform);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
    // ����� ĵ������ �°� ũ�� ����
    private void AdjustColliderToCanvasSize()
    {
        if (canvas != null)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();

            _boxCollider.size = new Vector2(canvasRect.sizeDelta.x * (1f / 90f), canvasRect.sizeDelta.y * (1f / 90f));
        }
    }
    // ĳ���Ͱ� ����� ������ ������ �ݴ� �������� ƨ���
    private void BounceOffField(Transform objTransform)
    {
        Vector3 objPosition = objTransform.position;
        Vector2 fieldCenter = _boxCollider.bounds.center;
        Vector2 fieldExtents = _boxCollider.bounds.extents;

        float newX = Mathf.Clamp(objPosition.x, fieldCenter.x - fieldExtents.x, fieldCenter.x + fieldExtents.x);
        float newY = Mathf.Clamp(objPosition.y, fieldCenter.y - fieldExtents.y, fieldCenter.y + fieldExtents.y);

        objTransform.position = new Vector3(newX, newY, objPosition.z);
    }
    private void Update()
    {
        GameObject[] summonCharacters = GameObject.FindGameObjectsWithTag("Summon");

        // ĳ���Ͱ� ������� ����� �� �ʱ� ��ġ�� �ǵ�����
        foreach (var summonCharacter in summonCharacters)
        {
            if (summonCharacter.transform.position.x > _boxCollider.bounds.max.x ||
                summonCharacter.transform.position.x < _boxCollider.bounds.min.x ||
                summonCharacter.transform.position.y > _boxCollider.bounds.max.y ||
                summonCharacter.transform.position.y < _boxCollider.bounds.min.y)
            {
                ReturnToInitialPosition(summonCharacter.transform);
            }
        }
    }
    private void ReturnToInitialPosition(Transform characterTransform)
    {
        characterTransform.position = initialPosition;
    }
}