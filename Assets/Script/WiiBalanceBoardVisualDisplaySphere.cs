using UnityEngine;
using System.Collections;

public class WiiBalanceBoardVisualDisplaySphere : WiimoteInfoDisplayBase 
{
	protected float gainScale = 0.01f;
	//값 변환[cm-> m 1/100]


	// Use this for initialization
	override protected void Output()
	{
		
		Vector3 localPos = new Vector3
			(balanceBoardData.copPos.y * gainScale ,
				gameObject.transform.localPosition.y,
				balanceBoardData.copPos.x * gainScale); 
		gameObject.transform.localPosition = localPos;

        
	}
}
