using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistCanvas : MonoBehaviour
{
    private static GameObject[] persistentObjects = new GameObject[3];
    public int objectIndex;
    private void Awake()
    {
        if(persistentObjects[objectIndex] == null){
            persistentObjects[objectIndex]=gameObject;
            DontDestroyOnLoad(gameObject);

        }
        else if(persistentObjects[objectIndex]!=gameObject){
            Destroy(gameObject);

        }
    }
}

