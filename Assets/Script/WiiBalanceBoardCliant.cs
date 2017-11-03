using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

//데이터 번호의 정의
//각 번호 순으로 메시지가 전달된다.
enum BalanceBoardInfo
{
    index,                  //
    Weight,                 //
    CoPPosX,                //
    CoPPosY,                //
    SensorLoadTopRight,     //
    SensorLoadTopLeft,      //
    SenserLoadBottomRight,  //
    SensorLoadBottomLeft,       //
    EnumMaxNumber			//
}

//밸런스 보드 데이터 목록
public class BalanceBoardDataList
{

    public List<BalanceBoardData> balanceBoardData = new List<BalanceBoardData>();
    public string message;  //message형 String 변수

    //string 형의 message를 int형 데이터로 변환한다.
    public bool parseMessage(string message_in)
    {
        if (message_in == null)
        {
            Debug.LogError("TCP message is null");
            return false;
        }
        message = message_in;

        string[] stArryData = message_in.Split(',');
        //Debug.LogWarning(stArryData[0]);
        //Debug.Log("개수："+ stArryData.Length+"요소 :"+(int)WiiBalanceBoardInfo.EnumMaxNumber);
        if (stArryData.Length < (int)BalanceBoardInfo.EnumMaxNumber)
        {
            Debug.LogError("데이터의 개수가 잘못되었습니다. 데이터를 삭제합니다. 개수：" + stArryData.Length + "正しい要素数:" + (int)BalanceBoardInfo.EnumMaxNumber);
            //데이터 개수 에러
            return false;
        }

        int _index;
        //컨트롤러 번호 받아오기
        if (!int.TryParse(stArryData[(int)BalanceBoardInfo.index].ToString(), out _index))
            Debug.LogWarning("failed to parse.index:" + stArryData[(int)BalanceBoardInfo.index]);

        //새 컨트롤러 추가
        while (balanceBoardData.Count < _index + 1)
        {
            balanceBoardData.Add(new BalanceBoardData());
            Debug.Log("adding / data.Count:" + balanceBoardData.Count + " index");
        }


        //값을 변환
        //weight[kg]
        if (!float.TryParse(stArryData[(int)BalanceBoardInfo.Weight].ToString(), out balanceBoardData[_index].weight))
            Debug.LogWarning("failed to parse.wight:" + stArryData[(int)BalanceBoardInfo.Weight]);

        //Center of Pressure X[cm]
        if (!float.TryParse(stArryData[(int)BalanceBoardInfo.CoPPosX].ToString(), out balanceBoardData[_index].copPos.x))
            Debug.LogWarning("failed to parse.copX:" + stArryData[(int)BalanceBoardInfo.CoPPosX]);

        //Center of Pressure Y[cm]
        if (!float.TryParse(stArryData[(int)BalanceBoardInfo.CoPPosY].ToString(), out balanceBoardData[_index].copPos.y))
            Debug.LogWarning("failed to parse.copY:" + stArryData[(int)BalanceBoardInfo.CoPPosY]);

        //sensor weight data[]

        if (!float.TryParse(stArryData[(int)BalanceBoardInfo.SensorLoadTopRight].ToString(), out balanceBoardData[_index].sensorLoad.TopRight))
            Debug.LogWarning("failed to parse.SensorKgTopRight:" + stArryData[(int)BalanceBoardInfo.SensorLoadTopRight]);

        if (!float.TryParse(stArryData[(int)BalanceBoardInfo.SensorLoadTopLeft].ToString(), out balanceBoardData[_index].sensorLoad.TopLeft))
            Debug.LogWarning("failed to parse.SensorKgTopLeft:" + stArryData[(int)BalanceBoardInfo.SensorLoadTopLeft]);

        if (!float.TryParse(stArryData[(int)BalanceBoardInfo.SenserLoadBottomRight].ToString(), out balanceBoardData[_index].sensorLoad.BottomRight))
            Debug.LogWarning("failed to parse.SensorKgBottomRight:" + stArryData[(int)BalanceBoardInfo.SenserLoadBottomRight]);

        if (!float.TryParse(stArryData[(int)BalanceBoardInfo.SensorLoadBottomLeft].ToString(), out balanceBoardData[_index].sensorLoad.BottomLeft))
            Debug.LogWarning("failed to parse.SensorKgBottomLeft:" + stArryData[(int)BalanceBoardInfo.SensorLoadBottomLeft]);



        return true;
    }

}

//WiiBalanceBoard 각 센서에 걸려있는 무게


/// <summary>
/// 수신 데이터 나타내기
/// </summary>
[System.Serializable]
public class BalanceBoardData
{
    /// <summary>
    /// WiiBalanceBoard각 센서의 무게 
    /// </summary>
    [System.Serializable]
	public struct SensorLoad{
    
        public float TopRight;
        public float TopLeft;
        public float BottomRight;
        public float BottomLeft;
    }

    public float weight;        //무게[kgf]
    public Vector2 copPos;      //하중 중심[cm]
    public SensorLoad sensorLoad;		//WiiBalanceBoard각 센서의 무게（4개）
}

[System.Serializable]
public class WiiBalanceBoardCliant : MonoBehaviour
{
    //Socket tcpSocket;
    TcpClient tcpClient;
    Thread readThread;
    bool flg_continue = true;
    public string DistIPAddress = "127.0.0.1";                  //컴퓨터 IP 주소
    public int portNum = 8888;                                  //포트번호
    public int timeout = 2000;


    public BalanceBoardDataList recvBalanceBoardDatalist = new BalanceBoardDataList();

    // Use this for initialization
    void Start()
    {

        IPAddress serverIP = IPAddress.Parse(DistIPAddress);

        if (tcpClient != null)
            return;

        tcpClient = new TcpClient();
        tcpClient.ReceiveTimeout = 2000;    //２초 후 타임 아웃
        tcpClient.SendTimeout = 2000;       //２초 후 타임아웃
        tcpClient.Connect(serverIP, portNum);
        Debug.Log("init client");

        readThread = new Thread(new ThreadStart(recvTask));     //스레드에서 호출 함수 등록
        readThread.IsBackground = true;
        readThread.Start();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        flg_continue = false;
        //readThread종료 대기
        if (readThread != null)
        {
            Debug.Log("waiting for read thread kill.");
            readThread.Join();
            Debug.Log("read thread killed");
        }
        //TCP클라이언트(소켓) 닫기
        if (tcpClient != null)
        {
            Debug.Log("tcpClient Closing...");
            tcpClient.Close();
            Debug.Log("tcpClient Closed.");

        }


    }

    //수신 메시지를 처리
    private void recvTask()
    {
        NetworkStream ns = tcpClient.GetStream();
        if (ns == null)
        {
            Debug.LogError("tcpCliant.Getstream() is failed.");
            return;
        }
        Debug.Log("GetStream Success");
        //networkStream setup
        ns.ReadTimeout = 2000;
        ns.WriteTimeout = 2000;
        System.Text.Encoding enc = System.Text.Encoding.UTF8;


        byte[] resBytes = new byte[1];                                
                                                                        //스레드에서 돌리기
        while (flg_continue)
        {
            //읽기 쓰기 제한 시간을 10초로
            //기본 값을 무한으로 제한
            //(.NET Framework 2.0이상 필요)

            //서버 전송 데이터 수신하기
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            int resSize = 0;
            do
            {
                //데이터 일부를 받음
                //					Debug.Log ("ns.Read()");
                resSize = ns.Read(resBytes, 0, resBytes.Length);
                //					Debug.Log ("readed... " + resSize + " byte");

                if (!(resSize > 0))
                    break;                                          

                //수신한 데이터를 memoryStream에 축적
                ms.Write(resBytes, 0, resSize);

            } while (resBytes[resSize - 1] != '\n' && ns.DataAvailable);
				
            string resMsg = enc.GetString(ms.GetBuffer(), 0, (int)ms.Length);
            recvBalanceBoardDatalist.parseMessage(resMsg);
            //holdMessage(resMsg);
            ms.Close();

        }
        Thread.Sleep(1);
    }


}



