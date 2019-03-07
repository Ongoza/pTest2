﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class ColorTest 
{
    public Camera cam;
    private float[][] arrLoc = new float[8][];
    private float[][] arrColor = new float[8][];
    private GameObject[] listCards = new GameObject[8];
    private List<int> selNames = new List<int>();
    private string selNameData;
    private int selNamesCounter;
    private GameObject root;
    private int selectedColor;
    private MeshFilter quad1;
    private float defaultTime = 3f;
    private Main main;
    //private Utility utility;

    public ColorTest(Main mainScript, Utility mainUtility)
    {
        main = mainScript;
       // utility = mainUtility;
    }

    private void showResult()
    {
        float s = 0; float e = 0; float[] kArray = new float[] { 8.1f, 6.8f, 6, 5.3f, 4.7f, 4, 3.2f, 1.8f };
        //string listStr = ""; for (int i = 0; i < selNames.Count; i++) {listStr += selNames[i].ToString ();}
        //		Debug.Log ("selNames1="+listStr);
        for (int i = 0; i < selNames.Count; i++)
        {
            if (i < 3) { if (selNames[i] == 0 || selNames[i] == 6 || selNames[i] == 7) { s += kArray[i]; } }
            if (i > 4) { if (selNames[i] == 1 || selNames[i] == 2 || selNames[i] == 3 || selNames[i] == 4) { s += kArray[7 - i]; } }
            if (selNames[i] == 2 || selNames[i] == 3 || selNames[i] == 4) { e += kArray[i]; }
            //Debug.Log("Calculate: i=" +i+" color="+selNames[i]+" s=" +s+" e="+e); //
            //String data = Arrays.toString(selNames);
        }
        float resS = s / 42;
        float resE = (e - 9) / 12;
        Debug.Log("result==" + resS + " " + resE);
        GameObject newCanvas = new GameObject("ResultCanvas");
        Canvas c = newCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.WorldSpace;
        newCanvas.AddComponent<CanvasScaler>();
        newCanvas.AddComponent<GraphicRaycaster>();
        RectTransform NewCanvasRect = newCanvas.GetComponent<RectTransform>();
        //NewCanvasRect.Rotate(120,40,0);
        //NewCanvasRect.localRotation =  Quaternion.Euler(25,180,0);
        //newCanvas.transform.SetParent(Camera.main.transform, true);
        NewCanvasRect.localPosition = new Vector3(-8, 0, -30);
        NewCanvasRect.sizeDelta = new Vector2(1400, 1000);
        NewCanvasRect.localScale = new Vector3(0.07f, 0.07f, 1f);
        GameObject panel = new GameObject("ResultPanel");
        panel.AddComponent<CanvasRenderer>();
        Image img = panel.AddComponent<Image>();
        img.color = new Vector4(0.3f, 0.3f, 0.7f, 0.3f);
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(NewCanvasRect, true);
        panelTransform.localScale = new Vector3(0.3f, 0.3f, 1f);
        panelTransform.localRotation = Quaternion.AngleAxis(180, Vector3.up);
        panelTransform.localPosition = new Vector3(0, 0, 0);
        panelTransform.sizeDelta = new Vector2(1400, 1000);
        
        //GameObject txtObj = utility.CreateText(panelTransform, 340, 390, 1000, 100, "Your results:", 60, 0, TextAnchor.LowerLeft);
        //txtObj.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        Sprite sprite = Resources.Load<Sprite>("Textures/pieChart");
        Sprite spriteCircle = Resources.Load<Sprite>("Textures/pieChartCircle");
        //Debug.Log ("Sprite=" + sprite.texture.height);
        // stress
        string perS = (Math.Round(resS * 100)).ToString();
        //GameObject txtObjStress = utility.CreateText(panelTransform, -240, -340, 500, 100, "Stress: " + perS + "%", 60, 0, TextAnchor.LowerLeft);
        GameObject stress = new GameObject("StressBar");
        stress.transform.SetParent(panelTransform, false);
        Image imgStress = stress.AddComponent<Image>();
        imgStress.sprite = sprite;
        imgStress.type = Image.Type.Filled;
        imgStress.fillAmount = resS;
        imgStress.color = new Vector4(0.8f, 0.02f, 0.02f, 1f);
        RectTransform stressPieTran = stress.GetComponent<RectTransform>();
        stressPieTran.localScale = new Vector3(8f, 8f, 1f);
        stressPieTran.localPosition = new Vector3(-340, 43, 0);
        GameObject stressCircle = new GameObject("StressBarCircle");
        stressCircle.transform.SetParent(stressPieTran, false);
        Image imgStressCircle = stressCircle.AddComponent<Image>();
        imgStressCircle.sprite = spriteCircle;

        string perE = (Math.Round(resE * 100)).ToString();
        //GameObject txtObjEfficiency = utility.CreateText(panelTransform, 380, -340, 500, 100, "Efficiency: " + perE + "%", 60, 0, TextAnchor.LowerLeft);
        GameObject efficiency = new GameObject("EfficiencyBar");
        efficiency.transform.SetParent(panelTransform, false);
        Image imgEfficiency = efficiency.AddComponent<Image>();
        imgEfficiency.sprite = sprite;
        imgEfficiency.type = Image.Type.Filled;
        imgEfficiency.fillAmount = resE;
        imgEfficiency.color = new Vector4(0f, 0f, 0.8f, 1f);
        RectTransform efficiencyPieTran = efficiency.GetComponent<RectTransform>();
        efficiencyPieTran.localScale = new Vector3(8f, 8f, 1f);
        efficiencyPieTran.localPosition = new Vector3(340, 43, 0);
        GameObject efficiencyCircle = new GameObject("efficiencyBarCircle");
        efficiencyCircle.transform.SetParent(efficiencyPieTran, false);
        Image imgefficiencyCircle = efficiencyCircle.AddComponent<Image>();
        imgefficiencyCircle.sprite = spriteCircle;


        GameObject bt1 = new GameObject("btnExit");
        RectTransform br1 = bt1.AddComponent<RectTransform>();
        br1.sizeDelta = new Vector2(400, 70);
        Image imgBtn = bt1.AddComponent<Image>();
        imgBtn.color = new Vector4(0.5f, 0.5f, 0.8f, 1);
        imgBtn.material.color = new Vector4(1f, 1f, 1f, 0.7f);
        br1.SetParent(panelTransform, true);
        br1.localPosition = new Vector3(500, -560, 0);
        br1.rotation = Quaternion.AngleAxis(-180, Vector3.up);
        br1.localScale = new Vector3(1, 1, 1);
        // HandleClickExit();
        //utility.CreateText(br1, 0, 0, 500, 100, "To Main Menu >>", 40, 1, TextAnchor.MiddleCenter);
        EventTrigger be = bt1.AddComponent<EventTrigger>();
        //EventTrigger.Entry entry = new EventTrigger.Entry();
        //entry.eventID = EventTriggerType.PointerClick;
        //entry.callback.AddListener((eventData) => { main.OnClickTimed; });
        //be.triggers.Add(entry);
        EventTrigger.Entry entryEnterGaze = new EventTrigger.Entry();
        entryEnterGaze.eventID = EventTriggerType.PointerEnter;
        entryEnterGaze.callback.AddListener((eventData) => { main.OnEnterTimed("color", "btnExit", true); });
        be.triggers.Add(entryEnterGaze);
        EventTrigger.Entry entryExitGaze = new EventTrigger.Entry();
        entryExitGaze.eventID = EventTriggerType.PointerExit;
        entryExitGaze.callback.AddListener((eventData) => { main.OnExitTimed(); });
        be.triggers.Add(entryExitGaze);
    }

    public GameObject showColors(string btType)
    {
        //selObj = new selColors ();       
        selectedColor = -1;
        //quad1.transform.rotation = Quaternion.Euler(0, 180, 45);
        root = new GameObject();
        root.name = "rootColors";
        root.transform.position = new Vector3(0, 0, 16);
        //root.transform.rotation.Set(0, 0, 1,0.2f);
        //root.transform.Rotate(Vector3.up,90);
        createArrays();
        //Debug.Log("Hello");
        createBaseObjs(btType);
        //selNamesCounter = 0;
        return root;
    }

    private void createArrays()
    {
        // {"gray","blue","green","red","yellow","purple","brown","black" };
        arrColor[0] = new float[] { 171, 171, 171 };
        arrColor[1] = new float[] { 0, 0, 128 };
        arrColor[2] = new float[] { 3, 114, 21 };
        arrColor[3] = new float[] { 246, 6, 22 };
        arrColor[4] = new float[] { 251, 251, 2 };
        arrColor[5] = new float[] { 139, 0, 139 };
        arrColor[6] = new float[] { 139, 69, 19 };
        arrColor[7] = new float[] { 0, 0, 0 };
        float[] xLoc = { -3f, -1f, 1f, 3f };
        float[] yLoc = { 1f, -1f };
        for (int i = 0; i < 2; i++)
        {
            int h = 4;
            for (int j = 0; j < h; j++)
            {
                int k = i * h + j;
                arrLoc[k] = new float[] { xLoc[j], yLoc[i] };
            }
        }
    }


    private void createBaseObjs(string btType)
    {
        //Debug.Log("start create cards 2");
        GameObject newCanvas = new GameObject("Canvas");       
        Canvas c = newCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.WorldSpace;
        newCanvas.AddComponent<CanvasScaler>();
        newCanvas.AddComponent<GvrPointerGraphicRaycaster>();
        RectTransform NewCanvasRect = newCanvas.GetComponent<RectTransform>();
        NewCanvasRect.transform.position = new Vector3(0, 0, 0);
        NewCanvasRect.sizeDelta = new Vector2(120, 70);       
        NewCanvasRect.transform.localScale = new Vector3(0.025f, 0.025f, 1);
        newCanvas.transform.SetParent(root.transform,false);
        for (int i = 0; i < 8; i++)
        {
            listCards[i] = createCard2(i, NewCanvasRect, new Color(arrColor[i][0] / 255f, arrColor[i][1] / 255f, arrColor[i][2] / 255f, 1f), btType);
            listCards[i].transform.Translate(arrLoc[i][0] * 2, arrLoc[i][1] * 2, 0);
        }
    }

    private GameObject createCard2(int i, RectTransform root, Color color, string btType)
    {
        GameObject button = new GameObject("b_" + i);
        Image img = button.AddComponent<Image>();
        //RawImage img = button.AddComponent<RawImage>();
        img.color = color;
        button.transform.SetParent(root, false);

        EventTrigger be = button.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) => { main.OnClickTimed();});
        be.triggers.Add(entry);
        EventTrigger.Entry entryEnterGaze = new EventTrigger.Entry();
        entryEnterGaze.eventID = EventTriggerType.PointerEnter;
        entryEnterGaze.callback.AddListener((eventData) => { main.OnEnterTimed(btType, i.ToString(), true); });
        be.triggers.Add(entryEnterGaze);
        EventTrigger.Entry entryExitGaze = new EventTrigger.Entry();
        entryExitGaze.eventID = EventTriggerType.PointerExit;
        entryExitGaze.callback.AddListener((eventData) => { main.OnExitTimed(); });
        be.triggers.Add(entryExitGaze);
        return button;
    }

    //void handleClick(int i)
    //{
    //    isTimer = false;
    //    workTime = defaultTime;
    //    timedPointer.SetFloat("_Angle", 360);
    //    selectedColor = -1;
    //    if (i == 9)
    //    {
    //        StartCoroutine(fadeScene(1.0f, false, new Color(0.2f, 0.2f, 0.2f, 0f), "Main"));
    //    }
    //    else
    //    {
    //        if (!selNames.Contains(i))
    //        {
    //            selNames.Add(i);
    //            Destroy(listCards[i].GetComponent<EventTrigger>());
    //            StartCoroutine(FadeTo(i));
    //            if (selNames.Count > 6)
    //            {
    //                if (selNames.Count == 7)
    //                {
    //                    int j = 0;
    //                    while (j < 8)
    //                    {
    //                        if (!selNames.Contains(j))
    //                        {
    //                            selNames.Add(j);
    //                            Debug.Log("Already clicked=" + j);
    //                            Destroy(listCards[j].GetComponent<EventTrigger>());
    //                            StartCoroutine(FadeTo(j));
    //                        }
    //                        j++;
    //                    }
    //                }
    //                Destroy(GameObject.Find("StartCanvas"));
    //                globalData.addcTest(selNames);
    //                StartCoroutine(waitResult());
    //            }
    //        }
    //        else { Debug.Log("Already clicked=" + i); }
    //    }
    //}

    //private IEnumerator waitResult()
    //{
    //    yield return new WaitForSeconds(2);
    //    showResult();
    //}

   
    //public void onClickTimed(int i)
    //{
    //    Debug.Log("Start Click timed=" + i);
    //    isTimer = false; workTime = defaultTime;
    //    timedPointer.SetFloat("_Angle", 360);
    //    if (selectedColor > -1)
    //    {
    //        handleClick(selectedColor);
    //        selectedColor = -1;
    //    }
    //    else { Debug.Log("Start Click timed selected = -1"); }

    //}

    //private GameObject CreateText(Transform parent, float x, float y, float w, float h, string message, int fontSize, int fontStyle, TextAnchor achor)
    //{
    //    GameObject textObject = new GameObject("Text");
    //    textObject.transform.SetParent(parent, false);
    //    RectTransform trans = textObject.AddComponent<RectTransform>();
    //    trans.sizeDelta = new Vector2(w, h);
    //    trans.anchoredPosition3D = new Vector3(0, 0, 0);
    //    trans.anchoredPosition = new Vector2(x, y);
    //    trans.transform.rotation = Quaternion.AngleAxis(-180, Vector3.up);
    //    trans.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    //    trans.localPosition.Set(0, 0, 0);
    //    textObject.AddComponent<CanvasRenderer>();
    //    Text text = textObject.AddComponent<Text>();
    //    text.supportRichText = true;
    //    text.text = message;
    //    text.fontSize = fontSize;
    //    if (fontStyle == 1) { text.fontStyle = FontStyle.Bold; }
    //    text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
    //    text.alignment = achor;
    //    //text.alignment = TextAnchor.MiddleCenter;
    //    //text.horizontalOverflow = HorizontalWrapMode.Overflow;
    //    text.color = new Color(0, 0, 0);
    //    return textObject;
    //}
}
