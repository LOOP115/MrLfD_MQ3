using UnityEngine;


public class InvisibleFranka : MonoBehaviour
{
    
    private GameObject[] visuals = new GameObject[FrankaConstants.NumParts];
    private const string visual = "/Visuals";

    void Start()
    {
        // Get visuals
        var linkName = string.Empty;
        for (var i = 0; i < FrankaConstants.NumParts; i++)
        {
            if (i < FrankaConstants.LinkNamesAll.Length)
            {
                linkName += FrankaConstants.LinkNamesAll[i];
                var visualName = linkName + visual;
                visuals[i] = transform.Find(visualName).gameObject;
            }
            else if (i == FrankaConstants.NumParts - 3)
            {
                var visualName = FrankaConstants.HandTCPName + visual;
                visuals[i] = transform.Find(visualName).gameObject;
            }
            else if (i == FrankaConstants.NumParts - 2)
            {
                var visualName = FrankaConstants.FingerName[0] + visual;
                visuals[i] = transform.Find(visualName).gameObject;
            }
            else if (i == FrankaConstants.NumParts - 1)
            {
                var visualName = FrankaConstants.FingerName[1] + visual;
                visuals[i] = transform.Find(visualName).gameObject;
            }
        }
    }

    public void SetVisibility(bool visible)
    {
        foreach (var visual in visuals)
        {
            visual.SetActive(visible);
        }
    }

}
