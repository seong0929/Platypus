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
        if (collision.gameObject.CompareTag("Summon")) // Summon과 충돌 감지
        {
            var summon = collision.gameObject.GetComponent<Summon>();

            if ((summon != null) && (collision.gameObject != this.transform.parent.gameObject)) // 자신을 제외한 summon과 만나면 파괴
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