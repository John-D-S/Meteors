using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private float meteorSpawnHeight;
    [SerializeField] private float levelWidth;
    [SerializeField] private GameObject meteor;
    [SerializeField] private int maxMeteorSize = 3;
    [SerializeField] float secondsBetweenMeteorSpawning = 5;

    [SerializeField] private Vector2 playerSpawnPosition;
    [SerializeField] private GameObject Player;
    [SerializeField] private int playerSpawns = 3;
    public int PlayerSpawns
    {
        set
        {
            if (value > -1)
            {
                playerSpawns = value;
                if (livesDisplay)
                {
                    livesDisplay.text = $"Lives: {PlayerSpawns}";
                }
            }
            else
            {
                SceneManager.LoadScene("Main", LoadSceneMode.Single);                
            }
        }
        get
        {
            return playerSpawns;
        }
    }

    [SerializeField] private TextMeshProUGUI livesDisplay;
    private int score;
    [SerializeField] private TextMeshProUGUI scoreDisplay;
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector3)playerSpawnPosition, 1);
    }

    public void SpawnPlayer()
    {
        if (PlayerSpawns > -1)
        {
            GameObject player = Instantiate(Player, playerSpawnPosition, Quaternion.identity);
            Player playerComponent = player.GetComponent<Player>();
            if (playerComponent)
            {
                playerComponent.spawner = this;
            }

            PlayerSpawns -= 1;
        }
        else
        {
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }

    public void AddToScore(int amount)
    {
        score += amount;
        scoreDisplay.text = $"Score: {score}";
    }

    public IEnumerator SpawnMeteors()
    {
        while (true)
        {
            GameObject instantiatedMeteor = Instantiate(meteor,
                Vector3.up * meteorSpawnHeight + Vector3.right * Random.Range(-levelWidth, levelWidth),
                Quaternion.identity);
            Meteor instantiatedMeteorComponent = instantiatedMeteor.GetComponent<Meteor>();
            if (instantiatedMeteorComponent)
            {
                instantiatedMeteorComponent.Size = Random.Range(1, maxMeteorSize);
                instantiatedMeteorComponent.spawner = this;
            }
            yield return new WaitForSeconds(secondsBetweenMeteorSpawning);
        }
    }
    
    private void Start()
    {
        SpawnPlayer();
        StartCoroutine(SpawnMeteors());
        //PlayerSpawns = playerSpawns;
        AddToScore(0);
    }
}
