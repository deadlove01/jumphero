using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public float maxX = 2.5f;
    public float minX = -2.5f;
    public float maxY = -3.75f;
    public float minY = -4.75f;
    public float cameraSmooth = 1.7f;
    public int poolNumber = 3;
    public static GameManager Instance;

    private bool isGameOver = false;
    private float desiredX = 0;
    private bool isCamMoving = false;

    [SerializeField] private GameObject platformPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject gameoverHolder;

    [SerializeField]
    private Queue<GameObject> pools;
    public bool GameOver
    {
        get { return isGameOver; }
    }

    
    private void Awake()
    {
        Instance = this;
        pools = new Queue<GameObject>();
        gameoverHolder.SetActive(false);
    }

    void Start()
    {
        desiredX = Camera.main.transform.position.x;
        Init();
    }

    void Update()
    {
        if (isCamMoving)
        {
            FollowCamera();
        }
    }
    void FollowCamera()
    {
        var cam = Camera.main;
        var camTrans = cam.transform.position;
        var x = camTrans.x;
        x = Mathf.Lerp(x, desiredX, cameraSmooth * Time.deltaTime);

        Camera.main.transform.position = new Vector3(x, camTrans.y, camTrans.z);

        if (camTrans.x >= desiredX - 0.05f)
        {
            isCamMoving = false;
        }
    }

    public void CreateNewPlatform(float lastPlatformX)
    {
        print("create new platform");
        CreatePlatform();
        desiredX = lastPlatformX + maxX;
        print("desiredX: "+desiredX);
        isCamMoving = true;
    }
    public void EndGame()
    {
        gameoverHolder.SetActive(true);
        ScoreManager.Instance.UpdateFinalScore();
        gameoverHolder.GetComponent<Animator>().SetBool("Show", true);
        isGameOver = true;
        print("End game");

    }



    private void Init()
    {
        // create first platform
        var firstPos = new Vector3(0, Random.Range(minY, maxY), 0);
        var platform1 = Instantiate(platformPrefab, firstPos, Quaternion.identity);
        platform1.gameObject.tag = "Untagged";
        pools.Enqueue(platform1);

        // create player 
        firstPos.y += 4.5f;
        Instantiate(playerPrefab, firstPos, Quaternion.identity);

        // create second platform
        var go =Instantiate(platformPrefab, new Vector3(Random.Range(maxX*2, maxX*2-1.5f),
                Random.Range(minY, maxY), 0)
            , Quaternion.identity);

        pools.Enqueue(go);
    }
    private void CreatePlatform()
    {
        var camX = Camera.main.transform.position.x;
        var newMaxX = maxX * 2 + camX;
        var newPos = new Vector3(Random.Range(newMaxX, newMaxX - 1.2f),
            Random.Range(maxY, maxY - 1.2f), 0);
        if (pools.Count >= poolNumber)
        {
            var go = pools.Dequeue();
            go.tag = "Platform";
            go.transform.position = newPos;
            pools.Enqueue(go);
        }
        else
        {
            var go = Instantiate(platformPrefab, newPos
                , Quaternion.identity);
            pools.Enqueue(go);
        }
       
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
