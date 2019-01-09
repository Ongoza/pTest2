using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Main : MonoBehaviour
{
    // global temp variables
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
    private GameObject rootMenu; // menu
    private GameObject rootCam; // camera
    private GameObject rootObj; // objects in tests
    private GameObject rootKeyoard; // keyboard

    // variables for cursor
    private float defaultTime = 1f; // time in sec focus on an obj for select
    private Material timedPointer;
    private string curfocusObj = "";
    private string curfocusType = "";
    private float workTime;
    private bool isTimer=false; // display select cursor    

    // variables for tests
    private int nHor = 10;   // Total number of objects for 6 = 16, for 10 =32 
    private float distanceMax = 6f; // min distance from camera to an object
    private float distanceMin = 16.1f; // max distance from camera to an object
    private int numerSeletedObjects = 5; // number of objects for selection
    private int initTestTime = 120; // time for test in sec
    // list of objects colors
    // {"gray","blue","green","red","yellow","purple","brown","black" }
    private readonly Color[] arrColor = new Color[8]{
        new Color(171f / 255f, 171f / 255f, 171f / 255f, 1f),
        new Color(0f, 0f, 128f / 255f, 1f),
        new Color(3f / 255f, 114f / 255f, 21f / 255f, 1f),
        new Color(246f / 255f, 6f / 255f, 22f / 255f, 1f),
        new Color(251f / 255f, 251f / 255f, 2f / 255f, 1f),
        new Color(139f / 255f, 0f, 139f / 255f, 1f),
        new Color(139f / 255f, 69f / 255f, 19f / 255f, 1f),
        new Color(0f, 0f, 0f, 1f)
    };

    private int selectedColorIndex = 2; // index of selected objects color
    private bool isTimerShow = false; // display timer during a test
    private float testTime;// test time left
    private Text timerText;
    GameObject TimerCanvas; // timer object
    private Material primitivesMaterial;    
    private int[] objCounter = new int[5] {0,0,0,0,0};    
    private int nVer;    
    private float aStartH;
    private float aStartV;
    private float aStartR;
    
    // Start is called before the first frame update
    void Start()
    {
        defaultFont = Font.CreateDynamicFontFromOSFont("Roboto", 1);
        camFade = GameObject.Find("camProtector");
        rootCam = GameObject.Find("rootCam");
        GameObject ObjEventSystem = GameObject.Find("GvrEventSystem");
        if (ObjEventSystem)
        { SceneEventSystem = ObjEventSystem.GetComponent<EventSystem>();
            if (SceneEventSystem) {
            //    SceneEventSystem.enabled = false;
            }
        } else { Debug.Log("Can not find Event System");}

        uisprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        //Sprite uisprite = (Sprite) Resources.Load("UI/Skin/UISprite");
        //Debug.Log(Data.getMessage("Intro"));
        primitivesMaterial = new Material(Shader.Find("Specular"));
        GameObject camTimedPointer = GameObject.Find("GvrReticlePointer");        
        timedPointer = camTimedPointer.GetComponent<Renderer>().material;
        nVer = nHor*2;
        aStartH = 2 * Mathf.PI / nHor;
        aStartV = 2 * Mathf.PI / nVer;
        aStartR = aStartH / 4f;
        //CreateObjsArray();
        
        string msg = Data.getMessage("Intro");
        //string msg = "This app allows to choose your future profession.\nPlease path the psychological test before.\n";
        //ShowMessage(msg,"test1");
        ShowKeyboard();
        //TextEmail.text = "Hello";
        //ShowTimer();
        // StartCoroutine(fadeScene(2f, true, new Color(0.2f, 0.2f, 0.2f, 1), "Main"));
        //StartCoroutine(RotateCamera(2f, 0.0001f, "End rotation"));
    }

    // create objects koordinates list around the camera in random locations
    void CreateObjsArray()
    {
        rootObj = new GameObject("rootObj");
        // create the list of a coordinate of objects        
        List<Vector3> locations = new List<Vector3>();
        bool pi_0 = true;
        bool pi_2 = true;
        for (int i = 0; i < nVer / 2; i++)
        {
            float evalution = i * aStartV * 2.0f;
            if (1.1f < evalution && 1.9f > evalution)
            { // on the top put only one object
                if (pi_0)
                {
                    pi_0 = false;
                    locations.Add(SphericalToCartesianPlusRandom(Mathf.PI / 2f, evalution));
                }
            }
            else if (4.0f < evalution && 5.2f > evalution)
            { // on the bottom put only one object
                if (pi_2)
                {
                    pi_2 = false;
                    locations.Add(SphericalToCartesianPlusRandom(Mathf.PI * 1.5f, evalution));
                }
            }
            else
            {
                for (int j = 0; j < nHor / 2; j++)
                {
                    var polar = j * aStartH;
                    locations.Add(SphericalToCartesianPlusRandom(polar, evalution));
                }
            }
        }
        //Debug.Log(locations.Count + " Array of objects coor =" + string.Join(",", locations));
        int max = locations.Count;
        //create objects for selection by a tester
        for (int i = 0; i < numerSeletedObjects; i++)
        {
            int newIndex = Random.Range(0, max);
            Vector3 loc = locations[newIndex];
            locations.RemoveAt(newIndex);
            max--;
            Color col = arrColor[selectedColorIndex];
            string objName = "Piramid_5_" + i;
            GameObject obj5 = CreatPyramid3();
            SetObjParams(obj5, objName, loc, col);
        }
        //create rnadom objects
        foreach (Vector3 loc in locations)
        {
            Color col = arrColor[Random.Range(0, 8)];
            int typeObj = Random.Range(0, 4);
            string[] types = new string[4] { "Cube", "Sphere", "Capsule", "Cylinder" };
            string objName = types[typeObj] +"_" + typeObj + "_" + objCounter[typeObj];
            objCounter[typeObj]++;
            switch (typeObj)
            {
                case 0:
                    GameObject obj1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    SetObjParams(obj1, objName, loc, col);
                    break;
                case 1:
                    GameObject obj2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    SetObjParams(obj2, objName, loc, col);
                    break;
                case 2:
                    GameObject obj3 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    SetObjParams(obj3, objName, loc, col);
                    break;
                case 3:
                    GameObject obj4 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    SetObjParams(obj4, objName, loc, col);
                    break;
                default:
                    print("Incorrect obj type.");
                    break;
            }
        }
        Debug.Log("Created objects"+string.Join(",",objCounter.ToString()));
    }

    void SetObjParams(GameObject obj, string name, Vector3 loc, Color col)
    {
        obj.name = name;
        obj.transform.SetParent(rootObj.transform);
        obj.transform.position = loc;
        Renderer rend = obj.GetComponent<Renderer>();
        rend.material = primitivesMaterial;
        rend.material.color = col;
    }
     
    Vector3 SphericalToCartesianPlusRandom(float polar, float elevation)
	{
        Vector3 outCart;
        float distanceR = Random.Range(distanceMin, distanceMax);
        float elevationR = Random.Range(elevation - aStartR, elevation + aStartR);
        float polarR = Random.Range(polar - aStartR, polar + aStartR);
        float a = distanceR * Mathf.Cos(elevation);
        outCart.x = a * Mathf.Cos(polarR);
        outCart.y = distanceR * Mathf.Sin(elevationR);
        outCart.z = a * Mathf.Sin(polarR);
	    return outCart;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimer)
        {            
            workTime -= Time.deltaTime;
            if (workTime <= 0)
            {
                //Debug.Log("isTimer");
                onClickTimed();
               // addOevrlayInfo("Click!");
            }
            else
            {
                float percent = (1f - workTime / defaultTime) * 360;
                timedPointer.SetFloat("_Angle", percent);
                //Debug.Log("onUpdate =" + percent);
            }
        }
        if (isTimerShow)
        {
            // show current time
            testTime -= Time.deltaTime;
            //Debug.Log("TestTime=" + testTime.ToString());
            timerText.text = Mathf.Floor(testTime).ToString();
            if (testTime<0) {
                isTimerShow = false;
                if (TimerCanvas) { 
                    Destroy(TimerCanvas);
                }
            }
        }
    }


    GameObject CreatPyramid3()
    {       
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

    void creatPyramid4()
    {
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

    private void ShowMessage(string msg, string action)
    {
        rootMenu = new GameObject("rootMenu");
        rootMenu.transform.position = new Vector3(0, 1, 12);        
        GameObject newCanvas = new GameObject("CnvsMainMenu");        
        Canvas c = newCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.WorldSpace;
        newCanvas.transform.SetParent(rootMenu.transform);
        newCanvas.AddComponent<CanvasScaler>();
        newCanvas.AddComponent<GvrPointerGraphicRaycaster>();
        RectTransform NewCanvasRect = newCanvas.GetComponent<RectTransform>();
        NewCanvasRect.sizeDelta = new Vector2(400, 100);
        NewCanvasRect.localScale = new Vector3(0.07f, 0.07f, 1f);
        NewCanvasRect.localPosition = new Vector3(0, 0, 0);
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image i = panel.AddComponent<Image>();
        i.color = new Vector4(1, 1, 1, 0.7f);
        i.sprite = uisprite;
        i.type = Image.Type.Sliced;
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(NewCanvasRect, true);
        panelTransform.localScale = new Vector3(0.2f, 0.2f, 1f);
        panelTransform.localPosition = new Vector3(0, 0, 0);
        panelTransform.sizeDelta = new Vector2(1200, 300);
        //panelTransform.rotation = Quaternion.AngleAxis(-180, Vector3.up);

        CreateText(panelTransform, 0, 16, 1200, 300, msg, 50, 0, TextAnchor.MiddleCenter);
        GameObject bt0 = new GameObject("btnTest");
        RectTransform br = bt0.AddComponent<RectTransform>();
        br.sizeDelta = new Vector2(300, 60);
        Image img = bt0.AddComponent<Image>();        
        img.sprite = uisprite;
        img.color = new Vector4(0.5f, 0.5f, 0.8f, 1);
        //img.material.color = new Vector4(1f, 1f, 1f, 0.7f);
        img.type = Image.Type.Sliced;
        Button bt = bt0.AddComponent<Button>();
        bt0.transform.SetParent(panelTransform, true);
        bt0.transform.localPosition = new Vector3(33, -80, 0);
        //bt0.transform.rotation = Quaternion.AngleAxis(-180, Vector3.up);
        bt0.transform.localScale = new Vector3(1, 1, 1);
        CreateText(bt.transform, 0, 0, 300, 50, "Start", 40, 1, TextAnchor.MiddleCenter);
        bt.onClick.AddListener(onClickTimed);
        EventTrigger be = bt0.AddComponent<EventTrigger>();
        EventTrigger.Entry entryEnterGaze = new EventTrigger.Entry();
        entryEnterGaze.eventID = EventTriggerType.PointerEnter;
        entryEnterGaze.callback.AddListener((eventData) => { onEnterTimed(action,""); });
        be.triggers.Add(entryEnterGaze);
        EventTrigger.Entry entryExitGaze = new EventTrigger.Entry();
        entryExitGaze.eventID = EventTriggerType.PointerExit;
        entryExitGaze.callback.AddListener((eventData) => { onExitTimed(); });
        be.triggers.Add(entryExitGaze);

       // showTutorialsMenu(rootMenu);
    }

    private GameObject CreateText(Transform parent, float x, float y, float w, float h, string message, int fontSize, int fontStyle, TextAnchor achor)
    {
        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(parent);
        RectTransform trans = textObject.AddComponent<RectTransform>();
        trans.sizeDelta = new Vector2(w, h);
        trans.anchoredPosition3D = new Vector3(0, 0, 0);
        trans.anchoredPosition = new Vector2(x, y);        
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

    public void onEnterTimed(string type, string name)
    {
        Debug.Log("onEnterTimed=" + name + "=" + type);
        isTimer = true;
        workTime = defaultTime;
        timedPointer.SetFloat("_Angle", 0);
        curfocusObj = name;
        curfocusType = type;
    }
    public void onExitTimed()
    {
        isTimer = false;
        curfocusObj = "";
        curfocusType = "";
        workTime = defaultTime;
        timedPointer.SetFloat("_Angle", 360);
    }

    private void onClickTimed()
    {
        clickSelectEvent();
    }

    private void clickSelectEvent()
    {
        isTimer = false;
        //addOevrlayInfo("clickSelectEvent" + name);
        Debug.Log("clickSelectEvent=" + curfocusType + "="+ curfocusObj);
        switch (curfocusType)
        {
            case "test":
                Color color = new Color(0.2f, 0.2f, 0.2f, 0f);
                //StartCoroutine(fadeScene(0.5f, false, color, "colorTest"));
                break;
            case "btnModel":
                Debug.Log("clickSelectEvent 2 =" + curfocusType + "=");
                Destroy(rootMenu);
                //showTutorialContent(name);
                break;
            case "btnToMain":
                Debug.Log("clickSelectEvent 3 =" + curfocusType + "=");                                
                //Destroy(rootMenu);
                //ShowMainMenu();
                break;
            case "Part_1":
                //globalData.setCurVideoName(0);
                //tryStartVideo();
                break;
            case  "Keyboard":
                //Debug.Log("clickSelectEvent 3 =" + curfocusType + "="+ curfocusObj);
                if (curfocusObj.Contains("DEL")){
                    string str = TextEmail.text;
                    if (str.Length > 0){TextEmail.text = str.Substring(0, str.Length - 1);}
                } else{ TextEmail.text += curfocusObj;}
                break;
            case "Enter":
                Debug.Log("clickSelectEvent 4 =" + curfocusType + "=" + curfocusObj);
                userData["Enail"] = TextEmail.text;
                Destroy(rootKeyoard);
                break;
            default:
                Debug.Log("clickSelectEvent not found action for " + name);
                break;
        }
        onExitTimed();
    }

    private void ShowTimer()
    {
        TimerCanvas = new GameObject("Timer");
        Canvas c = TimerCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.WorldSpace;
        TimerCanvas.AddComponent<CanvasScaler>();
        RectTransform NewCanvasRect = TimerCanvas.GetComponent<RectTransform>();
        //NewCanvasRect.Rotate(120,40,0);
        //NewCanvasRect.localRotation = Quaternion.Euler(0, 180, 0);
        TimerCanvas.transform.SetParent(Camera.main.transform, true);
        NewCanvasRect.localPosition = new Vector3(0, 5f, 10);
        NewCanvasRect.sizeDelta = new Vector2(200, 20);
        NewCanvasRect.localScale = new Vector3(0.1f, 0.1f, 1f);
        GameObject panel = new GameObject("TimerPanel");
        panel.AddComponent<CanvasRenderer>();
        Image i = panel.AddComponent<Image>();
        i.sprite = uisprite;
        i.type = Image.Type.Sliced;
        i.color = new Vector4(1f, 1f, 1f, 1f);
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(NewCanvasRect, true);
        panelTransform.localScale = new Vector3(0.3f, 0.3f, 1f);
        panelTransform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        panelTransform.localPosition = new Vector3(0, 0, 0);
        panelTransform.sizeDelta = new Vector2(30, 20);
        testTime = initTestTime;        
        GameObject txtObj = CreateText(panelTransform, 0, 0, 30, 20, testTime.ToString(), 12, 0, TextAnchor.MiddleCenter);
        txtObj.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        timerText = txtObj.GetComponent<Text>();
        //overlayText.color = new Color(0, 0.8f, 0);
        isTimerShow = true;
    }
    IEnumerator fadeScene(float duration, bool startNewScene, Color color, string sceneName)
    {
        if (camFade) { 
            //GameObject camFade = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            camFade.GetComponent<Renderer>().enabled = true;
            //Debug.Log("Start fade scene " + color);
            float startTransparent = 0f;
            float endTransparent = 1f;
            float smoothness = 0.05f;
            float progress = 0;
            float increment = smoothness / duration; //The amount of change to apply.
            if (startNewScene == true)
            {
                startTransparent = 1f;
                endTransparent = 0f;
            }
            Color colorStart = new Color(color.r, color.g, color.b, startTransparent);
            camFade.GetComponent<Renderer>().materials[0].color = colorStart;
            Color colorEnd = new Color(colorStart.r, colorStart.g, colorStart.b, endTransparent);
            while (progress < 1)
            {
                progress += increment;
                //Debug.Log(progress);
                camFade.GetComponent<Renderer>().materials[0].color = Color.Lerp(colorStart, colorEnd, progress);
                yield return new WaitForSeconds(smoothness);
            }
            yield return null;
            if (startNewScene == true)
            {
                Debug.Log("Start scene " + sceneName+" tr="+ startNewScene);
                camFade.GetComponent<Renderer>().enabled = false;
                if (SceneEventSystem) { SceneEventSystem.enabled = true; }
            }
            else
            {
                Debug.Log("Start scene " + sceneName + " tr=" + startNewScene);
                
                //UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            }
        }
        else
        {
            Debug.Log("Can not find camFade object");
        }
    }

    IEnumerator RotateCamera(float duration, float speedUp, string newAction)
    {
        Transform camTransform = rootCam.transform;            
        float progress = 300;
        float smooth = 0.0001f; 
        if (speedUp < 0) { smooth = 0.05f; }
        float angleDelta = 1f;
        float newAngle = 0f;
        while (progress > 0 && smooth >0)
            {
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
    
    void StartAction(float speedUp)
    {
        StartCoroutine(RotateCamera(2f, -speedUp, "End rotation"));
    }

    void ShowKeyboard()
    {
        int len = 12;
        int width = 60;
        int startPosX = -342;
        int startPosY = 125;
        int i = 0;
        int j = 0;               
        rootKeyoard = new GameObject("rootKeyoard");
        Canvas c = rootKeyoard.AddComponent<Canvas>();
        c.renderMode = RenderMode.WorldSpace;
        rootKeyoard.transform.SetParent(rootKeyoard.transform);
        rootKeyoard.AddComponent<CanvasScaler>();
        rootKeyoard.AddComponent<GvrPointerGraphicRaycaster>();
        RectTransform NewCanvasRect = rootKeyoard.GetComponent<RectTransform>();
        NewCanvasRect.sizeDelta = new Vector2(770, 350);
        NewCanvasRect.localScale = new Vector3(0.07f, 0.07f, 1f);
        NewCanvasRect.localPosition = new Vector3(0, 0, 0);
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image img = panel.AddComponent<Image>();
        img.color = new Vector4(1, 1, 1, 0.7f);
        img.sprite = uisprite;
        img.type = Image.Type.Sliced;
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(NewCanvasRect, true);
        panelTransform.localScale = new Vector3(0.2f, 0.2f, 1f);
        panelTransform.localPosition = new Vector3(0, 0, 0);
        panelTransform.sizeDelta = new Vector2(770, 330);

        Dictionary<string, string> keyDictionary = Data.getKeys();
        foreach (KeyValuePair<string, string> item in keyDictionary)
        {
            float delta = 0;
            if (item.Key.Length > 1) { width = 60 * 2; i += 1; delta = 30; }
            if (i >= len) {j++; i = 0;}
            //CreateText(panelTransform, i*60, j*60, 1200, 300, item.Value, 50, 0, TextAnchor.MiddleCenter);
            GameObject bt0 = new GameObject(item.Key);
            RectTransform br = bt0.AddComponent<RectTransform>();
            br.sizeDelta = new Vector2(width, 60);
            Image imgKey = bt0.AddComponent<Image>();
            imgKey.sprite = uisprite;
            imgKey.color = new Vector4(0.5f, 0.5f, 0.8f, 1);
            //img.material.color = new Vector4(1f, 1f, 1f, 0.7f);
            imgKey.type = Image.Type.Sliced;
            Button bt = bt0.AddComponent<Button>();
            bt0.transform.SetParent(panelTransform, true);
            bt0.transform.localPosition = new Vector3(startPosX+i * (60+2)-delta, startPosY - j * (60+2), 0);
            //bt0.transform.rotation = Quaternion.AngleAxis(-180, Vector3.up);
            bt0.transform.localScale = new Vector3(1, 1, 1);
            CreateText(bt.transform, 0, 0, width, 60, item.Value, 40, 1, TextAnchor.MiddleCenter);
            bt.onClick.AddListener(onClickTimed);
            EventTrigger be = bt0.AddComponent<EventTrigger>();
            EventTrigger.Entry entryEnterGaze = new EventTrigger.Entry();
            entryEnterGaze.eventID = EventTriggerType.PointerEnter;
            entryEnterGaze.callback.AddListener((eventData) => { onEnterTimed("Keyboard",item.Key); });
            be.triggers.Add(entryEnterGaze);
            EventTrigger.Entry entryExitGaze = new EventTrigger.Entry();
            entryExitGaze.eventID = EventTriggerType.PointerExit;
            entryExitGaze.callback.AddListener((eventData) => { onExitTimed(); });
            be.triggers.Add(entryExitGaze);
           // Debug.Log(j + "-" + i + " " + item.Key + "" + item.Value);
            i++;
            //if (item.Key.Length > 1) {i += 1; }
        }
        GameObject bt0k = new GameObject("Enter");
        RectTransform brOk = bt0k.AddComponent<RectTransform>();
        brOk.sizeDelta = new Vector2(150, 60);
        Image imgKeyOk = bt0k.AddComponent<Image>();
        imgKeyOk.sprite = uisprite;
        imgKeyOk.color = new Vector4(0.5f, 0.5f, 0.8f, 1);
        //img.material.color = new Vector4(1f, 1f, 1f, 0.7f);
        imgKeyOk.type = Image.Type.Sliced;
        Button btOk_bt = bt0k.AddComponent<Button>();
        bt0k.transform.SetParent(panelTransform, true);
        bt0k.transform.localPosition = new Vector3(0, -213, 0);
        //bt0.transform.rotation = Quaternion.AngleAxis(-180, Vector3.up);
        bt0k.transform.localScale = new Vector3(1, 1, 1);
        CreateText(btOk_bt.transform, 0, 0, width, 60, "Save", 40, 1, TextAnchor.MiddleCenter);
        btOk_bt.onClick.AddListener(onClickTimed);
        EventTrigger beOk = bt0k.AddComponent<EventTrigger>();
        EventTrigger.Entry entryEnterGazeOk = new EventTrigger.Entry();
        entryEnterGazeOk.eventID = EventTriggerType.PointerEnter;
        entryEnterGazeOk.callback.AddListener((eventData) => { onEnterTimed("Enter", ""); });
        beOk.triggers.Add(entryEnterGazeOk);
        EventTrigger.Entry entryExitGazeOk = new EventTrigger.Entry();
        entryExitGazeOk.eventID = EventTriggerType.PointerExit;
        entryExitGazeOk.callback.AddListener((eventData) => { onExitTimed(); });
        beOk.triggers.Add(entryExitGazeOk);
        rootKeyoard.transform.position = new Vector3(0, -2.3f, 12);
        rootKeyoard.transform.Rotate(Vector3.left, -30);

        GameObject InputMsg = new GameObject("InputMsg");
        InputMsg.transform.SetParent(NewCanvasRect);
        RectTransform brInputMsg = InputMsg.AddComponent<RectTransform>();
        brInputMsg.localScale = new Vector3(1.2f, 0.2f, 1f);
        brInputMsg.sizeDelta = new Vector2(270, 40);
        //brInputMsg.Rotate(Vector3.left, -150);
        brInputMsg.localPosition = new Vector3(0, 135, 4);
        Image imgInputMsg = InputMsg.AddComponent<Image>();
        imgInputMsg.sprite = uisprite;
        imgInputMsg.color = new Vector4(0.8f, 0.8f, 0.8f, 1);
        imgInputMsg.type = Image.Type.Sliced;
        CreateText(brInputMsg, 0, 1f, 270, 40, Data.getMessage("Email"), 12, 1, TextAnchor.MiddleCenter);

        GameObject Input = new GameObject("Input");
        Input.transform.SetParent(NewCanvasRect);
        RectTransform brInput = Input.AddComponent<RectTransform>();
        brInput.localScale = new Vector3(0.5f, 0.5f, 1f);
        brInput.sizeDelta = new Vector2(450, 6);
        brInput.Rotate(Vector3.left,-150);
        brInput.localPosition = new Vector3(0, 94, 6);                
        Image imgInput = Input.AddComponent<Image>();
        imgInput.sprite = uisprite;
        imgInput.color = new Vector4(0.9f, 0.9f, 0.9f, 1);
        imgInput.type = Image.Type.Sliced;
        GameObject InputText = CreateText(brInput, 0, -0.5f, 350, 50, "", 12, 1, TextAnchor.MiddleCenter);
        TextEmail = InputText.GetComponent<Text>();
        InputText.transform.localScale = new Vector3(1.2f,0.6f,1);
    }
}
