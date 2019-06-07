using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
// using System.IO; //save to file

// 
// records table
// button "repeat previus test" for each test
// server php
// website
// save controllers actions

public class Main : MonoBehaviour{
    // global variables
    private float[] trackingTime = new float[] { 30f, 0f }; // [0] time in sec while moving are recording for each action, [1] - current timer
    private float defaultTime = 1f; // time in sec focus on an obj for select
    private int curScene = 0; //  start scene index 
    private bool isDebug = false; // VR debug enable
    private bool isNet = true; // network enable
    private bool isNetFile = false; // save network data to a local file
    private bool sendDatafromFile = false; // send data to the server from a local file
    public int precisionDec = 1000; // number dec after point in head movement control saved action: 1 - 0, 10-0.0, 100 - 0.00, 1000 -0.000

    private bool dataSended = false; //if data already sended to the server
    private int actFrmsCnt = 0; // frames counter between user actions;
    private float deviceOrientation = 0f; // left or right device headset orientation    
    private bool isTimerShowResult = false; // timer in sec how long a user read results [trigger,default,current] before send data to server
    private float[] userSceneDataTime = new float[] { 0.0f,0f} ; // [0] timer for scena, [1] timer for action
    private bool trDirect = false; // to control or do not control a user head direction
    public float baseLoc = 2; //  Y location for menus
    public GameObject camFade; // camera Fade object
    private string userLang = "English"; // default language
    public Text TextInput; // user email   
    public EventSystem SceneEventSystem; // turn on/off eventSystem 
    private bool isActionSave = false;  // trigger for saving user actions
    private int[] lastAction; // store a current action for tracking changes
    private GameObject rootObj; // root of objects for group manipulations
    private GameObject gmArrow; // array for a user head direction
    private Connection connection;   // connection object
    private Text TextDebug; // Debug object
    private UserData userData; // user data
    //private string userInput = ""; // current user inputs    
    private Material timedPointer; // pointer on an active object
    private string curfocusObj = ""; // current object in a focus
    private string curfocusType = ""; // current object type in a focus
    private int[] curfocusObjCode = new int[11] {-1,0,0,0,0,0,0,0,0,0,0 }; // a current object descritption in focus
    //curfocusObjCode[0]-  -1 - non avtive object, 0 - active object, >0 - right active object index
    //curfocusObjCode[1]-  (0-4) cur type index of object
    //curfocusObjCode[2]-  (0-7) cur index of selected objects color    
    //curfocusObjCode[3]-  (0-4) right type index of object
    //curfocusObjCode[4]-  (0-7) right index of selected objects color
    //curfocusObjCode[5]-  position.x to object
    //curfocusObjCode[6]-  position.y to object
    //curfocusObjCode[7]-  position.z to object
    //curfocusObjCode[8]-  rotation.x to object
    //curfocusObjCode[9]-  rotation.y to object
    //curfocusObjCode[10]-  rotation.z to object
    private float workTime; // timer for user actions
    private bool  animVR = false; // tr for update if need an animation
    private bool isTimer = false; // display select cursor 
    public TestData testData; // user test data
    public int[,] testsConfig = new int[2,6] { // testsConfig[x,0] - (0-4) type index of an object for search in the first test
        { 4, 4, 5, 0, 0, 0 }, // testsConfig[x,1] - (0-7) index of a selected object color
        { 4, 4, 5, 0, 0, 20 }}; // testsConfig[x,2] total number of objects for selection in tests
                               // testsConfig[x,3] total number of right founeded objects in a selection in test
                               // testsConfig[x,4] total number of all founeded objects in a selection test
                               // testsConfig[curTestIndex,5] time limit for the second test in sec. 0 - unlimit
    public int curTestIndex = 0; //a current test number
    public bool isHintDisplay = false; // display hint during a test
    private bool isDisplayTimer = false;// display timer during a test
    public float testTime;// test time left
    public Text hintText; //overlay hint text
    //private Text runText; //overlay runtime data text debug
    public GameObject TimerCanvas; // timer object
    private Utility utility;
    private Utility3D utility3D;
    private ColorTest colorTest;
    //fixing actions time variables
    //save user actions during one scene
    private SnenaMotionData curSnenaMotionData;
    private int curQuestionKey; // curent question key 
    private int questionsCount; // total number of questions
    private GameObject txtVR; // animation object in intro
    private GameObject modalWindow; // a modal window object

    void Start(){
        int checkGyro = 0; trDirect = false;
        // pass gyroscope cheking in editor mode
        #if UNITY_EDITOR
            checkGyro = 1;
        #else
            if(SystemInfo.supportsGyroscope){checkGyro = 1;}
        #endif

        GameObject ObjEventSystem = GameObject.Find("GvrEventSystem");
        //runText = GameObject.Find("run").GetComponent<Text>();
        if (ObjEventSystem){
            SceneEventSystem = ObjEventSystem.GetComponent<EventSystem>();
           // if (SceneEventSystem) { SceneEventSystem.enabled = false; }
        } else { Debug.Log("Can not find Event System"); }
       // Camera.main.GetComponent<GvrPointerPhysicsRaycaster>().enabled = false;
        GameObject camTimedPointer = GameObject.Find("GvrReticlePointer");
        timedPointer = camTimedPointer.GetComponent<Renderer>().material;       
        string lng = Application.systemLanguage.ToString();
        if (Data.isLanguge(lng)){userLang = lng;}
        //userLang = "Spanish";
        utility = new Utility(this, isDebug);
        utility3D = new Utility3D(this, utility);
        colorTest = new ColorTest(this, utility);
        camFade = GameObject.Find("camProtector");
        connection = new Connection(Data.getConnectionData()["ServerIP"] + ":" + Data.getConnectionData()["ServerPort"], isNet, utility);
        string deviceDesc = "";
        
        try
        {deviceDesc = "#_" + SystemInfo.deviceModel + ",#_" + SystemInfo.deviceType + ",#_" + SystemInfo.deviceName + ",#_" + SystemInfo.operatingSystem;
        }catch (System.Exception e) { Debug.Log(e); }
        //Debug.Log("deviceDesc: " + deviceDesc + connection.deviceUUID);
        //utility.logDebug("Init");
        userData = new UserData(){name = "", email = "", birth = "", gender = "",
            input = Input.touchSupported.ToString(),
            zone = System.TimeZoneInfo.Local.ToString(),
            deviceID = connection.deviceUUID,
            lang = userLang, ip = "",
            txtVersion = Data.getVersion(),
            gyro = checkGyro, 
            deviceInfo = deviceDesc
        };
        gmArrow = GameObject.Find("Arrow");
        if (gmArrow){
            gmArrow.GetComponent<Image>().enabled = true;
            gmArrow.SetActive(false);
        }
        initTestData();
        if (sendDatafromFile) { testConnection(); }
        //Debug.Log("start "+trDirect);
        if (checkGyro > 0){
            StartCoroutine(utility.FadeScene(1f, true, new Color(0.2f, 0.2f, 0.2f, 1), checkGyro));
        } else {
            //StartCoroutine(utility.PauseInit(7,0));
            rootObj = utility.ShowMessage(Data.getMessage(userLang, "gyroWarn"), "Next", Data.getMessage(userLang, "btnStart"), new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
           // Camera.main.GetComponent<GvrPointerPhysicsRaycaster>().enabled = true;
            SceneEventSystem.enabled = true;
            curScene = -1;
        }
    }

    public string getLng() { return userLang;}

    private void Update(){
        //Debug.Log("update " + trDirect);
        //if (Input.GetKeyDown(KeyCode.Escape)){
        //    // Android close icon or back button tapped.
        //    Application.Quit();
        //}
        if (trDirect){ // show array to a text message
            float a = 0; float b = 0;
            float y = Camera.main.transform.eulerAngles.y;
            float z = Camera.main.transform.eulerAngles.z;            
            if (y > 50 && y < 310){ if(y > 180) { a = 0.01f;} else { a = 180; }}
            if (a != 0 ) {
                if (z > 120 && z < 240) { b = 180; } // flip if device has a wrong orientation
                gmArrow.transform.localEulerAngles = new Vector3(0, a, b);
                gmArrow.SetActive(true);
            } else {if (gmArrow) { gmArrow.SetActive(false); }}
        }
        if(animVR){if (txtVR){txtVR.transform.Rotate(Vector3.back, 1);}}
        userSceneDataTime[0] += Time.deltaTime;
        userSceneDataTime[1] += Time.deltaTime;
        if(isActionSave) {            
            if(trackingTime[0] > trackingTime[1]){
                trackingTime[1] += Time.deltaTime;
                actFrmsCnt++;                
                int[] curAction = new int[16]{
                    actFrmsCnt, // frames number from previus action
                    Mathf.RoundToInt(userSceneDataTime[0] * 1000), // time from scene start
                    Mathf.RoundToInt(Camera.main.transform.eulerAngles.x*precisionDec), // head direction
                    Mathf.RoundToInt(Camera.main.transform.eulerAngles.y*precisionDec), // head direction
                    Mathf.RoundToInt(Camera.main.transform.eulerAngles.z*precisionDec), // head direction
                    curfocusObjCode[0], // right or not                    
                    curfocusObjCode[1], // cur obj type 
                    curfocusObjCode[2], // cur obj color
                    curfocusObjCode[3], // right obj type
                    curfocusObjCode[4], // right obj color
                    curfocusObjCode[5], // obj position.x 
                    curfocusObjCode[6], // obj position.y 
                    curfocusObjCode[7], // obj position.z 
                    curfocusObjCode[8], // obj angle.x
                    curfocusObjCode[9], // obj angle.y
                    curfocusObjCode[10] // obj angle.z
                };
                bool trNewData = false;                
                for (int i = 2; i < curAction.Length; i++){ // skip time and frame counter
                    if (curAction[i] != lastAction[i]) { trNewData = true; break; }
                }
                if (trNewData){  
                    // curSnenaMotionData.act.Add(new int[2]);
                    curSnenaMotionData.act.Add(new UserActivity{a = curAction });
                    // Debug.Log(JsonUtility.ToJson(curSnenaMotionData));    
                    actFrmsCnt = 0;
                    for (int i = 2; i < curAction.Length; i++) { lastAction[i] = curAction[i]; }
                }
            } else { isActionSave = false;// limit time actions recording
               // Debug.Log("reach a records time limit "+ trackingTime);               
            }            
        }
        if (isTimer){ // display selecting pointer
            //Debug.Log("onUpdate main ");
            workTime -= Time.deltaTime;
            if (workTime <= 0) { OnClickTimed();
            }else{ if (timedPointer){ timedPointer.SetFloat("_Angle", (1f - workTime / defaultTime) * 360); } }
        }
        if (isHintDisplay){ //display hint in test
            string msgTimer = "";
            if (isDisplayTimer){
                testTime -= Time.deltaTime;
                msgTimer += string.Format(Data.getMessage(userLang, "Test_timer"), Mathf.Floor(testTime).ToString());
                if (testTime < 0){ testEnd();}
                if (hintText) {
                    hintText.text = string.Format(Data.getMessage(userLang, "Test_hint"), Data.getMessage(userLang, "color_" + testsConfig[curTestIndex, 1]), Data.getMessage(userLang, "obj_" + testsConfig[curTestIndex, 0]), testsConfig[curTestIndex, 4], testsConfig[curTestIndex, 2])
                        + msgTimer;
                }
            }
        }
    }

    // on the cusror over an active object in a scena
    public void OnEnterTimed(string type, string name, bool isButton){
        Debug.Log("onEnterTimed main " + name + "=" + type+" butt="+ isButton);
        utility.logDebug("EnterTimed");
        isTimer = true;
        workTime = defaultTime;
        if (timedPointer) { timedPointer.SetFloat("_Angle", 0); }
        curfocusObj = name;
        curfocusType = type;
        string[] arrName = name.Split('_');
        curfocusObjCode = new int[11] { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        int.TryParse(arrName[0], out curfocusObjCode[0]);        
        if (!isButton){
            int.TryParse(arrName[1], out curfocusObjCode[1]);
            int.TryParse(arrName[2], out curfocusObjCode[2]);
            curfocusObjCode[3] = testsConfig[curTestIndex, 0];
            curfocusObjCode[4] = testsConfig[curTestIndex, 1];            
        }
        GameObject gm = GameObject.Find(curfocusObj);        
        if (gm) {
            curfocusObjCode[5] = Mathf.RoundToInt(gm.transform.position.x * precisionDec);
            curfocusObjCode[6] = Mathf.RoundToInt(gm.transform.position.y * precisionDec);
            curfocusObjCode[7] = Mathf.RoundToInt(gm.transform.position.z * precisionDec);
            curfocusObjCode[8] = Mathf.RoundToInt(gm.transform.eulerAngles.x * precisionDec);
            curfocusObjCode[9] = Mathf.RoundToInt(gm.transform.eulerAngles.y * precisionDec);
            curfocusObjCode[10] = Mathf.RoundToInt(gm.transform.eulerAngles.z * precisionDec);
        }
        //Debug.Log(name+"="+string.Join(";", curfocusObjCode));        
    }

    // on the cusror move out of over an active object in a scena
    public void OnExitTimed(){
        Debug.Log("ExitTimed main");
        utility.logDebug("ExitTimed");
        isTimer = false;
        curfocusObj = "";
        curfocusType = "";
        curfocusObjCode = new int[11] { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        workTime = defaultTime;
        if (timedPointer) { timedPointer.SetFloat("_Angle", 360); }
        trackingTime[1] = 0f;
    }

    public void OnClickTimed(){ClickSelectEvent();}

    // on the cusror timed click on an active object in a scena
    private void ClickSelectEvent(){
        isTimer = false;       
        Debug.Log("clickSelectEvent=" + curfocusType + "="+ curfocusObj);
        utility.logDebug("ClickTimed");
        switch (curfocusType) {
            case "Next": // start next scena
                GameObject model = GameObject.Find("Model");
                if (model) { Destroy(model); }
                StartCoroutine(utility.rotateText(rootObj, false, 2, "NextScene", 1));
                break;
            case "Test": // test scenes activity
                GameObject gm = GameObject.Find(curfocusObj);
                StartCoroutine(utility3D.appearObject(gm, false));
                //Debug.Log(testData.selectedObjectsList.Count + "="+ JsonUtility.ToJson(testData.selectedObjectsList));
                testData.selectedObjectsList[testData.selectedObjectsList.Count - 1].objs.Add(
                    new TestObject() {
                        time = Mathf.RoundToInt(userSceneDataTime[1] * 1000),
                        i = curfocusObjCode[0],
                        lx = curfocusObjCode[5],
                        ly = curfocusObjCode[6],
                        lz = curfocusObjCode[7],
                        rx = curfocusObjCode[8],
                        type = curfocusObjCode[1], // cur obj type 
                        color = curfocusObjCode[2], // cur obj color
                    });
                userSceneDataTime[1] = 0f;
                testsConfig[curTestIndex, 4]++;
                // if this is a right object                
                if (curfocusObjCode[0] != 0) { testsConfig[curTestIndex, 3]++; }
                //Debug.Log("obj:"+ curfocusObj + " result right:" + testsConfig[curTestIndex, 3] + " total:" + testsConfig[curTestIndex, 4]);
                // if a testing person reach a limit of objects
                if (testsConfig[curTestIndex, 4] >= testsConfig[curTestIndex, 2]) { testEnd();
                } else {
                    string msg = string.Format(Data.getMessage(userLang, "Test_hint"), Data.getMessage(userLang, "color_" + testsConfig[curTestIndex, 1]), Data.getMessage(userLang, "obj_" + testsConfig[curTestIndex, 0]), testsConfig[curTestIndex, 4], testsConfig[curTestIndex, 2]);
                    if (isHintDisplay) { msg += string.Format(Data.getMessage(userLang, "Test_timer"), Mathf.Floor(testTime).ToString()); }
                    if (hintText) { hintText.text = msg; }
                }
                // Debug.Log("result2: right:"+testsConfig[curTestIndex, 3] + " total:"+testsConfig[curTestIndex, 4]);
                break;
            case "Keyboard": // email input scena, typing
                if (curfocusObj.Contains("DEL")) {
                    string str = TextInput.text;
                    if (str.Length > 0) { TextInput.text = str.Substring(0, str.Length - 1); }
                } else { if (TextInput) { TextInput.text += curfocusObj; } }
                break;
            case "KeyboardDigit": // input date
                string strDigit = TextInput.text;
                int pos = strDigit.IndexOf('X');
                char[] chars = strDigit.ToCharArray();
                if (curfocusObj.Contains("DEL")){
                    Debug.Log(" digts = "+ strDigit + " pos="+pos);
                    if (pos > 0) {
                        int jDig;
                        for (jDig = pos-1; jDig >= 0; jDig--) {
                            if (!chars[jDig].Equals(' ')){
                                chars[jDig] = 'X';
                                TextInput.text = new string(chars);
                                break;
                            }
                        }                        
                    }
                }else {if(TextInput){
                        chars[pos] = curfocusObj.ToCharArray()[0];
                        TextInput.text = new string(chars);
                        if (pos > 22){
                            StartCoroutine(utility.rotateText(rootObj, false, 2, "NextScene", 1));
                        }
                    } }
                break;
            case "EnterName": // save user name
                userData.name = TextInput.text;
                StartCoroutine(utility.rotateText(rootObj, false, 2, "NextScene", 1));
                break;
            case "EnterEmail": // save user email
                userData.email = TextInput.text;
                StartCoroutine(utility.rotateText(rootObj, false, 2, "NextScene", 1));
                break;
            case "Exit": Application.Quit(); break; // exit from app
            case "CloseHelp": if (modalWindow) { GameObject.Destroy(modalWindow); }; rootObj.SetActive(true); break;
            case "ShowHelp":
                rootObj.SetActive(false);
                if (modalWindow) { GameObject.Destroy(modalWindow); }
                string[] arrNameHelp = curfocusObj.Split('_');
                string msgHelp = string.Format(Data.getMessage(userLang, "dsc" + arrNameHelp[0]), arrNameHelp[1]);
                modalWindow = utility.ShowMessage(msgHelp, "CloseHelp", Data.getMessage(userLang, "btnClose"), new Vector2(1400, 700), TextAnchor.MiddleCenter, new Vector2(0, 40));
                break; // exit from app
            case "selOneCol": // select only one color
                StartCoroutine(colorTest.FadeTo(GameObject.Find("b_" + curfocusObj)));
                int colorIndex = 0;
                int.TryParse(curfocusObj, out colorIndex);
                testsConfig[0, 1] = colorIndex;
                testsConfig[1, 1] = colorIndex;
                //Debug.Log(testsConfig[1, 1]);
                StartCoroutine(utility.rotateText(rootObj, false, 2, "NextScene", 1));
                break;
            case "selAllCol": // select colors one by one
                StartCoroutine(colorTest.FadeTo(GameObject.Find("b_" + curfocusObj)));
                int colorI = 0;
                int.TryParse(curfocusObj, out colorI);
                testData.colorTestResult.selected.Add(new TestAnswer {id = colorI, nm = 0, t= Mathf.RoundToInt(userSceneDataTime[1] * 1000) });
                userSceneDataTime[1] = 0f;
                if (testData.colorTestResult.selected.Count > 6) {
                    float s = 0; float e = 0; float[] kArray = new float[] { 8.1f, 6.8f, 6, 5.3f, 4.7f, 4, 3.2f, 1.8f };
                    for (int i = 0; i < testData.colorTestResult.selected.Count; i++) {
                        if (i < 3) { if (testData.colorTestResult.selected[i].id == 0 || testData.colorTestResult.selected[i].id == 6 || testData.colorTestResult.selected[i].id == 7) { s += kArray[i]; } }
                        if (i > 4) { if (testData.colorTestResult.selected[i].id == 1 || testData.colorTestResult.selected[i].id == 2 || testData.colorTestResult.selected[i].id == 3 || testData.colorTestResult.selected[i].id == 4) { s += kArray[7 - i]; } }
                        if (testData.colorTestResult.selected[i].id == 2 || testData.colorTestResult.selected[i].id == 3 || testData.colorTestResult.selected[i].id == 4) { e += kArray[i]; }
                        //Debug.Log("Calculate: i=" +i+" color="+testData.colorTestResult.selected[i].id + " s=" +s+" e="+e); //
                    }
                    testData.colorTestResult.stress = Mathf.RoundToInt(100 * s / 42);
                    testData.colorTestResult.energy = Mathf.RoundToInt(100 * (e - 9) / 12);
                    testData.colorTestResult.totalTime = Mathf.RoundToInt(userSceneDataTime[0] * 1000);
                    //Debug.Log("Color test result= " + JsonUtility.ToJson(testData.colorTestResult));
                    StartCoroutine(utility.rotateText(rootObj, false, 5, "NextScene", 1));
                }
                break;
            case "NextQuestion": // text test action
                int surKey = 0;
                int.TryParse(curfocusObj, out surKey);
                testData.textTestResult.answers.Add(new TestAnswer {id = curQuestionKey, nm = surKey, t = Mathf.RoundToInt(userSceneDataTime[1] * 1000) });
                userSceneDataTime[1] = 0f;
                //Debug.Log("item = " + JsonUtility.ToJson(testData.textTestResult.answers));
                if (surKey == 0) {
                    if (Data.Answers["+"].Contains(curQuestionKey)) { testData.textTestResult.extra++; }
                    if (Data.Answers["n"].Contains(curQuestionKey)) { testData.textTestResult.stabil++; }
                } else {
                    if (Data.Answers["-"].Contains(curQuestionKey)) { testData.textTestResult.extra++; }
                }                
                if (questionsCount > testData.textTestResult.answers.Count) {
                    StartCoroutine(utility.rotateText(rootObj, false, 5, "NextQuestion", 1));
                } else {
                    float cnt = Data.getQuestionsCount(userLang) / 2;
                    float resExtra = (testData.textTestResult.extra - cnt / 2) / cnt;
                    float resStabil = (testData.textTestResult.stabil - cnt / 2) / cnt;
                    testData.textTestResult.totalTime = Mathf.RoundToInt(userSceneDataTime[0] * 1000);
                    float a = Mathf.Atan2(resStabil, resExtra);
                    string[] pTypeName = new string[2];
                    float[] pTypeValue = new float[2];
                    float mid = Mathf.PI / 4;
                    float pi_2 = Mathf.PI / 2;
                    testData.textTestResult.Power = Mathf.RoundToInt(Mathf.Sqrt(Mathf.Pow(resExtra, 2) + Mathf.Pow(resStabil, 2)));
                    float mod_a = Mathf.Abs(a);
                    if (mod_a <= mid) {
                        pTypeName[0] = "Choleric";
                        pTypeName[1] = "Sanguine";
                        pTypeValue[0] = Mathf.Abs(mid + a) / pi_2;
                        pTypeValue[1] = Mathf.Abs(mid - a) / pi_2;
                    } else if (mod_a >= 3 * mid) {
                        pTypeName[0] = "Phlegmatic";
                        pTypeName[1] = "Melancholic";
                        if (a > 0) {
                            pTypeValue[0] = Mathf.Abs(a - 3 * mid) / pi_2;
                            pTypeValue[1] = Mathf.Abs(5 * mid - a) / pi_2;
                        } else {
                            pTypeValue[0] = Mathf.Abs(-5 * mid - a) / pi_2;
                            pTypeValue[1] = Mathf.Abs(-3 * mid - a) / pi_2;
                        }
                    } else {
                        if (testData.textTestResult.stabil > 0) {
                            pTypeName[0] = "Choleric";
                            pTypeName[1] = "Melancholic";
                            pTypeValue[0] = Mathf.Abs(3 * mid - a) / pi_2;
                            pTypeValue[1] = Mathf.Abs(mid - a) / pi_2;
                        } else {
                            pTypeName[0] = "Phlegmatic";
                            pTypeName[1] = "Sanguine";
                            pTypeValue[0] = Mathf.Abs(-mid - a) / pi_2;
                            pTypeValue[1] = Mathf.Abs(-3 * mid - a) / pi_2;
                        }
                    }                    
                    testData.textTestResult.Name1 = pTypeName[0];
                    testData.textTestResult.Value1 = Mathf.RoundToInt(100 * pTypeValue[0]);
                    testData.textTestResult.Name2 = pTypeName[1];
                    testData.textTestResult.Value2 = Mathf.RoundToInt(100 * pTypeValue[1]);
                    //Debug.Log("Test result2 = " + JsonUtility.ToJson(testData.textTestResult));
                    isActionSave = false;
                    testData.snenasMotionData.Add(curSnenaMotionData);
                    sendDataToServer();
                    dataSended = true;                    
                    StartCoroutine(utility.rotateText(rootObj, false, 5, "NextScene", 1));
                }
                break;
            case "LangSw": animVR = false; StartCoroutine(utility.rotateText(rootObj, false, 2, "NextScene", -1)); break;
            case "LangSwMenu": // change language action
                Debug.Log("obj= " +curfocusObj);
                userLang = curfocusObj;
                StartCoroutine(utility.rotateText(rootObj, false, 2, "NextScene", 1));
                break;
            case "GenderSwMenu":
                Debug.Log("obj= " + curfocusObj);
                userData.gender = curfocusObj;
                StartCoroutine(utility.rotateText(rootObj, false, 2, "NextScene", 1));
                break;
            case "Back": StartCoroutine(utility.rotateText(rootObj, false, 2, "NextScene", -1)); break;
            case "Repeat": // repeat all tests
                initTestData();
                // curSnenaMotionData = new SnenaMotionData() { i = 0, act = new List<int[]>() };
                curSnenaMotionData = new SnenaMotionData() { i = 0, act = new List<UserActivity>() };                
                curScene = 3;
                dataSended = false;
                NextScene(0);
                break;
            default: Debug.Log("clickSelectEvent not found action for " + name); break;
        }
        OnExitTimed();
    }
    //void StartAction(float speedUp) { StartCoroutine(RotateCamera(2f, -speedUp, "End rotation")); }

    // switch between scenas
    public void NextScene(int delta){ // switch scenes  
        //utility.logDebug("NextScene cur="+ curScene +" delta="+ delta);
        //utility.logDebug("Next");
        isActionSave = false;
        isHintDisplay = false;
        isDisplayTimer = false;
        animVR = false;
        trDirect = false;
        SceneEventSystem.enabled = false;        
        //Camera.main.GetComponent<GvrPointerPhysicsRaycaster>().enabled = false;
       // Debug.Log("curScene=" + curScene +" delta="+ delta+" "+ JsonUtility.ToJson(curSnenaMotionData));                
        curScene += delta;
        if (!dataSended) {
            if (delta > 0) { testData.snenasMotionData.Add(curSnenaMotionData); }
            curfocusObjCode = new int[11] { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            //curSnenaMotionData = new SnenaMotionData(){i = curScene, act = new List<int[]>()};
            Debug.Log(JsonUtility.ToJson(curSnenaMotionData));
            curSnenaMotionData = new SnenaMotionData() { i = 0, act = new List<UserActivity>() };
        }
        if (rootObj) { Destroy(rootObj);}
        if (TimerCanvas) { Destroy(TimerCanvas);}
        switch (curScene){
            case -1: // change language menu
                rootObj = utility.ShowSwitchlngMenu(userLang);
                StartCoroutine(utility.rotateText(rootObj, true, 2, "", 0));
                trDirect = true;
                break;
            case 0: // show a intro message
                Debug.Log("Start 0 scene");
                rootObj = utility.ShowMessage(Data.getMessage(userLang, "Intro"), "Next", Data.getMessage(userLang, "btnNext"), new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
                utility.CreateButton(rootObj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform, "LangSw", Data.getMessage(userLang, "btnLangSw"), "LangSw", "", new Vector3(400, 230, 0), new Vector2(400, 60));
                try {
                    txtVR = (GameObject)Instantiate(Resources.Load<GameObject>("VR"));
                    addVR(rootObj, new Vector3(0f, 5, 0));
                    animVR = true; }
                catch (System.Exception e) { Debug.Log("Can not load VR nodel"); }                
                trDirect = true;                
                break;
            case 1: // show a select gender message
                rootObj = utility.ShowGenderMenu(userLang);                
                trDirect = true;
                break;
            case 2: // show a select birthday message
                rootObj = utility.ShowDigitsKeyboard(userLang, "Birth");
                trDirect = true;
                break;
            case 3: // show a start test 1 message
                curTestIndex = 0;
                string msg1 = string.Format(Data.getMessage(userLang, "Test1"), Data.getMessage(userLang, "color_" + testsConfig[curTestIndex, 0]), Data.getMessage(userLang, "obj_" + testsConfig[curTestIndex, 0]));
                //Debug.Log("Test1=" + msg1);
                rootObj = utility.ShowMessage(msg1, "Next", Data.getMessage(userLang, "btnStart"), new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
                StartCoroutine(utility.rotateText(rootObj, true, 2, "newObj", 0));
                trDirect = true;
                break;
            case 4: // start test 1
                curTestIndex = 0;
                rootObj = utility3D.CreateObjsArray(false);
                isHintDisplay = true;
                isDisplayTimer = false;
                break;
            case 5: // show a start test 2 message
                curTestIndex = 1;
                string msg2 = string.Format(Data.getMessage(userLang, "Test2"), Data.getMessage(userLang, "color_" + testsConfig[curTestIndex, 1]), Data.getMessage(userLang, "obj_" + testsConfig[curTestIndex, 0]), testsConfig[curTestIndex, 5]);
                rootObj = utility.ShowMessage(msg2, "Next", Data.getMessage(userLang, "btnStart"), new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
                utility3D.createTarget(rootObj, testsConfig[0, 1]);
                trDirect = true;
                break;
            case 6: //start test 2 with a time limit
                curTestIndex = 1;
                rootObj = utility3D.CreateObjsArray(true);
                isHintDisplay = true;
                isDisplayTimer = true;
                break;
            case 7: // show a color test start message
                rootObj = utility.ShowMessage(Data.getMessage(userLang, "IntroColTest"), "Next", Data.getMessage(userLang, "btnStart"), new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
                trDirect = true;
                break;
            case 8: // start color test 
                //Debug.Log("Start color test "+JsonUtility.ToJson(testData.colorTestResult));
                if(testData.colorTestResult.selected != null) { testData.colorTestResult.selected.Clear();
                } else { testData.colorTestResult.selected = new List<TestAnswer>();}
                Debug.Log("Start color test " + JsonUtility.ToJson(testData.colorTestResult));
                testData.colorTestResult.energy = 0;
                testData.colorTestResult.stress = 0;
                testData.colorTestResult.totalTime = 0;
                //Debug.Log("Start color test " + JsonUtility.ToJson(testData.colorTestResult));
                rootObj = colorTest.showColors("selAllCol");
                GameObject msgObj2 = utility.ShowMessage(Data.getMessage(userLang, "selAllCol"), "", Data.getMessage(userLang, "btnStart"), new Vector2(1200, 200), TextAnchor.MiddleCenter, new Vector2(0, 20));
                msgObj2.transform.SetParent(rootObj.transform);
                msgObj2.transform.position = new Vector3(0, 5.4f, 16);
                trDirect = true;
                break;
            case 9: // show a start text test message
                string strTxtIntro = string.Format(Data.getMessage(userLang, "IntroTextTest"), Data.getQuestionsCount(userLang));
                rootObj = utility.ShowMessage(strTxtIntro, "Next", Data.getMessage(userLang, "btnStart"), new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
                trDirect = true;
                break;
            case 10: // start a text test                
                initTable();                
                NextQuestion();
                trDirect = true;
                break;
            case 11: // show results
                //initTable(); // for test only!!
                rootObj = utility.showResult(userLang, testData.colorTestResult.stress, testData.colorTestResult.energy, testData.textTestResult.Name1, testData.textTestResult.Name2, testData.textTestResult.Value1, testData.textTestResult.Value2, testData.textTestResult.Power);
                StartCoroutine(utility.rotateText(rootObj, true, 2, "", 0));
                trDirect = true;
                isTimerShowResult = true;
                break;
            case 12: // show a start text test message
                rootObj = utility.ShowMessage(Data.getMessage(userLang, "msgAbout"), "Back", Data.getMessage(userLang, "btnBack"), new Vector2(1200, 600), TextAnchor.MiddleCenter, new Vector2(0, 40));
                StartCoroutine(utility.rotateText(rootObj, true, 2, "", 0));
                trDirect = true;
                break;
            default: Debug.Log("Not Found current scene index"); break;
        }
        userSceneDataTime[0] = 0f;
        userSceneDataTime[1] = 0f;
        trackingTime[1] = 0f;
        SceneEventSystem.enabled = true;
        actFrmsCnt = 0;
        if (!dataSended) { isActionSave = true; }
    }

    private void initTable() {
        if (testData.textTestResult.answers != null) { testData.textTestResult.answers.Clear(); }
        else { testData.textTestResult.answers = new List<TestAnswer>(); }
        testData.textTestResult.extra = 0;
        testData.textTestResult.stabil = 0;
        testData.textTestResult.totalTime = 0;
        testData.textTestResult.Name1 = "";
        testData.textTestResult.Name2 = "";
        testData.textTestResult.Value1 = 0;
        testData.textTestResult.Value2 = 0;
        testData.textTestResult.Power = 0;
    }

    // send data to server
    private void sendDataToServer(){
        Debug.Log("start sending data to the server");
        isActionSave = false;
        isTimerShowResult = false;
        if (testData!=null) { 
            if(testData.snenasMotionData.Count>0){
                if (isNetFile) {
                    try { 
                        //string json = JsonUtility.ToJson(testData);
                        //string dataPath = Path.Combine(Application.persistentDataPath, "CharacterData.txt");
                        //using (StreamWriter streamWriter = File.CreateText("c:\\11\\unityJson.txt")){streamWriter.Write(json);}
                        //Debug.Log("successful save data to file");
                    } catch (System.Exception e) { Debug.Log("Can not save data to a file"); }
                }
                connection.putDataBlob("/putVrData", testData);
            } else{Debug.Log("No data send to server");}
        }
    }

    private void initTestData(){
        testData = new TestData(){
            userStartTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"),
            userData = userData,
            ipInfo = "",
            snenasMotionData = new List<SnenaMotionData>(),
            rightObjectsList = new List<TestObjects>(),
            selectedObjectsList = new List<TestObjects>(),
            colorTestResult = new ColorTestResult(),
            textTestResult = new TextTestResult()
        };
        //curSnenaMotionData = new SnenaMotionData() { i = 0, act = new List<int[]>() };
        curSnenaMotionData = new SnenaMotionData() { i = 0, act = new List<UserActivity>() };
        lastAction = new int[16];
    }

    public void destroy(GameObject gm){ if (gm) { Destroy(gm); }}


    // on finish a test
    private void testEnd() {
        hintText = null;
        isHintDisplay = false;
        if (testData.selectedObjectsList[testData.selectedObjectsList.Count - 1].time == 0) { 
            testData.selectedObjectsList[testData.selectedObjectsList.Count - 1].time = Mathf.RoundToInt(userSceneDataTime[0] * 1000);
            testData.selectedObjectsList[testData.selectedObjectsList.Count - 1].rights = testsConfig[curTestIndex, 3];
            if (TimerCanvas){ Destroy(TimerCanvas); }        
            //Debug.Log("Ended Test="+ curTestIndex+" json=" + JsonUtility.ToJson(testData));
            NextScene(1);
        }
    }

    private void NextQuestion() {
        questionsCount = Data.getQuestionsCount(userLang);
        string[] curQuestion = Data.getQuestionIndex(userLang, testData.textTestResult.answers.Count);
        int.TryParse(curQuestion[0], out curQuestionKey);
        string notes = (testData.textTestResult.answers.Count + 1).ToString() + "/" + questionsCount.ToString();
        rootObj = utility.ShowDialog(curQuestion[1], notes, "NextQuestion", Data.getMessage(userLang, "yes"), Data.getMessage(userLang, "not"), new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
        StartCoroutine(utility.rotateText(rootObj, true, 5, "", 0));
    }

    public void startNewAnimation(string nextAnimation, int index) {
        switch (nextAnimation){
            case "NextScene" :
                NextScene(index);
                break;
            case "NextQuestion":
                if (rootObj) { Destroy(rootObj); }
                NextQuestion();
                break;
            case "newObj":
                Debug.Log("start obj animation");
                StartCoroutine(utility3D.appearObject(utility3D.createTarget(rootObj, testsConfig[0, 1]), true)); 
                break;
            default: break;                
        }
    }

    public GameObject addVR(GameObject root, Vector3 loc){
        try {
            txtVR.name = "Model";
            txtVR.transform.position = loc;
            txtVR.transform.SetParent(root.transform, false);
            txtVR.transform.Rotate(Vector3.right,90);
            txtVR.transform.Rotate(Vector3.forward, 180);
            txtVR.transform.localScale = new Vector3(3, 3, 3);           
        }
        catch (System.Exception e) { Debug.Log("Can not add to scena VR nodel"); }
        return txtVR;
    }

    private void testConnection(){
        // test connection module with data from a file
        //connection.putDataString("/putTest", "{'test': 'data'}");
        //string str = "El perro de San Roque no tiene rabo porque Ramón Rodríguez se lo ha robado.";
        //for (int i = 0; i < 20; i++){
        //    str += str;
        //}
        //byte[] zip = ZlibStream.CompressString(str);
        //string str2 = System.Text.Encoding.UTF8.GetString(zip);
        ////using (StreamWriter streamWriter = File.CreateText("c:\\11\\test1.txt")){streamWriter.Write(str);}
        //using (StreamWriter streamWriter = File.CreateText("c:\\11\\test1.zip")) { streamWriter.Write(str2); }
        //Debug.Log("successful save data to file");
        connection.putDataBlob("/putVrData", testData);
        //string txtfromfile = "";
        //try { 
        //using (StreamReader streamreader = File.OpenText("c:\\11\\unityjson.txt")) { txtfromfile = streamreader.ReadToEnd(); }
        //TestData testdata2 = JsonUtility.FromJson<TestData>(txtfromfile);
        //Debug.Log("loaded net data \n "+JsonUtility.ToJson(testdata2));            
        //    connection.putDataBlob("/putVrData", testdata2);
        //} catch (System.Exception e) { Debug.Log("Can not send data to server"); }
    }

}
