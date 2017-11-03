using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WiiBalanceBoardInputOutput : MonoBehaviour {
	
	public AudioClip[] BGMList ; //1. intro 2. ingame 3.boss 4.asSoonStart 5.stay 6.start_vo 7. jump 8.damage 9.die 10.dieorlive
	public GameObject MainBGM ;
	AudioSource BGM;
	AudioSource BGM_E;
	public int index = 0;

	[SerializeField]
	protected WiiBalanceBoardCliant wiiBalanceBoardCliant;

	[SerializeField]
	protected BalanceBoardData balanceBoardData;

	protected void Start(){
		BGM = MainBGM.GetComponent<AudioSource> ();
		BGM_E = GetComponent<AudioSource>();
		if (wiiBalanceBoardCliant == null) {
			Debug.LogError ("WiiBalanceBoardCliant is null");
		}
		BalanceList [11] = (balanceBoardData.sensorLoad.TopRight + balanceBoardData.sensorLoad.TopLeft + balanceBoardData.sensorLoad.BottomLeft + balanceBoardData.sensorLoad.BottomRight) / 4;

		ShowText.text =" 발을 올리면 시작합니다 ";

		BGM.clip = BGMList [0];
		BGM.Play();
		BGM_E.clip = BGMList[3];
		BGM_E.Play();

	}

	void Update () {
		if (!GetInputData ()) {
			
			return;
		}
		Output ();

		if (BalanceList [11] < BalanceList [6]) {
			BalanceList [11] = BalanceList [6];
		}
	}

	bool GetInputData ()
	{
		
		if (wiiBalanceBoardCliant == null) {
			Debug.LogError ("BalanceBoardCliant is null");
			return false;
		}
	
		if (wiiBalanceBoardCliant.recvBalanceBoardDatalist.balanceBoardData == null) {
			Debug.LogError ("Error");
			return false;
		}

		if (!(index < wiiBalanceBoardCliant.recvBalanceBoardDatalist.balanceBoardData.Count)) {
			Debug.LogWarning (" index :" 
				+ index
				+ " count:"
				+ wiiBalanceBoardCliant.recvBalanceBoardDatalist.balanceBoardData.Count);
			return false;
		}

	
		balanceBoardData = wiiBalanceBoardCliant.recvBalanceBoardDatalist.balanceBoardData [index];
		return true;
	}



	//-----------------------------------------------------------------------------------------------------------

	//-----------------------------------------------------------------------------------------------------------

	//-----------------------------------------------------------------------------------------------------------

	//-----------------------------------------------------------------------------------------------------------

	//-----------------------------------------------------------------------------------------------------------

	public Text ShowText;


	float Timer = 10.0f;
	float Count = 0.0f;
	public bool isStart = false;
	public bool isBalance = false;
	public float Speed = 1.0f;
	public bool isGround = true; // 땅 위 있을경우
	public bool isLimit1 = false;
	public bool isLimit2 = false;
	public bool isBoss = false;
	public Animator BossAnim;

	float[] BalanceList = new float[12]; // 평균 배열  [6]은 전체 평균 , [7]은 상단 평균 , [8]은 하단 평균 , [9]는 왼쪽 평균 , [10] 오른쪽 평균 ,[11] 실시간 평균

	public void wiiController(){
		if (isStart == false) {
		


			if (((balanceBoardData.sensorLoad.TopRight + balanceBoardData.sensorLoad.TopLeft + balanceBoardData.sensorLoad.BottomLeft + balanceBoardData.sensorLoad.BottomRight) / 4) >= (5.0f + BalanceList [11])) {
			
				isStart = true;
			}
		}
			
		if (isBalance == false && isStart == true) {
			Count += Time.deltaTime;

			if (Count >= 2.0f && Count <= 2.02f) {
				ShowText.text =" 발판의 평균을 구합니다 ";
				print ("발판의 평균을 구합니다.");
			}
			if (Count >= 5.0f && Count <= 5.02f) {
				BGM_E.clip = BGMList [4];
				BGM_E.Play ();
					
				BalanceList[0] = (balanceBoardData.sensorLoad.TopRight + balanceBoardData.sensorLoad.TopLeft + balanceBoardData.sensorLoad.BottomLeft + balanceBoardData.sensorLoad.BottomRight) / 4;
				print ("첫번째 평균을 추출 합니다. " + BalanceList [0]);
				ShowText.text =" 움직이지 마세요 ";
			}
			if (Count >= 6.0f && Count <= 6.02f) {
				BalanceList[1] = (balanceBoardData.sensorLoad.TopRight + balanceBoardData.sensorLoad.TopLeft + balanceBoardData.sensorLoad.BottomLeft + balanceBoardData.sensorLoad.BottomRight) / 4;
				print ("두번째 평균을 추출 합니다. " + BalanceList [1]);
			}
			if (Count >= 7.0f && Count <= 7.02f) {
				BalanceList[2] = (balanceBoardData.sensorLoad.TopRight + balanceBoardData.sensorLoad.TopLeft + balanceBoardData.sensorLoad.BottomLeft + balanceBoardData.sensorLoad.BottomRight) / 4;
				print ("세번째 평균을 추출 합니다. " + BalanceList [2]);
			}
			if (Count >= 8.0f && Count <= 8.02f) {
				BalanceList[3] = (balanceBoardData.sensorLoad.TopRight + balanceBoardData.sensorLoad.TopLeft + balanceBoardData.sensorLoad.BottomLeft + balanceBoardData.sensorLoad.BottomRight) / 4;
				print ("네번째 평균을 추출 합니다. " + BalanceList [3]);
			}
			if (Count >= 9.0f && Count <= 9.02f) {
				BalanceList[4] = (balanceBoardData.sensorLoad.TopRight + balanceBoardData.sensorLoad.TopLeft + balanceBoardData.sensorLoad.BottomLeft + balanceBoardData.sensorLoad.BottomRight) / 4;
				print ("다섯번째 평균을 추출 합니다. " + BalanceList [4]);
			}
			if (Count >= Timer) {
				ShowText.text =" 데이터 평균값 추출 완료! 게임을 시작합니다. ";
				print ("데이터 평균값 추출 완료");
				BalanceList [5] = (BalanceList [0] + BalanceList [1] + BalanceList [2] + BalanceList [3] + BalanceList [4] ) / 5;
				print ("평균값 : " + BalanceList[5]);
				isBalance = true;
				BGM.clip = BGMList [1];
				BGM.Play ();
				BGM_E.clip = BGMList [5];
				BGM_E.Play ();	
			}
		
		}

		if (isBalance == true && isGround == true) {
			
			ShowText.text ="";
			BalanceList [9] = (balanceBoardData.sensorLoad.TopLeft + balanceBoardData.sensorLoad.BottomLeft) / 2;
			BalanceList [10] = (balanceBoardData.sensorLoad.TopRight + balanceBoardData.sensorLoad.BottomRight) / 2;
			BalanceList [11] = (balanceBoardData.sensorLoad.TopRight + balanceBoardData.sensorLoad.TopLeft + balanceBoardData.sensorLoad.BottomLeft + balanceBoardData.sensorLoad.BottomRight) / 4;


				if (isLimit1 == true) {
					if ((BalanceList [9] - BalanceList [5]) >= (BalanceList [5] / 3) && (BalanceList [5] - BalanceList [10]) >= (BalanceList [5] / 4)) {
						this.transform.Translate (0, 0, 0);
					} 

					if ((BalanceList [10] - BalanceList [5]) >= (BalanceList [5] / 3) && (BalanceList [5] - BalanceList [9]) >= (BalanceList [5] / 4)) {
						this.transform.Translate (Speed * Time.deltaTime, 0, 0);
					}
				}

				if (isLimit2 == true) {
					if ((BalanceList [9] - BalanceList [5]) >= (BalanceList [5] / 3) && (BalanceList [5] - BalanceList [10]) >= (BalanceList [5] / 4)) {
						this.transform.Translate (-Speed * Time.deltaTime, 0, 0);
					} 

					if ((BalanceList [10] - BalanceList [5]) >= (BalanceList [5] / 3) && (BalanceList [5] - BalanceList [9]) >= (BalanceList [5] / 4)) {
						this.transform.Translate (0, 0, 0);
					}
				}

				if (isLimit1 == false && isLimit2 == false) {
					if ((BalanceList [9] - BalanceList [5]) >= (BalanceList [5] / 3) && (BalanceList [5] - BalanceList [10]) >= (BalanceList [5] / 4)) {
						this.transform.Translate (-Speed * Time.deltaTime, 0, 0);
					}

					if ((BalanceList [10] - BalanceList [5]) >= (BalanceList [5] / 3) && (BalanceList [5] - BalanceList [9]) >= (BalanceList [5] / 4)) {
						this.transform.Translate (Speed * Time.deltaTime, 0, 0);
					}
				}

				if ((BalanceList [11] - BalanceList [5]) >= (BalanceList [5] / 2)) {
					BGM_E.clip = BGMList [6];
					BGM_E.Play ();
					//this.GetComponent<Rigidbody> ().velocity = Vector3.up * 6.0f;
					this.GetComponent<Rigidbody> ().MovePosition (transform.position + Vector3.up * 6.0f);
					isGround = false;
				}
			}
			
			
			



	}



	protected virtual void Output ()
	{
		wiiController ();
	}



	void OnCollisionEnter(Collision coll){
		if (coll.transform.tag == "map") {
			isGround = true;
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Limit") {
			isLimit1 = true;
		}

		if (col.gameObject.tag == "Limit2") {
			isLimit2 = true;
		}

	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.tag == "Limit") {
			isLimit1 = false;
		}
		if (col.gameObject.tag == "Limit2") {
			isLimit2 = false;
		}
	}
}
