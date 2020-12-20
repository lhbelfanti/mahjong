using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

	private int currentState;

	// Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
	    switch (currentState)
	    {
		    case 0:
			    enabled = false;
			    break;
		    case 1:
			    enabled = true;
			    break;
	    }
    }

    public int CurrentState
    {
	    get => currentState;
	    set => currentState = value;
    }
}
