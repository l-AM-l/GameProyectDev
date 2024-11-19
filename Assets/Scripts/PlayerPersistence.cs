using System;
using UnityEngine;

public class PlayerPersistence : MonoBehaviour
{
    private void Start(){
        transform.position = new Vector3(PlayerPrefs.GetFloat("x"), PlayerPrefs.GetFloat("y"),PlayerPrefs.GetFloat("z"));
    }

    private void Update(){
        PlayerPrefs.SetFloat("x", transform.position.x);
        PlayerPrefs.SetFloat("y", transform.position.y);
        PlayerPrefs.SetFloat("z", transform.position.z);
    }
}
