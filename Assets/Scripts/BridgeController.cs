using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BridgeController : MonoBehaviour
{
    private GateController gate;
    private TilemapRenderer tilemap;
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<TilemapRenderer>();
        tilemap.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableBridge(GateController gate)
    {
        tilemap.enabled = true;
        GameObject.Destroy(gate.gameObject);
    }
}
