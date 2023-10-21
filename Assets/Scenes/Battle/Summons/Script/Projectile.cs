using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Sprite[] Seeds;
    private SpriteRenderer _renderer;
    public float damageAmount;

    private void Start()
    {
        _renderer = this.GetComponent<SpriteRenderer>();
        _renderer.sprite = Seeds[0];
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Summon")) // Summon�� �浹 ����
        {
            var summon = collision.gameObject.GetComponent<Summon>();

            if ((summon != null) && (collision.gameObject != this.transform.parent.gameObject)) // �ڽ��� ������ summon�� ������ �ı�
            {
                summon.TakeDamage(damageAmount);
                Destroy(gameObject);
            }
        }
    }
    public void ChangeTheSprite(int idx)
    {
        _renderer.sprite = Seeds[idx];
    }
}