using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public Food food;
    public Transform spawnArea;
    public int numberOfFoodToSpawn;
    public float timeBetweenSpawns;

    int foodRemainingToSpawn;
    int numberOfActiveFood;
    float nextSpawnTime;


    private void Start()
    {
        NextSpawn();
    }

    private void Update()
    {
        if(foodRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            print("Spawn Food");
            foodRemainingToSpawn--;
            nextSpawnTime = Time.time + timeBetweenSpawns;

            float boundsX = spawnArea.localScale.x / 2;
            float boundsZ = spawnArea.localScale.z / 2;
            print(boundsX);
            Food spawnedFood = Instantiate(food, new Vector3(Random.Range(boundsX, -boundsX), 0.75f, Random.Range(boundsZ, -boundsZ)), Quaternion.identity) as Food;
            spawnedFood.OnEat += OnFoodEat;
        }
    }

    public void NextSpawn()
    {
        foodRemainingToSpawn = numberOfFoodToSpawn;
        numberOfActiveFood = numberOfFoodToSpawn;
    }

    void OnFoodEat()
    {
        print("Food Eaten");
        numberOfActiveFood--;

        if(numberOfActiveFood == 0)
        {
            NextSpawn();
        }
    }

}
