using UnityEngine;
using System.Collections;


public class WiimoteInfoDisplayBase: MonoBehaviour
{

	public int index = 0;

	[SerializeField]
	protected WiiBalanceBoardCliant wiiBalanceBoardCliant;

	[SerializeField]
	protected BalanceBoardData balanceBoardData;

	//[Wii Controller informations]
//	protected BalanceBoardData 

	// Use this for initialization
	protected void Start ()
	{

//		wiiBalanceBoardCliant = wiiBalanceBoardCliantObject.GetComponent<WiiBalanceBoardCliant> ();
		if (wiiBalanceBoardCliant == null) {
			Debug.LogError ("WiiBalanceBoardCliant is null");
		}
	}

	// Update is called once per frame
	void Update ()
	{

		if (!GetInputData ()) {

			return;
		}

		Output ();
	}

	//
	/// <summary>
	/// Gets the data.
	/// </summary>
	/// <returns><c>true</c>, if data was gotten, <c>false</c> otherwise.</returns>
	bool GetInputData ()
	{

		if (wiiBalanceBoardCliant == null) {
			Debug.LogError ("BalanceBoardCliant is null");
			return false;
		}

		if (wiiBalanceBoardCliant.recvBalanceBoardDatalist.balanceBoardData == null) {
			Debug.LogError ("목록이 없습니다.");
			return false;
		}
		//컨트롤러 체크 
		if (!(index < wiiBalanceBoardCliant.recvBalanceBoardDatalist.balanceBoardData.Count)) {
			Debug.LogWarning ("장치 수 부족 index(컨트롤러 번호[0시작]):" 
				+ index
				+ " count（총 컨트롤러 수）:"
				+ wiiBalanceBoardCliant.recvBalanceBoardDatalist.balanceBoardData.Count);
			return false;
		}

		//데이터 수신 여부 
		balanceBoardData = wiiBalanceBoardCliant.recvBalanceBoardDatalist.balanceBoardData [index];
		return true;
	}

	//자식 클래스
	protected virtual void Output ()
	{
	}

}
