using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    SpriteUpdater spriteUpdater;
    
    public float speed;
    float lastX = 0.0f;
    float lastUpdateTime = 0.0f;

    private void Awake()
    {
        spriteUpdater = GetComponent<SpriteUpdater>();
    }

    private void Update()
    {
        Move(); //이동
        if (Time.time - lastUpdateTime < 0.1f) //0.1초마다 레이어값 변경 함수를 실행하게
            return;

        lastUpdateTime = Time.time;
        spriteUpdater.ChangeLayer(); //레이어값 변경
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x != 0 && x != lastX) //입력 있는지, 입력한 방향이 전과 다른 방향인지 체크
        {
            spriteUpdater.SpriteFlip(x < 0); //스프라이트 렌더러 플립
        }

        Vector3 moveVelocity = new Vector3(x, y, 0).normalized; //노멀라이즈

        this.transform.position += moveVelocity * speed * Time.deltaTime;
        lastX = x;
    }
}
