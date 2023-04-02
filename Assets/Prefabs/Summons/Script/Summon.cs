using UnityEngine;

//소환수들의 공통적인 속성과 메서드를 다루는 스크립트
public class Summon : MonoBehaviour
{
    //ToDo: GameManager를 통해 픽 된 캐릿터 스탯 가져오기
    private float[] stats = { 0.3f};  //임시 스탯 이동속도
    //ToDo: GameManager에서 팀 판별 초기화
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
   
    void MoveForOpponent()
    {
        if (opponent != null)
        {
            float distance = Vector2.Distance(transform.position, opponent.transform.position);

            if (distance <= circleCollider.radius)
            {
                // 사거리 이내에 있으면 이동을 멈춤
                return;
            }

            Vector3 direction = (opponent.transform.position - transform.position).normalized;
            transform.position += direction * stats[0] * Time.deltaTime;
        }
    }
}
