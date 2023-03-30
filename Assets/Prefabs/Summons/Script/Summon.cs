using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//��ȯ������ �������� �Ӽ��� �޼��带 �ٷ�� ��ũ��Ʈ
public class Summon : MonoBehaviour
{
    //ToDo: GameManager�� ���� �� �� ĳ���� ���� ��������
    private float[] stats = { 10f, 0.5f };  //�ӽ� ���� �̵��ӵ��� ��Ÿ�
    //ToDo: GameManager���� �� �Ǻ� �ʱ�ȭ
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
        
        //ToDo: �� �Ǻ�
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
