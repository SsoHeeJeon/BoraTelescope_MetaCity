using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System;
using System.Threading;
using System.Net;

public class bora_client_test : ReceiveServer
{
    TcpClient client = null; // ���� ������ ���� client ���� ����
    StreamReader reader = null;
    StreamWriter writer = null;

    string dataToSend = null;  // ������ ���� �����͸� �����ϱ� ���� ���� ����
    string dateFromReceive = null; // ������ ���� ���� �����͸� �����ϱ� ���� ���� ����

    public static bool FailControlSystem = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Connect_Button()
    {
        // ��� ���� �÷��� ���� �ޱ� ���� ���� ����� //
        Thread Server_For_Mode_Status_Flag = new Thread(Make_Server_For_Mode_Status_Flag);
        Server_For_Mode_Status_Flag.Start();

        // ���� ���� �ϱ� // 
        client = new TcpClient();

        try
        {
            client.Connect("localhost", 5003); // �ý��� ��Ʈ�ѷ� ���� ���� ���� �ּ�

            NetworkStream stream = client.GetStream();  // ���� ���� ����

            Encoding encode = Encoding.GetEncoding("utf-8");

            reader = new StreamReader(stream, encode); // ������ ���� �б� ���ؼ�

            writer = new StreamWriter(stream, encode) // ���� ������ ���ؼ� 
            { AutoFlush = true }; // ���۰� �������� �� �����Ͱ� �Ⱥ������� �־?

            // mode status flag �� ��������
            dataToSend = "GIVE_ME_MODE_STATUS_FLAG_AND_ClIENT_VER_ID";

            try
            {
                writer.WriteLine(dataToSend); // ������ �Է��� ������ ����
                dateFromReceive = reader.ReadLine(); // �������� ���� �����͸� string ������ ����
                var splitMsg = dateFromReceive.Split(',');
                // string client_ver_id = (splitMsg[0]);
                // ��� ���� �÷��� �� ���� //
                //mode_status_flag_string.text = dateFromReceive;
                ContentsInfo.ContentsVersion = splitMsg[0];
                SceneOnOff(splitMsg[1]);
            }
            catch (Exception ex) // ������ �۽��� �ȵ� �� ���� �߻�
            {
                Debug.Log(ex.ToString());
                Debug.Log("������ ���� ����");
                Debug.Log("���� ���� �Ұ���");
            }

            Debug.Log("���� ���� ���� ���� �Ϸ�");

        }
        catch (Exception ex) // ������ ������ �ȵ� �� ���� �߻�
        {
            Debug.Log(ex.ToString());
            Debug.Log("���� ���� �Ұ���");

        }

    }

    /// <summary>
    ///  Control System ���� ����
    /// </summary>
    public void Disconnect_Button()
    {
        dataToSend = "���� ����"; // ���� ����

        try
        {
            writer.WriteLine(dataToSend); // ������ �Է��� ������ ����

        }
        catch (Exception ex) // ������ �۽��� �ȵ� �� ���� �߻�
        {
            Debug.Log(ex.ToString());
            Debug.Log("������ ���� ����");
            Debug.Log("���� ���� �Ұ���");
        }
        finally
        {
            client.Close(); // ���� Ŭ���̾�Ʈ ����
            Debug.Log("���� Ŭ���̾�Ʈ ����");

            //Connect_Text.text = "���� ���� �Ϸ�";
            Debug.Log("���� ���� �Ϸ�");
        }

    }


    public void Send_Log_Button(string imp_log)
    {
        //imp_log = "1001"; // �ӽ� �׽�Ʈ ��
        //dataToSend = "LOG, " + imp_log;

        try
        {
            writer.WriteLine(imp_log); // ������ �Է��� ������ ����
                                       //writer.WriteLine(dataToSend); // ������ �Է��� ������ ����

            //Debug.Log(imp_log);
            //Send_Log_Text.text = imp_log;
        }
        catch (Exception ex) // ������ �۽��� �ȵ� �� ���� �߻�
        {
            Debug.Log(ex.ToString());
            Debug.Log("������ ���� ����");
            Debug.Log("���� ���� �Ұ���");
        }

    }

    public void Send_Error_Button(string imp_error)
    {
        //imp_error = "2001"; // �ӽ� �׽�Ʈ ��
        //string imp_ouccr_time = "2022-02-03 16:48:25.224"; // �ӽ� �׽�Ʈ ��

        //dataToSend = "ERROR, " + imp_error + ", " + imp_ouccr_time; // 

        try
        {
            writer.WriteLine(imp_error); // ������ �Է��� ������ ����
                                         //writer.WriteLine(dataToSend); // ������ �Է��� ������ ����

            Debug.Log(imp_error);
            //Send_Error_Text.text = imp_error;

        }
        catch (Exception ex) // ������ �۽��� �ȵ� �� ���� �߻�
        {
            Debug.Log(ex.ToString());
            Debug.Log("������ ���� ����");
            Debug.Log("���� ���� �Ұ���");
        }
    }

    public void Make_Server_For_Mode_Status_Flag()
    {
        TcpListener tcpListener = null; // ���� ���� ������ ����� ���ؼ� ����

        Socket clientsocket = null; // Ŭ���̾�Ʈ�� ���� ���� ������ ����� ���ؼ� ����

        NetworkStream stream = null; // stream ������ ���� ����

        StreamReader reader = null; // ������ �����͸� �б� ���� ����

        StreamWriter writer = null; // �۽��� �����͸� ���� ���� ����

        IPAddress ipAd = IPAddress.Parse("127.0.0.1"); // �����ּ� : ����ȣ��Ʈ IP�ּ� : 127.0.0.1

        tcpListener = new TcpListener(ipAd, 5004); // �ý��� ��Ʈ�ѷ��� ��� ���� �÷��׸� ������ �ʿ��� �� 5004 ��Ʈ�� �����Ѵ�. 

        tcpListener.Start(); // ���� ���� ���� -> �� ���� ����



        string strMsg = null; // ���ŵ� ���ڸ� �ޱ� ���� ���� ����


        while (true)
        {
            clientsocket = tcpListener.AcceptSocket(); // Ŭ���̾�Ʈ ���� ���

            Debug.Log("�ý��� ��Ʈ�ѷ��� ��� ���� �÷��� �� �������� ���� ����");

            stream = new NetworkStream(clientsocket); // �Է��� ������ ���� stream ����, stream�� ������ ���ؼ� �ְ� ���� �����Ͱ� ���� �Ǿ� �ִ�.

            Encoding encode = Encoding.GetEncoding("utf-8"); // �ѱ��� �ν��ϱ� ���� enode ����

            reader = new StreamReader(stream, encode); // ���� ���� ������ �б�

            writer = new StreamWriter(stream, encode) { AutoFlush = true }; // �۽��� ������ ����


            while (true)
            {
                try
                {
                    strMsg = reader.ReadLine(); // ���� ��� ����, ���� ������ str ������ ����

                    // ��� ���� �÷��� �� ����
                    if (strMsg.Contains("MODE_STATUS_FLAG"))  // ��ɾ� : MODE_STATUS_FLAG,[mode_status_flag]
                    {

                        var splitStr = strMsg.Split(','); // strMSG ���뿡�� , �������� ���� �迭�� ����

                        // ��� ���� �÷��� �� ���� //
                        //mode_status_flag_string.text = splitStr[1];
                        Debug.Log(splitStr[1]);
                        SceneOnOff(splitStr[1]);
                        writer.WriteLine("��� ���� �÷��� �� ���� �Ϸ�"); // �ý��� ��Ʈ�ѷ� Ŭ���̾�Ʈ���� ���� �Ϸ� Ȯ�� ������
                        clientsocket.Close(); //Ŭ���̾�Ʈ ������ ��Ű�� ������ �ݴ´�.
                        Debug.Log("���� ���α׷� ���� ��� ���� ���� ����");
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.ToString());
                    Debug.Log("������ ����");
                    clientsocket.Close(); //Ŭ���̾�Ʈ ������ ��Ű�� ������ �ݴ´�.
                    break;
                }

            }
        }
    }
}
