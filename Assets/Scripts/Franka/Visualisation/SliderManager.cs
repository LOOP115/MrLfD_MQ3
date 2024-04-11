using UnityEngine;
using System.Collections;
using RosMessageTypes.CtrlInterfaces;
using UnityEngine.UI;
using Unity.Robotics.ROSTCPConnector;


public class SliderManager : MonoBehaviour
{
    private GameObject[] sliders = new GameObject[FrankaConstants.NumJoints];
    private float waitTime = 0.001f;

    // private RosConnector rosConnector;
    private ROSConnection rosConnection;

    private const string sliderPath = "/Dial-Hollow/Slider";
    private const string fillPath = "Fill Area/Fill";
    private const string backgroundPath = "OuterBorder";

    public Material sliderFillMaterial;
    private Material sliderBackgroundMaterial;


    void Start()
    {
        // rosConnector = FindObjectOfType<RosConnector>();
        rosConnection = ROSConnection.GetOrCreateInstance();
        
        // Get dials
        var linkName = string.Empty;
        for (var i = 0; i < FrankaConstants.NumJoints; i++)
        {
            linkName += FrankaConstants.LinkNamesFromBase[i];
            var sliderName = linkName + sliderPath;
            sliders[i] = transform.Find(sliderName).gameObject;
            InitializeSlider(sliders[i]);
        }
        sliderBackgroundMaterial = sliders[0].transform.Find(backgroundPath).GetComponent<Image>().material;
        Subscribe(true);
    }


    private void InitializeSlider(GameObject slider)
    {
        // Create a new material instance for this slider
        Material sliderMaterialInstance = new Material(sliderFillMaterial);

        Image targetImage = slider.transform.Find(fillPath).GetComponent<Image>();
        if (targetImage != null)
        {
            targetImage.material = sliderMaterialInstance;
        }

        Slider sliderComponent = slider.GetComponent<Slider>();
        // Set the initial value based color
        UpdateSliderColor(sliderMaterialInstance, sliderComponent.value);

        // Subscribe to the slider's value changed event
        sliderComponent.onValueChanged.AddListener((value) => {
            UpdateSliderColor(sliderMaterialInstance, value);
        });
    }

    private void UpdateSliderColor(Material material, float value)
    {
        // Update the material's shader property to reflect the slider's current value
        material.SetFloat("_SliderValue", value);
    }

    private void OnDestroy()
    {
        // Clean up created material instances to avoid memory leaks
        foreach (var slider in sliders)
        {
            Image targetImage = slider.transform.Find(fillPath).GetComponent<Image>();
            if (targetImage != null && targetImage.material != null)
            {
                Destroy(targetImage.material);
            }
        }
    }


    private void UpdateSliders(FrankaJointsMsg jointsMsg)
    {
        if (sliders.Length != jointsMsg.joints.Length)
        {
            Debug.LogWarning("Joint state message does not contain the expected number of joints.");
            return;
        }

        StartCoroutine(SetSliders(jointsMsg));
    }
    
    private IEnumerator SetSliders(FrankaJointsMsg jointsMsg)
    {
        for (int jointIndex = 0; jointIndex < FrankaConstants.NumJoints; jointIndex++)
        {
            if (jointIndex < sliders.Length)
            {
                SetSliderValue(sliders[jointIndex], (float)jointsMsg.joints[jointIndex], jointIndex);
            }
        }
        yield return new WaitForSeconds(waitTime);
    }

    private void SetSliderValue(GameObject slider, float jointValueRadians, int jointIndex)
    {
        Slider sliderComponent = slider.GetComponent<Slider>();
        FrankaConstants.JointLimits jointLimits = FrankaConstants.JointLimitsList[jointIndex];
        float jointValueDegrees = jointValueRadians * Mathf.Rad2Deg;
        
        float normalizedValue = (jointValueDegrees - jointLimits.minDegrees) / (jointLimits.maxDegrees - jointLimits.minDegrees);
        sliderComponent.value = normalizedValue;
    }

    public void Subscribe(bool unity=false)
    {
        // rosConnector.GetBridge().Subscribe<FrankaJointsMsg>(FrankaConstants.topicFrankaJoints, UpdateSliders);
        if (unity)
        {
            rosConnection.Subscribe<FrankaJointsMsg>(FrankaConstants.topicUnityFrankaJoints, UpdateSliders);
        }
        else
        {
            rosConnection.Subscribe<FrankaJointsMsg>(FrankaConstants.topicFrankaJoints, UpdateSliders);
        }
    }
    
    public void Unsubscribe(bool unity=false)
    {
        // rosConnector.GetBridge().Unsubscribe(FrankaConstants.topicFrankaJoints);
        if (unity)
        {
            rosConnection.Unsubscribe(FrankaConstants.topicUnityFrankaJoints);
        }
        else
        {
            rosConnection.Unsubscribe(FrankaConstants.topicFrankaJoints);
        }
    }

    public void ActivateSliders()
    {
        foreach (var slider in sliders)
        {
            Image targetImage = slider.transform.Find(fillPath).GetComponent<Image>();
            if (targetImage != null)
            {
                targetImage.material.SetFloat("_MinDist", 0.4f);
            }
            if (sliderBackgroundMaterial != null)
            {
                sliderBackgroundMaterial.SetFloat("_MinDist", 0.4f);
            }
        }
    }

    public void DeactivateSliders()
    {
        foreach (var slider in sliders)
        {
            Image targetImage = slider.transform.Find(fillPath).GetComponent<Image>();
            if (targetImage != null)
            {
                targetImage.material.SetFloat("_MinDist", 0.6f);
            }
            if (sliderBackgroundMaterial != null)
            {
                sliderBackgroundMaterial.SetFloat("_MinDist", 0.6f);
            }
        }
    }

}
