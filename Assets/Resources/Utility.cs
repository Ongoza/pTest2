using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Utility {
    private Font defaultFont; // defule text font
    private bool isDebug; // debug enable
    //private GameObject rootObj; // root of objects
    public Sprite uisprite; // default img for text background
    private Text TextDebug; // Debug object
    private Main main;
    private Material matFont;
    //private float stepAnimation; // animation step
    
    // constractor
    public Utility(Main mainScript, bool isVRLog) {
        main = mainScript;
        isDebug = isVRLog;
        GameObject goDebug = GameObject.Find("txtDebug");
        if (isDebug){
            if (goDebug) {
                TextDebug = goDebug.GetComponent<Text>();
                TextDebug.enabled = true;
            }
            else { Debug.Log("Can't find a debug obj");}
        } else{ Debug.Log("VR Debug off");}
        //Debug.Log("VR Debug is ="+ isDebug+" ="+ isVRLog);
        logDebug(" Ver "+Data.getVersion());
        try { 
            defaultFont = Font.CreateDynamicFontFromOSFont("Roboto", 1);       
            //logDebug("Utility init 2");
        }catch (System.Exception e){Debug.Log("Can not load font");}
        Texture2D tex = new Texture2D(128, 128);
        // uisprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        try{ uisprite = Resources.Load<Sprite>("button");
        }catch (System.Exception e){Debug.Log("Can not load sprite for button");}
        //logDebug("Utility init 3");
        try{ matFont = Resources.Load<Material>("matOver");
        }catch (System.Exception e){Debug.Log("Can not load mterial for font");}
        logDebug("ok");

        //Debug.Log(matFont);

    }

    // message dialog
    public GameObject ShowMessage(string msg, string action, string actionLabel, Vector2 size, TextAnchor anchor, Vector2 startLoc){
        //logDebug("ShowMessage");
        GameObject rootObj = new GameObject("rootObj");
        rootObj.transform.position = new Vector3(0,main.baseLoc,12);
        GameObject rootCanvas = CreateCanvas("rootMenu", new Vector3(0, 0, 0), size);
        rootCanvas.transform.SetParent(rootObj.transform,false);
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image i = panel.AddComponent<Image>();
        i.color = new Vector4(1, 1, 1, 0.9f);
        i.sprite = uisprite;
        i.type = Image.Type.Sliced;
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(rootCanvas.transform, true);
        panelTransform.localScale = new Vector3(1f, 1f, 1f);
        panelTransform.localPosition = new Vector3(0, 40, 0);
        panelTransform.sizeDelta = size;
        //panelTransform.rotation = Quaternion.AngleAxis(-180, Vector3.up);        
        CreateText(panelTransform, startLoc, new Vector2(size.x - 150, size.y), msg, 50, 0, anchor);
        if (!action.Equals("")) { 
            CreateButton(panelTransform, "Button", actionLabel, action, "0_10_10", new Vector3(0, -40 - size.y / 2, 0), new Vector2(300, 60));
        }
        // showTutorialsMenu(rootMenu);
        return rootObj;
    }

    // canavas for text messages
    public GameObject CreateCanvas(string name, Vector3 loc, Vector2 size){
        //logDebug("CreateCanvas");
        GameObject objCanvas = new GameObject(name);
        Canvas c = objCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.WorldSpace;
        objCanvas.AddComponent<CanvasScaler>();
        RectTransform NewCanvasRect = objCanvas.GetComponent<RectTransform>();
        GvrPointerGraphicRaycaster action = objCanvas.AddComponent<GvrPointerGraphicRaycaster>();
        //action.enabled = false;
        NewCanvasRect.localPosition = loc;
        NewCanvasRect.sizeDelta = size;
        NewCanvasRect.localScale = new Vector3(0.014f, 0.014f, 1f);
        return objCanvas;
    }

    public void logDebug(string msg){
        if (isDebug){ if (TextDebug){ TextDebug.text += msg + "=>"; Debug.Log(msg); }
        }else{Debug.Log(msg);}
    }

    // text object
    public GameObject CreateText(Transform parent, Vector2 loc, Vector2 size, string message, int fontSize, int fontStyle, TextAnchor achor){
        //logDebug("CreateText");
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
        text.material = matFont;
        text.fontSize = fontSize;
        if (fontStyle == 1) { text.fontStyle = FontStyle.Bold; }
        text.font = defaultFont;
        text.alignment = achor;
        //text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.color = new Color(0, 0, 0);
        return textObject;
    }

    // button object
    public GameObject CreateButton(Transform parent, string name, string label, string action1, string action2, Vector3 loc, Vector2 size){
        //logDebug("CreateButton 1");
        GameObject bt0 = new GameObject(name);
        RectTransform br = bt0.AddComponent<RectTransform>();
        br.sizeDelta = size;
        Image img = bt0.AddComponent<Image>();
        img.sprite = uisprite;
        img.color = new Vector4(0.76f, 0.76f, 0.76f, 1);
        //img.material.color = new Vector4(1f, 1f, 1f, 0.7f);
        img.type = Image.Type.Sliced;
        Button bt = bt0.AddComponent<Button>();
        bt0.transform.SetParent(parent, true);
        bt0.transform.localPosition = loc;
        //bt0.transform.rotation = Quaternion.AngleAxis(-180, Vector3.up);
        bt0.transform.localScale = new Vector3(1, 1, 1);
        CreateText(bt.transform, new Vector2(0, 0), size, label, 40, 1, TextAnchor.MiddleCenter);
        bt.onClick.AddListener(main.OnClickTimed);
        EventTrigger be = bt0.AddComponent<EventTrigger>();
        EventTrigger.Entry entryEnterGaze = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnterGaze.callback.AddListener((eventData) => { main.OnEnterTimed(action1, action2, true); });
        be.triggers.Add(entryEnterGaze);
        EventTrigger.Entry entryExitGaze = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExitGaze.callback.AddListener((eventData) => { main.OnExitTimed(); });
        be.triggers.Add(entryExitGaze);
        //logDebug("CreateButton end");
        return bt0;
    }

    //  a text input dialog
    public GameObject ShowKeyboard(string userLang, string Label){ // show keyboard input
        int len = 12; int width = 60; int startPosX = -342; int startPosY = 125; int i = 0; int j = 0;
        GameObject rootObj = CreateCanvas("rootKeyoard", new Vector3(0, -3, 12), new Vector2(770, 350));
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image img = panel.AddComponent<Image>();
        img.color = new Vector4(1, 1, 1, 1f);
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
            if (i >= len) { j++; i = 0; }
            GameObject btKey = CreateButton(panelTransform, item.Key, item.Value, "Keyboard", item.Key, new Vector3(startPosX + i * (60 + 2) - delta, startPosY - j * (60 + 2), 0), new Vector2(width, 60));
            Image imgKey = btKey.GetComponent<Image>();
            imgKey.color = new Vector4(0.5f, 0.5f, 0.8f, 1);
            i++;
        }
        CreateButton(rootObj.transform, "Enter", Data.getMessage(userLang, "btnNext"), "Enter"+Label, "0_10_10", new Vector3(0, -230, 0), new Vector2(150, 60));
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
        CreateText(brInputMsg, new Vector2(0, 1f), new Vector2(800, 180), Data.getMessage(main.getLng(), "msg"+Label), 40, 1, TextAnchor.MiddleCenter);
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
        main.TextInput = InputText.GetComponent<Text>();
        return rootObj;
    }

    public GameObject ShowDigitsKeyboard(string userLang, string Label)
    { // show digits keyboard input
        int len = 12; int width = 60; int startPosX = -250; int startPosY = 90; int i = 0; int j = 0;
        GameObject rootObj = CreateCanvas("rootKeyoard", new Vector3(0, -3, 12), new Vector2(770, 150));
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image img = panel.AddComponent<Image>();
        img.color = new Vector4(1, 1, 1, 1f);
        img.sprite = uisprite;
        img.type = Image.Type.Sliced;
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(rootObj.transform, true);
        panelTransform.localScale = new Vector3(1f, 1f, 1f);
        panelTransform.localPosition = new Vector3(0, 70, 0);
        panelTransform.sizeDelta = new Vector2(770, 130);
        Dictionary<string, string> keyDictionary = new Dictionary<string, string>();
        float delta = 30; 
        for (i = 0; i < 10; i++){            
            GameObject btKey = CreateButton(panelTransform, i.ToString(), i.ToString(), "KeyboardDigit", i.ToString(), new Vector3(startPosX + i * (60 + 2) - delta, 0, 0), new Vector2(width, 60));
            Image imgKey = btKey.GetComponent<Image>();
            imgKey.color = new Vector4(0.5f, 0.5f, 0.8f, 1);
        }
        CreateButton(rootObj.transform, "Del", Data.getMessage(userLang, "btnDelete"), "KeyboardDigit", "DEL", new Vector3(240, 170, 0), new Vector2(220, 60));
        GameObject InputMsg = new GameObject("InputMsg");
        InputMsg.transform.SetParent(rootObj.transform);
        RectTransform brInputMsg = InputMsg.AddComponent<RectTransform>();
        brInputMsg.localScale = new Vector3(1f, 1f, 1f);
        brInputMsg.sizeDelta = new Vector2(800, 190);
        brInputMsg.localPosition = new Vector3(0, 360, 0);
        Image imgInputMsg = InputMsg.AddComponent<Image>();
        imgInputMsg.sprite = uisprite;
        imgInputMsg.color = new Vector4(0.8f, 0.8f, 0.8f, 1);
        imgInputMsg.type = Image.Type.Sliced;
        CreateText(brInputMsg, new Vector2(0, 1f), new Vector2(800, 180), Data.getMessage(userLang, "msg" + Label), 40, 1, TextAnchor.MiddleCenter);

        GameObject InputMsg2 = new GameObject("InputMsg2");
        InputMsg2.transform.SetParent(rootObj.transform);
        RectTransform brInputMsg2 = InputMsg2.AddComponent<RectTransform>();
        brInputMsg2.localScale = new Vector3(1f, 1f, 1f);
        brInputMsg2.sizeDelta = new Vector2(500, 60);
        brInputMsg2.localPosition = new Vector3(-120, 235, 0);
        Image imgInputMsg2 = InputMsg2.AddComponent<Image>();
        imgInputMsg2.sprite = uisprite;
        imgInputMsg2.color = new Vector4(0.8f, 0.8f, 0.8f, 1);
        imgInputMsg2.type = Image.Type.Sliced;

        CreateText(brInputMsg2, new Vector2(-160, 0), new Vector2(100, 60), Data.getMessage(userLang, "msgYear"), 40, 1, TextAnchor.MiddleCenter);
        CreateText(brInputMsg2, new Vector2(0, 0), new Vector2(100, 60), Data.getMessage(userLang, "msgMonth"), 40, 1, TextAnchor.MiddleCenter);
        CreateText(brInputMsg2, new Vector2(160, 0), new Vector2(100, 60), Data.getMessage(userLang, "msgDay"), 40, 1, TextAnchor.MiddleCenter);

        GameObject Input = new GameObject("Input");
        Input.transform.SetParent(rootObj.transform);
        RectTransform brInput = Input.AddComponent<RectTransform>();
        brInput.localScale = new Vector3(1f, 1f, 1f);
        brInput.sizeDelta = new Vector2(500, 50);
        brInput.localPosition = new Vector3(-120, 170, 0);
        Image imgInput = Input.AddComponent<Image>();
        imgInput.sprite = uisprite;
        imgInput.color = new Vector4(0.9f, 0.9f, 0.9f, 1);
        imgInput.type = Image.Type.Sliced;
        GameObject InputText = CreateText(brInput, new Vector2(0, -0.5f), new Vector2(400, 50), "XX         XX         XX", 40, 1, TextAnchor.MiddleCenter);
        main.TextInput = InputText.GetComponent<Text>();
        return rootObj;
    }

    // select dialog object
    public GameObject ShowDialog(string msg, string notes, string action, string actionLabel0, string actionLabel1, Vector2 size, TextAnchor anchor, Vector2 startLoc){
        //logDebug("ShowMessage");
        GameObject rootObj = new GameObject("rootObj");
        rootObj.transform.position = new Vector3(0, main.baseLoc, 12);
        GameObject rootCanvas = CreateCanvas("rootMenu", new Vector3(0, 0, 0), size);
        rootCanvas.transform.SetParent(rootObj.transform, false);
        GameObject panel = new GameObject("Panel");
        panel.AddComponent<CanvasRenderer>();
        Image i = panel.AddComponent<Image>();
        i.color = new Vector4(1, 1, 1, 0.9f);
        i.sprite = uisprite;
        i.type = Image.Type.Sliced;
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(rootCanvas.transform, true);
        panelTransform.localScale = new Vector3(1f, 1f, 1f);
        panelTransform.localPosition = new Vector3(0, 40, 0);
        panelTransform.sizeDelta = size;
        //panelTransform.rotation = Quaternion.AngleAxis(-180, Vector3.up);        
        CreateText(panelTransform, startLoc, new Vector2(size.x - 150, size.y), msg, 50, 0, anchor);
        if(!notes.Equals("")){CreateText(panelTransform, new Vector2(size.x*0.4f - 20, size.y*0.4f-20), new Vector2(size.x/4,100), notes, 40, 0, anchor);}
        CreateButton(panelTransform, "Button0", actionLabel0, action, "0", new Vector3(-size.x / 5, -40 - size.y / 2, 0), new Vector2(300, 60));
        CreateButton(panelTransform, "Button1", actionLabel1, action, "1", new Vector3(size.x / 5, -40 - size.y / 2, 0), new Vector2(300, 60));

        // showTutorialsMenu(rootMenu);
        return rootObj;
    }

    // porgressbar object
    private GameObject progressBar(string name, string label, int value, Vector2 loc, Transform root) {
        GameObject gm = new GameObject(name);
        gm.transform.position = new Vector3(loc.x, loc.y, 0);
        gm.transform.SetParent(root, false);
        
        Texture2D tex2 = new Texture2D(600, 600);
        Sprite sprite = Sprite.Create(tex2, new Rect(0.0f, 0.0f, 600, 100), new Vector2(300f, 300f), 100.0f);
        //Sprite spriteBack = Sprite.Create(tex2, new Rect(0.0f, 0.0f, 600, 100), new Vector2(300f, 300f), 100.0f);
        string sValue = value.ToString();
        CreateText(gm.transform, new Vector2(820, 10), new Vector2(1000, 70), label+":", 60, 0, TextAnchor.LowerRight);
        // drow background
        GameObject gm1 = new GameObject(name+"_back");
        gm1.transform.position = new Vector3(-120, 0, 0);
        gm1.transform.SetParent(gm.transform, false);        
        Image img = gm1.AddComponent<Image>();
        img.sprite = sprite;
        img.material = matFont;
        img.fillMethod = Image.FillMethod.Horizontal;
        img.type = Image.Type.Filled;
        img.fillOrigin = (int) Image.OriginHorizontal.Right;
        img.fillAmount = 1;
        img.color = new Vector4(0.68f, 0.68f, 0.7f, 1f);
        gm1.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 70);
        // drow current value
        GameObject gm2 = new GameObject(name + "_value");
        gm2.transform.position = new Vector3(-120, 0, 0);
        gm2.transform.SetParent(gm.transform, false);        
        Image img2 = gm2.AddComponent<Image>();
        img2.sprite = sprite;
        img2.material = matFont;
        img2.fillMethod = Image.FillMethod.Horizontal;
        img2.type = Image.Type.Filled;
        img2.fillOrigin = (int)Image.OriginHorizontal.Right;
        img2.fillAmount = (float) (value/100f);
        img2.color = new Vector4(0.34f, 0.41f, 0.94f, 1f);
        gm2.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 70);
        //+
        CreateText(gm2.transform, new Vector2(-450, 10), new Vector2(1000, 70), sValue + "%", 60, 0, TextAnchor.LowerLeft);
        CreateButton(gm.transform, "?"+name,"?", "ShowHelp", name+"_"+value, new Vector3(280, 0, 0), new Vector2(65, 65));
        return gm;
    }

    // results panel object
    public GameObject showResult(string userLang, int resSt, int resEn, string name1, string name2, int value1, int value2, int value3){
        Vector2 size = new Vector2(1200, 500);
        GameObject newCanvas = CreateCanvas("rootMenu", new Vector3(0, main.baseLoc, 12), size);
        GameObject panel = new GameObject("ResultPanel");
        panel.AddComponent<CanvasRenderer>();
        Image img = panel.AddComponent<Image>();
        img.sprite = uisprite;
        img.color = new Vector4(0.7f, 0.7f, 0.75f, 0.5f);
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(newCanvas.transform, true);
        panelTransform.localScale = new Vector3(1f, 1f, 1f);
        panelTransform.localRotation = Quaternion.AngleAxis(180, Vector3.up);
        panelTransform.localPosition = new Vector3(0, 0, 0);
        panelTransform.sizeDelta = new Vector2(1600, 900);        
        CreateText(panelTransform, new Vector2(0, 340), new Vector2(1400, 70), Data.getMessage(userLang, "Res_C"), 60, 0, TextAnchor.LowerCenter);
        progressBar("Stress", Data.getMessage(userLang, "Res_C_S"), resSt, new Vector2(-150, 240),panelTransform);
        progressBar("Effi", Data.getMessage(userLang, "Res_C_E"), resEn, new Vector2(-150, 140), panelTransform);

        CreateText(panelTransform, new Vector2(0, 0), new Vector2(1400, 70), Data.getMessage(userLang, "Res_T"), 60, 0, TextAnchor.LowerCenter);    
        progressBar(name1, Data.getMessage(userLang, name1), value1, new Vector2(-150, -100), panelTransform);
        progressBar(name2, Data.getMessage(userLang, name2), value2, new Vector2(-150, -200), panelTransform);
        progressBar("power", Data.getMessage(userLang, "Power"), value3, new Vector2(-150, -300), panelTransform);

        CreateButton(panelTransform, "btnExit", Data.getMessage(userLang, "btnExit"), "Exit", "0", new Vector3(100-size.x / 2, 20 - size.y, 0), new Vector2(300, 65));
        CreateButton(panelTransform, "btnRepeat", Data.getMessage(userLang, "btnRepeat"), "Repeat", "0", new Vector3(0, 20 - size.y, 0), new Vector2(300, 65));
        CreateButton(panelTransform, "btnAbout", Data.getMessage(userLang, "btnAbout"), "Next", "0", new Vector3(-100 + size.x / 2, 20 - size.y, 0), new Vector2(300, 65));

        return newCanvas;
    }

    public IEnumerator PauseInit(float timer, int scene){
        float currCountdownValue = timer;
        while (currCountdownValue > 0){
            //Debug.Log("Countdown: " + currCountdownValue);
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        main.NextScene(scene);
    }

    // a fade transition between scenas
    public IEnumerator FadeScene(float duration, bool startNewScene, Color color, int scene){
        if (main.camFade) {
            main.camFade.GetComponent<Renderer>().enabled = true;
            float startTransparent = 0f; float endTransparent = 1f; float smoothness = 0.05f; float progress = 0;
            float increment = smoothness / duration; //The amount of change to apply.
            if (startNewScene == true){ startTransparent = 1f; endTransparent = 0f;}
            Color colorStart = new Color(color.r, color.g, color.b, startTransparent);
            main.camFade.GetComponent<Renderer>().materials[0].color = colorStart;
            Color colorEnd = new Color(colorStart.r, colorStart.g, colorStart.b, endTransparent);
            while (progress < 1){
                progress += increment;
                main.camFade.GetComponent<Renderer>().materials[0].color = Color.Lerp(colorStart, colorEnd, progress);
                yield return new WaitForSeconds(smoothness);
            }
            if (startNewScene == true){
                Debug.Log("Start scene " + scene + " tr=" + startNewScene);
                main.camFade.GetComponent<Renderer>().enabled = false;
                if (main.SceneEventSystem) { main.SceneEventSystem.enabled = true; }
                main.NextScene(0); 
            } else { Application.Quit(); }
        } else { Debug.Log("Can not find camFade object"); main.NextScene(0);}
    }

    // a rotate transition between scenas
    public IEnumerator RotateCamera(float duration, float speedUp, string newAction) {
        Transform camTransform = Camera.main.transform;
        float progress = 300;
        float smooth = 0.0001f;
        if (speedUp < 0) { smooth = 0.05f; }
        float angleDelta = 1f;
        float newAngle = 0f;
        while (progress > 0 && smooth > 0) {
            progress--;
            // smooth = Mathf.Lerp(smooth, speedMax, smooth);
            smooth += speedUp;
            newAngle += angleDelta;
            Quaternion target = Quaternion.Euler(newAngle, newAngle, newAngle);
            //Debug.Log(progress + " " + smooth);            
            //transform.rotation = Quaternion.Slerp(transform.rotation, target, smooth);
            yield return null;
        }
        Debug.Log("Start new action " + newAction);
        // StartAction(speedUp);
    }

    // text animation for switching text messages
    public IEnumerator rotateText(GameObject gm, bool appear, float step, string nextAnimation, int index){
        enableAction(gm, false);
        Debug.Log("start animation for "+gm.name);
        //float step = stepAnimation;
        float currCountdownValue = 100f;
        Transform transform = gm.transform;        
        if (appear) { step = -step; transform.Rotate(Vector3.left, currCountdownValue); }        
        while (currCountdownValue > 0){
            transform.Rotate(Vector3.left, step);
            currCountdownValue-=Mathf.Abs(step);
            yield return null;            
        }
        enableAction(gm, true);
        if (nextAnimation != "") { main.startNewAnimation(nextAnimation,index); }
        //main.trDirect = true;
        Debug.Log("Animation is completed");
    }

    public void enableAction(GameObject obj, bool act) {
        if (obj) {
            GvrPointerGraphicRaycaster action = obj.GetComponent<GvrPointerGraphicRaycaster>();
            if (action) { action.enabled = act; }}}

    public GameObject ShowSwitchlngMenu(string userLang){
        GameObject rootObj =  ShowMessage(Data.getMessage(userLang, "msgLangSw"), "", "", new Vector2(1200, 150), TextAnchor.MiddleCenter, new Vector2(0, 0));
        Dictionary<string, string> langs = Data.getLanguages();
        int cnt  = langs.Count, j=-350;
        var enumerator = langs.GetEnumerator();
        while (enumerator.MoveNext()){
            CreateButton(rootObj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform, "Button"+j, enumerator.Current.Value, "LangSwMenu", enumerator.Current.Key, new Vector3(j, -150, 0), new Vector2(300, 65));
            j += 350;
        }
        return rootObj;    
    }

    public GameObject ShowGenderMenu(string userLang){
        GameObject rootObj = ShowMessage(Data.getMessage(userLang, "msgGender"), "", "", new Vector2(1200, 150), TextAnchor.MiddleCenter, new Vector2(0, 0));
        CreateButton(rootObj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform, "Male", Data.getMessage(userLang, "btnMale"), "GenderSwMenu", "", new Vector3(-200, -150, 0), new Vector2(300, 65));
        CreateButton(rootObj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform, "Female", Data.getMessage(userLang, "btnFemale"), "GenderSwMenu", "", new Vector3(200, -150, 0), new Vector2(300, 65));
        CreateButton(rootObj.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform, "Unknown", Data.getMessage(userLang, "btnUnknown"), "GenderSwMenu", "", new Vector3(0, -250, 0), new Vector2(600, 65));        
        return rootObj;
    }
}
