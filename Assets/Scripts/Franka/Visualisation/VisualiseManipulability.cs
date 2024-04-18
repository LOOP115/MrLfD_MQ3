using UnityEngine;
using RosMessageTypes.CtrlInterfaces;
using UnityEngine.UI;
using Unity.Robotics.ROSTCPConnector;


public class VisualiseManipulability : MonoBehaviour
{
    private RosConnector rosConnector;

    private GameObject slider;
    private const string sliderPath = "world/VisualManip/Slider";
    private const string fillPath = "Fill Area/Fill";
    private const string backgroundPath = "OuterBorder";

    private Material sliderFillMaterial;
    private Material sliderBackgroundMaterial;


    void Start()
    {
        rosConnector = FindObjectOfType<RosConnector>();

        slider = transform.Find(sliderPath).gameObject;

        InitializeSlider(slider);

        Subscribe();
    }


    private void InitializeSlider(GameObject slider)
    {
        // Create a new material instance for this slider
        sliderBackgroundMaterial = slider.transform.Find(backgroundPath).GetComponent<Image>().material;
        sliderBackgroundMaterial.SetFloat("_MinDist", 1.0f);
        
        sliderFillMaterial = slider.transform.Find(fillPath).GetComponent<Image>().material;
        sliderFillMaterial.SetFloat("_MinDist", 1.0f);
        
        Slider sliderComponent = slider.GetComponent<Slider>();
        // Set the initial value based color
        UpdateSliderColor(sliderFillMaterial, sliderComponent.value);

        // Subscribe to the slider's value changed event
        sliderComponent.onValueChanged.AddListener((value) => {
            UpdateSliderColor(sliderFillMaterial, value);
        });
    }

    private void UpdateSliderColor(Material material, float value)
    {
        // Update the material's shader property to reflect the slider's current value
        material.SetFloat("_SliderValue", value);
    }


    private void UpdateSlider(ManipulabilityMsg manip)
    {
        // Update the slider values based on the incoming message
        slider.GetComponent<Slider>().value = (float)manip.value * 10;
    }

    public void Subscribe()
    {
        rosConnector.GetBridge().Subscribe<ManipulabilityMsg>(FrankaConstants.topicManipulability, UpdateSlider);
    }
    
    public void Unsubscribe()
    {
        rosConnector.GetBridge().Unsubscribe(FrankaConstants.topicManipulability);
    }

    public void ActivateSliders()
    {
        sliderBackgroundMaterial.SetFloat("_MinDist", 0.0f);
        sliderFillMaterial.SetFloat("_MinDist", 0.0f);
    }

    public void DeactivateSliders()
    {
        sliderBackgroundMaterial.SetFloat("_MinDist", 1.0f);
        sliderFillMaterial.SetFloat("_MinDist", 1.0f);
    }

}
