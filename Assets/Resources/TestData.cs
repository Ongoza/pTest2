[System.Serializable] public class TestData{
    public string startDateTime;
    public string deviceID;    
    public int test2Time;
    public string email;
    public int[,] testsResult;
    public SnenaMotionData[] snenaMotionData;
}

[System.Serializable] public class SnenaMotionData{
    public int sceneIndex;
    public UserActivities[] userActivities;
}

[System.Serializable] public class UserActivities {
    public float time;
    public float[] activity;
}