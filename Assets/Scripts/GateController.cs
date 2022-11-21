using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : Typer
{
    private Rigidbody2D gateRigidbody;
    private BridgeController bridgeController;
    private PlayerMovement player;
    public bool IsTutorial = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        gateRigidbody = GetComponent<Rigidbody2D>();    //Redundant but required
        string pickedWord = WordBank.PickGateWord();
        base.Init(pickedWord);
    }

    // Update is called once per frame
    void Update()
    {
        TyperUpdate();
    }

    public override void WordCompleted()
    {
        bridgeController = GetComponentInParent<BridgeController>();
        // Player has build-bridge powerup or tutorial realm is acquired
        if(player.powerupData.PowerupValue(PowerupData.PowerupType.BUILD_BRIDGE) == 1f || (IsTutorial && player.GetRealm().acquired))
            bridgeController.EnableBridge();
        else
        {
            StartCoroutine(base.ChangeWarningColor());
            Debug.Log("can't build bridge");
        }
    }
}
