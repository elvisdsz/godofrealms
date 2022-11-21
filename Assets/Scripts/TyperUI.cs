using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TyperUI : Typer
{
    public string typerWord;
    public UnityEvent functionOnWordComplete;

    // Start is called before the first frame update
    void Start()
    {
        this.ignoreCircleCheck = true;
        Init(typerWord);
    }

    // Update is called once per frame
    void Update()
    {
        TyperUpdate();
    }

    public override void WordCompleted()
    {
        functionOnWordComplete.Invoke();
    }

    public void TestPrint()
    {
        Debug.Log("It's working!!");
    }

}
