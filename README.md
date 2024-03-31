# Franka_Quest3

This is the Unity endpoint for [Franka_XR](https://github.com/LOOP115/Franka_XR_Hub), offering features such as an XR control interface for Franka on Quest 3.

<br>

## Requirements

- Unity Editor Version: `2022.3.19f1 (>= 2022.3)`
- Packages
  - [Meta XR All-in-One SDK](https://assetstore.unity.com/packages/tools/integration/meta-xr-all-in-one-sdk-269657)
  - [ROS TCP Connector](https://github.com/Unity-Technologies/ROS-TCP-Connector)
  - [URDF Importer](https://github.com/Unity-Technologies/URDF-Importer)

<br>

## Franka Control Panel

### Initialize the virtual Franka

- **Spawn:**
  - Move your right hand and press **A** to spawn Franka at the position of your right controller.
  - Other toggles become available only after a Franka is spawned.
  - Only one Franka is allowed.
- **Remove:** Remove the spawned Franka.
- **Reset**: Move the Franka to its default initial configuration, which is automatically applied upon spawning.
- **Default Joint Parameters:**
  - Stiffness: 10000
  - Damping: 100
  - Force Limit: 1000
  - Speed: 30
  - Torque: 100
  - Acceleration: 10


## Control Modes

Several control modes are provided for Franka. Activate them by toggling the buttons. Note that only one mode can be active at a time; activating a new mode automatically disables the others.

### Base Lock

Lock or unlock the base link, controlling Franka's **global position**.

- Use the **right thumbstick** to move Franka horizontally.
- Press **Y** to move up, **X** to move down.
- Use the **left thumbstick** to rotate Franka around the Y axis.

### Joint Controller

Move Franka's joints with Quest 3 controllers.

- Press **A** and **B** to switch the joint you want to move.
- Use the **right thumbstick** to move the joint clockwise and counter-clockwise.

### Follow Target

Adjust the target's position in Unity using Quest 3 controllers or hand movements. Once the target's position is modified, it is published immediately. Franka then receives these updates and reaches the target using MoveIt2. Simultaneously, the trajectory is published back to Unity, synchronizing the virtual Franka with its counterparts in Gazebo or the real world.

- Follow this [link](https://github.com/LOOP115/franka_ctrl) to configure the ROS machine before start.
- Press **A** or **pinch** your thumb and index finger together to change the target's position, teleporting it to your right hand's location.

<br>

## Design Elements

- Panels
  - Main Menu
  - Franka Control Panel
  - Manipulability
  - Live view of Franka's wrist camera
  - LfD
  - Sorting

- Main Menu
  - Toggle the buttons to open or close tabs
  - Buttons to open/close all tabs
- Sorting
  - Spawn red or blue spheres
  - Delete red or blue spheres
  - Clear all spheres
  - Lock / Unlock the spheres
- LfD
  - Past demonstrations
  - Learning outcome preview

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
