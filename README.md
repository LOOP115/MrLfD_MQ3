# Mixed Reality Workstation for LfD

<br>

## Requirements

- Unity Editor Version: `2022.3.19f1 (>= 2022.3)`
- Packages
  - [Meta XR All-in-One SDK](https://assetstore.unity.com/packages/tools/integration/meta-xr-all-in-one-sdk-269657)
  - [Depth API](https://github.com/oculus-samples/Unity-DepthAPI)
  - [ROS TCP Connector](https://github.com/Unity-Technologies/ROS-TCP-Connector)
  - [URDF Importer](https://github.com/Unity-Technologies/URDF-Importer)

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

<br>

## Franka

### Controller Parameters

- Stiffness: 10000
- Damping: 100
- Force Limit: 1000
- Speed: 30
- Torque: 100
- Acceleration: 10

### Franka Control Panel

#### Initialize the virtual Franka

- **Spawn:**
  - Move your right hand and press **A** to spawn Franka at the position of your right controller.

  - Only one Franka is allowed.

- **Remove:** Remove the spawned Franka.

- **Adjust the Position:**
  - Use the **right thumbstick** to move Franka horizontally.
  - Press **Y** to move up, **X** to move down.
  - Use the **left thumbstick** to rotate Franka around the Y axis.
- **Lock:** Lock the base link to disable adjusting Franka's position.
- **Reset**: Move the Franka to default initial configuration.

#### Joint Controller

- Press **A** and **B** to switch the joint you want to move.
- Use the **right thumbstick** to move the joint clockwise and counter-clockwise.

<br>

## Design Elements

- Panels
  - Franka Control Panel
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
  - Quit game

<br>

## Troubleshooting

### Problems with Android build

#### [Fail to export Android package due to URDF importer](https://github.com/Unity-Technologies/URDF-Importer/issues/212)

- Enter into `Packages/com.unity.robotics.urdf-importer/Runtime/UnityMeshImporter/Plugins/AssimpNet/Native/win/x86/`
- Delete `assimp.dll` and `assimp.dll.meta`
- Note: Packages are stored in `Library/PackageCache/` if using Unity's Package Manager.

#### [Unable to build due to namespace errors](https://github.com/Unity-Technologies/Unity-Robotics-Hub/issues/215)

- Tick the `Android` checkbox in `Platforms` of `Unity.Robotics.URDFImporter.asmdef`

