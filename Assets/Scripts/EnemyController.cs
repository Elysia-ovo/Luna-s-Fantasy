using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //�������
    public bool vertical;
    public float speed = 5;
    //�������
    private int direction = 1;
    //����ı�ʱ����
    public float changeTime = 5;
    //��ʱ��
    private float timer;
    //����������ã�Ϊ��ʹ�ø�������ƶ�
    private Rigidbody2D rigidbody2d;
    //����������������ã�Ϊ�˲��Ŷ���
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.enterBattle)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (timer<0)
        {
            direction = -direction;
            timer = changeTime;
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.enterBattle)
        {
            return;
        }
        Vector3 pos = rigidbody2d.position;
        if (vertical)//��ֱ�����ƶ�
        {
            animator.SetFloat("LookX",0);
            animator.SetFloat("LookY", direction);
            pos.y = pos.y + speed * direction * Time.fixedDeltaTime;
        }
        else//ˮƽ�����ƶ�
        {
            animator.SetFloat("LookX", direction);
            animator.SetFloat("LookY", 0);
            pos.x = pos.x + speed * direction * Time.fixedDeltaTime;
        }
        rigidbody2d.MovePosition(pos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Luna"))
        {
            GameManager.Instance.EnterOrExitBattle();
            GameManager.Instance.SetMonster(gameObject);
        }
    }
}