using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton Code
    public static ObjectPooling Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool tempPool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < tempPool.size; i++)
            {
                GameObject obj = Instantiate(tempPool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(tempPool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag_, Vector3 position_, Quaternion rotation_)
    {
        if (!poolDictionary.ContainsKey(tag_))
        {
            Debug.Log("The tag " + tag_ + " doesn't work");

            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag_].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position_;
        objectToSpawn.transform.rotation = rotation_;

        poolDictionary[tag_].Enqueue(objectToSpawn);

        //return the object
        return objectToSpawn;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
