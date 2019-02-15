using UnityEngine;
using System.Text;
using System.Net;

public class Connection {

    private string serverUrl = "";
    private string deviceUUID = "unity";
    private bool connEnable = false;
    private string ip = "";
    private string lang = "ru";
    private string ipInfo = "";

    public Connection(string serverString, bool isEnable) {
        serverUrl = serverString;
        connEnable = isEnable;
        deviceUUID = SystemInfo.deviceUniqueIdentifier.ToString();
        Debug.Log("Create Connection obj= "+ serverString+"  "+ deviceUUID);
    }

    public void putDataString(string cmd, string data){
        if (connEnable) { 
            try{
                //JsonUtility.ToJson();
                string docs = "{\"user\":\"" + deviceUUID+"\"lang\":\"" + lang + "\",\"time\":\"" + System.DateTime.UtcNow.ToString("yyyy/MM/dd/HH:mm") + "\",\"data\":\"" + data + "\"}";            
                //if (type==0) { strDataEnd += "_test"; }
                string uriStr = "http://" + serverUrl + cmd;
                Debug.Log("start Upload url=" + uriStr);
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                byte[] byteDocs = Encoding.UTF8.GetBytes(docs);
                client.UploadDataCompleted += new UploadDataCompletedEventHandler(UploadDataResult);
                client.UploadDataAsync(new System.Uri(uriStr), "POST", byteDocs);
                Debug.Log("start Upload");
            }
            catch (System.Exception e) { Debug.Log("Connection error: " + e); }
        }
    }

    private void UploadDataResult(object sender, UploadDataCompletedEventArgs e)
    {
        if (connEnable)
        {
            try
            {
                Debug.Log("Upload result catch!!");
                byte[] data = (byte[])e.Result;
                string reply = System.Text.Encoding.UTF8.GetString(data);
                Debug.Log("!!!!!Upload result: " + reply);
            }
            catch (System.Exception ee) { Debug.Log("!!!!!Upload data error: " + ee); }
        }
    }

    public void putDataBlob(string cmd, TestData data)
    {
        if (connEnable)
        {
            try
            {
                string uriStr = "http://" + serverUrl + cmd;
                Debug.Log("start Upload url=" + uriStr);

                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                byte[] byteDocs = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
                client.UploadDataCompleted += new UploadDataCompletedEventHandler(UploadBlobResult);
                client.UploadDataAsync(new System.Uri(uriStr), "POST", byteDocs);
                Debug.Log("start Upload");
            }
            catch (System.Exception e) { Debug.Log("Connection error: " + e); }
        }
    }

    private void UploadBlobResult(object sender, UploadDataCompletedEventArgs e)
    {
        if (connEnable)
        {
            try
            {
                Debug.Log("Upload result catch!!");
                byte[] data = (byte[])e.Result;
                string reply = System.Text.Encoding.UTF8.GetString(data);
                Debug.Log("!!!!!Upload result: " + reply);
            }
            catch (System.Exception ee) { Debug.Log("!!!!!Upload data error: " + ee); }
        }
    }
    private void getIP() {       
        if (connEnable)
        {
            try
            {
                string url = "http://api.ipstack.com/check?access_key=f32a4157e76eb9ed5347f0c4869d0c3e&format=1'";
                Debug.Log("Get IP");
                //byte[] data = (byte[])e.Result;
                //string reply = System.Text.Encoding.UTF8.GetString(data);
                //Debug.Log("!!!!!Upload result: " + reply);
            }
            catch (System.Exception ee) { Debug.Log("!!!!!Upload data error: " + ee); }
        }
    }
}
