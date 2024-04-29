using UnityEngine;

public class WallsManager : MonoBehaviour
{
    private MeshRenderer[] wallRenderers;

    void Start()
    {
        wallRenderers = GetComponentsInChildren<MeshRenderer>();
        SetWallsVisibility(false);  // Start with all walls invisible
    }

    void SetWallsVisibility(bool visible)
    {
        foreach (var renderer in wallRenderers)
        {
            renderer.enabled = visible;
        }
    }

    // This will be called by child wall objects when they collide
    public void ChildCollisionEnter()
    {
        SetWallsVisibility(true);
    }

    // This will be called by child wall objects when they end collision
    public void ChildCollisionExit()
    {
        SetWallsVisibility(false);
    }
}
