using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpPower = 10f;
    bool isJump = false;

    CharacterController cc;

    //�߷� ����
    float gravity = -20f;
    //���� �ӷ� ����
    float yVelocity = 0;

    //ü�¹� ui
    public Slider hpBar;
    int maxHp = 20;
    public int hp = 20;

    //�ǰ� ȿ��
    public GameObject hitEffect;
    private void Start()
    {
        cc = gameObject.GetComponent<CharacterController>();
    }
    private void Update()
    {
        hpBar.value = (float)hp / (float)maxHp;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);

        dir = dir.normalized;

        //���� ��ǥ �̵�
        dir = Camera.main.transform.TransformDirection(dir);

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        //���÷� �ٲ��� ������ �־���
        cc.Move(dir * moveSpeed * Time.deltaTime);
        if (isJump && cc.collisionFlags == CollisionFlags.Below)
        {
            isJump = false;
            //ĳ���� ���� �ӵ��� 0���� ����� (�����Ǵ� �����ӵ� ���� �ذ�)
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
        if(hp > 0)
        {
            StartCoroutine(PlayerHitEffect());
        }
    }
    IEnumerator PlayerHitEffect()
    {
        hitEffect.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        hitEffect.SetActive(false);
    }
}
