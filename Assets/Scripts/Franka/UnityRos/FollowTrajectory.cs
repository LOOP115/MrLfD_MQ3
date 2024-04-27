using UnityEngine;

public class FollowTrajectory : MonoBehaviour
{

    public GameObject frankaIKPrefab;

    private GameObject franakIK;


    public void SpawnFrankaIK()
    {
        if (frankaIKPrefab != null)
        {
            franakIK = Instantiate(frankaIKPrefab, transform.position, transform.rotation);
            franakIK.transform.SetParent(transform);
            // if (franakIK != null && franakIK.GetComponent<InvisibleFranka>() != null)
            // {
            //     franakIK.GetComponent<InvisibleFranka>().SetVisibility(false);
            // }
        }
        else
        {
            Debug.LogError("FrankaIK prefab is not assigned.");
        }
    }

    public void RemoveFrankaIK()
    {
        if (franakIK != null)
        {
            Destroy(franakIK);
        }
    }

}