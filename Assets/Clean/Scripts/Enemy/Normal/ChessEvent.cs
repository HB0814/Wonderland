using UnityEngine;

public class ChessEvent : MonoBehaviour
{
    [SerializeField] GameObject player;

    int rook;
    [SerializeField] GameObject rook_T;
    [SerializeField] GameObject rook_B;
    [SerializeField] GameObject rook_L;
    [SerializeField] GameObject rook_R;

    Transform rook_T_;
    Transform rook_B_;
    Transform rook_L_;
    Transform rook_R_;

    int bishop;
    [SerializeField] GameObject bishop_TR;
    [SerializeField] GameObject bishop_TL;
    [SerializeField] GameObject bishop_BR;
    [SerializeField] GameObject bishop_BL;

    Transform bishop_TR_;
    Transform bishop_TL_;
    Transform bishop_BR_;
    Transform bishop_BL_;

    private void Start()
    {
        rook_T_ = rook_T.transform;
    }

    private void OnEnable()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        Invoke(nameof(SetPosAndEvent), 0.1f);
    }

    private void SetPosAndEvent()
    {
        transform.position = player.transform.position;
        int ran = Random.Range(0, 2);

        if (ran == 0)
            RookEvent();
        else if (ran == 1)
            BishopEvent();
    }

    private void RookEvent()
    {
        rook = Random.Range(0, 6);

        switch(rook)
        {
            case 0:
                rook_T.gameObject.SetActive(true);
                rook_B.gameObject.SetActive(true);
                break;

            case 1:
                rook_L.gameObject.SetActive(true);
                rook_R.gameObject.SetActive(true);
                break;

            case 2:
                rook_T.gameObject.SetActive(true);
                rook_R.gameObject.SetActive(true);
                break;

            case 3:
                rook_T.gameObject.SetActive(true);
                rook_L.gameObject.SetActive(true);
                break;

            case 4:
                rook_B.gameObject.SetActive(true);
                rook_R.gameObject.SetActive(true);
                break;

            case 5:
                rook_B.gameObject.SetActive(true);
                rook_L.gameObject.SetActive(true);
                break;
        }
    }

    private void BishopEvent()
    {
        bishop = Random.Range(0, 6);

        switch (bishop)
        {
            case 0:
                bishop_TR.gameObject.SetActive(true);
                bishop_BL.gameObject.SetActive(true);
                break;

            case 1:
                bishop_TL.gameObject.SetActive(true);
                bishop_BR.gameObject.SetActive(true);
                break;

            case 2:
                bishop_TR.gameObject.SetActive(true);
                bishop_BR.gameObject.SetActive(true);
                break;

            case 3:
                bishop_TL.gameObject.SetActive(true);
                bishop_BL.gameObject.SetActive(true);
                break;

            case 4:
                bishop_TR.gameObject.SetActive(true);
                bishop_BR.gameObject.SetActive(true);
                break;

            case 5:
                bishop_BR.gameObject.SetActive(true);
                bishop_BL.gameObject.SetActive(true);
                break;
        }
    }
}
