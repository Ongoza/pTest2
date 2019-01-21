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
    public string deviceID; 
    public string userEmail;
    public List<TestObjects> rightObjectsList;
    public List<TestObjects> selectedObjectsList;
    public List<SnenaMotionData> snenasMotionData;    
}

[System.Serializable] public class SnenaMotionData{
    public int sceneIndex;
    public List<UserActivity> userActivities;
}

[System.Serializable] public class UserActivity{
    public int time;
    public int[] action;
}

[System.Serializable] public class TestObjects{
    public int testIndex;
    public int testTimer;
    public int testingTime;
    public int numRightAnswers;
    public List<TestObject> objectsList;
}

[System.Serializable] public class TestObject{
    public int time;
    public int objectIndex;
    public int objectColorIndex;
    public int objectTypeIndex;
    public int location_x;
    public int location_y;
    public int location_z;
    public int rotation_x;
    public int rotation_y;
    public int rotation_z;
}
