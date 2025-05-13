using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("적 원거리 공격 설정")]
    public float speed; //이동 크기
    public float damage; //데미지
    public float lifeTime = 5f; //지속시간
    [SerializeField] private Player player; //플레이어 스크립트
    private Rigidbody2D rb; //리지드바디
    string projectileType; //투사체의 타입

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //활성화 시
    private void OnEnable()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();

        if(player != null)
        {
            Vector2 dir = (player.transform.position - transform.position).normalized; //발사 방향
            rb.AddForce(dir * speed, ForceMode2D.Impulse); //AddForce함수로 순간적인 힘을 가해 발사
        }

        Invoke(nameof(DisableObject), lifeTime); //5초 뒤 투사체 비활성화 함수 실행
    }

    //비활성화 함수
    private void DisableObject()
    {
        gameObject.SetActive(false);
    }

    //비활성화 시
    private void OnDisable()
    {
        rb.linearVelocity = Vector2.zero; //이동 크기 0
        CancelInvoke(); //인보크 취소
    }

    //충돌 시
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")) //플레이어 충돌 시
        {
            player.TakeDamage(damage); //플레이어의 몬스터의 피해량 값을 전달하여 피격 함수 실행
        }
    }
}
