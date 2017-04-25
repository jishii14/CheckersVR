using UnityEngine;
using UnityEngine.UI;
using System;

public enum MyColor { EMPTY, WHITE, BLACK};
public class Space {
	public float x;
	public float z;
	public float y;
	public MyColor c;
    public MyColor pieceColor;
	public bool isFilled;
	public GameObject contained;

	public Space(float xCoord, float zCoord, MyColor color, bool filled) {
		this.x = xCoord;
		this.z = zCoord;
		this.c = color;
		this.isFilled = filled;
		this.contained = null;
		y = 0.15f;
	}

    public void fill(GameObject go, int curI, int curJ, MyColor pieceC, int recI, int recJ) {
        if (pieceColor == MyColor.WHITE) {
            Behavior.removeWhite(contained);
            Behavior.remainingWhites--;
            Behavior.secretVelocity = Behavior.secretVelocity * 2.5f;
        } else if (pieceColor == MyColor.BLACK)
        {
            Behavior.removeBlack(contained);
            Behavior.remainingBlacks--;
        } else {
            recI = 0; recJ = 0; // Stop recursing
        }

        pieceColor = pieceC;
        Behavior.spaces[curI, curJ].pieceColor = MyColor.EMPTY;
        contained = go;
        Behavior.spaces[curI, curJ].contained = null;
        isFilled = true;
        Behavior.spaces[curI, curJ].isFilled = false;

        if (recI != 0 && recJ != 0)
            Behavior.spaces[curI + recI * 2, curJ + recJ * 2].fill(go, curI + recI, curJ + recJ, pieceC, recI, recJ);
        else
            go.transform.position = new Vector3(x, y, z);
    }
}

public class Behavior : MonoBehaviour {
    public Text turnText;
	//public Text countText;
	static GameObject[] whites;
	static GameObject[] blacks;
    static GameObject secretPlatform;
    public static int remainingWhites;
    public static int remainingBlacks;
	private float count;
	public static Space[,] spaces = new Space[8, 8];
    public static int turn = 1;
	float xOffset = 3.5f;
	float zOffset = 3.5f;
	float y = .15f;
	Vector3 dist;
	float posX; // Initial
	float posY;
	float posZ;
    int currentI;
    int currentJ;
	float distX;
	float distY;
	float distZ;
    public static float secretVelocity = 1;

    public static void removeBlack(GameObject go) {
        Boolean hasRemoved = false;
        for(int i = 0; i < remainingBlacks; i++) {
            if(blacks[i] == go) {
                go.transform.Translate(new Vector3(0, -200, 0));
                hasRemoved = true;
            }

            if (hasRemoved && i + 1 != remainingBlacks) {
                blacks[i] = blacks[i + 1];
            }
        }
    }

    public static void removeWhite(GameObject go) {
        Boolean hasRemoved = false;
        for(int i = 0; i < remainingWhites; i++) {
            if(whites[i] == go) {
                go.transform.Translate(new Vector3(0, -200, 0));
                hasRemoved = true;
            }

            if (hasRemoved && i + 1 != remainingWhites) {
                whites[i] = whites[i + 1];
            }
        }
    }

    void toggleTurn()
    {
        Text txt = (Text) FindObjectOfType(typeof(Text));

        if (turn == 0)
        {
            turn = 1;
            txt.text = "Computer's Turn\t Whites Remaining: " + remainingWhites.ToString();
        }
        else
        {
            if (remainingBlacks == 0)
                remainingBlacks = 12;

            txt.text = "Your Turn\t Blacks Remaining: " + remainingBlacks.ToString();
            turn = 0;
        }
    }

    void endGame() {
        Text txt = (Text)FindObjectOfType(typeof(Text));
        txt.text = "YOU WIN!";
    }

    void makeWhiteMove()
    {

        if (remainingWhites == 0)
            endGame();
        else
        {
            System.Random rand = new System.Random();

            int coord = rand.Next(0, remainingWhites);

            Boolean hasMadeMove = false;
            int initCoord = coord;

            // SEE IF ANY OFFENSIVE MOVES CAN BE MADE
            while (!hasMadeMove)
            {
                setCurrIJ(whites[coord].transform);

                if (!hasMadeMove && currentI + 1 < 8 && currentJ - 1 >= 0 && spaces[currentI + 1, currentJ - 1].isFilled == true && spaces[currentI + 1, currentJ - 1].pieceColor == MyColor.BLACK)
                {
                    int testI = currentI + 1;
                    int testJ = currentJ - 1;
                    Boolean isPossible = false;
                    while (testI < 8 && testJ >= 0)
                    {
                        if (spaces[testI, testJ].isFilled == false)
                            isPossible = true;
                        else if (spaces[testI, testJ].pieceColor == MyColor.WHITE)
                            testI = 10;
                        testI++; testJ--;
                    }

                    if (isPossible)
                    {
                        spaces[currentI + 1, currentJ - 1].fill(spaces[currentI, currentJ].contained, currentI, currentJ, MyColor.WHITE, 1, -1);
                        hasMadeMove = true;
                    }
                }
                if (!hasMadeMove && currentI + 1 < 8 && currentJ + 1 < 8 && spaces[currentI + 1, currentJ + 1].isFilled == true && spaces[currentI + 1, currentJ + 1].pieceColor == MyColor.BLACK)
                {
                    int testI = currentI + 1;
                    int testJ = currentJ + 1;
                    Boolean isPossible = false;
                    while (testI < 8 && testJ < 8)
                    {
                        if (spaces[testI, testJ].isFilled == false)
                            isPossible = true;
                        else if (spaces[testI, testJ].pieceColor == MyColor.WHITE)
                            testI = 10;
                        testI++; testJ++;
                    }

                    if (isPossible)
                    {
                        spaces[currentI + 1, currentJ + 1].fill(spaces[currentI, currentJ].contained, currentI, currentJ, MyColor.WHITE, 1, 1);
                        hasMadeMove = true;
                    }
                }
                if (!hasMadeMove && currentI - 1 >= 0 && currentJ + 1 < 8 && spaces[currentI - 1, currentJ + 1].isFilled == true && spaces[currentI - 1, currentJ + 1].pieceColor == MyColor.BLACK)
                {
                    int testI = currentI - 1;
                    int testJ = currentJ + 1;
                    Boolean isPossible = false;
                    while (testI >= 0 && testJ < 8)
                    {
                        if (spaces[testI, testJ].isFilled == false)
                            isPossible = true;
                        else if (spaces[testI, testJ].pieceColor == MyColor.WHITE)
                            testI = -10;
                        testI--; testJ++;
                    }

                    if (isPossible)
                    {
                        spaces[currentI - 1, currentJ + 1].fill(spaces[currentI, currentJ].contained, currentI, currentJ, MyColor.WHITE, -1, 1);
                        hasMadeMove = true;
                    }
                }
                if (!hasMadeMove && currentI - 1 >= 0 && currentJ - 1 >= 0 && spaces[currentI - 1, currentJ - 1].isFilled == true && spaces[currentI - 1, currentJ - 1].pieceColor == MyColor.BLACK)
                {
                    int testI = currentI - 1;
                    int testJ = currentJ - 1;
                    Boolean isPossible = false;
                    while (testI >= 0 && testJ >= 0)
                    {
                        if (spaces[testI, testJ].isFilled == false)
                            isPossible = true;
                        else if (spaces[testI, testJ].pieceColor == MyColor.WHITE)
                            testI = -10;
                        testI--; testJ--;
                    }

                    if (isPossible)
                    {
                        spaces[currentI - 1, currentJ - 1].fill(spaces[currentI, currentJ].contained, currentI, currentJ, MyColor.WHITE, 1, 1);
                        hasMadeMove = true;
                    }
                }


                coord = (coord + 1) % remainingWhites;

                if (initCoord == coord)
                    break;
            }

            // TRY TO MAKE A NORMAL MOVE
            while (!hasMadeMove)
            {
                setCurrIJ(whites[coord].transform);

                if (currentI + 1 < 8 && currentJ - 1 >= 0 && spaces[currentI + 1, currentJ - 1].isFilled == false)
                {
                    spaces[currentI + 1, currentJ - 1].fill(spaces[currentI, currentJ].contained, currentI, currentJ, MyColor.WHITE, 0, 0);
                    hasMadeMove = true;
                }
                else if (currentI + 1 < 8 && currentJ + 1 < 8 && spaces[currentI + 1, currentJ + 1].isFilled == false)
                {
                    spaces[currentI + 1, currentJ + 1].fill(spaces[currentI, currentJ].contained, currentI, currentJ, MyColor.WHITE, 0, 0);
                    hasMadeMove = true;
                }
                else if (currentI - 1 >= 0 && currentJ + 1 < 8 && spaces[currentI - 1, currentJ + 1].isFilled == false)
                {
                    spaces[currentI - 1, currentJ + 1].fill(spaces[currentI, currentJ].contained, currentI, currentJ, MyColor.WHITE, 0, 0);
                    hasMadeMove = true;
                }
                else if (currentI - 1 >= 0 && currentJ - 1 >= 0 && spaces[currentI - 1, currentJ - 1].isFilled == false)
                {
                    spaces[currentI - 1, currentJ - 1].fill(spaces[currentI, currentJ].contained, currentI, currentJ, MyColor.WHITE, 0, 0);
                    hasMadeMove = true;
                }


                coord = (coord + 1) % remainingWhites;

                if (initCoord == coord)
                    hasMadeMove = true;
            }

            toggleTurn();
        }

        Text txt = (Text)FindObjectOfType(typeof(Text));
        if (remainingBlacks == 0)
            txt.text = "YOU LOSE!";
    }

    void setCurrIJ(Transform transform) {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Math.Abs(spaces[i, j].x - transform.position.x) < 0.2f && Math.Abs(spaces[i, j].z - transform.position.z) < 0.2f)
                {
                    currentI = i;
                    currentJ = j;
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        secretPlatform = GameObject.FindGameObjectWithTag("SecretPlatform");

        whites = GameObject.FindGameObjectsWithTag("White");
        blacks = GameObject.FindGameObjectsWithTag("Black");
        if (spaces[0, 0] == null)
        {
            toggleTurn();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    spaces[i, j] = new Space((xOffset - i), (zOffset - j), ((i + j) % 2 == 0) ? MyColor.BLACK : MyColor.WHITE, false);
                }
            }

            count = 0;

            SetCountText();
            remainingWhites = 0;
            remainingBlacks = 0;

            foreach (GameObject go in whites)
            {
                remainingWhites++;

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (Math.Abs(spaces[i, j].x - go.transform.position.x) < 0.2f && Math.Abs(spaces[i, j].z - go.transform.position.z) < 0.2f)
                        {
                            spaces[i, j].contained = go;
                            spaces[i, j].isFilled = true;
                            spaces[i, j].pieceColor = MyColor.WHITE;
                            i = 8; j = 8;
                        }
                    }
                }
            }

            foreach (GameObject go in blacks)
            {
                remainingBlacks++;

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (Math.Abs(spaces[i, j].x - go.transform.position.x) < 0.2f && Math.Abs(spaces[i, j].z - go.transform.position.z) < 0.2f)
                        {
                            spaces[i, j].contained = go;
                            spaces[i, j].isFilled = true;
                            spaces[i, j].pieceColor = MyColor.BLACK;
                            i = 8; j = 8;
                        }
                    }
                }
            }

        }

        distX = transform.position.x;
        distY = transform.position.y;
        distZ = transform.position.z;

        // FIND THE ACTUAL PIECE AND SAVE ITS BEGINNING COORDINATES
        setCurrIJ(transform);
    }

	// Update is called once per frame
	void Update () {
        //secretPlatform.transform.position = new Vector3(secretPlatform.transform.position.x, secretPlatform.transform.position.y - 0.001f * secretVelocity, secretPlatform.transform.position.z);
	}

	void SetCountText ()
	{
		//countText.text = "Count: " + count.ToString ();
	}

    float mouseX = 0;
    float mouseY = 0;
    float mouseZ = 0;
	void OnMouseDown(){
        if (turn == 0)
        {
            posX = transform.position.x;
            posY = transform.position.y;
            posZ = transform.position.z;

            mouseX = Input.mousePosition.x;
            mouseY = Input.mousePosition.y;
            mouseZ = Input.mousePosition.z;
        }

        setCurrIJ(transform);
    }
		
	void OnMouseDrag(){
        if (turn == 0)
        {
            float mouseDragX = Input.mousePosition.x - mouseX;
            float mouseDragY = Input.mousePosition.y - mouseY;
            float mouseDragZ = Input.mousePosition.z - mouseZ;
            mouseX = Input.mousePosition.x;
            mouseY = Input.mousePosition.y;
            mouseZ = Input.mousePosition.z;

            transform.Translate(new Vector3(mouseDragX / 80, 0, mouseDragY / 80));
        }
	}

	void OnMouseUp() {
		float curX = transform.position.x;
		float curY = transform.position.y;
		float curZ = transform.position.z;

        Boolean hasFilled = false;
		for (int i = 0; i < 8 && !hasFilled; i++) { // DISTX DISTY contain the original, CURX contains current
			for (int j = 0; j < 8 && !hasFilled; j++) {
				if (Math.Abs(spaces [i, j].x - curX) < 0.4 && Math.Abs(spaces [i, j].z - curZ) < 0.4 && spaces[i, j].c == MyColor.BLACK && spaces[i,j].isFilled == false) {
                    if (Math.Abs(i - currentI) == 1 && Math.Abs(j - currentJ) == 1)
                    {
                        Vector3 curPos = new Vector3(spaces[i, j].x, spaces[i, j].y, spaces[i, j].z);
                        transform.position = curPos;
                        hasFilled = true;
                        distX = spaces[i, j].x;
                        distY = spaces[i, j].y;
                        distZ = spaces[i, j].z;
                        spaces[i, j].isFilled = true;
                        spaces[currentI, currentJ].isFilled = false;
                        spaces[i, j].contained = spaces[currentI, currentJ].contained;
                        spaces[i, j].pieceColor = MyColor.BLACK;
                        spaces[currentI, currentJ].pieceColor = MyColor.EMPTY;
                        spaces[currentI, currentJ].contained = null;
                        currentI = i;
                        currentJ = j;
                        toggleTurn();
                        Invoke("makeWhiteMove", 1f);
                    } else if (Math.Abs(i - currentI) == Math.Abs(j - currentJ)) {
                        if (currentI < i && currentJ < j) {
                            if (spaces[currentI + 1, currentJ + 1].isFilled && spaces[currentI + 1, currentJ + 1].pieceColor == MyColor.WHITE) {
                                spaces[currentI + 1, currentJ + 1].fill(spaces[currentI, currentJ].contained, currentI, currentJ, MyColor.BLACK, 1, 1);
                                hasFilled = true;
                                distX = spaces[i, j].x;
                                distY = spaces[i, j].y;
                                distZ = spaces[i, j].z;
                                toggleTurn();
                                Invoke("makeWhiteMove", 3.0f);
                            }
                        }
                        else if (currentI < i && currentJ > j) {
                            if (spaces[currentI + 1, currentJ - 1].isFilled && spaces[currentI + 1, currentJ - 1].pieceColor == MyColor.WHITE) {
                                spaces[currentI + 1, currentJ - 1].fill(spaces[currentI, currentJ].contained, currentI, currentJ, MyColor.BLACK, 1, -1);
                                hasFilled = true;
                                distX = spaces[i, j].x;
                                distY = spaces[i, j].y;
                                distZ = spaces[i, j].z;
                                toggleTurn();
                                Invoke("makeWhiteMove", 3.0f);
                            }
                        }
                        else if (currentI > i && currentJ < j) {
                            if (spaces[currentI - 1, currentJ + 1].isFilled && spaces[currentI - 1, currentJ + 1].pieceColor == MyColor.WHITE) {
                                spaces[currentI - 1, currentJ + 1].fill(spaces[currentI, currentJ].contained, currentI, currentJ, MyColor.BLACK, -1, 1);
                                hasFilled = true;
                                distX = spaces[i, j].x;
                                distY = spaces[i, j].y;
                                distZ = spaces[i, j].z;
                                toggleTurn();
                                Invoke("makeWhiteMove", 3.0f);
                            }
                        }
                        else if (currentI > i && currentJ > j) {
                            if (spaces[currentI - 1, currentJ - 1].isFilled && spaces[currentI - 1, currentJ - 1].pieceColor == MyColor.WHITE) {
                                spaces[currentI - 1, currentJ - 1].fill(spaces[currentI, currentJ].contained, currentI, currentJ, MyColor.BLACK, -1, -1);
                                hasFilled = true;
                                distX = spaces[i, j].x;
                                distY = spaces[i, j].y;
                                distZ = spaces[i, j].z;
                                toggleTurn();
                                Invoke("makeWhiteMove", 3.0f);
                            }
                        }
                    }
                }
			}
		}

        if(!hasFilled) {
            transform.position = new Vector3(distX, distY, distZ);
        }
	}
}
