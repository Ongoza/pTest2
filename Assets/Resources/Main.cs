using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.IO; //save to file

// переключение языков на первом экране
// имя, выбор пола и дата рождения, почта
// сделать меню выбора теста для повторного прохождения
// Сделать стрелку указатель на текстовое сообщение
// Сделать заставку с анимацитей ввода текста(Психологические тесты) и появление 3д модели VR
// Поправить проверку на гиро и девайс инфо  
// переводы текста на английский, испанский, польский
// сделать анимацию повления/исчезновения вопросов
// андроид и аппл
// сделать 3 тест по поиску фигур с эмоциональными состояниями ( на трудном уровне,на легком,  с помехами, на невозможном уровне + эмоции наблюдателя (разные для разного пола))
// 3 тест будет загонять тестируемого в эмоциональное состояние, а наблюдатель усиливать эффект

public class Main : MonoBehaviour
{
    // global temp variables
    private int curScene = 11; //  start scene index 
    private bool isDebug = false; // VR debug enable
    private bool isNet = false; // network enable

    private float[] trackingTime = new float[] { 30f, 0f }; // [0] time in sec while moving are recording for each action, [1] - current timer
    private float[] timerShowResult = new float[] { 0f, 10f, 10f }; // timer in sec how long user read results [trigger,default,current] before send data to server
    private float[] userSceneDataTime = new float[] { 0.0f,0f} ; // [0] timer for scena, [1] timer for action

    //private int[] sleepTime = new int[2] { 20,0 }; // [0] time in min with no any actions for closeing the app, [1] - current timer
    public int precisionDec = 100; // number dec after point in movement control 1 - 0, 10-0.0, 100 - 0.00
    public float baseLoc = 2;
    private GameObject camFade; // camera Fade object
    public Text TextEmail; // user email   
    EventSystem SceneEventSystem; // turn on/off eventSystem   
    //private string startDateTime = ""; // date and time the test starting
    private bool isActionSave = false;     
    private float[] lastAction;
    // user data  
    // objects roots for group manipulations
    private GameObject rootObj; // root of objects 
    private Connection connection;   
    private Text TextDebug; // Debug object
    private string deviceID;
    public string userLang = "Russian";
    private string userZone;
    private string userEmail;
    // variables for cursor
    private float defaultTime = 1f; // time in sec focus on an obj for select
    private Material timedPointer;
    private string curfocusObj = "";
    private string curfocusType = "";
    private int[] curfocusObjCode = new int[11] {-1,0,0,0,0,0,0,0,0,0,0 };
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
    private float workTime;
    private bool isTimer = false; // display select cursor 
    public TestData testData;
    public int[,] testsConfig = new int[2,6] { // testsConfig[x,0] - (0-4) type index of an object for search in the first test
        { 4, 4, 5, 0, 0, 0 }, // testsConfig[x,1] - (0-7) index of a selected object color
        { 4, 4, 5, 0, 0, 20 }}; // testsConfig[x,2] total number of objects for selection in tests
                               // testsConfig[x,3] total number of right founeded objects in a selection in test
                               // testsConfig[x,4] total number of all founeded objects in a selection test
                               // testsConfig[curTestIndex,5] time limit for the second test in sec. 0 - unlimit
    public int curTestIndex = 0; //a current test number
    public bool isHintDisplay = false; // display hint during a test
    private int checkGyro = 0;
    private string deviceDesc;
    private bool isDisplayTimer = false;// display timer during a test
    public float testTime;// test time left
    public Text hintText;
    public GameObject TimerCanvas; // timer object
    private Utility utility;
    private Utility3D utility3D;
    private ColorTest colorTest;
    //fixing actions time variables
    //save user actions during one scene
    private SnenaMotionData curSnenaMotionData;
    // result settings
     // Text test variables
    private int curQuestionKey; // curent question key 
    private int questionsCount; // total number of questions

    void Start(){                    
        checkGyro = 0;
        #if UNITY_EDITOR
            Debug.Log("Unity Editor!!!!");
            checkGyro = 1;
        #else
        if( SystemInfo.supportsGyroscope){checkGyro = 1;}
        #endif
        deviceID = SystemInfo.deviceUniqueIdentifier.ToString();
        userZone = System.TimeZoneInfo.Local.ToString();
        GameObject ObjEventSystem = GameObject.Find("GvrEventSystem");
        if (ObjEventSystem)
        {
            SceneEventSystem = ObjEventSystem.GetComponent<EventSystem>();
            if (SceneEventSystem) { SceneEventSystem.enabled = false; }
        }
        else { Debug.Log("Can not find Event System"); }
        Camera.main.GetComponent<GvrPointerPhysicsRaycaster>().enabled = false;
        GameObject camTimedPointer = GameObject.Find("GvrReticlePointer");
        timedPointer = camTimedPointer.GetComponent<Renderer>().material;
        string lng = Application.systemLanguage.ToString();
        if (Data.isLanguge(lng)){userLang = lng;}
        Debug.Log("userLang="+userLang);
        utility = new Utility(this, isDebug);
        utility3D = new Utility3D(this, utility);
        colorTest = new ColorTest(this, utility);
        if (checkGyro > 0){ Init(); } else { StartCoroutine(PauseInit(7));}
    }

    void Init() {
        camFade = GameObject.Find("camProtector");
        try{
            deviceDesc = "#_"+SystemInfo.deviceModel+",#_"+ SystemInfo.deviceType + ",#_" + SystemInfo.deviceName + ",#_" + SystemInfo.operatingSystem;
            Debug.Log(deviceDesc);
        }catch (System.Exception e){Debug.Log(e);}
        //utility.logDebug("Init");
        initTestData();
        //utility.logDebug("Start 1");
        curSnenaMotionData = new SnenaMotionData() { i = 0, act = new List<UserActivity>() };
        lastAction = new float[14];        
        //utility.logDebug("Start 2");        
        // StartCoroutine(fadeScene(2f, true, new Color(0.2f, 0.2f, 0.2f, 1), "Main"));
        //StartCoroutine(RotateCamera(2f, 0.0001f, "End rotation"));
        string srv = Data.getConnectionData()["ServerIP"] + ":" + Data.getConnectionData()["ServerPort"];
        //Debug.Log(string.Join(";", Data.getConnectionData()["SeverPort"]));
        connection = new Connection(srv, isNet);
        //testConnection();
        Debug.Log("Start end");
        NextScene(0);
    }

    private void testConnection(){
        // Test connection module with data from a file
        connection.putDataString("/putTest", "{'test': 'data'}");
        string txtFromFile = "";
        using (StreamReader streamReader = File.OpenText("c:\\11\\unityJson.txt")){txtFromFile = streamReader.ReadToEnd();}
        Debug.Log("Start end 1");
        //Debug.Log(txtFromFile);
        TestData testData2 =  JsonUtility.FromJson<TestData>(txtFromFile);
        // Debug.Log("Start end");
       connection.putDataBlob("/putVrData", testData2);
    }

    public IEnumerator PauseInit(float timer)
    {
        float currCountdownValue = timer;
        while (currCountdownValue > 0)
        {
            //Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        Init();
    }

    

    void Update(){
        //if (Input.GetKeyDown(KeyCode.Escape)){
        //    // Android close icon or back button tapped.
        //    Application.Quit();
        //}
        userSceneDataTime[0] += Time.deltaTime;
        userSceneDataTime[1] += Time.deltaTime;
        if (isActionSave) {
            if (trackingTime[0] > trackingTime[1]){
                trackingTime[1] += Time.deltaTime;
                int[] curAction = new int[14]{
                   Mathf.RoundToInt(Camera.main.transform.eulerAngles.x*precisionDec),
                   Mathf.RoundToInt(Camera.main.transform.eulerAngles.y*precisionDec),
                   Mathf.RoundToInt(Camera.main.transform.eulerAngles.z*precisionDec),
                   curfocusObjCode[0], // right or not                    
                   curfocusObjCode[1], // cur obj type 
                   curfocusObjCode[2], // cur obj color
                   curfocusObjCode[3], // right obj type
                   curfocusObjCode[4], // right obj color
                   curfocusObjCode[5],// obj position.x 
                   curfocusObjCode[6],// obj position.y 
                   curfocusObjCode[7],// obj position.z 
                   curfocusObjCode[8],// obj angle.x
                   curfocusObjCode[9],// obj angle.y
                   curfocusObjCode[10]// obj angle.z
                };
                bool trNewData = false;
                for (int i = 0; i < curAction.Length; i++){
                    if (curAction[i] != lastAction[i]) { trNewData = true; break; }
                }
                if (trNewData){
                    //Debug.Log("0 curAction = " + string.Join(";", curAction)+ "0 lastAction = " + string.Join(";", lastAction));
                    curSnenaMotionData.act.Add(new UserActivity { t = Mathf.RoundToInt(userSceneDataTime[0] * 1000), a = curAction });
                    //Debug.Log("1 curAction = " + JsonUtility.ToJson(curSnenaMotionData.userActivities));
                    for (int i = 0; i < curAction.Length; i++) { lastAction[i] = curAction[i]; }
                }
            } else { // limit time actions recording
                Debug.Log("reach a records time limit "+ trackingTime);
                isActionSave = false;
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
        if(timerShowResult[0] > 0){
            // send data to server after waiting            
            if (timerShowResult[2] > 0) { timerShowResult[2]--;
            }else{ sendDataToServer();}
        }
    }

    // on the cusror over an active object in a scena
     public void OnEnterTimed(string type, string name, bool isButton){
        Debug.Log("onEnterTimed main " + name + "=" + type+" butt="+ isButton);
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
        switch (curfocusType){
            case "Next":  NextScene(1);  break;
            case "Test": // test scenes activity
                GameObject gm = GameObject.Find(curfocusObj);                
                if (gm) { Destroy(gm); } else { Debug.Log("Error! Can not find object for destroy " + curfocusObj); }
                //Debug.Log(testData.selectedObjectsList.Count + "="+ JsonUtility.ToJson(testData.selectedObjectsList));
                testData.selectedObjectsList[testData.selectedObjectsList.Count-1].objs.Add( 
                    new TestObject() {
                        time = Mathf.RoundToInt(userSceneDataTime[1]*1000),
                        i = curfocusObjCode[0],
                        lx = curfocusObjCode[5],
                        ly = curfocusObjCode[6],
                        lz = curfocusObjCode[7],
                        rx = curfocusObjCode[8],
                        type =  curfocusObjCode[1], // cur obj type 
                        color = curfocusObjCode[2], // cur obj color
                    });
                userSceneDataTime[1] = 0f;
                testsConfig[curTestIndex, 4]++;
                // if this is a right object                
                if (curfocusObjCode[0]!=0){ testsConfig[curTestIndex, 3]++; }
                //Debug.Log("obj:"+ curfocusObj + " result right:" + testsConfig[curTestIndex, 3] + " total:" + testsConfig[curTestIndex, 4]);
                // if a testing person reach a limit of objects
                if (testsConfig[curTestIndex, 4] >= testsConfig[curTestIndex, 2]) {
                    testEnd();                    
                } else { 
                    string msg = string.Format(Data.getMessage(userLang, "Test_hint"), Data.getMessage(userLang, "color_" + testsConfig[curTestIndex, 1]), Data.getMessage(userLang, "obj_" + testsConfig[curTestIndex, 0]), testsConfig[curTestIndex, 4], testsConfig[curTestIndex, 2]);
                    if (isHintDisplay) { msg += string.Format(Data.getMessage(userLang, "Test_timer"), Mathf.Floor(testTime).ToString());}
                    if (hintText) { hintText.text = msg; }
                }
               // Debug.Log("result2: right:"+testsConfig[curTestIndex, 3] + " total:"+testsConfig[curTestIndex, 4]);
                break;
            case  "Keyboard": // email input scena, typing
                if (curfocusObj.Contains("DEL")){
                    string str = TextEmail.text;
                    if (str.Length > 0) { TextEmail.text = str.Substring(0, str.Length - 1); }
                } else { if (TextEmail) { TextEmail.text += curfocusObj; } }
                break;
            case "Enter": // email input scena, finish
                userEmail = TextEmail.text;
                testData.userEmail = userEmail;                 
                NextScene(1);
                break;
            case "Exit": // exit from app
                sendDataToServer();
                Application.Quit();
                break;
            case "selOneCol":                
                StartCoroutine(FadeTo("b_"+ curfocusObj));
                int colorIndex = 0;
                int.TryParse(curfocusObj, out colorIndex);
                testsConfig[0, 1] = colorIndex;
                testsConfig[1, 1] = colorIndex;
                Debug.Log(testsConfig[1, 1]);
                NextScene(1);
                break;
            case "selAllCol":
                StartCoroutine(FadeTo("b_" + curfocusObj));
                int colorI = 0;
                int.TryParse(curfocusObj, out colorI);
                testData.colorTestResult.selected.Add(new int[] { colorI, Mathf.RoundToInt(userSceneDataTime[1] * 1000) });
                userSceneDataTime[1]=0f;
                if (testData.colorTestResult.selected.Count > 6){
                    float s = 0; float e = 0; float[] kArray = new float[] { 8.1f, 6.8f, 6, 5.3f, 4.7f, 4, 3.2f, 1.8f };
                    for (int i = 0; i < testData.colorTestResult.selected.Count; i++)
                    {
                        if (i < 3) { if (testData.colorTestResult.selected[i][0] == 0 || testData.colorTestResult.selected[i][0] == 6 || testData.colorTestResult.selected[i][0] == 7) { s += kArray[i]; } }
                        if (i > 4) { if (testData.colorTestResult.selected[i][0] == 1 || testData.colorTestResult.selected[i][0] == 2 || testData.colorTestResult.selected[i][0] == 3 || testData.colorTestResult.selected[i][0] == 4) { s += kArray[7 - i]; } }
                        if (testData.colorTestResult.selected[i][0] == 2 || testData.colorTestResult.selected[i][0] == 3 || testData.colorTestResult.selected[i][0] == 4) { e += kArray[i]; }
                        //Debug.Log("Calculate: i=" +i+" color="+testData.colorTestResult.selected[i][0]+" s=" +s+" e="+e); //
                        //String data = Arrays.toString(testData.colorTestResult.selected);
                    }
                    testData.colorTestResult.stress = Mathf.RoundToInt(s / 42);
                    testData.colorTestResult.energy = Mathf.RoundToInt((e - 9) / 12);
                    testData.colorTestResult.totalTime = Mathf.RoundToInt(userSceneDataTime[0] * 1000);
                    Debug.Log("Color test result= " + JsonUtility.ToJson(testData.colorTestResult));
                    NextScene(1);
                }
                break;
            case "NextQuestion":
                int surKey = 0;
                int.TryParse(curfocusObj, out surKey);
                testData.textTestResult.answers.Add(new int[] {curQuestionKey, surKey, Mathf.RoundToInt(userSceneDataTime[1] * 1000) });
                userSceneDataTime[1] = 0f;
                Debug.Log("item = " + curQuestionKey + " " + surKey);
                if (surKey == 0){                    
                    if (Data.Answers["+"].Contains(curQuestionKey)) { testData.textTestResult.extra++; }
                    if (Data.Answers["n"].Contains(curQuestionKey)) { testData.textTestResult.stabil++; }
                }else{
                    if (Data.Answers["-"].Contains(curQuestionKey)) { testData.textTestResult.extra++; }
                }
                Debug.Log("test result = " + JsonUtility.ToJson(testData.textTestResult));
                Destroy(rootObj);
                if(questionsCount > testData.textTestResult.answers.Count) { 
                    string[] curQuestion = Data.getQuestionIndex(userLang, testData.textTestResult.answers.Count);
                    int.TryParse(curQuestion[0], out curQuestionKey);
                    string notes = (testData.textTestResult.answers.Count + 1).ToString() + "/" + questionsCount.ToString();
                    rootObj = utility.ShowDialog(curQuestion[1], notes, "NextQuestion", Data.getMessage(userLang, "yes"), Data.getMessage(userLang, "not"), new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
                } else {
                    testData.textTestResult.totalTime = Mathf.RoundToInt(userSceneDataTime[0] * 1000);
                    NextScene(1); }
                break;
            case "Back": NextScene(-1); break;
            case "Repeat":
                sendDataToServer();
                curScene = 0;
                NextScene(0);
                break;
            default: Debug.Log("clickSelectEvent not found action for " + name); break;
        }
        OnExitTimed();
    }

    // a fade transition between scenas
    IEnumerator FadeScene(float duration, bool startNewScene, Color color, string sceneName){
        //utility.logDebug("FadeScene");
        if (camFade) { 
            //GameObject camFade = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            camFade.GetComponent<Renderer>().enabled = true;
            //Debug.Log("Start fade scene " + color);
            float startTransparent = 0f;
            float endTransparent = 1f;
            float smoothness = 0.05f;
            float progress = 0;
            float increment = smoothness / duration; //The amount of change to apply.
            if (startNewScene == true){
                startTransparent = 1f;
                endTransparent = 0f;
            }
            Color colorStart = new Color(color.r, color.g, color.b, startTransparent);
            camFade.GetComponent<Renderer>().materials[0].color = colorStart;
            Color colorEnd = new Color(colorStart.r, colorStart.g, colorStart.b, endTransparent);
            while (progress < 1){
                progress += increment;
                //Debug.Log(progress);
                camFade.GetComponent<Renderer>().materials[0].color = Color.Lerp(colorStart, colorEnd, progress);
                yield return new WaitForSeconds(smoothness);
            }
            yield return null;
            if (startNewScene == true){
                Debug.Log("Start scene " + sceneName+" tr="+ startNewScene);
                camFade.GetComponent<Renderer>().enabled = false;
                if (SceneEventSystem) { SceneEventSystem.enabled = true; }
            } else {
                Debug.Log("Start scene " + sceneName + " tr=" + startNewScene);               
            }
        } else { Debug.Log("Can not find camFade object");}
    }

    // a rotate transition between scenas
    IEnumerator RotateCamera(float duration, float speedUp, string newAction){
        Transform camTransform = Camera.main.transform;            
        float progress = 300;
        float smooth = 0.0001f; 
        if (speedUp < 0) { smooth = 0.05f; }
        float angleDelta = 1f;
        float newAngle = 0f;
        while (progress > 0 && smooth >0){
            progress--;
            // smooth = Mathf.Lerp(smooth, speedMax, smooth);
            smooth += speedUp;
            newAngle += angleDelta;
            Quaternion target = Quaternion.Euler(newAngle, newAngle, newAngle);
            //Debug.Log(progress + " " + smooth);            
            transform.rotation = Quaternion.Slerp(transform.rotation, target, smooth);
            yield return null; 
        }        
        Debug.Log("Start new action " + newAction);
       // StartAction(speedUp);
    }
    
    

    void StartAction(float speedUp){ StartCoroutine(RotateCamera(2f, -speedUp, "End rotation"));}  

    // switch between scenas
    void NextScene(int delta){ // switch scenes  
        utility.logDebug("NextScene");
        isActionSave = false;
        isHintDisplay = false;
        isDisplayTimer = false;
        SceneEventSystem.enabled = false;        
        Camera.main.GetComponent<GvrPointerPhysicsRaycaster>().enabled = false;
        //Debug.Log("curScene=" + curScene +" delta="+ delta+" "+ JsonUtility.ToJson(curSnenaMotionData));
        if (delta > 0){ testData.snenasMotionData.Add(curSnenaMotionData);}        
        curScene += delta;
        curfocusObjCode = new int[11] { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        curSnenaMotionData = new SnenaMotionData(){i = curScene, act = new List<UserActivity>()};
        if (rootObj) { Destroy(rootObj);}
        if (TimerCanvas) { Destroy(TimerCanvas);}
        switch (curScene){
            case 0: // show a start message
                rootObj = utility.ShowMessage(Data.getMessage(userLang, "Intro"), "Next", Data.getMessage(userLang, "btnNext"), new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
                break;
            case 1: // show a keyboard for email input
                rootObj = utility.ShowKeyboard(userLang);
                break;
            case 2: // start select a color
                rootObj = colorTest.showColors("selOneCol");
                string msg = Data.getMessage(userLang, "selOneCol");
                GameObject msgObj = utility.ShowMessage(msg, "", "", new Vector2(1200, 100), TextAnchor.MiddleCenter, new Vector2(0, 10));
                msgObj.transform.SetParent(rootObj.transform);
                msgObj.transform.position = new Vector3(0,4.4f,16);
                break;
            case 3: // show a start test message
                curTestIndex = 0;
                string msg1 = string.Format(Data.getMessage(userLang, "Test1"), Data.getMessage(userLang, "color_" + testsConfig[curTestIndex, 0]), Data.getMessage(userLang, "obj_" + testsConfig[curTestIndex, 0]));
                //Debug.Log("Test1=" + msg1);
                rootObj = new GameObject("root");
                GameObject rootMsg = utility.ShowMessage(msg1, "Next", Data.getMessage(userLang, "btnStart"), new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
                rootMsg.transform.SetParent(rootObj.transform);
                utility3D.createTarget(rootObj, testsConfig[0, 1]);
                break;
            case 4: // start test
                curTestIndex = 0;
                rootObj = utility3D.CreateObjsArray(false);
                isHintDisplay = true;
                isDisplayTimer = false;
                break;
            case 5: // show a start test message
                curTestIndex = 1;
                rootObj = new GameObject("root");
                string msg2 = string.Format(Data.getMessage(userLang, "Test2"), Data.getMessage(userLang, "color_" + testsConfig[curTestIndex, 1]), Data.getMessage(userLang, "obj_" + testsConfig[curTestIndex, 0]), testsConfig[curTestIndex, 5]);
                GameObject rootMsg2 = utility.ShowMessage(msg2, "Next", Data.getMessage(userLang, "btnStart"), new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
                rootMsg2.transform.SetParent(rootObj.transform);
                utility3D.createTarget(rootObj, testsConfig[0, 1]);
                break;
            case 6: //start test with a time limit
                curTestIndex = 1;
                rootObj = utility3D.CreateObjsArray(true);
                isHintDisplay = true;
                isDisplayTimer = true;
                break;
            case 7: // show a color test start message
                rootObj = utility.ShowMessage(Data.getMessage(userLang, "IntroColTest"), "Next", Data.getMessage(userLang, "btnStart"), new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
                break;
            case 8: // start color test 
                testData.colorTestResult.selected.Clear();
                rootObj = colorTest.showColors("selAllCol");
                GameObject msgObj2 = utility.ShowMessage(Data.getMessage(userLang, "IntroColTest"), "", Data.getMessage(userLang, "btnStart"), new Vector2(1200, 200), TextAnchor.MiddleCenter, new Vector2(0, 20));
                msgObj2.transform.SetParent(rootObj.transform);
                msgObj2.transform.position = new Vector3(0, 5.4f, 16);
                break;
            case 9: // show a start text test message
                rootObj = utility.ShowMessage(Data.getMessage(userLang, "IntroTextTest"), "Next", Data.getMessage(userLang, "btnStart"), new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40)); 
                break;
            case 10: // start a text test
                testData.textTestResult.answers.Clear();
                testData.textTestResult.extra = 0;
                testData.textTestResult.stabil = 0;
                testData.textTestResult.totalTime = 0;
                questionsCount = Data.getQuestionsCount(userLang);
                string[] curQuestion = Data.getQuestionIndex(userLang, testData.textTestResult.answers.Count);
                int.TryParse(curQuestion[0],out curQuestionKey);
                string notes = (testData.textTestResult.answers.Count + 1).ToString() +"/"+ questionsCount.ToString();
                rootObj = utility.ShowDialog(curQuestion[1], notes, "NextQuestion", Data.getMessage(userLang, "yes"), Data.getMessage(userLang, "not"), new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
                break;
            case 11: // show results
                float cnt = Data.getQuestionsCount(userLang)/2;
                float resF = (testData.textTestResult.extra - cnt/2)/ cnt;
                float resH = (testData.textTestResult.stabil - cnt/2) / cnt;
                rootObj = utility.showResult(userLang, testData.colorTestResult.stress, testData.colorTestResult.energy, resF, resH);
                break;
            case 12: // show a start text test message
                rootObj = utility.ShowMessage(Data.getMessage(userLang, "msgAbout"), "Back", Data.getMessage(userLang, "btnBack"), new Vector2(1200, 600), TextAnchor.MiddleCenter, new Vector2(0, 40));
                break;
            default: Debug.Log("Not Found current scene index"); break;
        }
        userSceneDataTime[0] = 0f;
        userSceneDataTime[1] = 0f;
        trackingTime[1] = 0f;
        SceneEventSystem.enabled = true;
        isActionSave = true;
    }

    // on the app stop
    void OnDisable(){
        //Debug.Log("PrintOnDisable: script was disabled");        
        sendDataToServer();
    }

    // on the app start
    void OnEnable(){ //Debug.Log("PrintOnEnable: script was enabled");
    }

    // send data to server
    private void sendDataToServer(){
        isActionSave = false;
        timerShowResult[0] = 0;
        if (testData!=null) { 
            if(testData.snenasMotionData.Count>0){
                // Debug.Log(testData);
                // string json = JsonUtility.ToJson(testData);
                // string dataPath = Path.Combine(Application.persistentDataPath, "CharacterData.txt");
                // using (StreamWriter streamWriter = File.CreateText("c:\\11\\unityJson.txt")){streamWriter.Write(json);}
                // Debug.Log(json);
                connection.putDataBlob("/putVrData", testData);
                initTestData();
                curSnenaMotionData = new SnenaMotionData() { i = 0, act = new List<UserActivity>() };                        
            }else{Debug.Log("No data send to server");}
        }
    }

    private void initTestData(){
        testData = new TestData()
        {
            startDateTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"),
            deviceID = deviceID,
            lang = userLang,
            gyro = checkGyro,
            deviceInfo = deviceDesc,
            userEmail = userEmail,
            userZone = userZone,
            snenasMotionData = new List<SnenaMotionData>(),
            rightObjectsList = new List<TestObjects>(),
            selectedObjectsList = new List<TestObjects>(),
            colorTestResult = new ColorTestResult(),
            textTestResult = new TextTestResult()
        };
    }

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

    private IEnumerator FadeTo(string objName)
    {
        GameObject obj = GameObject.Find(objName);
        if (obj) {
            Destroy(obj.GetComponent<EventTrigger>());
            float smoothness = 0.05f; float duration = 1f;
            Color colorStart = obj.GetComponent<Image>().color;
            Color colorEnd = new Color(colorStart.r, colorStart.g, colorStart.b, 0);
            float progress = 0; float increment = smoothness / duration; //The amount of change to apply.
            while (progress < 1)
            {
                progress += increment;
                if (obj) { 
                obj.GetComponent<Image>().color = Color.Lerp(colorStart, colorEnd, progress);
                    yield return new WaitForSeconds(smoothness);
                }
                else
                {
                    break;
                }
            };
            Destroy(obj);
        }
        yield return null;
    }
}
