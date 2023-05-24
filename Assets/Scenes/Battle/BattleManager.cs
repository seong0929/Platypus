using UnityEngine;

public class BattleManager : MonoBehaviour
{
    //Singleton Instance ����
    public static BattleManager instance = null;

    [Header("# Game Control")]
    public float GameTime;
    public float MaxGameTime = Constants.playtime;   //�����ð�
    private void Awake()
    {
        instance = this;
        //Scene �̵� �� ���� ���� �ʵ��� ó��
        //DontDestroyOnLoad(this.gameObject);
    }
    private void Update()
    {
        GameTime += Time.deltaTime;
        if (GameTime > MaxGameTime)
        {
            GameTime = MaxGameTime;
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
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
            summon.GetComponent<Summon>().myTeam = true;
            //else��Ʋ������ ������ ���� �ʾ�����, �Ѿ� �� ���
            summon.GetComponent<Summon>().myTeam = false;
        }
    }
}
