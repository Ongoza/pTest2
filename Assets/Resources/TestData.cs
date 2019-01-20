using System.Collections.Generic;

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
    public float[] action;
}

[System.Serializable] public class TestObjects{
    public int testIndex;
    public int testTime;
    public List<TestObject> objectsList;
}

[System.Serializable] public class TestObject{
    public int time;
    public float objectColorIndex;
    public float objectTypeIndex;
    public float location_x;
    public float location_y;
    public float location_z;
    public float rotation_x;
    public float rotation_y;
    public float rotation_z;
}