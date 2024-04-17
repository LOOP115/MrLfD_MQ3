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