using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WiiBalanceBoardDisplayTextBase : WiimoteInfoDisplayBase
{
	protected Text text;

	new void Start ()
	{
		//GetComponents
		text = this.GetComponent<Text> ();
		if (text == null) {
			Debug.LogWarning ("Text is null.[object]" + this.name );
		}
		base.Start();
	}

}
