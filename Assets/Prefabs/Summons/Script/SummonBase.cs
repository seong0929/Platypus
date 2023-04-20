using UnityEngine;

//��ȯ������ �������� �Ӽ��� �޼��带 �ٷ�� ��ũ��Ʈ
public class SummonBase : MonoBehaviour
{
    public string summonName;
    public GameObject opponent;

    protected CircleCollider2D circleCollider; //��Ÿ�

    private void Start()
    {
        circleCollider = GetComponentInChildren<CircleCollider2D>();
    }

    protected GameObject SearchOpponent()
    {
        GameObject[] opponents = GameObject.FindGameObjectsWithTag("Summon");
        if (opponents.Length == 0)
        {
            return null;
        }
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
}