using System.Collections.Generic;
using UnityEngine;

public class CategoryManager : MonoBehaviour
{
    public GameObject spherePrefab;
    public Material transparentBlue;
    public Material transparentRed;
    public Transform spawnPoint;

    public GameObject lockToggle;

    private List<GameObject> redSpheres = new List<GameObject>();
    private List<GameObject> blueSpheres = new List<GameObject>();

    public void SpawnBlueSphere()
    {
        GameObject sphere = Instantiate(spherePrefab, spawnPoint.position, Quaternion.identity);
        sphere.GetComponent<Renderer>().material = transparentBlue;
        blueSpheres.Add(sphere); // Track blue sphere
    }

    public void SpawnRedSphere()
    {
        GameObject sphere = Instantiate(spherePrefab, spawnPoint.position, Quaternion.identity);
        sphere.GetComponent<Renderer>().material = transparentRed;
        redSpheres.Add(sphere); // Track red sphere
    }

    public void DeleteLastRedSphere()
    {
        if (redSpheres.Count > 0)
        {
            GameObject toDelete = redSpheres[redSpheres.Count - 1];
            redSpheres.RemoveAt(redSpheres.Count - 1);
            Destroy(toDelete);
        }
    }

    public void DeleteLastBlueSphere()
    {
        if (blueSpheres.Count > 0)
        {
            GameObject toDelete = blueSpheres[blueSpheres.Count - 1];
            blueSpheres.RemoveAt(blueSpheres.Count - 1);
            Destroy(toDelete);
        }
    }

    public void ClearAllRedSpheres()
    {
        foreach (GameObject sphere in redSpheres)
        {
            Destroy(sphere);
        }
        redSpheres.Clear(); // Clear the list after destroying all spheres
    }

    public void ClearAllBlueSpheres()
    {
        foreach (GameObject sphere in blueSpheres)
        {
            Destroy(sphere);
        }
        blueSpheres.Clear(); // Clear the list after destroying all spheres
    }

    public void ClearAllSpheres()
    {
        ClearAllRedSpheres();
        ClearAllBlueSpheres();
    }

    public void toggleCategoryLock()
    {
        if (lockToggle != null)
        {
            ToggleImage toggleImage = lockToggle.GetComponent<ToggleImage>();
            if (toggleImage.Image1isActive())
            {
                SetCollidersEnabled(false);
            }
            else
            {
                SetCollidersEnabled(true);
            }
            toggleImage.SwitchToggleImage();
        }
    }

    public void SetCollidersEnabled(bool enabled)
    {
        foreach (var sphere in blueSpheres)
        {
            // Assuming the collider is directly attached to the sphere GameObject
            var collider = sphere.GetComponent<SphereCollider>();
            if (collider != null)
            {
                collider.enabled = enabled;
            }
        }
        foreach (var sphere in redSpheres)
        {
            // Assuming the collider is directly attached to the sphere GameObject
            var collider = sphere.GetComponent<SphereCollider>();
            if (collider != null)
            {
                collider.enabled = enabled;
            }
        }
    }

}
