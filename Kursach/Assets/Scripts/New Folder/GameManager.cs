using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform respawnDot;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float respawnTime;

    private float respawnTimeStart;

    private bool respawn;

    private CinemachineVirtualCamera Cam;

    private void Start()
    {
        Cam = GameObject.Find("Player Camera").GetComponent<CinemachineVirtualCamera>() ;
    }

    private void Update()
    {
        RespawnCheck();
    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    private void RespawnCheck()
    {
        if(Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, respawnDot);
            Cam.m_Follow = playerTemp.transform;
            respawn = false;
        }
    }
}
