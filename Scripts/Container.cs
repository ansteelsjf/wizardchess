using UnityEngine;
using System.Collections;

public class Container : MonoBehaviour
{
    public Move move;
    PieceController manager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<PieceController>();
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && move != null)
        {
            manager.SwapPieces(move);
        }
    }
}
