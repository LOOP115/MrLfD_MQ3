# Mixed Reality Workstation for LfD



## Requirements

- Unity Editor Version: `2022.3.19f1 (>= 2022.3)`
- Packages
  - [Meta XR All-in-One SDK](https://assetstore.unity.com/packages/tools/integration/meta-xr-all-in-one-sdk-269657)
  - [Depth API](https://github.com/oculus-samples/Unity-DepthAPI)
  - [ROS TCP Connector](https://github.com/Unity-Technologies/ROS-TCP-Connector)
  - [URDF Importer](https://github.com/Unity-Technologies/URDF-Importer)





## Resources

- [Meta Interaction SDK Overview](https://developer.oculus.com/documentation/unity/unity-isdk-interaction-sdk-overview/)
- [Meta XR Interaction SDK OVR Samples](https://assetstore.unity.com/packages/tools/integration/meta-xr-interaction-sdk-ovr-samples-268521)
- [Map Controllers](https://developer.oculus.com/documentation/unity/unity-ovrinput/)
- [Unity Robotics Hub](https://github.com/Unity-Technologies/Unity-Robotics-Hub/tree/main)
  - Setup [ROS Unity Integration](https://github.com/Unity-Technologies/Unity-Robotics-Hub/blob/main/tutorials/ros_unity_integration/README.md)
  - Setup [URDF Importer](https://github.com/Unity-Technologies/URDF-Importer)
    - [URDF Tutorial](https://github.com/Unity-Technologies/Unity-Robotics-Hub/blob/main/tutorials/urdf_importer/urdf_tutorial.md)
    - [URDF Tutorial Appendix](https://github.com/Unity-Technologies/Unity-Robotics-Hub/blob/main/tutorials/urdf_importer/urdf_appendix.md##Convex-Mesh-Collider)
- [Pick-and-Place Tutorial](https://github.com/Unity-Technologies/Unity-Robotics-Hub/tree/main/tutorials/pick_and_place)





## Franka Controller

### Parameters

- Stiffness: 10000
- Damping: 100
- Force Limit: 1000
- Speed: 30
- Torque: 100
- Acceleration: 10



### Basic Operations

- Use the **A** and **B** buttons to select the articulation body you want to move.
- Use the **right thumb stick** to move the articulation body clockwise and counter-clockwise.





## Design Elements

- Panels
  - Manipulability
  - Live view of Franka's wrist camera
- LfD mode
  - Past demonstrations
  - Learning outcome preview
- Sorting mode
  - Spawn red or blue spheres
  - Delete red or blue spheres
  - Clear all spheres
  - Lock / Unlock the spheres
- Home Menu
  - Tabs for different panels
    - Toggle Visibility
  - Buttons to open/close all tabs
- Stop Button





## Troubleshooting

### Problems with Android build

#### [Fail to export Android package due to URDF importer](https://github.com/Unity-Technologies/URDF-Importer/issues/212)

- Enter into `Packages/com.unity.robotics.urdf-importer/Runtime/UnityMeshImporter/Plugins/AssimpNet/Native/win/x86/`
- Delete `assimp.dll` and `assimp.dll.meta`
- Note: Packages are stored in `Library/PackageCache/` if using Unity's Package Manager.



#### [Unable to build due to namespace errors](https://github.com/Unity-Technologies/Unity-Robotics-Hub/issues/215)

- Tick the `Android` checkbox in `Platforms` of `Unity.Robotics.URDFImporter.asmdef`

