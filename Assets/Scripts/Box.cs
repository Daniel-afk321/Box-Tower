using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Box : MonoBehaviour
{
    public Text scoreText;
    public Color32[] gameColors = new Color32[4];
    public Material BoxMat;
    public GameObject GameOverPanel;

    private const float Bounds_size = 3.5f;
    private const float Box_Moving_Speed = 5.0f;
    private const float Erro_Margin = 0.1f;
    private const float Box_Bounds_Gain = 0.25f;
    private const int Combo_Start_Gain = 3;

    private GameObject[] box;
    private Vector2 BoxBounds = new Vector2(Bounds_size, Bounds_size);

    private int scoreCount = 0;
    private int combo = 0;
    private int boxIndex;
    private float tileTransition = 0.0f;
    private float tileSpeed = 2.5f;
    private float secondaryPosition;

    private bool isMovingOnX = true;
    private bool GameOver = false;

    private Vector3 desiredPosition;
    private Vector3 lastTilePosition;

    private void Start()
    {
        box = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            box[i] = transform.GetChild(i).gameObject;
            ColorMesh(box[i].GetComponent<MeshFilter>().mesh);
 
        }
           

        boxIndex = transform.childCount - 1;
    }

    private void CreateRubble(Vector3 pos, Vector3 scale)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.AddComponent<Rigidbody>();

        go.GetComponent<MeshRenderer>().material = BoxMat;
        ColorMesh(go.GetComponent<MeshFilter>().mesh);
    }

    private void Update()
    {
        if (GameOver)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceTile())
            {
                SpawnTile();
                scoreCount++;
                scoreText.text = scoreCount.ToString ();

            }
            else
            {
                EndGame();
            }

        }
        MoveTile();

        transform.position = Vector3.Lerp(transform.position, desiredPosition, Box_Moving_Speed * Time.deltaTime);
    }
    private void MoveTile()
    {
       

        tileTransition += Time.deltaTime * tileSpeed;
        if (isMovingOnX)
            box[boxIndex].transform.localPosition = new Vector3(Mathf.Sin(tileTransition) * Bounds_size, scoreCount, secondaryPosition);
        else
            box[boxIndex].transform.localPosition = new Vector3(secondaryPosition, scoreCount, Mathf.Sin(tileTransition) * Bounds_size);
    }

    private void SpawnTile()
    {
        lastTilePosition = box[boxIndex].transform.localPosition;
        boxIndex--;
        if (boxIndex < 0)
            boxIndex = transform.childCount - 1;

        desiredPosition = (Vector3.down) * scoreCount;

        box[boxIndex].transform.localPosition = new Vector3(0, scoreCount, 0);
        box[boxIndex].transform.localScale = new Vector3(BoxBounds.x, 1, BoxBounds.y);

        ColorMesh (box[boxIndex].GetComponent<MeshFilter> ().mesh);
    }

    private bool PlaceTile()
    {
        Transform t = box[boxIndex].transform;

        if (isMovingOnX)
        {
            float deltaX = lastTilePosition.x - t.position.x;
            if (Mathf.Abs(deltaX) > Erro_Margin)
            {
                //está cortando o quadrado
                combo = 0;
                BoxBounds.x -= Mathf.Abs(deltaX);
                if (BoxBounds.x <= 0)
                    return false;

                float middle = lastTilePosition.x + t.localPosition.x / 2;
                t.localScale = new Vector3(BoxBounds.x, 1, BoxBounds.y);
                CreateRubble
                    (
                      new Vector3((t.position.x > 0)
                      ? t.position.x + (t.localScale.x /2)
                      : t.position.x - (t.localScale.x /2)
                      ,t.position.y
                      ,t.position.z),
                      new Vector3(Mathf.Abs(deltaX), 1, t.localScale.z)
                    );
                t.localPosition = new Vector3(middle - (lastTilePosition.x / 2), scoreCount, lastTilePosition.z);
            }
            else
            {
                if (combo > Combo_Start_Gain)
                {
                    BoxBounds.x += Box_Bounds_Gain;
                    if (BoxBounds.x > Bounds_size)
                        BoxBounds.x = Bounds_size;
                    float middle = lastTilePosition.x + t.localPosition.x / 2;
                    t.localScale = new Vector3(BoxBounds.x, 1, BoxBounds.y);
                    t.localPosition = new Vector3(middle - (lastTilePosition.x / 2), scoreCount, lastTilePosition.z);
                }
                combo++;
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, lastTilePosition.z);
            }
        }
        else
        {
            float deltaZ = lastTilePosition.z - t.position.z;
            if (Mathf.Abs(deltaZ) > Erro_Margin)
            {

                combo = 0;
                BoxBounds.y -= Mathf.Abs(deltaZ);
                if (BoxBounds.y <= 0)
                    return false;

                float middle = lastTilePosition.z + t.localPosition.z / 2;
                t.localScale = new Vector3(BoxBounds.x, 1, BoxBounds.y);
                CreateRubble
                  (
                    new Vector3(t.position.x
                               , t.position.y
                              , (t.position.z > 0)
                              ? t.position.z + (t.localScale.z / 2)
                              : t.position.z - (t.localScale.z / 2)),
                    new Vector3(t.localScale.x,1,Mathf.Abs(deltaZ))
                  );
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - (lastTilePosition.z / 2));
            }
            else
            {
                if (combo > Combo_Start_Gain)
                {
                    if (BoxBounds.y > Bounds_size)
                        BoxBounds.y = Bounds_size;
                    BoxBounds.y += Box_Bounds_Gain;
                    float middle = lastTilePosition.z + t.localPosition.z / 2;
                    t.localScale = new Vector3(BoxBounds.x, 1, BoxBounds.y);
                    t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - (lastTilePosition.z / 2));
                }
                combo++;
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, lastTilePosition.z);
            }
        }
            secondaryPosition = (isMovingOnX)
            ? t.localPosition.x
            : t.localPosition.z;


            isMovingOnX = !isMovingOnX;
            return true;
        }

    private void ColorMesh(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        Color32[] colors = new Color32[vertices.Length];
        float f = Mathf.Sin (scoreCount * 0.25f);

        for (int i = 0; i < vertices.Length; i++)
            colors[i] = Lerp4(gameColors[0], gameColors[1], gameColors[2], gameColors[3],f);

        mesh.colors32 = colors;
    }

    private Color32 Lerp4(Color32 a,Color32 b,Color32 c,Color32 d,float t)
    {
        if (t < 0.33f)
        {
            return Color.Lerp(a,b, t / 0.33f);
        }
        else if (t < 0.66f)
        {
            return Color.Lerp(b,c, (t - 0.33f) / 0.33f);
        }
        else
        {
            return Color.Lerp(c,d, (t - 0.66f) / 0.66f);
        }
    }

    private void EndGame()
    {
        if (PlayerPrefs.GetInt("score") < scoreCount)
            PlayerPrefs.SetInt("score", scoreCount);
        GameOver = true;
        GameOverPanel.SetActive(true);
        box[boxIndex].AddComponent<Rigidbody>();
    }
    public void OnButtonClick(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
   
