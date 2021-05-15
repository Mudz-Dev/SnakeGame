using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour
{

    public Map[] maps;
    public int mapIndex;
    public Transform tilePrefab;
    public Transform obstaclePrefab;
    public Food foodPrefab;
    public Food superFoodPrefab;
    
    public List<Coord> allTileCoords;
    Queue<Coord> shuffledTileCoords;
    bool[,] obstacleMap;
    bool[,] foodMap;

    public float timeBetweenSpawns;
    public float timeBetweenSuperFoodSpawns;

    float nextSuperFoodTime;
    float superFoodDespawnTime;

    int numberOfActiveFood;
    int numberOfActiveSuperFood;


    [Range(0,1)]
    public float outlinePercentage;

    [Range(0, 10)]
    public int foodSpawnAmount = 3;

    Map currentMap;

    private void Start() {
        GenerateMap();
        StartCoroutine(SpawnFood());
        nextSuperFoodTime = nextSuperFoodTime = GetNextSuperFoodTime();
    }

    public void GenerateMap() {
        currentMap = maps[mapIndex];
        
        allTileCoords = new List<Coord>();
        for(int x = 0; x < currentMap.mapSize.x; x++) {
            for(int y = 0; y < currentMap.mapSize.y; y++) {
                allTileCoords.Add(new Coord(x, y));
            }
        }

        string holderName = "Generated Map";

        if(transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);            
        }
        
        Transform mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for(int x = 0; x < currentMap.mapSize.x; x++) {
            for(int y = 0; y < currentMap.mapSize.y; y++) {
                Vector3 tilePosition = CoordToPosition(x, y);
                Transform newTile = Instantiate(tilePrefab, tilePosition, Quaternion.Euler(Vector3.one * 90));
                newTile.localScale = Vector3.one * (1-outlinePercentage);
                newTile.parent = mapHolder;
            }
        }

        //Spawn Obstacles
        obstacleMap = new bool[(int)currentMap.mapSize.x,(int)currentMap.mapSize.y];

        for(int x = 0; x < currentMap.mapSize.x; x++) {
            for(int y = 0; y < currentMap.mapSize.y; y++) {
                if((x == 0 || x == currentMap.mapSize.x - 1) || (y == 0 || y == currentMap.mapSize.y - 1) && !obstacleMap[x, y])  {
                    Vector3 obstaclePosition = CoordToPosition(x, y);
                    Transform newObstacle = Instantiate(obstaclePrefab, obstaclePosition  + Vector3.up * 0.5f, Quaternion.identity) as Transform;
                    newObstacle.parent = mapHolder;
                    obstacleMap[x, y] = true;
                }
            }
        }

    }

    Vector3 CoordToPosition(int x, int y) {
        return new Vector3(-currentMap.mapSize.x/2 + 0.5f + x, 0, -currentMap.mapSize.y/2 + 0.5f + y);
    }

    public List<Coord> GetAvailableTileCoords() {
        List<Coord> availableCoords = new List<Coord>();

        for(int x = 0; x < currentMap.mapSize.x; x++) {
            for(int y = 0; y < currentMap.mapSize.y; y++) {
                Coord availableCoord = new Coord(x, y);
                if(!obstacleMap[x,y] && !foodMap[x,y] && currentMap.mapCentre != availableCoord) {
                    availableCoords.Add(availableCoord);
                }

            }
        }

        return availableCoords;

    }

    public IEnumerator SpawnFood() {
        foodMap = new bool[(int)currentMap.mapSize.x,(int)currentMap.mapSize.y];

        string holderName = "Spawned Food";

        DespawnFood(holderName);
        
        Transform foodHolder = new GameObject(holderName).transform;
        foodHolder.parent = transform;
        
        int seed = UnityEngine.Random.Range(1, 100);
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray<Coord>(GetAvailableTileCoords().ToArray(), seed));
        for(int i = 0; i < foodSpawnAmount; i++) {
                Coord randomCoord = shuffledTileCoords.Dequeue();

                Vector3 foodPosition = CoordToPosition(randomCoord.x, randomCoord.y);
                Food newFood = Instantiate(foodPrefab, foodPosition  + Vector3.up * 0.5f, Quaternion.identity) as Food;
                           
                newFood.foodType = Food.FoodTypes.Normal;
                newFood.transform.parent = foodHolder;

                Food newFoodScript = newFood.GetComponent<Food>();
                newFoodScript.OnEat = OnFoodEat;
                newFoodScript.OnSuperFoodEat = OnSuperFoodEat;

                foodMap[randomCoord.x, randomCoord.y] = true;
                numberOfActiveFood++;

                
                shuffledTileCoords.Enqueue(randomCoord);
                yield return new WaitForSeconds(timeBetweenSpawns);
        }

    }

    public void SpawnSuperFood() {
        numberOfActiveSuperFood = 0;
        foodMap = new bool[(int)currentMap.mapSize.x,(int)currentMap.mapSize.y];

        string holderName = "Spawned Super Food";

        DespawnFood(holderName);
        
        Transform foodHolder = new GameObject(holderName).transform;
        foodHolder.parent = transform;
        
        int seed = UnityEngine.Random.Range(1, 100);
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray<Coord>(GetAvailableTileCoords().ToArray(), seed));

        Coord randomCoord = shuffledTileCoords.Dequeue();

        Vector3 foodPosition = CoordToPosition(randomCoord.x, randomCoord.y);
        Food newFood = Instantiate(superFoodPrefab, foodPosition  + Vector3.up * 0.5f, Quaternion.identity) as Food;           
        newFood.foodType = Food.FoodTypes.SuperFood;
        newFood.transform.parent = foodHolder;

        Food newFoodScript = newFood.GetComponent<Food>();
        newFoodScript.OnEat = OnFoodEat;
        newFoodScript.OnSuperFoodEat = OnSuperFoodEat;

        foodMap[randomCoord.x, randomCoord.y] = true;

        numberOfActiveSuperFood++;
        superFoodDespawnTime = GetNextSuperDespawnTime();
        
        shuffledTileCoords.Enqueue(randomCoord);

    }


    void OnFoodEat()
    {
        print("Food Eaten");
        numberOfActiveFood--;

        if(numberOfActiveFood == 0)
        {
            StartCoroutine(SpawnFood());
        }
    }

    void OnSuperFoodEat() {
        print("Super Food Eaten");
        numberOfActiveSuperFood--;
    }

    void DespawnFood(string holderName) {
        if(transform.Find(holderName))
        {
            Destroy(transform.Find(holderName).gameObject);
        }
    }

    float elapsed;
    void Update() {
        elapsed = Time.deltaTime + elapsed;

        if(elapsed > nextSuperFoodTime) {
            elapsed = 0;
            nextSuperFoodTime = GetNextSuperFoodTime();

            if(numberOfActiveSuperFood == 0) {
                SpawnSuperFood();
            }
        }

        if(elapsed > superFoodDespawnTime && numberOfActiveSuperFood > 0) {
            elapsed = 0;
            nextSuperFoodTime = GetNextSuperFoodTime();
            numberOfActiveSuperFood = 0;
            DespawnFood("Spawned Super Food");
        }

    }

    float GetNextSuperFoodTime() {
        return Time.deltaTime + UnityEngine.Random.Range(4, 10);
    }

    float GetNextSuperDespawnTime() {
        return Time.deltaTime + UnityEngine.Random.Range(3, 9);
    }

    public Coord GetRandomFoodCoord() {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    [System.Serializable]
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
        public static bool Equals(Coord c1, Coord c2)
        {

            // TODO: write your implementation of Equals() here
            throw new System.NotImplementedException();

        }  
        
    }

    [System.Serializable]
    public class Map {

        public Coord mapSize;
        [Range(0, 1)]
        public float obstaclePercent;
        public int seed;
        public Color foregroundColour;
        public Color backgroundColour;

        public Coord mapCentre {
            get {
                return new Coord(mapSize.x/2, mapSize.y/2);
            }
        }

    }

}
