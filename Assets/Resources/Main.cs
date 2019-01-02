using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 1 куб 2 сфера 3 капсула 4 цилиндр 5 пирамида  = 25 figures + 8 colors
// 360/5 =72 или 0.4*Mathf.PI
// в рамках ячейки фигуры расположены случайно надо задать минимальное расстояние до границы ячейки и минМакс удаление от камеры
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
    // Start is called before the first frame update
    void Start()
    {
        primitivesMaterial = new Material(Shader.Find("Specular"));
    //    creactCube(new Vector3(-4, 0.5f, 0), new Color(0.777f, 08f, 0.604f, 1f));
    //    creactSphere(new Vector3(-2, 0.5f, 0), new Color(0.777f, 08f, 0f, 1f));
    //    creactCapsule(new Vector3(0, 0.5f, 0), new Color(0f, 0f, 0.604f, 1f));
    //    creactCylinder(new Vector3(2, 0.5f, 0), new Color(1f, 0f, 0f, 1f));
        creatPyramid3(new Vector3(4, 0.5f, 0), new Color(1f, 0f, 0f, 1f));
        creatPyramid4(new Vector3(0, 2f, 0), new Color(0.5f, 1f, 0.5f, 1f));

    }

    // Update is called once per frame
    void Update()
    {
        
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


    void creactCube(Vector3 loc, Color col)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        obj.transform.position = loc;
        Renderer rend = obj.GetComponent<Renderer>();
        rend.material = primitivesMaterial;
        rend.material.color = col;
    }
    void creactSphere(Vector3 loc, Color col)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        obj.transform.position = new Vector3(-2, 1.5f, 0);
        Renderer rend = obj.GetComponent<Renderer>();
        rend.material = primitivesMaterial;
        rend.material.color = col;
    }
    void creactCapsule(Vector3 loc, Color col)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        obj.transform.position = loc;
        Renderer rend = obj.GetComponent<Renderer>();
        rend.material = primitivesMaterial; ;
        rend.material.color = col;
    }
    void creactCylinder(Vector3 loc, Color col)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        obj.transform.position = loc;
        Renderer rend = obj.GetComponent<Renderer>();
        rend.material = primitivesMaterial;
        rend.material.color = new Color(0f, 1f, 0f, 1f);
    }

    void creatPyramid3(Vector3 loc, Color col)
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
        GameObject obj = new GameObject("Pyramid3");
        obj.transform.position = loc;
        MeshFilter mF = obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        Renderer rend = obj.GetComponent<Renderer>();
        rend.material = primitivesMaterial; ;
        rend.material.color = col;
        mF.sharedMesh = mesh;
    }

    void creatPyramid4(Vector3 loc, Color col)
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
        GameObject obj = new GameObject("Pyramid4");
        obj.transform.position = loc;
        MeshFilter mF = obj.AddComponent<MeshFilter>();
        obj.AddComponent<MeshRenderer>();
        Renderer rend = obj.GetComponent<Renderer>();
        rend.material = primitivesMaterial; ;
        rend.material.color = col;
        mF.sharedMesh = mesh;
    }

}
