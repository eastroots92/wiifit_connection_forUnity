using UnityEngine;
using System.Collections;
using System;



public class WiimoteServerLancher : MonoBehaviour
{

	System.Diagnostics.Process wiimoteServerProcess;

	public bool hideServerApplicationWindow = false;

	public bool lunchTheServerProcess = true;

	public string argments = "";


	public string applicationPath = "/WiimoteServer/WiimoteTest.exe";

	void Awake ()
	{
		if(!lunchTheServerProcess)
			return;
		
		string dir = "";
		string filepath = "";				
		if (Application.isEditor) {
			dir = Application.dataPath + "/..";
		} else {
			dir = Application.dataPath + "/..";
		}

		filepath = dir + applicationPath;	

		Debug.Log (filepath);

		wiimoteServerProcess = new System.Diagnostics.Process ();
		wiimoteServerProcess.StartInfo.FileName = filepath;
		wiimoteServerProcess.Exited += new EventHandler (wiimoteServerProcess_Exited);
		wiimoteServerProcess.EnableRaisingEvents = true;
		wiimoteServerProcess.StartInfo.Arguments = argments;
		if (hideServerApplicationWindow) {
			wiimoteServerProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		}
		//시작
		try {
			wiimoteServerProcess.Start ();
		} catch (Exception e) {
			//외부 프로세스를 시작할 수 없는 경우에 오류를 표시한다.
			Debug.LogError ("Failed to start process." + e.Message);
		}

	}


	//실행 종료시 외부 프로세스가 살아 있으면 자동 종료
	void OnApplicationQuit ()
	{
		if(!lunchTheServerProcess)
			return;
		
		if (wiimoteServerProcess.HasExited) {
			Debug.Log ("외부 프로세스를 종료함");
			return;
		}

		//외부 프로세스가 움직이고 있으면 종료시킨다.
		try {
			//메인 창 닫기
			wiimoteServerProcess.CloseMainWindow ();
			//프로세스 종료시 최대 2초 대기
			wiimoteServerProcess.WaitForExit (2);
			//프로세스 완료 유무 확인
			if (wiimoteServerProcess.HasExited) {
				Debug.Log ("외부 프로세스가 종료되었습니다.");
			} else {
				Debug.LogError ("외부 프로세스가 종료되지 않았습니다. 강제 종료시킵니다.");
				wiimoteServerProcess.Kill ();
			}
		} catch (Exception e) {
			Debug.LogError (e.Message);
		}

	}



	void wiimoteServerProcess_Exited (object sender, System.EventArgs e)
	{
		if(!lunchTheServerProcess)
			return;
		
		UnityEngine.Debug.Log ("WiimoteProssess_ExitEvent");
		wiimoteServerProcess.Dispose();		
	}




}
