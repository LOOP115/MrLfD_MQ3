# User Manual



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

### Reach Target

Adjust the target's position in Unity using Quest 3 controllers or hand movements. Once the target's position is modified, it is published immediately. Franka then receives these updates and reaches the target using MoveIt2. Simultaneously, the trajectory is published back to Unity, synchronizing the virtual Franka with its counterparts in Gazebo or the real world.

- Follow this [link](https://github.com/LOOP115/franka_ctrl) to configure the ROS machine before start.
- Press **A** or **pinch** your thumb and index finger together to change the target's position, teleporting it to your right hand's location.

### Follow Target



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

<br>