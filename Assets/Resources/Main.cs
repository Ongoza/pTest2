﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using System.IO; //save to file

//Сделать цветовой тест в виде класса на текущей сцене
//Добавить цветовой тест в конце теста
//выводить опросник Айзенка - сокращенный до 30-40 вопросов
//(по 15-20 на каждую шкалу)
//Выводить результаты
//  - уровень темперамента в трех шкалах(флегметик 40% сангвиник 60% устойчивость - 20%)
// - уровень стресса и эффективность
//- выбор цвета фигуры для поиска  
//- выбор формы фигуры для поиска

//переключение языков и определения языка системы Application.systemLanguage
//Изображение пирамиды в задании
//Сделать ограничение по времени записи движений для каждой сцены (15 сек на сцену)
//Сделать ограничение по времени на нахождение без движения и выход(1 минуту на одной сцене через меню подтверждения выхода (15 сек))
//Сделать стрелку указатель на текстовое сообщение
//Сделать заставку с анимацитей ввода текста(Психологические тесты) и появление 3д модели VR

//Поправить проверку на гиро и девайс инфо

//сделать страницу для отображения результата  - сделал
//Кнопки в тестовом тесте больше сделать - сделал
//Не показывает на других компьютерах - проверить!!!
//ограничение на размер записи - сделал
//Got a packet bigger than 'max_allowed_packet' bytes"  

public class Main : MonoBehaviour
{
    // Указатель на кнопку выход
    // global temp variables
    private int curScene = 0; //  start scene index 
    private bool isDebug = false; // debug enable
    private bool isNet = true; // network enable

    private int precisionDec = 100; // number dec after point in movement control 1 - 0, 10-0.0, 100 - 0.00
    private float baseLoc = 2;
    private Sprite uisprite; // default img for text background
    private GameObject camFade; // camera Fade object
    private Font defaultFont; // defule text font
    private Text TextEmail; // user email   
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
    private string userLang = "ru";
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
    private bool isTimer = true; // display select cursor 
    private TestData testData;

    // variables for tests
    //{ "Cube", "Sphere", "Capsule", "Cylinder","Pyramid" };
    // {"gray","blue","green","red","yellow","purple","brown","black" }
    private int[,] testsConfig = new int[2,6] { // testsConfig[x,0] - (0-4) type index of an object for search in the first test
        { 4, 4, 5, 0, 0, 0 }, // testsConfig[x,1] - (0-7) index of a selected object color
        { 4, 4, 5, 0, 0, 20 }}; // testsConfig[x,2] total number of objects for selection in tests
                               // testsConfig[x,3] total number of right founeded objects in a selection in test
                               // testsConfig[x,4] total number of all founeded objects in a selection test
                               // testsConfig[curTestIndex,5] time limit for the second test in sec. 0 - unlimit
    private int curTestIndex = 0; //a current test number

    private int nHor = 10;   // Total number of objects for 6 = 16, for 10 =32 
    private float distanceMax = 8f; // min distance from camera to an object
    private float distanceMin = 16.1f; // max distance from camera to an objec    
    // list of objects colors {"gray","blue","green","red","yellow","purple","brown","black" }
    private readonly List<Color> arrColor = new List<Color>(){ 
        new Color(141f / 255f, 141f / 255f, 141f / 255f, 1f),
        new Color(0f, 0f, 128f / 255f, 1f),
        new Color(3f / 255f, 114f / 255f, 21f / 255f, 1f),
        new Color(246f / 255f, 6f / 255f, 22f / 255f, 1f),
        new Color(251f / 255f, 251f / 255f, 2f / 255f, 1f),
        new Color(139f / 255f, 0f, 139f / 255f, 1f),
        new Color(139f / 255f, 69f / 255f, 19f / 255f, 1f),
        new Color(0f, 0f, 0f, 1f)
    };    
    private Vector3 btExitLoc;    
    private bool isHintDisplay = false; // display hint during a test
    private int checkGyro = 0;
    private string deviceDesc;
    private bool isDisplayTimer = false;// display timer during a test
    private float testTime;// test time left
    private Text hintText;
    GameObject TimerCanvas; // timer object
    public Material primitivesMaterial;    
    private int[] objCounter = new int[5] {0,0,0,0,0};    
    private int nVer;
    private float aStartH;
    private float aStartV;
    private float aStartR;    

    //fixing actions time variables
    //save user actions during one scene
    private SnenaMotionData curSnenaMotionData;
    private float userSceneDataTime =0.0f; // timer data
    // result settings
    private float[] timerShowResult = new float[]{0f, 10f, 10f}; // timer how long wait for the result [trigger,default,current]

    void Start(){             
        defaultFont = Font.CreateDynamicFontFromOSFont("Roboto", 1);
        GameObject goDebug = GameObject.Find("txtDebug");
        checkGyro = 0;
        #if UNITY_EDITOR
            Debug.Log("Unity Editor!!!!");
            checkGyro = 1;
        #else
        if( SystemInfo.supportsGyroscope){checkGyro = 1;}
        #endif
        deviceID = SystemInfo.deviceUniqueIdentifier.ToString();
        userZone = System.TimeZoneInfo.Local.ToString();
        //userLang =  Application.systemLanguage.ToString();
        //Debug.Log("userLang="+userLang);
        if (checkGyro>0) {Init(goDebug);
        }else{
            TextDebug = goDebug.GetComponent<Text>();
            TextDebug.text = Data.getMessage("Gyro");
            StartCoroutine(PauseInit(7, goDebug));
        }
    }

    void Init(GameObject goDebug) {
        camFade = GameObject.Find("camProtector");
        if (isDebug)
        {
            if (goDebug) { TextDebug = goDebug.GetComponent<Text>(); }
        }
        else { Destroy(goDebug); }
        try
        {
            deviceDesc = "#_"+SystemInfo.deviceModel+",#_"+ SystemInfo.deviceType + ",#_" + SystemInfo.deviceName + ",#_" + SystemInfo.operatingSystem;
            Debug.Log(deviceDesc);
        }
        catch (System.Exception e)
        {
            Debug.Log(e);
        }
            
        logDebug("Init");
        testData = new TestData()
        {
            deviceID = deviceID,
            lang = userLang,
            gyro = checkGyro,
            deviceInfo = deviceDesc,
            userZone = userZone,
            startDateTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"),
            snenasMotionData = new List<SnenaMotionData>(),
            rightObjectsList = new List<TestObjects>(),
            selectedObjectsList = new List<TestObjects>()
        };
        logDebug("Start 1");
        curSnenaMotionData = new SnenaMotionData() { i = 0, act = new List<UserActivity>() };
        lastAction = new float[14];
        GameObject ObjEventSystem = GameObject.Find("GvrEventSystem");
        if (ObjEventSystem)
        {
            SceneEventSystem = ObjEventSystem.GetComponent<EventSystem>();
            if (SceneEventSystem) { SceneEventSystem.enabled = false; }
        }
        else { Debug.Log("Can not find Event System"); }
        Camera.main.GetComponent<GvrPointerPhysicsRaycaster>().enabled = false;
        logDebug("Start 2");
        //uisprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        Texture2D tex = new Texture2D(128, 128);
        uisprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        GameObject camTimedPointer = GameObject.Find("GvrReticlePointer");
        timedPointer = camTimedPointer.GetComponent<Renderer>().material;
        nVer = nHor * 2;
        aStartH = 2 * Mathf.PI / nHor;
        aStartV = 2 * Mathf.PI / nVer;
        aStartR = aStartH / 4f;
        // StartCoroutine(fadeScene(2f, true, new Color(0.2f, 0.2f, 0.2f, 1), "Main"));
        //StartCoroutine(RotateCamera(2f, 0.0001f, "End rotation"));
        string srv = Data.getConnectionData()["ServerIP"] + ":" + Data.getConnectionData()["ServerPort"];
        //Debug.Log(string.Join(";", Data.getConnectionData()["SeverPort"]));
        connection = new Connection(srv, isNet);
        // Test connection module
        //connection.putDataString("/putTest", "{'test': 'data'}");
        // string txtFromFile = "";
        // using (StreamReader streamReader = File.OpenText("c:\\11\\unityJson.txt")){txtFromFile = streamReader.ReadToEnd();}
        // Debug.Log("Start end 1");
        //Debug.Log(txtFromFile);
        // TestData testData2 =  JsonUtility.FromJson<TestData>(txtFromFile);
        // Debug.Log("Start end");
        // connection.putDataBlob("/putVrData", testData2);
        Debug.Log("Start end");
        NextScene(0);
    }


    public IEnumerator PauseInit(float timer, GameObject goDebug)
    {
        float currCountdownValue = timer;
        while (currCountdownValue > 0)
        {
            Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        Init(goDebug);
    }
    void logDebug(string msg){
        if (isDebug){
            if (TextDebug){
                TextDebug.text += msg + "=>";
            } }
    }

    // create objects koordinates list around the camera in random locations
    void CreateObjsArray(bool isShowTime){
        logDebug("CreateObjsArray");
        rootObj = new GameObject("rootObj");
        rootObj.transform.position = new Vector3(0,baseLoc,0);
        testsConfig[curTestIndex,3] = 0;
        testsConfig[curTestIndex, 4] = 0;
        // create the list of a coordinate of objects        
        List<Vector3> locations = new List<Vector3>();
        bool pi_0 = true;
        bool pi_2 = true;
        for (int i = 0; i < nVer / 2; i++){
            float evalution = i * aStartV * 2.0f;
            if (1.1f < evalution && 1.9f > evalution){ // on the top put only one object
                if (pi_0){ pi_0 = false;
                    locations.Add(SphericalToCartesianPlusRandom(Mathf.PI / 2f, evalution));
                }
            }else if (4.0f < evalution && 5.2f > evalution){ // on the bottom put only one object. This is "Exit" button
                if (pi_2){ pi_2 = false;
                    float a = 10 * Mathf.Cos(evalution);
                    btExitLoc.x = a * Mathf.Cos(Mathf.PI * 1.5f);
                    btExitLoc.y = 10 * Mathf.Sin(evalution);
                    btExitLoc.z = a * Mathf.Sin(Mathf.PI * 1.5f);
                    //locations.Add(SphericalToCartesianPlusRandom(Mathf.PI * 1.5f, evalution));
                }
            }else{
                for (int j = 0; j < nHor / 2; j++){
                    float polar = j * aStartH;
                    locations.Add(SphericalToCartesianPlusRandom(polar, evalution));
                }
            }
        }        
        int max = locations.Count;
        //create objects for selection by a tested person
        List<int> objectTypes = new List<int>() { 0, 1, 2, 3, 4 };
        TestObjects rightObjectsList = new TestObjects() {
            i = curTestIndex,
            objs = new List<TestObject>()
        };        
        for (int j = 1; j < testsConfig[curTestIndex, 2]+1; j++){
            int newIndex = Random.Range(0, max);            
            string name = j+"_"+testsConfig[curTestIndex, 0] + "_" + testsConfig[curTestIndex, 1] + "_0" ;
            int rot = Random.Range(0, 360);
            //Debug.Log("created right "+name);
            SetUpObj(testsConfig[curTestIndex, 0], name, locations[newIndex], arrColor[testsConfig[curTestIndex, 1]], rot);
            rightObjectsList.objs.Add(
                new TestObject() {
                        i = j,
                        color = testsConfig[curTestIndex, 1], 
                        type = testsConfig[curTestIndex, 0],
                        lx = Mathf.RoundToInt(locations[newIndex].x* precisionDec),
                        ly = Mathf.RoundToInt(locations[newIndex].y * precisionDec),
                        lz = Mathf.RoundToInt(locations[newIndex].z * precisionDec),
                        rx = rot,
                        ry = rot,
                        rz = rot
                });
            locations.RemoveAt(newIndex);
            max--;
        }
        testData.rightObjectsList.Add(rightObjectsList);
        testData.selectedObjectsList.Add(
            new TestObjects() { 
                i = curTestIndex,
                timer = testsConfig[curTestIndex, 5],
                time = 0,
                rights = 0,
                objs = new List<TestObject>()
            });
        objectTypes.Remove(testsConfig[curTestIndex,0]);
        //Debug.Log(testData.selectedObjectsList.Count + " testData created =" + JsonUtility.ToJson(testData.selectedObjectsList));
        //create rnadom objects
        int cntObjs = 1;
        foreach (Vector3 loc in locations){
            int colIndex = Random.Range(0, 8);
            Color col = arrColor[colIndex];
            int typeObj = objectTypes[Random.Range(0, 4)];            
            string objName = "0_"+ typeObj +"_" + colIndex + "_" + objCounter[typeObj];
            objCounter[typeObj]++;
            cntObjs++;
            SetUpObj(typeObj, objName, loc, col, Random.Range(0, 360));
        }
        //Create "Exit" button       
        GameObject canExit = CreateCanwas("rootMenu", btExitLoc, new Vector2(100, 50));
        canExit.transform.SetParent(rootObj.transform);
        CreateButton(canExit.transform, "btExit", "Exit", "Next", "0_10_10", new Vector3(0, 0, 0), new Vector2(100, 50));
        canExit.transform.Rotate(Vector3.left, -60);
        Camera.main.GetComponent<GvrPointerPhysicsRaycaster>().enabled = true;
        // Create hint for a tested person
        TimerCanvas = new GameObject("Hint");
        Canvas c = TimerCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.WorldSpace;
        TimerCanvas.AddComponent<CanvasScaler>();
        RectTransform NewCanvasRect = TimerCanvas.GetComponent<RectTransform>();
        TimerCanvas.transform.SetParent(Camera.main.transform, true);
        NewCanvasRect.localPosition = new Vector3(0, 2.8f, 7);
        NewCanvasRect.sizeDelta = new Vector2(300, 40);
        NewCanvasRect.localScale = new Vector3(0.06f, 0.06f, 1f);
        GameObject panel = new GameObject("HintPanel");
        panel.AddComponent<CanvasRenderer>();
        Image img = panel.AddComponent<Image>();
        img.sprite = uisprite;
        img.type = Image.Type.Sliced;
        img.color = new Vector4(1f, 1f, 1f, 1f);
        string msg = string.Format(Data.getMessage("Test_hint"), Data.getMessage("color_" + testsConfig[curTestIndex, 1]), Data.getMessage("obj_" + testsConfig[curTestIndex, 0]), 0, testsConfig[curTestIndex, 2]);
        int height = 20;
        // Add to the hint message a timer informer
        //Debug.Log(testsConfig[curTestIndex, 5]);
        if (isShowTime){
            msg += string.Format(Data.getMessage("Test_timer"), testsConfig[curTestIndex, 5]);
            height = 40;
            testTime = testsConfig[curTestIndex, 5];
        }
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(NewCanvasRect, true);
        panelTransform.localScale = new Vector3(0.3f, 0.3f, 1f);
        panelTransform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        panelTransform.localPosition = new Vector3(0, 0, 0);
        panelTransform.sizeDelta = new Vector2(300, height);
        GameObject txtObj = CreateText(panelTransform, new Vector2(0,1.5f), new Vector2(300, height), msg, 12, 0, TextAnchor.MiddleCenter);
        txtObj.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        hintText = txtObj.GetComponent<Text>();        
        isHintDisplay = true;
        isDisplayTimer = isShowTime;
    }

    // Set up 3d paramaters for 3d objects for a test
    void SetUpObjParams(GameObject obj, string name, Vector3 loc, Color col, float rot){
        //logDebug("SetUpObjParams");
        if (obj) { 
            obj.name = name;
            obj.transform.SetParent(rootObj.transform);
            obj.transform.position = loc;            
            obj.transform.eulerAngles = new Vector3(rot, rot, rot);
            Renderer rend = obj.GetComponent<Renderer>();
            rend.material = primitivesMaterial;
            rend.material.color = col;
            obj.AddComponent<SphereCollider>();
            EventTrigger be = obj.AddComponent<EventTrigger>();
            EventTrigger.Entry entryEnterGaze = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            entryEnterGaze.callback.AddListener((eventData) => { OnEnterTimed("Test", name, false); });
            be.triggers.Add(entryEnterGaze);
            EventTrigger.Entry entryExitGaze = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            entryExitGaze.callback.AddListener((eventData) => { OnExitTimed(); });
            be.triggers.Add(entryExitGaze);
        }
    }

    // create 3d objects for a test 
    void SetUpObj(int typeObj, string name, Vector3 loc, Color col, float rot){
        //logDebug("SetUpObj");
        switch (typeObj){
            case 0: SetUpObjParams(GameObject.CreatePrimitive(PrimitiveType.Cube), name, loc, col, rot); break;
            case 1: SetUpObjParams(GameObject.CreatePrimitive(PrimitiveType.Sphere), name, loc, col, rot); break;
            case 2: SetUpObjParams(GameObject.CreatePrimitive(PrimitiveType.Capsule), name, loc, col, rot); break;
            case 3: SetUpObjParams(GameObject.CreatePrimitive(PrimitiveType.Cylinder), name, loc, col, rot); break;
            case 4:
                //Debug.Log("created 2 right " + name);
                SetUpObjParams(CreatPyramid3(), name, loc, col, rot); break;
            default: print("Error!!! Incorrect obj type."); break;
        }
    }

    // transform coordinates from Spherical To Cartesian and and some random parameters
    Vector3 SphericalToCartesianPlusRandom(float polar, float evalution){
        logDebug("Start");
        Vector3 outCart;
        float distanceR = Random.Range(distanceMin, distanceMax);
        float evalutionR = Random.Range(evalution - aStartR, evalution + aStartR);
        float polarR = Random.Range(polar - aStartR, polar + aStartR);
        float a = distanceR * Mathf.Cos(evalution);
        outCart.x = a * Mathf.Cos(polarR);
        outCart.y = distanceR * Mathf.Sin(evalutionR);
        outCart.z = a * Mathf.Sin(polarR);
	    return outCart;
    }

    GameObject CreatPyramid3(){       
        Mesh mesh = new Mesh();
        Vector3 p0 = new Vector3(0, 0, 0);
        Vector3 p1 = new Vector3(1, 0, 0);
        Vector3 p2 = new Vector3(0.5f, 0, Mathf.Sqrt(0.75f));
        Vector3 p3 = new Vector3(0.5f, Mathf.Sqrt(0.75f), Mathf.Sqrt(0.75f) / 3);
        mesh.Clear();
        mesh.vertices = new Vector3[]{p0,p1,p2,p0,p2,p3, p2,p1,p3,p0,p3,p1};
        mesh.triangles = new int[]{0,1,2,3,4,5,6,7,8,9,10,11};
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();       
        GameObject obj = new GameObject();       
        MeshFilter mF = obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        mF.sharedMesh = mesh;
        return obj;
    }

    void CreatPyramid4(){
        Mesh mesh = new Mesh();
        mesh.Clear();
        Vector3 p0 = new Vector3(1, 1, 0); //front right top
        Vector3 p1 = new Vector3(0, 1, 0); //front left top
        Vector3 p2 = new Vector3(1, 0, 0); //front right down
        Vector3 p3 = new Vector3(0, 0, 0); //front left down
        Vector3 p4 = new Vector3(1, 1, 1); //back right top
        Vector3 p5 = new Vector3(0, 1, 1); //back left top
        Vector3 p6 = new Vector3(1, 0, 1); //back right down
        Vector3 p7 = new Vector3(0, 0, 1); //back left down
        Vector3 p8 = new Vector3(0.5f, 1, 0.5f); //center top
        Vector3 p9 = new Vector3(0.5f, 0, 0.5f); //center down
        mesh.vertices = new Vector3[]{p0,p1,p2,p3, p4,p5,p6,p7,p8 };
        mesh.triangles = new int[]{ 8,2,3,8,6,2,8,3,7,8,7,6,3,2,7,2,6,7};         // front // right// left// back// bottom
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        GameObject obj = new GameObject();
        MeshFilter mF = obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        Renderer rend = obj.GetComponent<Renderer>();
        mF.sharedMesh = mesh;
    }

    // create a message object
    private void ShowMessage(string msg, string action, string actionLabel, Vector2 size, UnityEngine.TextAnchor anchor, Vector2 startLoc){
        logDebug("ShowMessage");
        rootObj = CreateCanwas("rootMenu", new Vector3(0, baseLoc, 12), size);
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image i = panel.AddComponent<Image>();
        i.color = new Vector4(1, 1, 1, 0.7f);
        i.sprite = uisprite;
        i.type = Image.Type.Sliced;
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(rootObj.transform, true);
        panelTransform.localScale = new Vector3(1f, 1f, 1f);
        panelTransform.localPosition = new Vector3(0, 40, 0);
        panelTransform.sizeDelta = size;
        //panelTransform.rotation = Quaternion.AngleAxis(-180, Vector3.up);        
        CreateText(panelTransform, startLoc, size, msg, 50, 0, anchor);
        CreateButton(panelTransform, "Button", actionLabel, action,"0_10_10",new Vector3(0, 60-size.y/2, 0), new Vector2(300,60));
       // showTutorialsMenu(rootMenu);
    }

    //create button object
    private GameObject CreateButton(Transform parent, string name, string label, string action1, string action2, Vector3 loc, Vector2 size){
        logDebug("CreateButton");
        GameObject bt0 = new GameObject(name);
        RectTransform br = bt0.AddComponent<RectTransform>();
        br.sizeDelta = size;
        Image img = bt0.AddComponent<Image>();
        img.sprite = uisprite;
        img.color = new Vector4(0.5f, 0.5f, 0.8f, 1);
        //img.material.color = new Vector4(1f, 1f, 1f, 0.7f);
        img.type = Image.Type.Sliced;
        Button bt = bt0.AddComponent<Button>();
        bt0.transform.SetParent(parent, true);
        bt0.transform.localPosition = loc;
        //bt0.transform.rotation = Quaternion.AngleAxis(-180, Vector3.up);
        bt0.transform.localScale = new Vector3(1, 1, 1);
        CreateText(bt.transform, new Vector2(0, 0), size, label, 40, 1, TextAnchor.MiddleCenter);
        bt.onClick.AddListener(OnClickTimed);
        EventTrigger be = bt0.AddComponent<EventTrigger>();
        EventTrigger.Entry entryEnterGaze = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnterGaze.callback.AddListener((eventData) => { OnEnterTimed(action1, action2, true); });
        be.triggers.Add(entryEnterGaze);
        EventTrigger.Entry entryExitGaze = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExitGaze.callback.AddListener((eventData) => { OnExitTimed(); });
        be.triggers.Add(entryExitGaze);
        return bt0;
    }

    // create text object
    private GameObject CreateText(Transform parent, Vector2 loc, Vector2 size, string message, int fontSize, int fontStyle, TextAnchor achor){
        logDebug("CreateText");
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(parent);
        RectTransform trans = textObject.AddComponent<RectTransform>();
        trans.sizeDelta = size;
        trans.anchoredPosition3D = new Vector3(0, 0, 0);
        trans.anchoredPosition = loc;        
        trans.localScale = new Vector3(1.0f, 1.0f, 0f);
        trans.localPosition.Set(0, 0, 0);
        textObject.AddComponent<CanvasRenderer>();
        Text text = textObject.AddComponent<Text>();
        text.supportRichText = true;
        text.text = message;
        text.fontSize = fontSize;
        if (fontStyle == 1) { text.fontStyle = FontStyle.Bold; }
        text.font = defaultFont;        
        text.alignment = achor;
        //text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.color = new Color(0, 0, 0);
        return textObject;
    }

    void Update(){
        //if (Input.GetKeyDown(KeyCode.Escape)){
        //    // Android close icon or back button tapped.
        //    Application.Quit();
        //}
        if (isActionSave) {
            userSceneDataTime += Time.deltaTime;
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
            if (trNewData) {
                //Debug.Log("0 curAction = " + string.Join(";", curAction)+ "0 lastAction = " + string.Join(";", lastAction));
                curSnenaMotionData.act.Add(new UserActivity { t = Mathf.RoundToInt(userSceneDataTime*1000), a = curAction });
                //Debug.Log("1 curAction = " + JsonUtility.ToJson(curSnenaMotionData.userActivities));
                for (int i = 0; i < curAction.Length; i++){lastAction[i] = curAction[i];}                
            }
        }
        if (isTimer){ // display selecting pointer
            workTime -= Time.deltaTime;
            if (workTime <= 0) { OnClickTimed();
            }else{ if (timedPointer){ timedPointer.SetFloat("_Angle", (1f - workTime / defaultTime) * 360); } }
        }
        if (isHintDisplay){ //display hint in test
            string msgTimer = "";
            if (isDisplayTimer){
                testTime -= Time.deltaTime;
                msgTimer += string.Format(Data.getMessage("Test_timer"), Mathf.Floor(testTime).ToString());
                if (testTime < 0){ testEnd();}
                if (hintText) {
                    hintText.text = string.Format(Data.getMessage("Test_hint"), Data.getMessage("color_" + testsConfig[curTestIndex, 1]), Data.getMessage("obj_" + testsConfig[curTestIndex, 0]), testsConfig[curTestIndex, 4], testsConfig[curTestIndex, 2])
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
     private void OnEnterTimed(string type, string name, bool isButton){
        //Debug.Log("onEnterTimed=" + name + "=" + type+" butt="+ isButton);
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
    private void OnExitTimed(){
        isTimer = false;
        curfocusObj = "";
        curfocusType = "";
        curfocusObjCode = new int[11] { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        workTime = defaultTime;
        if (timedPointer) { timedPointer.SetFloat("_Angle", 360); }
    }

    private void OnClickTimed(){ ClickSelectEvent();}

    // on the cusror timed click on an active object in a scena
    private void ClickSelectEvent(){
        isTimer = false;
        //addOevrlayInfo("clickSelectEvent" + name);
        //Debug.Log("clickSelectEvent=" + curfocusType + "="+ curfocusObj);
        switch (curfocusType){
            case "Next":  NextScene(1);  break;
            case "Test": // test scenes activity
                GameObject gm = GameObject.Find(curfocusObj);                
                if (gm) { Destroy(gm); } else { Debug.Log("Error! Can not find object for destroy " + curfocusObj); }
                //Debug.Log(testData.selectedObjectsList.Count + "="+ JsonUtility.ToJson(testData.selectedObjectsList));
                testData.selectedObjectsList[testData.selectedObjectsList.Count-1].objs.Add( 
                    new TestObject() {
                        time = Mathf.RoundToInt(userSceneDataTime*1000),
                        i = curfocusObjCode[0],
                        lx = curfocusObjCode[5],
                        ly = curfocusObjCode[6],
                        lz = curfocusObjCode[7],
                        rx = curfocusObjCode[8],
                        type =  curfocusObjCode[1], // cur obj type 
                        color = curfocusObjCode[2], // cur obj color
                    });
                testsConfig[curTestIndex, 4]++;
                // if this is a right object                
                if (curfocusObjCode[0]!=0){ testsConfig[curTestIndex, 3]++; }
                //Debug.Log("obj:"+ curfocusObj + " result right:" + testsConfig[curTestIndex, 3] + " total:" + testsConfig[curTestIndex, 4]);
                // if a testing person reach a limit of objects
                if (testsConfig[curTestIndex, 4] >= testsConfig[curTestIndex, 2]) {
                    testEnd();                    
                } else { 
                    string msg = string.Format(Data.getMessage("Test_hint"), Data.getMessage("color_" + testsConfig[curTestIndex, 1]), Data.getMessage("obj_" + testsConfig[curTestIndex, 0]), testsConfig[curTestIndex, 4], testsConfig[curTestIndex, 2]);
                    if (isHintDisplay) { msg += string.Format(Data.getMessage("Test_timer"), Mathf.Floor(testTime).ToString());}
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
            case "Exit":
                sendDataToServer();
                curScene = 0;
                NextScene(0); break;
            default: Debug.Log("clickSelectEvent not found action for " + name); break;
        }
        OnExitTimed();
    }

    // a fade transition between scenas
    IEnumerator FadeScene(float duration, bool startNewScene, Color color, string sceneName){
        logDebug("FadeScene");
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
                
                //UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
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
    
    // display the email input scena
    void ShowKeyboard() { // show keyboard input
        int len = 12; int width = 60; int startPosX = -342;  int startPosY = 125; int i = 0; int j = 0;
        rootObj = CreateCanwas("rootKeyoard", new Vector3(0,  -3, 12), new Vector2(770, 350));
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image img = panel.AddComponent<Image>();
        img.color = new Vector4(1, 1, 1, 0.7f);
        img.sprite = uisprite;
        img.type = Image.Type.Sliced;
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(rootObj.transform, true);
        panelTransform.localScale = new Vector3(1f, 1f, 1f);
        panelTransform.localPosition = new Vector3(0, 0, 0);
        panelTransform.sizeDelta = new Vector2(770, 330);
        Dictionary<string, string> keyDictionary = Data.getKeys();
        foreach (KeyValuePair<string, string> item in keyDictionary){
            float delta = 0;
            if (item.Key.Length > 1) { width = 60 * 2; i += 1; delta = 30; }
            if (i >= len) {j++; i = 0;}
            GameObject btKey = CreateButton(panelTransform, item.Key, item.Value, "Keyboard", item.Key, new Vector3(startPosX + i * (60 + 2) - delta, startPosY - j * (60 + 2), 0), new Vector2(width, 60));            
            Image imgKey = btKey.GetComponent<Image>();
            imgKey.color = new Vector4(0.5f, 0.5f, 0.8f, 1);
            i++;            
        }
        CreateButton(rootObj.transform, "Enter", "Start", "Enter","0_10_10", new Vector3(0, -230,0), new Vector2(150, 60));      
        GameObject InputMsg = new GameObject("InputMsg");
        InputMsg.transform.SetParent(rootObj.transform);
        RectTransform brInputMsg = InputMsg.AddComponent<RectTransform>();
        brInputMsg.localScale = new Vector3(1f, 1f, 1f);
        brInputMsg.sizeDelta = new Vector2(800, 240);
        brInputMsg.localPosition = new Vector3(0, 360, 0);
        Image imgInputMsg = InputMsg.AddComponent<Image>();
        imgInputMsg.sprite = uisprite;
        imgInputMsg.color = new Vector4(0.8f, 0.8f, 0.8f, 1);
        imgInputMsg.type = Image.Type.Sliced;
        CreateText(brInputMsg, new Vector2(0, 1f), new Vector2(800, 180), Data.getMessage("Email"), 40, 1, TextAnchor.MiddleCenter);   
        GameObject Input = new GameObject("Input");
        Input.transform.SetParent(rootObj.transform);
        RectTransform brInput = Input.AddComponent<RectTransform>();
        brInput.localScale = new Vector3(1f, 1f, 1f);
        brInput.sizeDelta = new Vector2(800, 50);      
        brInput.localPosition = new Vector3(0, 200, 0);                
        Image imgInput = Input.AddComponent<Image>();
        imgInput.sprite = uisprite;
        imgInput.color = new Vector4(0.9f, 0.9f, 0.9f, 1);
        imgInput.type = Image.Type.Sliced;
        GameObject InputText = CreateText(brInput, new Vector2(0, -0.5f), new Vector2(800, 50), "", 40, 1, TextAnchor.MiddleCenter);
        TextEmail = InputText.GetComponent<Text>();        
    }

    // create canavas for text messages
    GameObject CreateCanwas(string name, Vector3 loc, Vector2 size){
        logDebug("CreateCanwas");
        GameObject objCanvas = new GameObject(name);
        Canvas c = objCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.WorldSpace;
        objCanvas.AddComponent<CanvasScaler>();
        RectTransform NewCanvasRect = objCanvas.GetComponent<RectTransform>();
        objCanvas.AddComponent<GvrPointerGraphicRaycaster>();        
        NewCanvasRect.localPosition = loc;
        NewCanvasRect.sizeDelta = size;
        NewCanvasRect.localScale = new Vector3(0.014f, 0.014f, 1f);
        return objCanvas;
    }

    // switch between scenas
    void NextScene(int delta){ // switch scenes  
        logDebug("NextScene");
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
            case 0:                
                ShowMessage(Data.getMessage("Intro"), "Next","Start", new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
                break;
            case 1: ShowKeyboard(); break;
            case 2: curTestIndex = 0;
                string msg1 = string.Format(Data.getMessage("Test1"), Data.getMessage("color_"+ testsConfig[curTestIndex, 0]), Data.getMessage("obj_"+ testsConfig[curTestIndex, 0]));
                //Debug.Log("Test1=" + msg1);
                ShowMessage(msg1, "Next", "Start", new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 40));
                break;
            case 3:
                curTestIndex = 0;
                CreateObjsArray(false);
                break;
            case 4:                
                curTestIndex = 1;
                string msg2 = string.Format(Data.getMessage("Test2"), Data.getMessage("color_" + testsConfig[curTestIndex, 1]), Data.getMessage("obj_" + testsConfig[curTestIndex, 0]), testsConfig[curTestIndex, 5]);
                ShowMessage(msg2, "Next", "Start", new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 0));
                break;
            case 5: curTestIndex = 1; CreateObjsArray(true); break;
            case 6:
                timerShowResult[2] = timerShowResult[1];
                timerShowResult[0] = 1 ;                
                string msg3 = string.Format(Data.getMessage("Result"), testsConfig[0, 3], testsConfig[0, 2], testsConfig[1, 3], testsConfig[1, 2], Mathf.Floor(testsConfig[curTestIndex, 5] - testTime).ToString());
                ShowMessage(msg3, "Exit", "Repeat", new Vector2(1400, 600), TextAnchor.MiddleLeft, new Vector2(25,50));
                break;
            default: Debug.Log("Not Found current scene index"); break;
        }
        userSceneDataTime = 0.0f;
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
                testData = new TestData(){
                    startDateTime = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss"),
                    deviceID = deviceID,
                    lang = userLang,
                    gyro = checkGyro,
                    deviceInfo = deviceDesc,
                    userEmail = userEmail, 
                    userZone = userZone,
                    snenasMotionData = new List<SnenaMotionData>(),
                    rightObjectsList = new List<TestObjects>(),
                    selectedObjectsList = new List<TestObjects>()
                    
                }; 
                curSnenaMotionData = new SnenaMotionData() { i = 0, act = new List<UserActivity>() };                        
            }else{Debug.Log("No data send to server");}
        }
    }

    // on finish a test
    private void testEnd() {
        hintText = null;
        isHintDisplay = false;
        if (testData.selectedObjectsList[testData.selectedObjectsList.Count - 1].time == 0) { 
            testData.selectedObjectsList[testData.selectedObjectsList.Count - 1].time = Mathf.RoundToInt(userSceneDataTime * 1000);
            testData.selectedObjectsList[testData.selectedObjectsList.Count - 1].rights = testsConfig[curTestIndex, 3];
            if (TimerCanvas){ Destroy(TimerCanvas); }        
            //Debug.Log("Ended Test="+ curTestIndex+" json=" + JsonUtility.ToJson(testData));
            NextScene(1);
        }
    }
}
