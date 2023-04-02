using UnityEngine;

//��ȯ������ �������� �Ӽ��� �޼��带 �ٷ�� ��ũ��Ʈ
public class Summon : MonoBehaviour
{
    //ToDo: GameManager�� ���� �� �� ĳ���� ���� ��������
    private float[] stats = { 0.3f};  //�ӽ� ���� �̵��ӵ�
    //ToDo: GameManager���� �� �Ǻ� �ʱ�ȭ
    private bool myteam;

    private GameObject opponent;

    private CircleCollider2D circleCollider;

    private void Start()
    {
        circleCollider = GetComponentInChildren<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        opponent = SearchOpponent();
        MoveForOpponent();
    }

    GameObject SearchOpponent()
    {
        GameObject[] opponents = GameObject.FindGameObjectsWithTag("Summon");
        float nearestdistance = Mathf.Infinity;
        GameObject nearestOpponent = opponents[0];

        //ToDo: �� �Ǻ�
        foreach (GameObject opponent in opponents)
        {
            if (opponent == gameObject) continue;

            float distance = Vector2.Distance(transform.position, opponent.transform.position);
            if (nearestdistance > distance)
            {
                nearestdistance = distance;
                nearestOpponent = opponent;
            }
        }

        return nearestOpponent;
    }
   
    void MoveForOpponent()
    {
        if (opponent != null)
        {
            float distance = Vector2.Distance(transform.position, opponent.transform.position);

            if (distance <= circleCollider.radius)
            {
                // ��Ÿ� �̳��� ������ �̵��� ����
                return;
            }

            Vector3 direction = (opponent.transform.position - transform.position).normalized;
            transform.position += direction * stats[0] * Time.deltaTime;
        }
    }
}
