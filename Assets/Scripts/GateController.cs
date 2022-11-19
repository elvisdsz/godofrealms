using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : Typer
{
    private Rigidbody2D gateRigidbody;
    private BridgeController bridgeController;
    //public RealmManager realmManager;
    // Start is called before the first frame update
    void Start()
    {
        //realmManager = GetComponent<RealmManager>();
        //string pickedWord = WordBank.PickWord(realmManager, 1);
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
        bridgeController.EnableBridge(this);
    }
}
