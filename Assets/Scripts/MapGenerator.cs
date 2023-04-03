using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MapGenerator
{
    static int mapSize;
    private static int wallSize;
    private static int wallHeight;
    private static int baseSize = 3;
    
    static Room[,] Rooms;
    static int[,] Maze;
    static List<Tuple<int, int>> Visited = new List<Tuple<int, int>>();

    private static Transform _wallsHolder;

    class Room
    {
        int x;
        int y;
        Tuple<int, int> myDir;
        List<Tuple<int, int>> dir = new List<Tuple<int, int>>();
        public Queue<Tuple<int, int>> randomizedDir = new Queue<Tuple<int, int>>();
        Random _random = new Random();
        
        public Room(int x, int y)
        {
            this.x = x;
            this.y = y;
            myDir = new Tuple<int, int>(x, y);
            
            dir.Add(new Tuple<int, int>(x-1, y));
            dir.Add(new Tuple<int, int>(x+1, y));
            dir.Add(new Tuple<int, int>(x, y-1));
            dir.Add(new Tuple<int, int>(x, y+1));

            for (int idx = 4; idx > 0; idx--)
            {
                Tuple<int, int> chooseDir = dir[_random.Next(0, idx)];
                dir.Remove(chooseDir);
                randomizedDir.Enqueue(chooseDir);
            }
        }

        public Tuple<int, int> GetCurrentPos()
        {
            return myDir;
        }

        public Tuple<int, int> GetNextPos()
        {
            if (randomizedDir.Count == 0)
            {
                return null;
            }
            return randomizedDir.Dequeue();
        }
    }
    
    static int[,] DrawMap()
    {
        // Initialize Maze
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                Rooms[x, y] = new Room(x, y);
            }
        }
        int mazeLength = mapSize * 2 + 1;
        for (int x = 0; x < mazeLength; x++)
        {
            for (int y = 0; y < mazeLength; y++)
            {
                Maze[x, y] = 1;
            }
        }
        
        // Start Making Map
        MakeRoom(Rooms[0, 0]);
        
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
            if ((Maze[y - 1, x] + Maze[y + 1, x] == 2 && Maze[y, x - 1] + Maze[y, x + 1] == 0) || (Maze[y - 1, x] + Maze[y + 1, x] == 0 && Maze[y, x - 1] + Maze[y, x + 1] == 2))
            {
                Maze[y, x] = 0;
                holeNum--;
            }
        }
        
        // Generate Base
        for (int x = mapSize - baseSize; x <= mapSize + baseSize; x++)
        {
            for (int y = mapSize - baseSize; y <= mapSize + baseSize; y++)
            {
                Maze[y, x] = 0;
            }
        }
        
        return Maze;
    }

    static void MakeRoom(Room currentRoom)
    {
        Tuple<int, int> currentPos = currentRoom.GetCurrentPos();
        int currentX = currentPos.Item1;
        int currentY = currentPos.Item2;
        
        Visited.Add(new Tuple<int, int>(currentX, currentY));
        Maze[currentX * 2 + 1, currentY * 2 + 1] = 0;
        
        while (currentRoom.randomizedDir.Count != 0)
        {
            Tuple<int, int> nextDir = currentRoom.GetNextPos();
            int nextX = nextDir.Item1;
            int nextY = nextDir.Item2;

            if (0 <= nextX && nextX < mapSize && 0 <= nextY && nextY < mapSize)
            {
                if (!Visited.Contains(nextDir))
                {
                    Maze[currentX + nextX + 1, currentY + nextY + 1] = 0;
                    MakeRoom(Rooms[nextX, nextY]);
                }
            }
        }
    }
    
    public static int[,] InitMap(MazeConfiguration config)
    {
        mapSize = config.mapSize;
        wallSize = config.wallSize;
        wallHeight = config.wallHeight;
        GameObject wallPrefab = config.wallPrefab;

        Rooms = new Room[mapSize, mapSize];
        Maze = new int[mapSize * 2 + 1, mapSize * 2 + 1];

        int[,] map = DrawMap();

        if (_wallsHolder == null)
        {
            _wallsHolder = new GameObject("Walls").transform;
        }

        int mazeLength = mapSize * 2 + 1;
        for (int x = 0; x < mazeLength; x++)
        {
            for (int y = 0; y < mazeLength; y++)
            {
                if (map[x, y] == 1)
                {
                    GameObject.Instantiate(wallPrefab, new Vector3(x*wallSize, wallHeight / 2, y*wallSize), Quaternion.identity, _wallsHolder);
                }
            }
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
    public GameObject wallPrefab;
}