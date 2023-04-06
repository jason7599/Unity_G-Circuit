using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MapGenerator
{
    [Header("MapConfig")]
    private static int mapSize;
    private static int wallSize;
    private static int wallHeight;
    private static int baseSize = 3;
    
    [Header("Maze")]
    private static int[,] maze;
    
    [Header("Others")]
    private static Room[,] rooms;
    private static List<Tuple<int, int>> visited = new List<Tuple<int, int>>();
    private static Transform wallsHolder;

    class Room
    {
        public Queue<Tuple<int, int>> RandomizedDir = new Queue<Tuple<int, int>>();
        private Tuple<int, int> _myDir;
        private List<Tuple<int, int>> _dir = new List<Tuple<int, int>>();
        private Random _random = new Random();
        
        public Room(int x, int y)
        {
            _myDir = new Tuple<int, int>(x, y);
            
            _dir.Add(new Tuple<int, int>(x-1, y));
            _dir.Add(new Tuple<int, int>(x+1, y));
            _dir.Add(new Tuple<int, int>(x, y-1));
            _dir.Add(new Tuple<int, int>(x, y+1));

            for (int idx = 4; idx > 0; idx--)
            {
                Tuple<int, int> chooseDir = _dir[_random.Next(0, idx)];
                _dir.Remove(chooseDir);
                RandomizedDir.Enqueue(chooseDir);
            }
        }

        public Tuple<int, int> GetCurrentPos()
        {
            return _myDir;
        }

        public Tuple<int, int> GetNextPos()
        {
            if (RandomizedDir.Count == 0)
            {
                return null;
            }
            return RandomizedDir.Dequeue();
        }
    }
    
    static int[,] DrawMap()
    {
        // Initialize Maze
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                rooms[x, y] = new Room(x, y);
            }
        }
        int mazeLength = mapSize * 2 + 1;
        for (int x = 0; x < mazeLength; x++)
        {
            for (int y = 0; y < mazeLength; y++)
            {
                maze[x, y] = 1;
            }
        }
        
        // Start Making Map
        MakeRoom(rooms[0, 0]);
        
        // Generate Shortcuts
        Random ran = new Random();
        int holeNum = mapSize * 2;
        int mapLength = mazeLength - 1;
        while (true)
        {
            if (holeNum == 0)
            {
                break;
            }

            int x = ran.Next(1, mapLength);
            int y = ran.Next(1, mapLength);
            if ((maze[y - 1, x] + maze[y + 1, x] == 2 && maze[y, x - 1] + maze[y, x + 1] == 0) || (maze[y - 1, x] + maze[y + 1, x] == 0 && maze[y, x - 1] + maze[y, x + 1] == 2))
            {
                maze[y, x] = 0;
                holeNum--;
            }
        }
        
        // Generate Base
        for (int x = mapSize - baseSize; x <= mapSize + baseSize; x++)
        {
            for (int y = mapSize - baseSize; y <= mapSize + baseSize; y++)
            {
                maze[y, x] = 0;
            }
        }
        
        return maze;
    }

    static void MakeRoom(Room currentRoom)
    {
        Tuple<int, int> currentPos = currentRoom.GetCurrentPos();
        int currentX = currentPos.Item1;
        int currentY = currentPos.Item2;
        
        visited.Add(new Tuple<int, int>(currentX, currentY));
        maze[currentX * 2 + 1, currentY * 2 + 1] = 0;
        
        while (currentRoom.RandomizedDir.Count != 0)
        {
            Tuple<int, int> nextDir = currentRoom.GetNextPos();
            int nextX = nextDir.Item1;
            int nextY = nextDir.Item2;

            if (0 <= nextX && nextX < mapSize && 0 <= nextY && nextY < mapSize)
            {
                if (!visited.Contains(nextDir))
                {
                    maze[currentX + nextX + 1, currentY + nextY + 1] = 0;
                    MakeRoom(rooms[nextX, nextY]);
                }
            }
        }
    }
    
    public static int[,] InitMap(MazeConfiguration config)
    {
        mapSize = config.mapSize;
        wallSize = config.wallSize;
        wallHeight = config.wallHeight;
        baseSize = config.baseSize;
        GameObject wallPrefab = config.wallPrefab;

        rooms = new Room[mapSize, mapSize];
        maze = new int[mapSize * 2 + 1, mapSize * 2 + 1];

        int[,] map = DrawMap();
        Random _random = new Random();

        if (wallsHolder == null)
        {
            wallsHolder = new GameObject("Walls").transform;
        }

        // Generate Wall Prefabs
        int mazeLength = mapSize * 2 + 1;
        for (int x = 0; x < mazeLength; x++)
        {
            for (int y = 0; y < mazeLength; y++)
            {
                if (map[x, y] == 1)
                {
                    GameObject.Instantiate(wallPrefab, new Vector3(x*wallSize, wallHeight / 2, y*wallSize), Quaternion.identity, wallsHolder);
                }
            }
        }
        
        // Generate Exit
        
        
        // Spawn Items
        
        
        // Spawn Monsters
        List<GameObject> spawnedMonster = new List<GameObject>();
        GameObject[] monsterList = config.monsterPrefab;
        int monsterNum = 3;
        int setSpawnLengthInterval = 3;
        int spawnLengthInterval = setSpawnLengthInterval * 2 + 1;

        List<Tuple<int, int>> spawnLocations = new List<Tuple<int, int>>();
        spawnLocations.Add(new Tuple<int, int>(spawnLengthInterval, spawnLengthInterval));
        spawnLocations.Add(new Tuple<int, int>(spawnLengthInterval, mazeLength - 1 - spawnLengthInterval));
        spawnLocations.Add(new Tuple<int, int>(mazeLength - 1 - spawnLengthInterval, spawnLengthInterval));
        spawnLocations.Add(new Tuple<int, int>(mazeLength - 1 - spawnLengthInterval, mazeLength - 1 - spawnLengthInterval));
        Queue<Tuple<int, int>> randomizedLocation = new Queue<Tuple<int, int>>();
        for (int idx = 4; idx > 0; idx--)
        {
            Tuple<int, int> chooseLocation = spawnLocations[_random.Next(0, idx)];
            spawnLocations.Remove(chooseLocation);
            randomizedLocation.Enqueue(chooseLocation);
        }
        
        for (int i = 0; i < monsterNum; i++)
        {
            GameObject monster = monsterList[_random.Next(monsterList.Length)];
            if (spawnedMonster.Contains(monster))
            {
                i--;
                continue;
            }
            spawnedMonster.Add(monster);
            
            Tuple<int, int> spawnLocation = randomizedLocation.Dequeue();
            int spawnX = spawnLocation.Item1 * wallSize;
            int spawnY = spawnLocation.Item2 * wallSize;
            GameObject.Instantiate(monster, new Vector3(spawnX, 1, spawnY), Quaternion.identity);
        }

        return map;
    }

}

[System.Serializable]
public class MazeConfiguration
{
    public int mapSize;
    public int wallSize;
    public int wallHeight;
    public int baseSize;
    public GameObject wallPrefab;
    public GameObject[] monsterPrefab;
}
