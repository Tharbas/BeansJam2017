using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISpawner : MonoBehaviour {

    public int MaxNpcs = 45;
    public GameObject NpcSceneRoot;

    [SerializeField]
    private GameObject[] citizenPrefabs;
    
    private bool doSpawn;
	// Use this for initialization
	void Start () {
        doSpawn = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (doSpawn)
        {
            doSpawn = false;

            AISystem aiSys = GetComponent<AISystem>();
            for (int i = 0; i < MaxNpcs; ++i)
            {
                Vector3 startPos = new Vector3(Random.Range(-255, 255), 0, Random.Range(-130, 130));
                NavMeshHit hit;
                NavMesh.SamplePosition(startPos, out hit, 50, 1);
                GameObject NpcPrefab = this.citizenPrefabs[UnityEngine.Random.Range(0, this.citizenPrefabs.Length)];
                GameObject spawnedNpc = Instantiate(NpcPrefab, NpcSceneRoot.transform, true);
                spawnedNpc.transform.SetPositionAndRotation(hit.position, Quaternion.identity);
                spawnedNpc.name = "NPC_" + i;
                aiSys.AddNpc(spawnedNpc.GetComponent<AIEntity>());
            }
        }
	}
}
