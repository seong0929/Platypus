using UnityEngine;

public class TestSummon : MonoBehaviour
{
    public float MoveSpeed = 1f;
    public bool RightControl;
    public Vector3 TeleportRange;

    private void Update()
    {
        Move(RightControl);
        Teleport(RightControl);
    }
    private void Move(bool flag)
    {
        if (flag)
        {
            // 화살표
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontal, vertical, 0f) * MoveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }
        else
        {
            // wasd
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 movement = new Vector3(horizontal, vertical, 0f).normalized * MoveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }
    }
    private void Teleport(bool flag)
    {
        // 화살표 조작 시 M으로 순간이동
        if (flag & Input.GetKeyDown(KeyCode.M))
        {
            transform.position += TeleportRange;
        }
        // wasd 조작 시 F로 순간이동
        else if (flag & Input.GetKeyDown(KeyCode.F))
        {
            transform.position += TeleportRange;
        }
    }
}