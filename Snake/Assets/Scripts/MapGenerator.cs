using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Vector2 mapSize;
    
    public List<Coord> allTileCoords;
    Coord mapCentre;

    [Range(0,1)]
    public float outlinePercentage;

    private void Start() {
        GenerateMap();
    }

    public void GenerateMap() {

        allTileCoords = new List<Coord>();
        for(int x = 0; x < mapSize.x; x++) {
            for(int y = 0; y < mapSize.y; y++) {
                allTileCoords.Add(new Coord(x, y));
            }
        }

        mapCentre = new Coord((int)mapSize.x/2, (int)mapSize.y/2);

        string holderName = "Generated Map";

        if(transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }
        
        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for(int x = 0; x < mapSize.x; x++) {
            for(int y = 0; y < mapSize.y; y++) {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.one * 90));
                newTile.localScale = Vector3.one * (1-outlinePercentage);
                newTile.parent = mapHolder;
            }
        }

        bool[,] obstacleMap = new bool[(int)mapSize.x,(int)mapSize.y];

        for(int x = 0; x < mapSize.x; x++) {
            for(int y = 0; y < mapSize.y; y++) {
                if((x == 0 || x == mapSize.x - 1) || (y == 0 || y == mapSize.y - 1) && !obstacleMap[x, y])  {
                    Vector3 obstaclePosition = CoordToPosition(x, y);
                    Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity) as Transform;
                    newObstacle.parent = mapHolder;
                    obstacleMap[x, y] = true;
                }
            }
        }

    }

    Vector3 CoordToPosition(int x, int y) {
        return new Vector3(-mapSize.x/2 + 0.5f + x, 0, -mapSize.y/2 + 0.5f + y);
    }

    public struct Coord {
        public int x;
        public int y;

        public Coord(int _x, int _y) {
            x = _x;
            y = _y;
        }

        public static bool operator ==(Coord c1, Coord c2) {
            return c1.x == c2.x && c1.y == c2.y;
        }

        public static bool operator !=(Coord c1, Coord c2) {
            return !(c1 == c2);
        }
    }

}
