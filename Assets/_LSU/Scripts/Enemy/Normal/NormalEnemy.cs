using UnityEngine;

public class NormalEnemy : MonoBehaviour
{
    TakeDamage takeDamage;
    SpriteUpdater spriteUpdater;

    GameObject player; //플레이어
    public float range; //추격 범위
    
    float lastUpdateTime = 0; //업데이트 주기
    bool isVisible = true; //화면 내 여부
    float marginArea = 0.1f; //렌더링 여유범위

    public float maxHp; //최대체력
    public float hp; //현재체력
    public float addHp; //추가체력
    public float damage; //데미지
    public float offense; //공격력
    public float defense; //방어력
    public float speed; //이동속도
    public float knockbackDefense; //넉백저항력

    private void Awake()
    {
        takeDamage = GetComponent<TakeDamage>();
        spriteUpdater = GetComponent<SpriteUpdater>();
    }

    private void Update()
    {
        spriteUpdater.ChangeLayer(); //레이어값 변경

        float dis = Vector2.SqrMagnitude(transform.position - player.transform.position); //플레이어와의 거리 계산

        if (dis >= range) //추격 범위와의 비교를 통해 이동 여부
        {
            Move(); //이동 함수 호출
        }

        if (Time.time - lastUpdateTime < 0.1f) //0.1초마다 레이어값 변경 함수를 실행하게
            return;

        lastUpdateTime = Time.time; //마지막 업데이트 시간 변경

        spriteUpdater.spriteRenderer.enabled = IsVisible(); //렌더링 함수
        //현재 방향과 다를 때만 플립하게
        bool isFlip = (player.transform.position.x < transform.position.x) != spriteUpdater.spriteRenderer.flipX;
        if (isFlip)
            spriteUpdater.SpriteFlip(player.transform.position.x < transform.position.x); //스프라이트 렌더러 플립

    }

    //이동
    void Move()
    {
        Vector3 dir = (player.transform.position - transform.position).normalized; //노멀라이즈화를 통한 방향 벡터 구하기
        transform.position += dir * speed * Time.deltaTime; //해당 방향으로 이동
    }

    //렌더링 기능
    bool IsVisible()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position); //월드 좌표를 뷰포트로 변환하여 범위 내 오브젝트 위치 확인
        bool newVisibility = screenPoint.x > -marginArea && screenPoint.x < 1 + marginArea &&
                             screenPoint.y > -marginArea && screenPoint.y < 1 + marginArea;
                             //화면 내에 있는지 여부 확인

        if (isVisible != newVisibility) //화면 내 여부 변경 확인
        {
            isVisible = newVisibility; //상태 업데이트
            spriteUpdater.spriteRenderer.enabled = isVisible; //렌더러 상태 변경
        }

        return isVisible; //상태 반환
    }

    //플레이어 할당
    public void SetPlayer(GameObject _player)
    {
        player = _player;
    }

}
