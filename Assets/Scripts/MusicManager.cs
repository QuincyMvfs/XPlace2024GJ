using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip _victoryMusic;
    private AudioClip _currentMusic;
    private AudioSource _source;

    private void Awake()
    {
        _source = GetComponent<AudioSource>();
        _source.loop = true;
        _source.Play();

        EndOfLevel endOfLevel = new EndOfLevel();
        endOfLevel = FindObjectOfType<EndOfLevel>();
        if (endOfLevel != null)
        {
            endOfLevel.LevelFinished.AddListener(PlayVictoryMusic);
        }
    }

    public void PlayVictoryMusic()
    {
        _source.clip = _victoryMusic;
        _source.loop = false;
        _source.Play();
    }
}
