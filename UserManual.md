# User Manual

<br>

## UI & Interactions

Upon launching the app, the first menu you'll see on the right side allows you to manage the **visibility of all panels**. Below is a list of the currently available panels:

- **Franka Control Panel**
  - Manage and operate the Franka robot.
  - Several control modes are provided for Franka. Activate them by toggling the buttons.
  - Note that some modes cannot be active simultaneously. If this is the case, the toggle will be non-interactable until it can be activated.
- **Visualisation**
  - View the status and configuration of the Franka robot.
- **Classification**
  - Create XR categories for sorting tasks.

### Panel Interactions

The panels support multiple interaction methods, including both **controller and hand tracking**. Interactions include:

- **Poking**
- **Pinching**
- **Ray casting**
- **Grabbing**

[Demo](https://www.youtube.com/watch?v=7haFwOXZTTc&list=PLGZ6M30GmbVPnrU4zVaIsvYRqLYsf4KVH&index=1)

<br>

## Visualisation

- Two types of robot statuses are visualised:
  - **Joint Positions**
    - Dial rings indicating the joint positions turn red as the joint limits are approached.
  - **Manipulability**
    - This refers to the ability to alter the end effector's position depending on the joint configuration.

<br>

## Joint Controller

Move the virtual Franka's joints with Quest 3 controllers.

- Press **A** and **B** to switch the joint you want to move.
- Use the **right thumbstick** to move the joint clockwise and counter-clockwise.
- Note that the movement affects only the virtual Franka, not the real one.

Joints' parameters:

- Stiffness: 10000
- Damping: 100
- Force Limit: 1000
- Speed: 30
- Torque: 100
- Acceleration: 10

[Demo](https://www.youtube.com/watch?v=_qyciwFwKOM&list=PLGZ6M30GmbVPnrU4zVaIsvYRqLYsf4KVH&index=2)

<br>

## Spawn & Calibration

- **Spawn the virtual Franka**
  - Move your right hand and press **A** to spawn Franka at the position of your right controller.
  - Other toggles for Franka become available only after a Franka is spawned.
  - Only one Franka is allowed.
- **Remove**: Remove the spawned virtual Franka.
- **Reset**: Move the Franka to its default initial configuration.
- **Calibration**: Overlay the virtual Franka onto the real one.
  - Stand in front of the robotic arm
  - Press the meta button to reset the view
  - Spawn the virtual Franka here
  - If Franka is successfully connected, the gripper will perform a homing action.
  - You can unlock the base to adjust the spawn location
  - Remember to lock the base
  - Make the virtual Franka invisible


- **Base Lock**: Lock or unlock the base link, controlling Franka's **global position**.
  - Use the **right thumbstick** to move Franka horizontally.
  - Press **Y** to move up, **X** to move down.
  - Use the **left thumbstick** to rotate Franka around the Y axis.

[Demo](https://www.youtube.com/watch?v=MYe43h8-ORQ&list=PLGZ6M30GmbVPnrU4zVaIsvYRqLYsf4KVH&index=3)

<br>

## Gripper Controller

Activate the gripper controller to use the gripper. This mode is compatible with all other modes.

- Press **B** to open or close the gripper

[Demo](https://www.youtube.com/watch?v=MimzQL8xsu4&list=PLGZ6M30GmbVPnrU4zVaIsvYRqLYsf4KVH&index=4)

<br>

## Discrete Target Tracking

Adjust the target's position in Unity using Quest 3 controllers or hand movements. Once the target's position is modified, it is published immediately. Franka then receives these updates and reaches the target using MoveIt2. Simultaneously, the trajectory is published back to Unity, synchronizing the virtual Franka with its counterparts in Gazebo or the real world.

#### Reach Target

- Press **A** or **pinch** your thumb and index finger together to spawn a new target.
- [Demo](https://www.youtube.com/watch?v=ZxCSm_ia81g&list=PLGZ6M30GmbVPnrU4zVaIsvYRqLYsf4KVH&index=5)

#### Follow Target

* Grab and move the target using the controller or your hand. When the target is stationary, Franka will move towards it.
* [Demo](https://www.youtube.com/watch?v=662ubJplJiQ&list=PLGZ6M30GmbVPnrU4zVaIsvYRqLYsf4KVH&index=6)

<br>

## Kinesthetic Teaching

To use kinesthetic teaching, deactivate all control modes except for the gripper controller.

- You can still use the **B** button to operate the gripper.
- Ensure that visualizations are enabled.

[Demo](https://www.youtube.com/watch?v=XJglIi99uRQ&list=PLGZ6M30GmbVPnrU4zVaIsvYRqLYsf4KVH&index=7)

<br>

## Follow Trajectory

Operate the Franka in real-time simply by grabbing the target with your hand at the gripper and moving it. The Franka's end effector will track the target, using Inverse Kinematics (IK) to compute the desired joint positions and execute the trajectories.

- It's recommended to use the controller to grab and move the target. While hand tracking is supported, maintaining stable hand control in the air to smoothly operate the Franka can be challenging.
- Boundaries are implemented as reminders to manage the target's position carefully.

**Demo Tasks:**

- Pick & Place
- Object Stacking
- Object Insertion

**Demo Features:**

- [Free Movement & Boundaries](https://www.youtube.com/watch?v=sV0myw7R4Do&list=PLGZ6M30GmbVPnrU4zVaIsvYRqLYsf4KVH&index=8)
- [Task Demonstrations](https://www.youtube.com/watch?v=Y4CgoC1DD3E&list=PLGZ6M30GmbVPnrU4zVaIsvYRqLYsf4KVH&index=9)

<br>

## Classification

This feature is currently in the conceptual stage. It allows you to create categories and sort objects based on your own criteria. Future integration with the [YOLO_Quest3](https://github.com/LOOP115/YOLO_Quest3) project is planned.

**Interactions:**

- Spawn red or blue spheres.
- Delete red or blue spheres.
- Clear all spheres.
- Lock or unlock the spheres.

[Demo](https://www.youtube.com/watch?v=X-4VjS5yAd8&list=PLGZ6M30GmbVPnrU4zVaIsvYRqLYsf4KVH&index=10)

[YOLO on Quest 3 Streaming](https://www.youtube.com/watch?v=GH1-Qg6-V3I&list=PLGZ6M30GmbVPnrU4zVaIsvYRqLYsf4KVH&index=12)

<br>

## [Demo All-In-One](https://www.youtube.com/watch?v=FrHReF052ss&list=PLGZ6M30GmbVPnrU4zVaIsvYRqLYsf4KVH&index=11)