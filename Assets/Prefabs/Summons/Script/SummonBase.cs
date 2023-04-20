using UnityEngine;

//소환수들의 공통적인 속성과 메서드를 다루는 스크립트
public class SummonBase : MonoBehaviour
{
    public string summonName;
    public GameObject opponent;

    protected CircleCollider2D circleCollider; //사거리

    private void Start()
    {
        circleCollider = GetComponentInChildren<CircleCollider2D>();
    }

    private void FixedUpdate()
    {
        opponent = SearchOpponent();
    }

    GameObject SearchOpponent()
    {
        GameObject[] opponents = GameObject.FindGameObjectsWithTag("Summon");
        float nearestdistance = Mathf.Infinity;
        GameObject nearestOpponent = opponents[0];

        //ToDo: 팀 판별
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
