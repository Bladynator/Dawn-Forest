using UnityEngine;
using System.Collections;

public class Tool : Items 
{
    bool holding = false;
    
	
	void Start () 
	{
		
	}
	
	void Update () 
	{

	}

    public override void Activate()
    {
        base.Activate();
        holding = true;
    }
}
