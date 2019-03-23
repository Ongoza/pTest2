using System.Collections;
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
    //private List<int> selNames = new List<int>();
    private string selNameData;
    private int selNamesCounter;
    private GameObject root;
    private int selectedColor;
    private MeshFilter quad1;
    private float defaultTime = 3f;
    private Main main;
    //private Utility utility;

    public ColorTest(Main mainScript, Utility mainUtility){
        main = mainScript;
       // utility = mainUtility;
    }

    public GameObject showColors(string btType){
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

    private void createArrays(){
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
        for (int i = 0; i < 2; i++){
            int h = 4;
            for (int j = 0; j < h; j++){
                int k = i * h + j;
                arrLoc[k] = new float[] { xLoc[j], yLoc[i] };
            }
        }
    }


    private void createBaseObjs(string btType){
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
        for (int i = 0; i < 8; i++){
            listCards[i] = createCard2(i, NewCanvasRect, new Color(arrColor[i][0] / 255f, arrColor[i][1] / 255f, arrColor[i][2] / 255f, 1f), btType);
            listCards[i].transform.Translate(arrLoc[i][0] * 2, arrLoc[i][1] * 2, 0);
        }
    }

    private GameObject createCard2(int i, RectTransform root, Color color, string btType){
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
    public IEnumerator FadeTo(GameObject obj){
        if (obj){
            EventTrigger et = obj.GetComponent<EventTrigger>();
            if (et) { obj.GetComponent<EventTrigger>().enabled = false; }
            float smoothness = 0.05f; float duration = 1f;
            Color colorStart = obj.GetComponent<Image>().color;
            Color colorEnd = new Color(colorStart.r, colorStart.g, colorStart.b, 0);
            float progress = 0; float increment = smoothness / duration; //The amount of change to apply.
            while (progress < 1){
                progress += increment;
                if (obj){
                    obj.GetComponent<Image>().color = Color.Lerp(colorStart, colorEnd, progress);
                    yield return null;
                }
                else { yield break; }
            };
            main.destroy(obj);
        }
    }
}
