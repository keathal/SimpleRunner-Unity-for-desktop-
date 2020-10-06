using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public class TileManager : MonoBehaviour {
    public GameObject[] tilePrefabs;
    Transform playerTransform;
    float spawnZ = 0.0f;
    float tileLength = 20.0f;
    int amountTiles = 5;
    List<GameObject> activeTiles;
    int lastPrefabIndex=0;
	// Use this for initialization
	void Start () {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        activeTiles = new List<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
		if(playerTransform.position.z > (spawnZ-amountTiles*tileLength))
        {
            SpawnTile();
        }
       if(activeTiles.Count > amountTiles+1)
        {
            DeleteTile();
        }
    }
    void SpawnTile()
    {
        GameObject go;
        go = Instantiate(tilePrefabs[RandomIndex()]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(go);
        GameObject.FindGameObjectWithTag("Water").transform.position += new Vector3(0,0,20);
    }
    void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
    int RandomIndex()
    {
        if (tilePrefabs.Length <= 1)
            return 0;
        int RandomIndex = lastPrefabIndex;
        while(RandomIndex==lastPrefabIndex)
        {
            RandomIndex = Random.Range(0, tilePrefabs.Length);
        }
        lastPrefabIndex = RandomIndex;
        return RandomIndex;
    }


    public void Restart(string name)
    {
        Application.LoadLevel(name);
    }
    
}
