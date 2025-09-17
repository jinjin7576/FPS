using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpPower = 10f;
    bool isJump = false;

    int hp = 100;

    CharacterController cc;

    //중력 변수
    float gravity = -20f;
    //수직 속력 변수
    float yVelocity = 0;
    private void Start()
    {
        cc = gameObject.GetComponent<CharacterController>();
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        dir = dir.normalized;

        //로컬 좌표 이동
        dir = Camera.main.transform.TransformDirection(dir);

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        //로컬로 바꿔준 방향을 넣어줌
        cc.Move(dir * moveSpeed * Time.deltaTime);
        if (isJump && cc.collisionFlags == CollisionFlags.Below)
        {
            isJump = false;
            //캐릭터 수직 속도를 0으로 만들기 (누적되는 수적속도 문제 해결)
            yVelocity = 0;
        }
        if (Input.GetButtonDown("Jump") && !isJump)
        {
            yVelocity = jumpPower;
            isJump = true;
        }
    }

    public void DamageAction(int damage)
    {
        hp -= damage;
    }
}
