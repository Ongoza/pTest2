using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{
    // global temp variables
    private int curScene = 3; // a current scene index
    private Sprite uisprite; // default img for text background
    private GameObject camFade; // camera Fade object
    private Font defaultFont; // defule text font
    private Text TextEmail; // user email
    EventSystem SceneEventSystem; // turn on/off eventSystem   
    // user data
    private Dictionary<string, string> userData = new Dictionary<string, string>()
    {
        { "Email","" },
        { "Result1","" },
        {"Result2","" }
    };
    // objects roots for group manipulations
    private GameObject rootObj; // root of objects 

    // variables for cursor
    private float defaultTime = 1f; // time in sec focus on an obj for select
    private Material timedPointer;
    private string curfocusObj = "";
    private string curfocusType = "";
    private float workTime;
    private bool isTimer=false; // display select cursor    

    // variables for tests
    //{ "Cube", "Sphere", "Capsule", "Cylinder","Pyramid" };
    // {"gray","blue","green","red","yellow","purple","brown","black" }
    private int[,] testsConfig = new int[2,5] { // testConfig[x,0] - (0-4) type index of object for search in the first test
        { 4, 4, 5, 0, 0 }, // testConfig[x,1] - (0-7) index of selected objects color
        { 4, 4, 5, 0, 0 }}; // testConfig[x,2] total number of objects for selection in tests
                         // testConfig[x,3] total number of right founeded objects in a selection in test
                         // testConfig[x,4] total number of all founeded objects in a selection test
    private int curTestIndex = 0; //a current test number

    private int nHor = 10;   // Total number of objects for 6 = 16, for 10 =32 
    private float distanceMax = 6f; // min distance from camera to an object
    private float distanceMin = 16.1f; // max distance from camera to an objec    
    private int initTestTime = 60; // time for test in sec
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
    private bool isTimerShow = false; // display timer during a test
    private float testTime;// test time left
    private Text hintText;
    GameObject TimerCanvas; // timer object
    private Material primitivesMaterial;    
    private int[] objCounter = new int[5] {0,0,0,0,0};    
    private int nVer;
    private float aStartH;
    private float aStartV;
    private float aStartR;
    
    // Start is called before the first frame update
    void Start(){
        defaultFont = Font.CreateDynamicFontFromOSFont("Roboto", 1);
        camFade = GameObject.Find("camProtector");        
        GameObject ObjEventSystem = GameObject.Find("GvrEventSystem");
        if (ObjEventSystem){
            SceneEventSystem = ObjEventSystem.GetComponent<EventSystem>();
            if (SceneEventSystem) {
                SceneEventSystem.enabled = false;
            }
        } else { Debug.Log("Can not find Event System");}
        Camera.main.GetComponent<GvrPointerPhysicsRaycaster>().enabled = false;

        uisprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        //Sprite uisprite = (Sprite) Resources.Load("UI/Skin/UISprite");
        primitivesMaterial = new Material(Shader.Find("Specular"));
        GameObject camTimedPointer = GameObject.Find("GvrReticlePointer");        
        timedPointer = camTimedPointer.GetComponent<Renderer>().material;
        nVer = nHor*2;
        aStartH = 2 * Mathf.PI / nHor;
        aStartV = 2 * Mathf.PI / nVer;
        aStartR = aStartH / 4f;
        // StartCoroutine(fadeScene(2f, true, new Color(0.2f, 0.2f, 0.2f, 1), "Main"));
        //StartCoroutine(RotateCamera(2f, 0.0001f, "End rotation"));
        NextScene(0);
    }

    // create objects koordinates list around the camera in random locations
    void CreateObjsArray(bool isDisplayTimer){
        rootObj = new GameObject("rootObj");
        testsConfig[curTestIndex,3] = 0;
        testsConfig[curTestIndex, 4] = 0;
        // create the list of a coordinate of objects        
        List<Vector3> locations = new List<Vector3>();
        bool pi_0 = true;
        bool pi_2 = true;
        for (int i = 0; i < nVer / 2; i++){
            float evalution = i * aStartV * 2.0f;
            if (1.1f < evalution && 1.9f > evalution){ // on the top put only one object
                if (pi_0){
                    pi_0 = false;
                    locations.Add(SphericalToCartesianPlusRandom(Mathf.PI / 2f, evalution));
                }
            }else if (4.0f < evalution && 5.2f > evalution){ // on the bottom put only one object
                if (pi_2){
                    pi_2 = false;
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
        //Debug.Log(locations.Count + " Array of objects coor =" + string.Join(",", locations));
        int max = locations.Count;
        //create objects for selection by a tested person
        List<int> objectTypes = new List<int>() { 0, 1, 2, 3, 4 };        
        for (int j = 0; j < testsConfig[curTestIndex, 2]; j++){
            int newIndex = Random.Range(0, max);            
            SetUpObj(testsConfig[curTestIndex, 0], "rightObj_" + j, locations[newIndex], arrColor[testsConfig[curTestIndex, 1]]);
            locations.RemoveAt(newIndex);
            max--;
        }        
        objectTypes.Remove(testsConfig[curTestIndex,0]);
        //Debug.Log(locations.Count + " Array of objects coor =" + string.Join(",", objectTypes));
        //create rnadom objects 
        foreach (Vector3 loc in locations){
            int colIndex = Random.Range(0, 8);
            Color col = arrColor[colIndex];
            int typeObj = objectTypes[Random.Range(0, 4)];            
            string objName = typeObj +"_" + colIndex + "_" + objCounter[typeObj];
            objCounter[typeObj]++;
            SetUpObj(typeObj, objName, loc, col);
        }
        //Debug.Log("Created objects"+string.Join(",",objCounter.ToString()));        
        GameObject canExit = CreateCanwas("rootMenu", btExitLoc, new Vector2(100, 50));
        canExit.transform.SetParent(rootObj.transform);
        CreateButton(canExit.transform, "btExit", "Exit", "Next", "", new Vector3(0, 0, 0), new Vector2(100, 50));
        canExit.transform.Rotate(Vector3.left, -60);
        Camera.main.GetComponent<GvrPointerPhysicsRaycaster>().enabled = true;
        // Create hint for tested person
        TimerCanvas = new GameObject("Timer");
        Canvas c = TimerCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.WorldSpace;
        TimerCanvas.AddComponent<CanvasScaler>();
        RectTransform NewCanvasRect = TimerCanvas.GetComponent<RectTransform>();
        TimerCanvas.transform.SetParent(Camera.main.transform, true);
        NewCanvasRect.localPosition = new Vector3(0, 5f, 10);
        NewCanvasRect.sizeDelta = new Vector2(300, 40);
        NewCanvasRect.localScale = new Vector3(0.1f, 0.1f, 1f);
        GameObject panel = new GameObject("HintPanel");
        panel.AddComponent<CanvasRenderer>();
        Image img = panel.AddComponent<Image>();
        img.sprite = uisprite;
        img.type = Image.Type.Sliced;
        img.color = new Vector4(1f, 1f, 1f, 1f);
        string msg = string.Format(Data.getMessage("Test_hint"), Data.getMessage("color_" + testsConfig[curTestIndex, 1]), Data.getMessage("obj_" + testsConfig[curTestIndex, 0]), 0, testsConfig[curTestIndex, 2]);
        int height = 20;
        if (isDisplayTimer){
            msg += string.Format(Data.getMessage("Test_timer"), initTestTime);
            height = 40;
            testTime = initTestTime;
        }
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(NewCanvasRect, true);
        panelTransform.localScale = new Vector3(0.3f, 0.3f, 1f);
        panelTransform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        panelTransform.localPosition = new Vector3(0, 0, 0);
        panelTransform.sizeDelta = new Vector2(300, height);
        //Debug.Log(msg);
        GameObject txtObj = CreateText(panelTransform, new Vector2(0, 0), new Vector2(300, height), msg, 12, 0, TextAnchor.MiddleCenter);
        txtObj.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        hintText = txtObj.GetComponent<Text>();
        if (isDisplayTimer) {isTimerShow = true;}
    }

    void SetUpObjParams(GameObject obj, string name, Vector3 loc, Color col){
        if (obj) { 
            obj.name = name;
            obj.transform.SetParent(rootObj.transform);
            obj.transform.position = loc;
            Renderer rend = obj.GetComponent<Renderer>();
            rend.material = primitivesMaterial;
            rend.material.color = col;
            obj.AddComponent<SphereCollider>();
            EventTrigger be = obj.AddComponent<EventTrigger>();
            EventTrigger.Entry entryEnterGaze = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            entryEnterGaze.callback.AddListener((eventData) => { OnEnterTimed("Test", name); });
            be.triggers.Add(entryEnterGaze);
            EventTrigger.Entry entryExitGaze = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            entryExitGaze.callback.AddListener((eventData) => { OnExitTimed(); });
            be.triggers.Add(entryExitGaze);
        }
    }

    void SetUpObj(int typeObj, string name, Vector3 loc, Color col){
        //Debug.Log("typeObj " + typeObj);
        switch (typeObj){
            case 0: SetUpObjParams(GameObject.CreatePrimitive(PrimitiveType.Cube),name,loc,col); break;
            case 1: SetUpObjParams(GameObject.CreatePrimitive(PrimitiveType.Sphere), name, loc, col); break;
            case 2: SetUpObjParams(GameObject.CreatePrimitive(PrimitiveType.Capsule), name, loc, col); break;
            case 3: SetUpObjParams(GameObject.CreatePrimitive(PrimitiveType.Cylinder), name, loc, col); break;
            case 4: SetUpObjParams(CreatPyramid3(), name, loc, col); break;
            default: print("Error!!! Incorrect obj type."); break;
        }
    }

    Vector3 SphericalToCartesianPlusRandom(float polar, float evalution){
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

    private void ShowMessage(string msg, string action, string actionLabel, Vector2 size, UnityEngine.TextAnchor anchor, Vector2 startLoc){
        rootObj = CreateCanwas("rootMenu", new Vector3(0, 1, 12), size);
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
        CreateButton(panelTransform, "Button", actionLabel, action,"",new Vector3(0, 60-size.y/2, 0), new Vector2(300,50));
       // showTutorialsMenu(rootMenu);
    }

    private GameObject CreateButton(Transform parent, string name, string label, string action1, string action2, Vector3 loc, Vector2 size){
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
        entryEnterGaze.callback.AddListener((eventData) => { OnEnterTimed(action1, action2); });
        be.triggers.Add(entryEnterGaze);
        EventTrigger.Entry entryExitGaze = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExitGaze.callback.AddListener((eventData) => { OnExitTimed(); });
        be.triggers.Add(entryExitGaze);
        return bt0;
    }

    private GameObject CreateText(Transform parent, Vector2 loc, Vector2 size, string message, int fontSize, int fontStyle, TextAnchor achor){
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
        if (isTimer){ // display selecting pointer
            workTime -= Time.deltaTime;
            if (workTime <= 0) { OnClickTimed();
            }else{
                if (timedPointer){timedPointer.SetFloat("_Angle", (1f - workTime / defaultTime) * 360);}
            }
        }
        if (isTimerShow){// show current time timer
            testTime -= Time.deltaTime;
            if (hintText) {
                hintText.text = string.Format(Data.getMessage("Test_hint"), Data.getMessage("color_" + testsConfig[curTestIndex, 1]), Data.getMessage("obj_" + testsConfig[curTestIndex, 0]), testsConfig[curTestIndex, 4], testsConfig[curTestIndex, 2])
                    + string.Format(Data.getMessage("Test_timer"), Mathf.Floor(testTime).ToString());
                if (testTime < 0) {
                    isTimerShow = false;
                    if (TimerCanvas){
                        Destroy(TimerCanvas);
                        NextScene(1);
                    }
                }
            }
        }
    }

    private void OnEnterTimed(string type, string name){
        Debug.Log("onEnterTimed=" + name + "=" + type);
        isTimer = true;
        workTime = defaultTime;
        if (timedPointer) { timedPointer.SetFloat("_Angle", 0); }
        curfocusObj = name;
        curfocusType = type;
    }

    private void OnExitTimed(){
        isTimer = false;
        curfocusObj = "";
        curfocusType = "";
        workTime = defaultTime;
        if (timedPointer) { timedPointer.SetFloat("_Angle", 360); }
    }

    private void OnClickTimed(){ ClickSelectEvent();}

    private void ClickSelectEvent(){
        isTimer = false;
        //addOevrlayInfo("clickSelectEvent" + name);
        Debug.Log("clickSelectEvent=" + curfocusType + "="+ curfocusObj);
        switch (curfocusType){
            case "Next":  NextScene(1);  break;
            case "Test":
                Debug.Log("Test event "+ curfocusObj);
                GameObject gm = GameObject.Find(curfocusObj);
                if (gm) { Destroy(gm); } else { Debug.Log("Error! Can not find object for destroy " + curfocusObj); }
                testsConfig[curTestIndex, 4]++;
                if (curfocusObj.Contains("rightObj_")){ testsConfig[curTestIndex, 3]++;}
                if (testsConfig[curTestIndex, 4] >= testsConfig[curTestIndex, 2]) {
                    isTimerShow = false;
                    if (TimerCanvas) { Destroy(TimerCanvas);}
                    NextScene(1);
                } else { 
                    string msg = string.Format(Data.getMessage("Test_hint"), Data.getMessage("color_" + testsConfig[curTestIndex, 1]), Data.getMessage("obj_" + testsConfig[curTestIndex, 0]), testsConfig[curTestIndex, 4], testsConfig[curTestIndex, 2]);
                    if (isTimerShow){ msg += string.Format(Data.getMessage("Test_timer"), Mathf.Floor(testTime).ToString());}
                    if (hintText) { hintText.text = msg; }
                }
                break;
            case  "Keyboard":
                //Debug.Log("clickSelectEvent 3 =" + curfocusType + "="+ curfocusObj);
                if (curfocusObj.Contains("DEL")){
                    string str = TextEmail.text;
                    if (str.Length > 0) { TextEmail.text = str.Substring(0, str.Length - 1); }
                } else { if (TextEmail) { TextEmail.text += curfocusObj; } }
                break;
            case "Enter":
                Debug.Log("clickSelectEvent 4 =" + curfocusType + "=" + curfocusObj);
                userData["Enail"] = TextEmail.text;                
                NextScene(1);
                break;
            case "Exit": curScene = 0; NextScene(0); break;
            default: Debug.Log("clickSelectEvent not found action for " + name); break;
        }
        OnExitTimed();
    }

    IEnumerator FadeScene(float duration, bool startNewScene, Color color, string sceneName){
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
            Debug.Log(progress + " " + smooth);            
            transform.rotation = Quaternion.Slerp(transform.rotation, target, smooth);
            yield return null; 
        }        
        Debug.Log("Start new action " + newAction);
       // StartAction(speedUp);
    }
    
    void StartAction(float speedUp){ StartCoroutine(RotateCamera(2f, -speedUp, "End rotation"));}

    void ShowKeyboard() { // show keyboard input
        int len = 12; int width = 60; int startPosX = -342;  int startPosY = 125; int i = 0; int j = 0;
        rootObj = CreateCanwas("rootKeyoard", new Vector3(0, -3, 12), new Vector2(770, 350));
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
        CreateButton(rootObj.transform, "Enter", "Start", "Enter","", new Vector3(0, -230,0), new Vector2(150, 60));      
        GameObject InputMsg = new GameObject("InputMsg");
        InputMsg.transform.SetParent(rootObj.transform);
        RectTransform brInputMsg = InputMsg.AddComponent<RectTransform>();
        brInputMsg.localScale = new Vector3(1f, 1f, 1f);
        brInputMsg.sizeDelta = new Vector2(800, 140);
        brInputMsg.localPosition = new Vector3(0, 330, 0);
        Image imgInputMsg = InputMsg.AddComponent<Image>();
        imgInputMsg.sprite = uisprite;
        imgInputMsg.color = new Vector4(0.8f, 0.8f, 0.8f, 1);
        imgInputMsg.type = Image.Type.Sliced;
        CreateText(brInputMsg, new Vector2(0, 1f), new Vector2(800, 130), Data.getMessage("Email"), 40, 1, TextAnchor.MiddleCenter);   
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

    GameObject CreateCanwas(string name, Vector3 loc, Vector2 size){
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

    void NextScene(int delta){ // switch scenes   
        curScene += delta;
        SceneEventSystem.enabled = false;
        Camera.main.GetComponent<GvrPointerPhysicsRaycaster>().enabled = false;
        if (rootObj) { Destroy(rootObj);}
        if (TimerCanvas) { isTimerShow = false; Destroy(TimerCanvas);}
        switch (curScene){
            case 0: ShowMessage(Data.getMessage("Intro"), "Next","Start", new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 0));
                break;
            case 1: ShowKeyboard(); break;
            case 2: string msg1 = string.Format(Data.getMessage("Test1"), Data.getMessage("color_"+ testsConfig[curTestIndex, 0]), Data.getMessage("obj_"+ testsConfig[curTestIndex, 0]));
                Debug.Log("Test1=" + msg1);
                ShowMessage(msg1, "Next", "Start", new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 0));
                break;
            case 3:  curTestIndex = 0;  CreateObjsArray(false); break;
            case 4:  string msg2 = string.Format(Data.getMessage("Test2"), Data.getMessage("color_" + testsConfig[curTestIndex, 1]), Data.getMessage("obj_" + testsConfig[curTestIndex, 0]), initTestTime);
                ShowMessage(msg2, "Next", "Start", new Vector2(1200, 400), TextAnchor.MiddleCenter, new Vector2(0, 0));
                break;
            case 5:  curTestIndex = 1;  CreateObjsArray(true); break;
            case 6:  string msg3 = string.Format(Data.getMessage("Result"), testsConfig[0, 3], testsConfig[0, 2], testsConfig[1, 3], testsConfig[1, 2], Mathf.Floor(initTestTime - testTime).ToString());
                ShowMessage(msg3, "Exit", "Repeat", new Vector2(1400, 400), TextAnchor.MiddleLeft, new Vector2(25,50));
                break;
            default: Debug.Log("Not Found current scene index"); break;
        }
        SceneEventSystem.enabled = true;
    }
}
