using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : Typer
{
    private GameManagerScript gameManager;
    private Rigidbody2D gateRigidbody;
    private BridgeController bridgeController;
    private PlayerMovement player;
    public bool IsTutorial = false;
    public bool IsGoldGate = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManagerScript._instance;
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
        if(player.powerupData.PowerupValue(PowerupData.PowerupType.BUILD_BRIDGE) >= 0.5f || (IsTutorial && player.GetRealm().acquired))
        {
            if(IsGoldGate)
            {
                if(player.GetRealm().acquired)
                {
                    bridgeController.EnableBridge();
                    gameManager.OpenGoldGate();
                    AudioManager.instance.Play("OpenGoldenGate");
                }
                else
                {
                    StartCoroutine(base.ChangeWarningColor());
                    Debug.Log("have to acquire realm to open golden gate");
                    Subtitle._instance.ShowSubtitle("You have to acquire this realm to open golden gate.", 3f);
                }
            }
            else
            {
                bridgeController.EnableBridge();
                AudioManager.instance.Play("OpenGate");
            }
        }
        else
        {
            StartCoroutine(base.ChangeWarningColor());
            Debug.Log("can't build bridge");
            AudioManager.instance.Play("AccessDenied");
            if(!IsTutorial && player.powerupData.PowerupValue(PowerupData.PowerupType.BUILD_BRIDGE) < 0.5f)
                gameManager.BuildBridgePowerLow();
        }
    }
}
