using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WiiBalanceBoardMessageDisplay : WiiBalanceBoardDisplayTextBase
{
	string recvMessage = "";
	//float weight = 0f;
	//Vector2 copPos;
	override protected void Output(){
		recvMessage = wiiBalanceBoardCliant.recvBalanceBoardDatalist.message; 
		text.text = "ReceievedMessage:" + recvMessage;
	}

}