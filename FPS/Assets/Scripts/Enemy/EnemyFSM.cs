using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damage,
        Die,
    }
    EnemyState m_State;

    public float findDistance = 8f;

    Transform player;

    public float attackDistance = 2f;

    public float moveSpeed = 5f;

    CharacterController cc;
    NavMeshAgent agent;
    public Animator ani;

    float currentTime = 0;
    float attackDelay = 2f;

    public int attackPower = 3;

    Vector3 originPos;
    Vector3 originRot;
    public float moveDistance = 20f;

    public int hp = 15;
    private int maxHp = 15;
    public Slider hpBar;

    private void Start()
    {
        m_State = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        agent = GetComponent<NavMeshAgent>();
        cc = gameObject.GetComponent<CharacterController>();
        ani = gameObject.GetComponentInChildren<Animator>();

        originPos = transform.position;
        originRot = transform.eulerAngles;
        hp = maxHp;
    }

    private void Update()
    {
        switch (m_State)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move();
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damage:
                Damage();
                break;
            case EnemyState.Die:
                Die();
                break;
        }
        hpBar.value = (float)hp / (float)maxHp;
    }
    void Idle()
    {
        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            ani.SetTrigger("IdleToMove");
            print("���� ��ȯ : Idle -> Move");
        }
    }
    void Move()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        cc.Move(dir * moveSpeed * Time.deltaTime);

        //�̵� �������� �ٶ󺸰�
        transform.forward = dir;

        //ùŸ�� ������
        currentTime = attackDelay;

        if (Vector3.Distance(transform.position, originPos) > moveDistance)
        {
            m_State = EnemyState.Return;
            print("���� ��ȯ : move -> return");
        }
        else if (Vector3.Distance(player.position, transform.position) < attackDistance)
        {
            ani.SetTrigger("MoveToAttackDelay");
            m_State = EnemyState.Attack;
        }
    }
    void Attack()
    {

        if (Vector3.Distance(transform.position,player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if(currentTime > attackDelay)
            {
                print("����");
                ani.SetTrigger("StartAttack"); //�ִϸ��̼� �̺�Ʈ���� ���� ����
            }
        }
        else
        {
            m_State = EnemyState.Move;
            ani.SetTrigger("AttackToMove");
            print("������ȯ : attack -> move");
            currentTime = 0;
        }
    }
    public void AttackAction()
    {
        player.GetComponent<PlayerMove>().DamageAction(attackPower);
        currentTime = 0;
    }
    void Return()
    {
        Vector3 dir = (originPos - transform.position).normalized;
        cc.Move(dir * moveSpeed * Time.deltaTime);
        //�̵� �������� �ٶ󺸰�
        transform.forward = dir;
        if (Vector3.Distance(originPos, transform.position) < 0.1f)
        {
            transform.position = originPos;
            transform.rotation = Quaternion.Euler(originRot);

            hp = maxHp;
            ani.SetTrigger("MoveToIdle");
            m_State = EnemyState.Idle;
        }
    }

    void Damage()
    {
        StartCoroutine(DamageProcess());
    }
    IEnumerator DamageProcess()
    {
        yield return new WaitForSeconds(0.5f);

        m_State = EnemyState.Move;
        print("������ȯ : Damaged -> Move");
    }
    public void HitEnemy(int hitPower)
    {
        if (m_State == EnemyState.Damage || m_State == EnemyState.Die || m_State == EnemyState.Return)
        {
            return;
        }

        hp -= hitPower;
        if(hp > 0)
        {
            m_State = EnemyState.Damage;
            print("������ȯ : Any state -> Damage");
            Damage();
        }
        else
        {
            m_State = EnemyState.Die;
            print("���� ��ȯ : Any state -> Die");
            ani.SetTrigger("Die");
            Die();
        }
    }
    void Die()
    {
        StopAllCoroutines();
        StartCoroutine(DieProcess());
    }
    IEnumerator DieProcess()
    {
        cc.enabled = false;

        yield return new WaitForSeconds(2f);
        print("�Ҹ�");
        Destroy(gameObject);
    }
}
