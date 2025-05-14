using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChessEvent : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private List<GameObject> rooks = new();
    [SerializeField] private List<GameObject> bishops = new();

    [SerializeField] private List<Vector3> rooks_Vec = new();
    [SerializeField] private List<Vector3> bishops_Vec = new();

    [SerializeField] private List<GameObject> rooks_Warn = new();
    [SerializeField] private List<GameObject> bishops_Warn = new();

    private List<List<GameObject>> pieces;
    private List<List<GameObject>> warnings;
    private List<List<Vector3>> positions;

    private int type, num;
    private float lifeTime = 10f;
    private float timer = 0f;
    private bool isSpawn = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        pieces = new List<List<GameObject>> { rooks, bishops };
        warnings = new List<List<GameObject>> { rooks_Warn, bishops_Warn };
        positions = new List<List<Vector3>> { rooks_Vec, bishops_Vec };
    }

    private void Update()
    {
        if (isSpawn)
        {
            timer += Time.deltaTime;
            if (timer >= lifeTime)
                ChessDisable();
        }
    }

    public void Warn()
    {
        isSpawn = true;
        timer = 0f;

        type = Random.Range(0, 2); // 0: ∑Ë, 1: ∫ÒºÛ
        num = Random.Range(0, 6);  // 0~5 ∆–≈œ

        SetWarning(true);
        Invoke(nameof(SetEvent), 1f);
    }

    private void SetEvent()
    {
        SetWarning(false);
        SetPos();
        ActivatePattern();
    }

    private void SetWarning(bool enable)
    {
        foreach (int i in GetIndices(num))
            warnings[type][i].SetActive(enable);
    }

    private void SetPos()
    {
        transform.position = player.transform.position;

        for (int i = 0; i < pieces[type].Count; i++)
            pieces[type][i].transform.localPosition = positions[type][i];
    }

    private void ActivatePattern()
    {
        foreach (int i in GetIndices(num))
            pieces[type][i].SetActive(true);
    }

    private void ChessDisable()
    {
        isSpawn = false;

        foreach (var obj in rooks)
            obj.SetActive(false);

        foreach (var obj in bishops) 
            obj.SetActive(false);
    }

    private int[] GetIndices(int pattern)
    {
        return pattern switch
        {
            0 => new[] { 0, 1 },
            1 => new[] { 2, 3 },
            2 => new[] { 0, 3 },
            3 => new[] { 0, 2 },
            4 => new[] { 1, 3 },
            5 => new[] { 1, 2 },
            _ => new int[0],
        };
    }
}
