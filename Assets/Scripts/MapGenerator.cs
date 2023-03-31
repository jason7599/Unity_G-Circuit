using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MapGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    
    private const int MapSize = 50;
    Room[,] Rooms = new Room[MapSize, MapSize];
    int[,] Maze = new int[MapSize * 2 + 1, MapSize * 2 + 1];
    private List<Tuple<int, int>> Visited = new List<Tuple<int, int>>();

    class Room
    {
        private int x;
        private int y;
        private Tuple<int, int> myDir;
        private List<Tuple<int, int>> dir = new List<Tuple<int, int>>();
        public Queue<Tuple<int, int>> randomizedDir = new Queue<Tuple<int, int>>();
        private Random _random = new Random();
        
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
    
    int[,] DrawMap()
    {
        // Initialize the whole map with class: Room
        for (int x = 0; x < MapSize; x++)
        {
            for (int y = 0; y < MapSize; y++)
            {
                Rooms[x, y] = new Room(x, y);
            }
        }
        // Initialize the grid where 1 stands for wall and 0 for path
        int mazeLength = MapSize * 2 + 1;
        for (int x = 0; x < mazeLength; x++)
        {
            for (int y = 0; y < mazeLength; y++)
            {
                Maze[x, y] = 1;
            }
        }
        
        MakeRoom(Rooms[0, 0]);
        
        return Maze;
    }

    void MakeRoom(Room currentRoom)
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

            if (0 <= nextX && nextX < MapSize && 0 <= nextY && nextY < MapSize)
            {
                if (!Visited.Contains(nextDir))
                {
                    Maze[currentX + nextX + 1, currentY + nextY + 1] = 0;
                    MakeRoom(Rooms[nextX, nextY]);
                }
            }
        }
    }
    
    void Awake()
    {
        GenerateMap(DrawMap());
    }

    void GenerateMap(int[,] maze)
    {
        int mazeLength = MapSize * 2 + 1;
        for (int x = 0; x < mazeLength; x++)
        {
            for (int y = 0; y < mazeLength; y++)
            {
                if (Maze[x, y] == 1)
                {
                    GameObject wall = Instantiate(wallPrefab);

                    Vector3 wallPos = new Vector3(x*2, 10, y*2);
                    wall.transform.position = wallPos;
                }
            }
        }
    }
}
