using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Sprite[] Seeds;
    private SpriteRenderer _renderer;
    private Sprite _curSprite;

    private void Start()
    {
        _renderer = this.GetComponent<SpriteRenderer>();
        _renderer.sprite = Seeds[1];
        _curSprite = _renderer.sprite;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Summon")) // Summon�� �浹 ����
        {
            if (collision.gameObject != this.transform.parent.gameObject) // �ڽ��� ������ summon�� ������ �ı�
            {
                Destroy(gameObject);
            }
        }
    }
    public void ChangeTheSprite()
    {
        if(_curSprite == Seeds[0])
        {
            _renderer.sprite = Seeds[1];
        }
        else
        {
            _renderer.sprite = Seeds[0];
        }
    }
}