using UnityEngine;
using System.Text;
using System.Net;
//using Facebook.Unity;
using System.Collections.Generic;
using Ionic.Zlib;

public class Connection{
    private string serverUrl = "";
    public string deviceUUID = "unity";
    private bool connEnable = false;
    public string ip = "local";
    public string ipInfo = "";
    private string lang = "ru";
    private List<string> perms = new List<string>() {};
    private Utility utility;

    public Connection(string serverString, bool isEnable, Utility util){
        serverUrl = serverString;
        connEnable = isEnable;
        Utility utility = util;
        utility.logDebug("Conn");
        deviceUUID = SystemInfo.deviceUniqueIdentifier.ToString();
        //FB.Init(InitCallback);
    }

    public void putDataString(string cmd, string data){
        if (connEnable){
            try{                
                string docs = "{\"user\":\"" + deviceUUID + "\"lang\":\"" + lang + "\",\"time\":\"" + System.DateTime.UtcNow.ToString("yyyy/MM/dd/HH:mm") + "\",\"data\":\"" + data + "\"}";
                //JsonUtility.ToJson(docs);
                string uriStr = "http://" + serverUrl + cmd;
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                byte[] byteDocs = Encoding.UTF8.GetBytes(docs);
                client.UploadDataCompleted += new UploadDataCompletedEventHandler(UploadDataResult);
                client.UploadDataAsync(new System.Uri(uriStr), "POST", byteDocs);
                Debug.Log("start Upload");
            }  catch (System.Exception e) { Debug.Log("Connection error: " + e); }
        }
    }

    private void UploadDataResult(object sender, UploadDataCompletedEventArgs e){
        if (connEnable){
            try{
                byte[] data = (byte[])e.Result;
                string reply = System.Text.Encoding.UTF8.GetString(data);
                //Debug.Log("!!!!!Upload result: " + reply);
            } catch (System.Exception ee) { Debug.Log("!!!!!Upload data error: " + ee); }
        }
    }

    public void putDataBlob(string cmd, TestData data){
        if (connEnable){
            try{
                string uriStr = "http://" + serverUrl + cmd;
                Debug.Log("start Upload url=" + uriStr);
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                //byte[] byteDocs = Encoding.UTF8.GetBytes(JsonUtility.ToJson(data));
                byte[] byteDocs = ZlibStream.CompressString(JsonUtility.ToJson(data));
                client.UploadDataCompleted += new UploadDataCompletedEventHandler(UploadBlobResult);
                client.UploadDataAsync(new System.Uri(uriStr), "POST", byteDocs);
                
                Debug.Log("Start Uploading result");
            } catch (System.Exception e) {
                Debug.Log("Connection error: " + e);
            }
        }
    }

    private void UploadBlobResult(object sender, UploadDataCompletedEventArgs e){
        if (connEnable){
            try {
                byte[] data = (byte[])e.Result;
                string reply = System.Text.Encoding.UTF8.GetString(data);
                Debug.Log("!!!!!Upload result: " + reply);
            } catch (System.Exception ee) { Debug.Log("!!!!!Upload data error: " + ee); }
       }
    }

    private void getIP(){
        if (connEnable){
            try{
                string url = "http://api.ipstack.com/check?access_key=f32a4157e76eb9ed5347f0c4869d0c3e&format=1'";
                Debug.Log("Get IP");
                //byte[] data = (byte[])e.Result;
                //string reply = System.Text.Encoding.UTF8.GetString(data);
                //Debug.Log("!!!!!Upload result: " + reply);
            } catch (System.Exception ee) { Debug.Log("!!!!!Upload data error: " + ee); }
        }
    }

    //private void AuthCallback(ILoginResult result){
    //    try { Debug.Log("VRPT=AuthCallback "+JsonUtility.ToJson(result));
    //    if (FB.IsLoggedIn){
    //        var aToken = Facebook.Unity.AccessToken.CurrentAccessToken;
    //        Debug.Log("VRPT=" + JsonUtility.ToJson(aToken));
    //    } else { Debug.Log("VRPT=User cancelled login");}
    //   } catch (System.Exception e) { Debug.Log(e); }
    //}

    //private void InitCallback(){
    //    if (FB.IsInitialized) {
    //        FB.ActivateApp();
    //        FB.LogInWithReadPermissions(perms, AuthCallback);
    //    } else { Debug.Log("VRPT=Failed to Initialize the Facebook SDK");}
    //}

}
