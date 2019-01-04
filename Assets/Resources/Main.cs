using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO
// set numbers of objects for search
// ввод почты в начале для идентификации
// хранение информации на сервере
// онлайн тест бумажный со сохранением статуса и возможностью повторного прохождения
//
// фиксируем:
// - время каждого события (время между попаданием объекта в фокус и покиданием фокуса или выбором объекта)
// - траекторию движения (в сферических координатах), скорость вращения и ускорение вращения камеры
// - точность наведения на центр объекта, тип объекта, положение объекта и его размер
// - правильность выполнения задания
// - время выполнения задания
// - 


public class Main : MonoBehaviour
{
    private Material primitivesMaterial;
    private GameObject objRoot;
    private int total_objs = 0;
    private int[] objCounter = new int[5];
    private int nHor = 10;   // Total number of objects for 6 = 16, for 10 =32 
    private int nVer;
    private float distanceMax = 6f;
    private float distanceMin = 16.1f;
    private float aStartH;
    private float aStartV;
    private float aStartR;    
    private Color[] arrColor = new Color[8];
    // Start is called before the first frame update
    void Start()
    {
        objRoot = new GameObject("objRoot");
        primitivesMaterial = new Material(Shader.Find("Specular"));
        nVer = nHor*2;
        aStartH = 2 * Mathf.PI / nHor;
        aStartV = 2 * Mathf.PI / nVer;
        aStartR = aStartH / 4f;        
        arrColor[0] = new Color(171f / 255f, 171f / 255f, 171f / 255f, 1f);
        arrColor[1] = new Color(0f, 0f, 128f / 255f, 1f);
        arrColor[2] = new Color(3f / 255f, 114f / 255f, 21f / 255f, 1f);
        arrColor[3] = new Color(246f / 255f, 6f / 255f, 22f / 255f, 1f);
        arrColor[4] = new Color(251f / 255f, 251f / 255f, 2f / 255f, 1f);
        arrColor[5] = new Color(139f / 255f, 0f, 139f / 255f, 1f);
        arrColor[6] = new Color(139f / 255f, 69f / 255f, 19f / 255f, 1f);
        arrColor[7] = new Color(0f, 0f, 0f, 1f);        
        CreateGridObj();   

    }

    // create objects around the camera in random locations
    void CreateGridObj()
    {           
        bool pi_0 = true;
        bool pi_2 = true;       
        for (int i = 0; i < nVer/2; i++ )
        {
            float evalution = i * aStartV * 2.0f;            
            if (1.1f < evalution && 1.9f > evalution)
            { // on the top put only one object
                if (pi_0) {
                    pi_0 = false;
                    CreateObject("pi/2" , Mathf.PI/2f, evalution);
                }
            }
            else if (4.0f < evalution && 5.2f > evalution)
            { // on the bottom put only one object
                if (pi_2)
                {
                    pi_2 = false;                    
                    CreateObject("pi_2", Mathf.PI * 1.5f, evalution);
                }
            }
            else { 
                for (int j = 0; j < nHor/2; j++)
                {                    
                    var polar = j * aStartH;
                    CreateObject(i.ToString() + "_" + j.ToString(), polar, evalution);
                }
            }
        }
        Debug.Log("Total objects="+ total_objs + " by types "+string.Join(",", objCounter));
        // creatPyramid4(new Vector3(0, 2f, 0), new Color(0.5f, 1f, 0.5f, 1f));
    }

    //create object with random parameters
    void CreateObject(string name, float polar, float evalution)
    {
        // {"gray","blue","green","red","yellow","purple","brown","black" };
        Color col = arrColor[Random.Range(0, 8)];
        float distanceR = Random.Range(distanceMin, distanceMax);
        float evalutionR = Random.Range(evalution - aStartR, evalution + aStartR);
        float polarR = Random.Range(polar - aStartR, polar + aStartR);
        Vector3 loc = SphericalToCartesian(distanceR, polarR, evalutionR);
        int typeObj = Random.Range(0, 5);
        //int typeObj = 2;
        string objName = name + "_" + typeObj + "_" + total_objs;
        //Debug.Log(objName);
        objCounter[typeObj]++;             
        total_objs++;
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
            case 4:
                GameObject obj5 = CreatPyramid3();
                SetObjParams(obj5, objName, loc, col);
                break;
            default:
                print("Incorrect obj type.");
                break;
        }        
    }

    void SetObjParams(GameObject obj, string name, Vector3 loc, Color col)
    {
        obj.name = name;
        obj.transform.SetParent(objRoot.transform);
        obj.transform.position = loc;
        Renderer rend = obj.GetComponent<Renderer>();
        rend.material = primitivesMaterial;
        rend.material.color = col;
    }

     //converting from cartesian coordinates to spherical coordinates https://blog.nobel-joergensen.com/2010/10/22/spherical-coordinates-in-unity/
    Vector3 SphericalToCartesian(float radius, float polar, float elevation)
	{
	   Vector3 outCart;
       float a = radius * Mathf.Cos(elevation);
       outCart.x = a * Mathf.Cos(polar);
       outCart.y = radius * Mathf.Sin(elevation);
       outCart.z = a * Mathf.Sin(polar);
	   return outCart;
    }

    // Update is called once per frame
    void Update()
    {

    }


    GameObject CreatPyramid3()
    {       
        Mesh mesh = new Mesh();
        Vector3 p0 = new Vector3(0, 0, 0);
        Vector3 p1 = new Vector3(1, 0, 0);
        Vector3 p2 = new Vector3(0.5f, 0, Mathf.Sqrt(0.75f));
        Vector3 p3 = new Vector3(0.5f, Mathf.Sqrt(0.75f), Mathf.Sqrt(0.75f) / 3);
        mesh.Clear();
        mesh.vertices = new Vector3[]{
            p0,p1,p2,
            p0,p2,p3,
            p2,p1,p3,
            p0,p3,p1
        };

        mesh.triangles = new int[]{
            0,1,2,
            3,4,5,
            6,7,8,
            9,10,11
        };
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

        mesh.vertices = new Vector3[]{
            p0,p1,p2,p3,  
            p4,p5,p6,p7,p8
        };

        mesh.triangles = new int[]{
            // front
            8,2,3,             
            // right
           8,6,2 //third            
            // left
            ,8,3,7 //third
            // back
            ,8,7,6 //third            
            // bottom
            ,3,2,7 //third
            ,2,6,7 //fourth
        };

        //mesh.uv = new Vector2[]{
        //    new Vector2(0,1),
        //    new Vector2(0,0),
        //    new Vector2(1,1),
        //    new Vector2(1,0),
        //};

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        GameObject obj = new GameObject();
        MeshFilter mF = obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        Renderer rend = obj.GetComponent<Renderer>();
        mF.sharedMesh = mesh;
    }

}
