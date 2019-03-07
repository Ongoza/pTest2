using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Utility3D
{
    private int nHor = 10;   // Total number of objects for 6 = 16, for 10 =32 
    private float distanceMax = 8f; // min distance from camera to an object
    private float distanceMin = 16.1f; // max distance from camera to an objec  
    public Material primitivesMaterial;
    private int[] objCounter = new int[5] { 0, 0, 0, 0, 0 };
    // list of objects colors {"gray","blue","green","red","yellow","purple","brown","black" }
    // variables for tests
    //{ "Cube", "Sphere", "Capsule", "Cylinder","Pyramid" };
    // {"gray","blue","green","red","yellow","purple","brown","black" }
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
    private int nVer;
    private float aStartH;
    private float aStartV;
    private float aStartR;
    public Vector3 btExitLoc;
    private Main main;
    private Utility utility;

    public Utility3D(Main mainScript, Utility mainUtility)
    {
        main = mainScript;
        utility = mainUtility;
        nVer = nHor* 2;
        aStartH = 2 * Mathf.PI / nHor;
        aStartV = 2 * Mathf.PI / nVer;
        aStartR = aStartH / 4f;

    }

    // create objects koordinates list around the camera in random locations
    public GameObject CreateObjsArray(bool isShowTime)
    {
        utility.logDebug("CreateObjsArray");
        GameObject rootObj = new GameObject("rootObj");
        rootObj.transform.position = new Vector3(0, main.baseLoc, 0);
        main.testsConfig[main.curTestIndex, 3] = 0;
        main.testsConfig[main.curTestIndex, 4] = 0;
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
            { // on the bottom put only one object. This is "Exit" button
                if (pi_2)
                {
                    pi_2 = false;
                    float a = 10 * Mathf.Cos(evalution);
                    btExitLoc.x = a * Mathf.Cos(Mathf.PI * 1.5f);
                    btExitLoc.y = 10 * Mathf.Sin(evalution);
                    btExitLoc.z = a * Mathf.Sin(Mathf.PI * 1.5f);
                    //locations.Add(SphericalToCartesianPlusRandom(Mathf.PI * 1.5f, evalution));
                }
            }
            else
            {
                for (int j = 0; j < nHor / 2; j++)
                {
                    float polar = j * aStartH;
                    locations.Add(SphericalToCartesianPlusRandom(polar, evalution));
                }
            }
        }
        int max = locations.Count;
        //create objects for selection by a tested person
        List<int> objectTypes = new List<int>() { 0, 1, 2, 3, 4 };
        TestObjects rightObjectsList = new TestObjects()
        {
            i = main.curTestIndex,
            objs = new List<TestObject>()
        };
        for (int j = 1; j < main.testsConfig[main.curTestIndex, 2] + 1; j++)
        {
            int newIndex = Random.Range(0, max);
            string name = j + "_" + main.testsConfig[main.curTestIndex, 0] + "_" + main.testsConfig[main.curTestIndex, 1] + "_0";
            int rot = Random.Range(0, 360);
            //Debug.Log("created right "+name);
            SetUpObj(rootObj,main.testsConfig[main.curTestIndex, 0], name, locations[newIndex], arrColor[main.testsConfig[main.curTestIndex, 1]], rot);
            rightObjectsList.objs.Add(
                new TestObject()
                {
                    i = j,
                    color = main.testsConfig[main.curTestIndex, 1],
                    type = main.testsConfig[main.curTestIndex, 0],
                    lx = Mathf.RoundToInt(locations[newIndex].x * main.precisionDec),
                    ly = Mathf.RoundToInt(locations[newIndex].y * main.precisionDec),
                    lz = Mathf.RoundToInt(locations[newIndex].z * main.precisionDec),
                    rx = rot,
                    ry = rot,
                    rz = rot
                });
            locations.RemoveAt(newIndex);
            max--;
        }
        main.testData.rightObjectsList.Add(rightObjectsList);
        main.testData.selectedObjectsList.Add(
            new TestObjects()
            {
                i = main.curTestIndex,
                timer = main.testsConfig[main.curTestIndex, 5],
                time = 0,
                rights = 0,
                objs = new List<TestObject>()
            });
        objectTypes.Remove(main.testsConfig[main.curTestIndex, 0]);
        //Debug.Log(testData.selectedObjectsList.Count + " testData created =" + JsonUtility.ToJson(testData.selectedObjectsList));
        //create rnadom objects
        int cntObjs = 1;
        foreach (Vector3 loc in locations)
        {
            int colIndex = Random.Range(0, 8);
            Color col = arrColor[colIndex];
            int typeObj = objectTypes[Random.Range(0, 4)];
            string objName = "0_" + typeObj + "_" + colIndex + "_" + objCounter[typeObj];
            objCounter[typeObj]++;
            cntObjs++;
            SetUpObj(rootObj,typeObj, objName, loc, col, Random.Range(0, 360));
        }
        //Create "Exit" button       
        GameObject canExit = utility.CreateCanwas("rootMenu", btExitLoc, new Vector2(100, 50));
        canExit.transform.SetParent(rootObj.transform);
        utility.CreateButton(canExit.transform, "btExit", "Exit", "Next", "0_10_10", new Vector3(0, 0, 0), new Vector2(100, 50));
        canExit.transform.Rotate(Vector3.left, -60);
        Camera.main.GetComponent<GvrPointerPhysicsRaycaster>().enabled = true;
        // Create hint for a tested person
        main.TimerCanvas = new GameObject("Hint");
        Canvas c = main.TimerCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.WorldSpace;
        main.TimerCanvas.AddComponent<CanvasScaler>();
        RectTransform NewCanvasRect = main.TimerCanvas.GetComponent<RectTransform>();
        main.TimerCanvas.transform.SetParent(Camera.main.transform, true);
        NewCanvasRect.localPosition = new Vector3(0, 2.8f, 7);
        NewCanvasRect.sizeDelta = new Vector2(300, 40);
        NewCanvasRect.localScale = new Vector3(0.06f, 0.06f, 1f);
        GameObject panel = new GameObject("HintPanel");
        panel.AddComponent<CanvasRenderer>();
        Image img = panel.AddComponent<Image>();
        img.sprite = utility.uisprite;
        img.type = Image.Type.Sliced;
        img.color = new Vector4(1f, 1f, 1f, 1f);
        string msg = string.Format(Data.getMessage(main.userLang, "Test_hint"), Data.getMessage(main.userLang, "color_" + main.testsConfig[main.curTestIndex, 1]), Data.getMessage(main.userLang, "obj_" + main.testsConfig[main.curTestIndex, 0]), 0, main.testsConfig[main.curTestIndex, 2]);
        int height = 20;
        // Add to the hint message a timer informer
        //Debug.Log(testsConfig[main.curTestIndex, 5]);
        if (isShowTime)
        {
            msg += string.Format(Data.getMessage(main.userLang, "Test_timer"), main.testsConfig[main.curTestIndex, 5]);
            height = 40;
            main.testTime = main.testsConfig[main.curTestIndex, 5];
        }
        RectTransform panelTransform = panel.GetComponent<RectTransform>();
        panel.transform.SetParent(NewCanvasRect, true);
        panelTransform.localScale = new Vector3(0.3f, 0.3f, 1f);
        panelTransform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        panelTransform.localPosition = new Vector3(0, 0, 0);
        panelTransform.sizeDelta = new Vector2(300, height);
        GameObject txtObj = utility.CreateText(panelTransform, new Vector2(0, 1.5f), new Vector2(300, height), msg, 12, 0, TextAnchor.MiddleCenter);
        txtObj.transform.localRotation = Quaternion.AngleAxis(0, Vector3.right);
        main.hintText = txtObj.GetComponent<Text>();
        return rootObj;
    }

    // Set up 3d paramaters for 3d objects for a test
    private void SetUpObjParams(GameObject rootObj, GameObject obj, string name, Vector3 loc, Color col, float rot)
    {
        //utility.logDebug("SetUpObjParams");
        if (obj)
        {
            obj.name = name;
            obj.transform.SetParent(rootObj.transform, false);
            obj.transform.position = loc;
            obj.transform.eulerAngles = new Vector3(rot, rot, rot);
            Renderer rend = obj.GetComponent<Renderer>();
            rend.material = primitivesMaterial;
            rend.material.color = col;
            obj.AddComponent<SphereCollider>();
            EventTrigger be = obj.AddComponent<EventTrigger>();
            EventTrigger.Entry entryEnterGaze = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
            entryEnterGaze.callback.AddListener((eventData) => { main.OnEnterTimed("Test", name, false); });
            be.triggers.Add(entryEnterGaze);
            EventTrigger.Entry entryExitGaze = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
            entryExitGaze.callback.AddListener((eventData) => { main.OnExitTimed(); });
            be.triggers.Add(entryExitGaze);
        }
    }

        public void createTarget(GameObject root, int colIndex)
        {
            GameObject obj = CreatPyramid3();
            obj.name = "Model";
            Renderer rend = obj.GetComponent<Renderer>();
            rend.material = primitivesMaterial;
            rend.material.color = getColor(colIndex);
            obj.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            obj.transform.position = new Vector3(0, 2.7f, 11);
            obj.transform.Rotate(0, 77, 0);
            obj.transform.SetParent(root.transform, false);
        }

    // create 3d objects for a test 
    void SetUpObj(GameObject rootObj, int typeObj, string name, Vector3 loc, Color col, float rot)
    {
        //utility.logDebug("SetUpObj");
        switch (typeObj)
        {
            case 0: SetUpObjParams(rootObj, GameObject.CreatePrimitive(PrimitiveType.Cube), name, loc, col, rot); break;
            case 1: SetUpObjParams(rootObj, GameObject.CreatePrimitive(PrimitiveType.Sphere), name, loc, col, rot); break;
            case 2: SetUpObjParams(rootObj, GameObject.CreatePrimitive(PrimitiveType.Capsule), name, loc, col, rot); break;
            case 3: SetUpObjParams(rootObj, GameObject.CreatePrimitive(PrimitiveType.Cylinder), name, loc, col, rot); break;
            case 4:
                //Debug.Log("created 2 right " + name);
                SetUpObjParams(rootObj, CreatPyramid3(), name, loc, col, rot); break;
            default: Debug.Log("Error!!! Incorrect obj type."); break;
        }
    }

    // transform coordinates from Spherical To Cartesian and and some random parameters
    Vector3 SphericalToCartesianPlusRandom(float polar, float evalution)
    {
        utility.logDebug("Start");
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

    public GameObject CreatPyramid3()
    {
        Mesh mesh = new Mesh();
        Vector3 p0 = new Vector3(0, 0, 0);
        Vector3 p1 = new Vector3(1, 0, 0);
        Vector3 p2 = new Vector3(0.5f, 0, Mathf.Sqrt(0.75f));
        Vector3 p3 = new Vector3(0.5f, Mathf.Sqrt(0.75f), Mathf.Sqrt(0.75f) / 3);
        mesh.Clear();
        mesh.vertices = new Vector3[] { p0, p1, p2, p0, p2, p3, p2, p1, p3, p0, p3, p1 };
        mesh.triangles = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        GameObject obj = new GameObject();
        MeshFilter mF = obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        mF.sharedMesh = mesh;
        return obj;
    }

    void CreatPyramid4()
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
        mesh.vertices = new Vector3[] { p0, p1, p2, p3, p4, p5, p6, p7, p8 };
        mesh.triangles = new int[] { 8, 2, 3, 8, 6, 2, 8, 3, 7, 8, 7, 6, 3, 2, 7, 2, 6, 7 };         // front // right// left// back// bottom
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        GameObject obj = new GameObject();
        MeshFilter mF = obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        Renderer rend = obj.GetComponent<Renderer>();
        mF.sharedMesh = mesh;
    }

    public GameObject CreateObjsMenu(int colIndex)
    {
        GameObject rootObj = new GameObject("rootObj");
        Color col = arrColor[colIndex];
        SetUpObjParams(rootObj, GameObject.CreatePrimitive(PrimitiveType.Cube), "0", new Vector3(-5,0,16), col, 35);
        SetUpObjParams(rootObj, GameObject.CreatePrimitive(PrimitiveType.Sphere), "1", new Vector3(-2.5f, 0, 16), col, 35);
        SetUpObjParams(rootObj, GameObject.CreatePrimitive(PrimitiveType.Capsule), "2", new Vector3(0, 0, 16), col, 35);
        SetUpObjParams(rootObj, GameObject.CreatePrimitive(PrimitiveType.Cylinder), "3", new Vector3(2.5f, 0, 16), col, 35);
        SetUpObjParams(rootObj, CreatPyramid3(), "4", new Vector3(5f, 0, 16), col, 35);
        return rootObj; 
    }

    public Color getColor(int i){return arrColor[i];}
}
