using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("타겟 설정")]
    [SerializeField] private Transform target; // 따라갈 대상 (플레이어)
    
    [Header("카메라 설정")]
    [SerializeField] private float smoothSpeed = 5f; // 카메라 이동 부드러움
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10); // 카메라와 타겟의 거리
    
    [Header("제한 설정")]
    [SerializeField] private bool useConstraints = false;
    [SerializeField] private Vector2 xConstraints = new Vector2(-10f, 10f);
    [SerializeField] private Vector2 yConstraints = new Vector2(-5f, 5f);
    
    private void LateUpdate()
    {
        if (target == null) return;
        
        // 목표 위치 계산
        Vector3 desiredPosition = target.position + offset;
        
        // 제한이 활성화된 경우 위치 제한 적용
        if (useConstraints)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, xConstraints.x, xConstraints.y);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, yConstraints.x, yConstraints.y);
        }
        
        // 부드러운 이동
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        
        // 항상 타겟을 바라보도록 설정
        transform.LookAt(target);
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
    }
} 