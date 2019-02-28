using System.Collections.Generic;

// x - time from start scene
// userDataList[x][0] - head.rotation.x
// userDataList[x][1] - head.rotation.y
// userDataList[x][2] - head.rotation.z
// userDataList[x][3] - (>0) is ray is inside wrong object 0 - is inside right object, (-1) - is outside object
// userDataList[x][4] - distance between center of object and a ray
// userDataList[x][5] - 0-7 color of current object
// userDataList[x][6] - 0-4 type of current object
// userDataList[x][7] - 0-7 color of right object
// userDataList[x][8] - 0-4 type of right object
// User ations in all (7) scenes

[System.Serializable] public class TestData{
    public string startDateTime;
    public string userZone;
    public string deviceID; 
    public string userEmail;
    public string lang;
    public string ip;
    public int gyro;
    public string ipInfo;
    public string deviceInfo;
    public List<TestObjects> rightObjectsList;
    public List<TestObjects> selectedObjectsList;
    public List<SnenaMotionData> snenasMotionData;    
}

[System.Serializable] public class SnenaMotionData{
    public int i;
    public List<UserActivity> act;
}

[System.Serializable] public class UserActivity{
    public int t;
    public int[] a;
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
