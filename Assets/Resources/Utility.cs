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

    public Utility(Main mainScript, bool isVRLog) {
        main = mainScript;
        isDebug = isVRLog;
        Debug.Log("VR Debug is "+ isDebug);
        defaultFont = Font.CreateDynamicFontFromOSFont("Roboto", 1);
        GameObject goDebug = GameObject.Find("txtDebug");
        Texture2D tex = new Texture2D(128, 128);
        uisprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        if (isDebug) {            
            if (goDebug) { TextDebug = goDebug.GetComponent<Text>(); }
            else {
                Debug.Log("Can't find a debug obj");
            }
        }
        else
        {
            Debug.Log("VR Debug off");
        }
        }

    public GameObject ShowMessage(string msg, string action, string actionLabel, Vector2 size, TextAnchor anchor, Vector2 startLoc)
    {
        //logDebug("ShowMessage");
        GameObject rootObj = CreateCanwas("rootMenu", new Vector3(0, main.baseLoc, 12), size);
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
        if (!action.Equals("")) { 
            CreateButton(panelTransform, "Button", actionLabel, action, "0_10_10", new Vector3(0, 60 - size.y / 2, 0), new Vector2(300, 60));
        }
        // showTutorialsMenu(rootMenu);
        return rootObj;
    }

    // create canavas for text messages
    public GameObject CreateCanwas(string name, Vector3 loc, Vector2 size)
    {
        //logDebug("CreateCanwas");
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

    public void logDebug(string msg)
    {
        if (isDebug)
        {
            if (TextDebug)
            {
                TextDebug.text += msg + "=>";
            }
        }
        else
        {
            Debug.Log(msg);
        }
    }

    // create text object
    public GameObject CreateText(Transform parent, Vector2 loc, Vector2 size, string message, int fontSize, int fontStyle, TextAnchor achor)
    {
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
        text.fontSize = fontSize;
        if (fontStyle == 1) { text.fontStyle = FontStyle.Bold; }
        text.font = defaultFont;
        text.alignment = achor;
        //text.horizontalOverflow = HorizontalWrapMode.Overflow;
        text.color = new Color(0, 0, 0);
        return textObject;
    }

    //create button object
    public GameObject CreateButton(Transform parent, string name, string label, string action1, string action2, Vector3 loc, Vector2 size)
    {
        //logDebug("CreateButton 1");
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

    // display the email input scena
    public GameObject ShowKeyboard()
    { // show keyboard input
        int len = 12; int width = 60; int startPosX = -342; int startPosY = 125; int i = 0; int j = 0;
        GameObject rootObj = CreateCanwas("rootKeyoard", new Vector3(0, -3, 12), new Vector2(770, 350));
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
        foreach (KeyValuePair<string, string> item in keyDictionary)
        {
            float delta = 0;
            if (item.Key.Length > 1) { width = 60 * 2; i += 1; delta = 30; }
            if (i >= len) { j++; i = 0; }
            GameObject btKey = CreateButton(panelTransform, item.Key, item.Value, "Keyboard", item.Key, new Vector3(startPosX + i * (60 + 2) - delta, startPosY - j * (60 + 2), 0), new Vector2(width, 60));
            Image imgKey = btKey.GetComponent<Image>();
            imgKey.color = new Vector4(0.5f, 0.5f, 0.8f, 1);
            i++;
        }
        CreateButton(rootObj.transform, "Enter", "Start", "Enter", "0_10_10", new Vector3(0, -230, 0), new Vector2(150, 60));
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
        CreateText(brInputMsg, new Vector2(0, 1f), new Vector2(800, 180), Data.getMessage(main.userLang, "Email"), 40, 1, TextAnchor.MiddleCenter);
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
        main.TextEmail = InputText.GetComponent<Text>();
        return rootObj;
    }

    public GameObject ShowDialog(string msg, string notes, string action, string actionLabel0, string actionLabel1, Vector2 size, TextAnchor anchor, Vector2 startLoc)
    {
        //logDebug("ShowMessage");
        GameObject rootObj = CreateCanwas("rootMenu", new Vector3(0, main.baseLoc, 12), size);
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
        if(!notes.Equals("")){CreateText(panelTransform, new Vector2(size.x*0.4f, size.y*0.4f), new Vector2(size.x/4,100), notes, 40, 0, anchor);}
        CreateButton(panelTransform, "Button0", actionLabel0, action, "0", new Vector3(-size.x / 5, 60 - size.y / 2, 0), new Vector2(300, 60));
        CreateButton(panelTransform, "Button1", actionLabel1, action, "1", new Vector3(size.x / 5, 60 - size.y / 2, 0), new Vector2(300, 60));

        // showTutorialsMenu(rootMenu);
        return rootObj;
    }
}
