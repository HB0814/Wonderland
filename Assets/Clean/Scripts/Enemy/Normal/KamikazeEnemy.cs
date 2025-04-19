using UnityEngine;

public class KamikazeEnemy : Enemy
{
    [Header("자폭 설정")]
    public bool isReady = false;
    public float explosionReadyRange = 6.0f; //폭발 준비 시작 범위
    public float explosionMoveSpeed = 3.0f; //폭발 준비 시 이동속도
    public float explosionRange = 1.0f; //폭발  범위
    public float explosionDamage = 50.0f; //폭발 데미지
    public GameObject explosionEffect; //폭발 이펙트 -> 게임오브젝트 대신 파티클시스템 사용할수도
    public GameObject center;

    protected override void Update()
    {
        base.Update();

        float dis = Vector2.Distance(transform.position, player.transform.position); //거리 계산
        
        // 폭발 범위 안에 들어오면 자폭
        if (dis <= explosionReadyRange && !isReady)
        {
            isReady = true;
            ExplodeReaby(); //자폭 준비
        }
    }

    //폭발 애니메이션 재생 -> 인보크로 일정 시간 뒤 폭발 -> 폭발 시 데미지
    void ExplodeReaby()
    {
        //폭발 애니메이션 실행
        animator.SetTrigger("isReady");
        moveSpeed = explosionMoveSpeed; //이동속도 변경

        Invoke(nameof(Explode), 2.5f); //2.5초 뒤 폭발
    }

    void Explode()
    {
        Debug.Log("자폭!!!");
        // 폭발 이펙트 생성 or 폭발 이펙트 재생
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 폭발 범위 내의 플레이어 검출
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRange);
        foreach (Collider2D hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Player"))
            {
                if (_player != null)
                {
                    _player.TakeDamage(explosionDamage); //폭발 데미지
                }
            }
        }

        //자폭 후 제거, 비활성화
        //Destroy(gameObject);

        //animator.ResetTrigger("Explode"); //애니메이션 트리거 초기화
        gameObject.SetActive(false); //오브젝트 비활성화
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        isReady = false;
        animator.ResetTrigger("isReady");
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(center.transform.position, explosionRange);
    //}

} 