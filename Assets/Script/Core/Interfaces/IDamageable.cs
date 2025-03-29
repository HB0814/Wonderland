public interface IDamageable
{
    /// <summary>
    /// 데미지를 받는 메서드
    /// </summary>
    /// <param name="damage">받을 데미지량</param>
    void TakeDamage(float damage);
    
    /// <summary>
    /// 현재 체력을 반환하는 프로퍼티
    /// </summary>
    float CurrentHealth { get; }
    
    /// <summary>
    /// 최대 체력을 반환하는 프로퍼티
    /// </summary>
    float MaxHealth { get; }
    
    /// <summary>
    /// 객체가 살아있는지 여부를 반환하는 프로퍼티
    /// </summary>
    bool IsAlive { get; }
} 