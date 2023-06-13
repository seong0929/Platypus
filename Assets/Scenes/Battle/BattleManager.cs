using UnityEngine;
using TMPro;

public class BattleManager : MonoBehaviour
{
    //Singleton Instance ����
    public static BattleManager instance = null;

    [Header("# Game Control")]
    public float GameTime;  // ��� �ð�
    public float MaxGameTime = Constants.Play_TIME;   //�����ð�
    public TMP_Text TimerText;       // Ÿ�̸� UI
    
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        GameTime += Time.deltaTime;
        if (GameTime > MaxGameTime)
        {
            // ToDo: ���� ����
        }
        UpdateTimerUI();
    }
    //�Ͻ�����
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    // �����
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    //ToDo: ��Ʋ������ �Ѿ�� ��ȯ���� �μ��� �ֱ�
    public void AssignTeam(GameObject[] summons)
    {
        //ToDo: ��Ʋ������ user�� �����ߴ� �� �� �ߴ� �� �Ǵ��ϴ� �Լ� �ʿ�
        foreach (GameObject summon in summons)
        {
            //if() ��Ʋ������ ������ �ߴٸ�
            summon.GetComponent<Summon>().MyTeam = true;
            //else��Ʋ������ ������ ���� �ʾ�����, �Ѿ� �� ���
            summon.GetComponent<Summon>().MyTeam = false;
        }
    }
    // UI�� Ÿ�̸� ���� ǥ��
    private void UpdateTimerUI()
    {
        float remainingTime = Mathf.Max(MaxGameTime - GameTime, 0f);

        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);

        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
