using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnSpawner : MonoBehaviour
{
    public GameObject[] leftColumnPrefabs;  
    public GameObject[] rightColumnPrefabs; 
    public float spawnRate = 2f;            
    public float columnSpeed = 5f;         
    public Transform leftSpawnPoint;        
    public Transform rightSpawnPoint;       

    private bool spawnOnLeft = true;      

    

    private void Start()
    {
        StartCoroutine(DelayedStartSpawning());     
    }

    IEnumerator DelayedStartSpawning()
{
    yield return new WaitForSeconds(2f); // Wait for 2 seconds after the game starts
    
    StartCoroutine(SpawnColumns()); // Start spawning columns
}

    IEnumerator SpawnColumns()
    {
    
        while (true)
        {
            if (spawnOnLeft)
            {
                SpawnColumn(leftColumnPrefabs, leftSpawnPoint);  
            }
            else
            {
                SpawnColumn(rightColumnPrefabs, rightSpawnPoint); 
            }

            spawnOnLeft = !spawnOnLeft;      
            yield return new WaitForSeconds(spawnRate);  
        }
    }

    void SpawnColumn(GameObject[] columnPrefabs, Transform spawnPoint)
    {
        
        GameObject columnPrefab = columnPrefabs[Random.Range(0, columnPrefabs.Length)];

        
        GameObject column = Instantiate(columnPrefab, spawnPoint.position, Quaternion.identity);

        // Move column down
        StartCoroutine(MoveColumnDown(column));
    }

    IEnumerator MoveColumnDown(GameObject column)
    {
        while (column.transform.position.y > -10)  
        {
            column.transform.Translate(Vector3.down * columnSpeed * Time.deltaTime);
            yield return null;  
        }

        
        Destroy(column);
    }
}
