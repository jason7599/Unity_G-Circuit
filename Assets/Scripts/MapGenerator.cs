using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MapGenerator : MonoBehaviour
{
    private const int MapSize = 50;
    Room[,] Rooms = new Room[MapSize, MapSize];
    int[,] Maze = new int[MapSize * 2 + 1, MapSize * 2 + 1];
    private List<Tuple<int, int>> Visited;

    class Room
    {
        private int x;
        private int y;
        private Tuple<int, int> myDir;
        private List<Tuple<int, int>> dir = new List<Tuple<int, int>>();
        public List<Tuple<int, int>> randomizedDir = new List<Tuple<int, int>>();
        private Random _random = new Random();
        
        public Room(int x, int y)
        {
            this.x = x;
            this.y = y;
            this.myDir = new Tuple<int, int>(x, y);
            
            this.dir.Add(new Tuple<int, int>(x-1, y));
            this.dir.Add(new Tuple<int, int>(x+1, y));
            this.dir.Add(new Tuple<int, int>(x, y-1));
            this.dir.Add(new Tuple<int, int>(x, y+1));

            for (int idx = 4; idx > 0; idx--)
            {
                Tuple<int, int> chooseDir = this.dir[_random.Next(0, idx)];
                this.randomizedDir.Add(chooseDir);
                this.dir.Remove(chooseDir);
            }
        }

        public Tuple<int, int> GetCurrentPos()
        {
            return this.myDir;
        }

        public Tuple<int, int> GetNextPos()
        {
            Tuple<int, int> returnDir = randomizedDir[0];
            this.randomizedDir.Remove(returnDir);
            return returnDir;
        }
    }
    
    int[,] DrawMap()
    {
        for (int x = 0; x < Rooms.Length; x++)
        {
            for (int y = 0; y < Rooms.Length; y++)
            {
                Rooms[x, y] = new Room(x, y);
            }
        }
        for (int x = 0; x < Maze.Length; x++)
        {
            for (int y = 0; y < Maze.Length; y++)
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
        while (currentRoom.randomizedDir != null)
        {
            Tuple<int, int> nextDir = currentRoom.GetNextPos();
            int nextX = nextDir.Item1;
            int nextY = nextDir.Item2;

            if ((0 <= nextX) && (nextX < MapSize) && (0 <= nextY) && (nextY < MapSize))
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
        DrawMap();
        GenerateMap();
    }

    private void GenerateMap()
    {

    }
}
