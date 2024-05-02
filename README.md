# XRFranka Meta

This is the Unity endpoint for [XRFranka](https://github.com/LOOP115/Franka_XR_Hub). It is built based on the Meta SDK and can run natively on the Meta Quest 3.

<br>

## Requirements

- Unity Editor Version: `2022.3.19f1 (>= 2022.3)`
- Packages
  - [Meta XR All-in-One SDK](https://assetstore.unity.com/packages/tools/integration/meta-xr-all-in-one-sdk-269657)
  - [DepthAPI](https://github.com/oculus-samples/Unity-DepthAPI)
  - [ROS TCP Connector](https://github.com/Unity-Technologies/ROS-TCP-Connector)
  - [URDF Importer](https://github.com/Unity-Technologies/URDF-Importer)

<br>

## Troubleshooting

### Problems with Android build

#### [Fail to export Android package due to URDF importer](https://github.com/Unity-Technologies/URDF-Importer/issues/212)

- Enter into `Packages/com.unity.robotics.urdf-importer/Runtime/UnityMeshImporter/Plugins/AssimpNet/Native/win/x86/`
- Delete `assimp.dll` and `assimp.dll.meta`
- Note: Packages are stored in `Library/PackageCache/` if using Unity's Package Manager.

#### [Unable to build due to namespace errors](https://github.com/Unity-Technologies/Unity-Robotics-Hub/issues/215)

- Tick the `Android` checkbox in `Platforms` of `Unity.Robotics.URDFImporter.asmdef`

<br>

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
