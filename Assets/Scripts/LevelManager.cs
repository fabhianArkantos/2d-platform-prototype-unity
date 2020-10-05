using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Transform spawnPoint;
    public GameObject playerPrefab;
    public CinemachineVirtualCameraBase cam;

    private void Awake()
    {
        instance = this;
    }

    public void Respawn()
    {
        GameObject player = (GameObject) Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        player.name = "Player";
        cam.Follow = player.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
