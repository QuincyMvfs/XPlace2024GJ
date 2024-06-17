using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioReferences : MonoBehaviour
{
    

    public static AudioReferences Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null) { Debug.Log("One too many audio managers"); }

        Instance = this;
    }

    public void PlaySoundAtLocation(Vector3 location, AudioSource audio)
    {
    }
}
