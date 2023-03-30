using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//소환수들의 공통적인 속성과 메서드를 다루는 스크립트
public class Summon : MonoBehaviour
{
    //ToDo: GameManager를 통해 픽 된 캐릿터 스탯 가져오기
    private float[] stats = { 10f, 0.5f };  //임시 스탯 이동속도와 사거리
    //ToDo: GameManager에서 팀 판별 초기화
    private bool myteam;
    private void Update()
    {
        GameObject opponent = SearchOpponent();
        MoveForOpponent(opponent);
    }
    GameObject SearchOpponent()
    {
        GameObject[] opponents = GameObject.FindGameObjectsWithTag("Summon");
        float nearestdistance = Mathf.Infinity;
        GameObject nearestOpponent = opponents[0];
        
        //ToDo: 팀 판별
        foreach (GameObject opponent in opponents) {
            float distance = Vector2.Distance(transform.position, opponent.transform.position);
            if (nearestdistance>distance)
            {
                nearestdistance = distance;
                nearestOpponent = opponent;
            }
        }
        return nearestOpponent;
    }
    void MoveForOpponent(GameObject opponent)
    {
        transform.position = Vector2.MoveTowards(transform.position, opponent.transform.position, stats[0] * Time.deltaTime);
    }
}
