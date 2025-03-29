using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 오브젝트 풀링 시스템
/// 자주 생성/파괴되는 게임 오브젝트들을 재사용하여 성능을 최적화
/// </summary>
public class ObjectPool : MonoBehaviour
{
    /// <summary>
    /// 싱글톤 인스턴스 - 전역적으로 접근 가능한 단일 객체
    /// </summary>
    public static ObjectPool Instance { get; private set; }

    /// <summary>
    /// 풀링할 오브젝트의 정보를 담는 구조체
    /// </summary>
    [System.Serializable]
    public class Pool
    {
        public string tag;              // 풀 식별자
        public GameObject prefab;        // 생성할 프리팹
        public int size;                // 풀 초기 크기
        public int maxSize = 100;       // 최대 풀 크기
        public bool autoExpand = true;   // 자동 확장 여부
    }

    [Header("Enemy Pools")]
    [SerializeField] private Pool[] enemyPools;

    [Header("Projectile Pools")]
    [SerializeField] private Pool[] projectilePools;

    [Header("Effect Pools")]
    [SerializeField] private Pool[] effectPools;

    // 태그별 오브젝트 풀 딕셔너리
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<string, Pool> poolInfoDictionary;

    /// <summary>
    /// 초기화 - 싱글톤 설정 및 풀 생성
    /// </summary>
    private void Awake()
    {
        InitializeSingleton();
        InitializePools();
    }

    private void InitializeSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        poolInfoDictionary = new Dictionary<string, Pool>();

        // 모든 풀 정보를 하나의 배열로 통합
        var allPools = new List<Pool>();
        allPools.AddRange(enemyPools);
        allPools.AddRange(projectilePools);
        allPools.AddRange(effectPools);

        // 각 풀 초기화
        foreach (var pool in allPools)
        {
            if (pool == null) continue;
            InitializePool(pool);
        }
    }

    /// <summary>
    /// 개별 풀 초기화 - 지정된 크기만큼 오브젝트 생성
    /// </summary>
    private void InitializePool(Pool pool)
    {
        if (pool.prefab == null || string.IsNullOrEmpty(pool.tag))
        {
            Debug.LogWarning($"[{nameof(ObjectPool)}] Invalid pool configuration for tag: {pool.tag}");
            return;
        }

        if (poolDictionary.ContainsKey(pool.tag))
        {
            Debug.LogWarning($"[{nameof(ObjectPool)}] Pool with tag {pool.tag} already exists!");
            return;
        }

        Queue<GameObject> objectPool = new Queue<GameObject>();

        // 풀 크기만큼 오브젝트 미리 생성
        for (int i = 0; i < pool.size; i++)
        {
            CreateNewObject(pool, objectPool);
        }

        poolDictionary.Add(pool.tag, objectPool);
        poolInfoDictionary.Add(pool.tag, pool);
    }

    private void CreateNewObject(Pool pool, Queue<GameObject> objectPool)
    {
        GameObject obj = Instantiate(pool.prefab);
        obj.SetActive(false);
        objectPool.Enqueue(obj);
        obj.transform.SetParent(transform);
    }

    /// <summary>
    /// 풀에서 오브젝트를 가져와 활성화
    /// </summary>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"[{nameof(ObjectPool)}] Pool with tag {tag} doesn't exist.");
            return null;
        }

        Queue<GameObject> objectPool = poolDictionary[tag];
        Pool poolInfo = poolInfoDictionary[tag];

        // 풀이 비어있고 자동 확장이 활성화된 경우 새로 생성
        if (objectPool.Count == 0 && poolInfo.autoExpand && objectPool.Count < poolInfo.maxSize)
        {
            CreateNewObject(poolInfo, objectPool);
        }

        // 오브젝트가 없는 경우 null 반환
        if (objectPool.Count == 0)
        {
            Debug.LogWarning($"[{nameof(ObjectPool)}] Pool {tag} is empty and cannot expand further.");
            return null;
        }

        // 오브젝트 활성화 및 위치/회전 설정
        GameObject objectToSpawn = objectPool.Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        // IPooledObject 인터페이스 구현체 초기화
        if (objectToSpawn.TryGetComponent<IPooledObject>(out var pooledObj))
        {
            pooledObj.OnObjectSpawn();
        }

        return objectToSpawn;
    }

    /// <summary>
    /// 오브젝트를 풀로 반환
    /// </summary>
    public void ReturnToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"[{nameof(ObjectPool)}] Pool with tag {tag} doesn't exist.");
            return;
        }

        if (objectToReturn == null)
        {
            Debug.LogWarning($"[{nameof(ObjectPool)}] Attempted to return null object to pool {tag}");
            return;
        }

        objectToReturn.SetActive(false);
        poolDictionary[tag].Enqueue(objectToReturn);
    }

    /// <summary>
    /// 특정 풀의 현재 크기 반환
    /// </summary>
    public int GetPoolSize(string tag)
    {
        return poolDictionary.ContainsKey(tag) ? poolDictionary[tag].Count : 0;
    }

    /// <summary>
    /// 특정 풀의 최대 크기 반환
    /// </summary>
    public int GetPoolMaxSize(string tag)
    {
        return poolInfoDictionary.ContainsKey(tag) ? poolInfoDictionary[tag].maxSize : 0;
    }

    /// <summary>
    /// 특정 풀의 자동 확장 여부 설정
    /// </summary>
    public void SetPoolAutoExpand(string tag, bool autoExpand)
    {
        if (poolInfoDictionary.ContainsKey(tag))
        {
            poolInfoDictionary[tag].autoExpand = autoExpand;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
} 