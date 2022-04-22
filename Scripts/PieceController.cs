using UnityEngine;
using System.Collections;

public class PieceController : MonoBehaviour
{
    Minimax ab = new Minimax();
    private bool _kingDead = false;
    float timer = 0;
    Board _board;
    public Piece piece;
    public bool finishs;
    private AudioSource runaudio;
    private AudioSource fightaudio;
    private AudioSource[] audiosource;
	void Start ()
    {
        _board = Board.Instance;
        _board.SetupBoard();
        audiosource = gameObject.GetComponents<AudioSource>();
        runaudio = audiosource[0];
        fightaudio = audiosource[1];


	}
    
    public void EventFinish(bool finish)
    {
        finishs = finish;
    }
	void Update ()
    {
        if (_kingDead)
        {
            Debug.Log("WINNER!");
            //UnityEditor.EditorApplication.isPlaying = false;
            Application.Quit();
        }
        if (!playerTurn && timer < 3)
        {
            timer += Time.deltaTime;
        }
        else if (!playerTurn && timer >= 3 )
        {
            Move move = ab.GetMove();
            _DoAIMove(move);
            timer = 0;
            finishs = false;
        }
	}

    public bool playerTurn = true;

    void _DoAIMove(Move move)
    {
        Tile firstPosition = move.firstPosition;
        Tile secondPosition = move.secondPosition;

        if (secondPosition.CurrentPiece && secondPosition.CurrentPiece.Type == Piece.pieceType.KING)
        {
            SwapPieces(move);
            _kingDead = true;
        }
        else
        {
            SwapPieces(move);
        }
    }
    

    public void SwapPieces(Move move)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
        foreach (GameObject o in objects)
        {
            Destroy(o);
        }

        Tile firstTile = move.firstPosition;
        Tile secondTile = move.secondPosition;

        firstTile.CurrentPiece.MovePiece(new Vector3(-move.secondPosition.Position.x, 0, move.secondPosition.Position.y));
        runaudio.Play();
        if (secondTile.CurrentPiece != null)
        {
            if (secondTile.CurrentPiece.Type == Piece.pieceType.KING)
                _kingDead = true;
            Debug.Log(secondTile.CurrentPiece.gameObject);
            Destroy(secondTile.CurrentPiece.gameObject);
            fightaudio.Play();
            //piece.Capricepiece(secondTile.CurrentPiece.gameObject);
           
            //piece.Fight(true);
            //Debug.Log("destroy");
        }
            

        secondTile.CurrentPiece = move.pieceMoved;
        firstTile.CurrentPiece = null;
        secondTile.CurrentPiece.position = secondTile.Position;
        secondTile.CurrentPiece.HasMoved = true;

        playerTurn = !playerTurn;
    }
}
