using UnityEngine;
using System.Collections;

public class WiiBalanceBoardController : WiiBalanceBoardInputOutput {
	
	protected virtual void Output ()
	{
		wiiController ();
		print ("hello");
	}

}
