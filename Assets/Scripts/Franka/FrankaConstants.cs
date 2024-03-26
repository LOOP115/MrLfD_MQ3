public static class FrankaConstants
{
    public static readonly int NumJoints = 7;
    
    public static readonly float[] StartJointPositionsRadians = new float[]
    {
        0.0f, // Joint 1
        -0.7853981633974483f, // Joint 2, equivalent to -45 degrees
        0.0f, // Joint 3
        -2.356194490192345f, // Joint 4, equivalent to -135 degrees
        0.0f, // Joint 5
        1.5707963267948966f, // Joint 6, equivalent to 90 degrees
        0.7853981633974483f // Joint 7, equivalent to 45 degrees
    };
    
    public static readonly string[] LinkNames = new string[]
    {
        "world/panda_link0/panda_link1",
        "/panda_link2",
        "/panda_link3",
        "/panda_link4",
        "/panda_link5",
        "/panda_link6",
        "/panda_link7"
    };

    
    public static readonly int NumFingers = 2;

    public static readonly float FingerOpen = 0.04f;
    
    public static readonly string[] FingerName = new string[]
    {
        "world/panda_link0/panda_link1/panda_link2/panda_link3/panda_link4/panda_link5/panda_link6/panda_link7/panda_link8/panda_hand/panda_rightfinger",
        "world/panda_link0/panda_link1/panda_link2/panda_link3/panda_link4/panda_link5/panda_link6/panda_link7/panda_link8/panda_hand/panda_leftfinger"
    };
    
}
