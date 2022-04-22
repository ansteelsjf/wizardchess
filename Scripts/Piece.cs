using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

public class Piece : MonoBehaviour
{
    public enum pieceType { KING, QUEEN, BISHOP, ROOK, KNIGHT, PAWN, UNKNOWN = -1};
    public enum playerColor { BLACK, WHITE, UNKNOWN = -1};

    [SerializeField] private pieceType _type = pieceType.UNKNOWN;
    [SerializeField] private playerColor _player = playerColor.UNKNOWN;
    public pieceType Type
    {
        get { return _type; }
    }
    public playerColor Player
    {
        get { return _player; }
    }

    public Sprite pieceImage = null;
    public Vector2 position;
    private Vector3 moveTo;
    private PieceController manager;

    private Rules factory = new Rules(Board.Instance);
    private List<Move> moves = new List<Move>();

    private bool _hasMoved = false;
    public bool HasMoved
    {
        get { return _hasMoved; }
        set { _hasMoved = value; }
    }
    public NavMeshAgent agent;
    public GameObject targets;
    public bool fights;
    public Animator animator;
    public Animator targetanim;
    public PieceController PieceController;
    


    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && _player == playerColor.WHITE && manager.playerTurn)
        {
            moves.Clear();
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Highlight");
            foreach (GameObject o in objects)
            {
                Destroy(o);
            }

            moves = factory.GetMoves(this, position);
            foreach (Move move in moves)
            {
                if (move.pieceKilled == null)
                {
                    GameObject instance = Instantiate(Resources.Load("MoveCube")) as GameObject;
                    instance.transform.position = new Vector3(-move.secondPosition.Position.x, 0, move.secondPosition.Position.y);
                    instance.GetComponent<Container>().move = move;
                }
                else if (move.pieceKilled != null)
                {
                    GameObject instance = Instantiate(Resources.Load("KillCube")) as GameObject;
                    instance.transform.position = new Vector3(-move.secondPosition.Position.x, 0, move.secondPosition.Position.y);
                    instance.GetComponent<Container>().move = move;
                }
            }
            GameObject i = Instantiate(Resources.Load("CurrentPiece")) as GameObject;
            i.transform.position = this.transform.position;
        }
    }

    public void MovePiece(Vector3 position)
    {
        moveTo = position;
        animator.SetInteger("state", 1);

    }
    public void Capricepiece(GameObject gameobject)
    {
        targets = gameobject;
        Destroy(targets);
    }
    public void Fight(bool fight)
    {
        fights = fight;
    }

    void Start()
    {
        moveTo = this.transform.position;
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<PieceController>();
        Animator animator = GetComponentInChildren<Animator>();
        animator.SetInteger("state", 0);
        fights = false;
    }

    void Update()
    {
        
        agent = GetComponentInChildren<NavMeshAgent>();
        agent.destination = moveTo;


        if (Mathf.Abs(agent.transform.position.x - moveTo.x) < 0.1f && Mathf.Abs(agent.transform.position.z - moveTo.z) < 0.1f)
        {
           
            this.transform.position = moveTo;
            agent.transform.position = moveTo;
   
        }



        animator = GetComponentInChildren<Animator>();
        


        
     
        if (Mathf.Abs(this.transform.position.x-moveTo.x)<0.1f && Mathf.Abs(this.transform.position.z - moveTo.z) < 0.1f)
    
        {
            animator.SetInteger("state", 0);
        }
        
        Debug.Log(targets);
        if (agent.enabled && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance <= 0)
        {
            
            animator.SetInteger("state", 0);


        }

        
        if (fights == true)
        {

            if (agent.enabled && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance > 1f)
            {
                animator.SetInteger("state", 1);
                targetanim = targets.GetComponentInChildren<Animator>();
                targetanim = targets.GetComponent<Animator>();
                targetanim.SetInteger("state", 0);
            }
            else if (agent.enabled && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 1f)
            {


                StartCoroutine(Event());

            }
        }
        else StopAllCoroutines();

        IEnumerator Event()

        {
            animator.SetInteger("state", 2);

            targetanim = targets.GetComponent<Animator>();
            targetanim.SetInteger("state", 3);

            //Sound(2);

            yield return new WaitForSeconds(2);



            targetanim.SetInteger("state", 4);



            yield return new WaitForSeconds(2);

            //Destroy(targets);

            //Sound(3);
            targets.SetActive(false);
            fights = false;
            animator.SetInteger("state", 0);
            PieceController.EventFinish(true);

        }
     

    }
}

