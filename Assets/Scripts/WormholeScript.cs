using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormholeScrpt : MonoBehaviour
{
    private PlayerController playerShipController;
    public GameObject Destination;
    public string PortalName;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == playerShipController.gameObject.name )
        {
            playerShipController.transform.position = Destination.transform.position;
        }
    }
    void Start()
    {
        playerShipController = GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }
}
