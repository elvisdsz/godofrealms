using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BridgeController : MonoBehaviour
{
    private GateController gate;
    private TilemapRenderer tilemap;
    private BridgeController bridgeController;
    private PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<TilemapRenderer>();
        tilemap.enabled = false;
        bridgeController = GetComponent<BridgeController>();
        player = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableBridge()
    {
        tilemap.enabled = true;
        for(int i = 0; i < this.transform.childCount; i++)
        {
            GameObject gO = this.transform.GetChild(i).gameObject;
            GameObject.Destroy(gO);
        }
    }
}
