using Unity.Robotics;
using RotationDirection = Unity.Robotics.UrdfImporter.Control.RotationDirection;
using ControlType = Unity.Robotics.UrdfImporter.Control.ControlType;
using FKRobot = Unity.Robotics.UrdfImporter.Control.FKRobot;
using UnityEngine;


namespace Franka.Control
{
    public class JointController : MonoBehaviour
    {
        private ArticulationBody[] articulationBodies;
        private ArticulationBody[] articulationChain;
        // Stores original colors of the part being highlighted
        private Color[] prevColor;
        private int previousIndex;

        [InspectorReadOnly(hideInEditMode: true)]
        public string selectedJoint;
        
        [HideInInspector]
        public int selectedIndex;

        public ControlType control = ControlType.PositionControl;
        public float stiffness = 10000f;
        public float damping = 100f;
        public float forceLimit = 1000f;
        public float speed = 30f; // Units: degree/s
        public float torque = 100f; // Units: Nm or N
        public float acceleration = 10f;// Units: m/s^2 / degree/s^2

        [Tooltip("Color to highlight the currently selected join")]
        public Color highLightColor = new Color(1.0f, 0, 0, 1.0f);

        private bool controllerActive = false;


        void Start()
        {
            previousIndex = selectedIndex = 0;
            this.gameObject.AddComponent<FKRobot>();
            articulationBodies = this.GetComponentsInChildren<ArticulationBody>();
            int defDyanmicVal = 10;
            foreach (ArticulationBody joint in articulationBodies)
            {
                joint.gameObject.AddComponent<FrankaJointControl>();
                joint.jointFriction = defDyanmicVal;
                joint.angularDamping = defDyanmicVal;
                ArticulationDrive currentDrive = joint.xDrive;
                currentDrive.forceLimit = forceLimit;
                joint.xDrive = currentDrive;
            }

            articulationChain = new ArticulationBody[FrankaConstants.NumJoints];
            var linkName = string.Empty;
            for (var i = 0; i < FrankaConstants.NumJoints; i++)
            {
                linkName += FrankaConstants.LinkNames[i];
                articulationChain[i] = transform.Find(linkName).GetComponent<ArticulationBody>();
            }
            // DisplaySelectedJoint(selectedIndex);
            StoreJointColors(selectedIndex);
        }

        void SetSelectedJointIndex(int index)
        {
            if (articulationChain.Length > 0)
            {
                selectedIndex = (index + articulationChain.Length) % articulationChain.Length;
            }
        }

        void Update()
        {
            if (!controllerActive)
            {
                return;
            }

            bool SelectionInput1 = OVRInput.GetDown(OVRInput.Button.One);
            bool SelectionInput2 = OVRInput.GetDown(OVRInput.Button.Two);

            SetSelectedJointIndex(selectedIndex); // to make sure it is in the valid range
            UpdateDirection(selectedIndex);

            if (SelectionInput2)
            {
                SetSelectedJointIndex(selectedIndex - 1);
                Highlight(selectedIndex);
            }
            else if (SelectionInput1)
            {
                SetSelectedJointIndex(selectedIndex + 1);
                Highlight(selectedIndex);
            }

            UpdateDirection(selectedIndex);
        }

        public void setControllerState(bool state)
        {
            controllerActive = state;
            if (controllerActive)
            {
                previousIndex = 6;
                selectedIndex = 0;
                StoreJointColors(previousIndex);
                Highlight(selectedIndex);
            }
            else
            {
                ResetJointColors(selectedIndex);
            }
        }


        /// <summary>
        /// Highlights the color of the robot by changing the color of the part to a color set by the user in the inspector window
        /// </summary>
        /// <param name="selectedIndex">Index of the link selected in the Articulation Chain</param>
        private void Highlight(int selectedIndex)
        {
            if (selectedIndex == previousIndex || selectedIndex < 0 || selectedIndex >= articulationChain.Length)
            {
                return;
            }

            // reset colors for the previously selected joint
            ResetJointColors(previousIndex);

            // store colors for the current selected joint
            StoreJointColors(selectedIndex);

            // DisplaySelectedJoint(selectedIndex);
            Renderer[] rendererList = articulationChain[selectedIndex].transform.Find("Visuals").GetComponentsInChildren<Renderer>();

            // set the color of the selected join meshes to the highlight color
            foreach (var mesh in rendererList)
            {
                Color originalColor = MaterialExtensions.GetMaterialColor(mesh);
                Color adjustedColor = Color.Lerp(originalColor, highLightColor, 0.5f);
                MaterialExtensions.SetMaterialColor(mesh.material, adjustedColor);
            }

        }

        void DisplaySelectedJoint(int selectedIndex)
        {
            if (selectedIndex < 0 || selectedIndex >= articulationChain.Length)
            {
                return;
            }
            selectedJoint = articulationChain[selectedIndex].name + " (" + selectedIndex + ")";
        }

        /// <summary>
        /// Stores original color of the part being highlighted
        /// </summary>
        /// <param name="index">Index of the part in the Articulation chain</param>
        private void StoreJointColors(int index)
        {
            Renderer[] materialLists = articulationChain[index].transform.Find("Visuals").GetComponentsInChildren<Renderer>();
            prevColor = new Color[materialLists.Length];
            for (int counter = 0; counter < materialLists.Length; counter++)
            {
                prevColor[counter] = MaterialExtensions.GetMaterialColor(materialLists[counter]);
            }
        }

        /// <summary>
        /// Resets original color of the part being highlighted
        /// </summary>
        /// <param name="index">Index of the part in the Articulation chain</param>
        private void ResetJointColors(int index)
        {
            Renderer[] previousRendererList = articulationChain[index].transform.Find("Visuals").GetComponentsInChildren<Renderer>();
            for (int counter = 0; counter < previousRendererList.Length; counter++)
            {
                MaterialExtensions.SetMaterialColor(previousRendererList[counter].material, prevColor[counter]);
            }
        }

        public void UpdateControlType(FrankaJointControl joint)
        {
            joint.controltype = control;
            if (control == ControlType.PositionControl)
            {
                ArticulationDrive drive = joint.joint.xDrive;
                drive.stiffness = stiffness;
                drive.damping = damping;
                joint.joint.xDrive = drive;
            }
        }

        /// <summary>
        /// Sets the direction of movement of the joint on every update
        /// </summary>
        /// <param name="jointIndex">Index of the link selected in the Articulation Chain</param>
        private void UpdateDirection(int jointIndex)
        {
            if (jointIndex < 0 || jointIndex >= articulationChain.Length)
            {
                return;
            }

            float moveDirection = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;
            FrankaJointControl current = articulationChain[jointIndex].GetComponent<FrankaJointControl>();
            if (previousIndex != jointIndex)
            {
                FrankaJointControl previous = articulationChain[previousIndex].GetComponent<FrankaJointControl>();
                previous.direction = RotationDirection.None;
                previousIndex = jointIndex;
            }

            if (current.controltype != control)
            {
                UpdateControlType(current);
            }

            if (moveDirection > 0)
            {
                current.direction = RotationDirection.Positive;
            }
            else if (moveDirection < 0)
            {
                current.direction = RotationDirection.Negative;
            }
            else
            {
                current.direction = RotationDirection.None;
            }
        }

        // public void OnGUI()
        // {
        //     GUIStyle centeredStyle = GUI.skin.GetStyle("Label");
        //     centeredStyle.alignment = TextAnchor.UpperCenter;
        //     GUI.Label(new Rect(Screen.width / 2 - 200, 10, 400, 20), "Press left/right arrow keys to select a robot joint.", centeredStyle);
        //     GUI.Label(new Rect(Screen.width / 2 - 200, 30, 400, 20), "Press up/down arrow keys to move " + selectedJoint + ".", centeredStyle);
        // }
    }

}
