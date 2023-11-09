using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKill : MonoBehaviour
{
    
    public GameObject spawn;
    public float timeSpawn;
    private float spawnTime;

    bool isFinished;

    private void Start()
    {
        StartCoroutine(Die());
        timeSpawn = 7f;
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(12f);
        isFinished = true;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if(!isFinished && spawnTime > timeSpawn)
        {
            Instantiate(spawn, this.gameObject.transform);
            timeSpawn = 0.5f;
            spawnTime = 0f;
        }
    }
}
