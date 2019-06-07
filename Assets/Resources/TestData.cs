using System.Collections.Generic;

[System.Serializable] public class TestData{
    public string userStartTime;
    public UserData userData;
    public string ipInfo;
    public List<TestObjects> rightObjectsList;
    public List<TestObjects> selectedObjectsList;
    public List<SnenaMotionData> snenasMotionData;
    public TextTestResult textTestResult;
    public ColorTestResult colorTestResult;

}

[System.Serializable] public class SnenaMotionData{
    public int i; // scene namber
    public List<UserActivity> act; // action description
    // public List<int[]> act;
}

[System.Serializable] public class UserActivity{
    public int[] a;
    // time in msec from start scene
    // frames number from previus action
    // head.rotation.x in miliDeg
    // head.rotation.y in miliDeg
    // ead.rotation.z in miliDeg
    // curfocusObjCode[0], // right or not                    
    // curfocusObjCode[1], // cur obj type 
    // curfocusObjCode[2], // cur obj color
    // curfocusObjCode[3], // right obj type
    // curfocusObjCode[4], // right obj color
    // curfocusObjCode[5], // obj position.x 
    // curfocusObjCode[6], // obj position.y 
    // curfocusObjCode[7], // obj position.z 
    // curfocusObjCode[8], // obj angle.x
    // curfocusObjCode[9], // obj angle.y
    // curfocusObjCode[10] // obj angle.z
}

[System.Serializable] public class UserData{
    public string name;
    public string email;
    public string birth;
    public string gender;
    public string input;
    public string zone;
    public string deviceID;
    public string lang;
    public string ip;
    public string txtVersion;
    public int gyro;
    public string deviceInfo;
    //public string userID;
}

[System.Serializable] public class TestObjects{
    public int i;
    public int timer;
    public int time;
    public int rights;
    public List<TestObject> objs;
}

[System.Serializable] public class TestObject{
    public int time;
    public int i;
    public int color;
    public int type;
    public int lx;
    public int ly;
    public int lz;
    public int rx;
    public int ry;
    public int rz;
}

[System.Serializable] public class TextTestResult{
    public List<TestAnswer> answers; 
    public int extra;
    public int stabil;
    public int totalTime;
    public string Name1;
    public string Name2;
    public int Value1;
    public int Value2;
    public int Power;
}

[System.Serializable] public class ColorTestResult {
    public List<TestAnswer> selected;
    public int energy;
    public int stress;
    public int totalTime;
}

[System.Serializable] public class TestAnswer {
    public int id;
    public int nm;
    public int t;
}