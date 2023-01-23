using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int dungeonWidth;
    public int dungeonHeight;
    public RoomData[,] dungeonData;
    public List<RoomData> possibleRooms;
    private int heightPos = 0;
    private int widthPos = 0;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else 
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        dungeonData = new RoomData[dungeonHeight,dungeonWidth];
        GenerateDungeon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateDungeon()
    {
        for (int i = 0; i < dungeonHeight; i++)
        {
            for (int j = 0; j < dungeonWidth; j++)
            {
                dungeonData[i,j] = GetRandomRoom(); //TODO prendre en compte les portes ?
            }
        }
    }

    RoomData GetRandomRoom()
    {
        int k = 0;
        int index = Random.Range(0,possibleRooms.Count);
        foreach(RoomData r in possibleRooms)
        {
            if (k == index) return r;
            k++;
        }
        return null;
    }

    public void GoToNextRoom(Door.Corner corner)
    {
        switch (corner)
        {
            case Door.Corner.Up: heightPos++; break;
            case Door.Corner.Left: widthPos++; break;
            case Door.Corner.Right: widthPos--; break;
            case Door.Corner.Down: heightPos--; break;
        }
        Instantiate(dungeonData[heightPos,widthPos].roomPrefab, 50 * (Vector3.right * heightPos + Vector3.forward * widthPos), Quaternion.identity);
        RoomBehaviour.Instance.closeDoor(corner);
    }
}