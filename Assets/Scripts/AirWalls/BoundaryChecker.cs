using UnityEngine;

public class BoundaryChecker : MonoBehaviour
{
    public GameObject target;

    public GameObject upWall;
    public GameObject bottomWall;
    public GameObject leftWall;
    public GameObject rightWall;
    public GameObject frontWall;
    public GameObject backWall;

    private MeshRenderer UpWallRenderer;
    private MeshRenderer BottomWallRenderer;
    private MeshRenderer LeftWallRenderer;
    private MeshRenderer RightWallRenderer;
    private MeshRenderer FrontWallRenderer;
    private MeshRenderer BackWallRenderer;

    private MeshRenderer[] wallRenderers;


    void Start()
    {
        UpWallRenderer = upWall.GetComponent<MeshRenderer>();
        BottomWallRenderer = bottomWall.GetComponent<MeshRenderer>();
        LeftWallRenderer = leftWall.GetComponent<MeshRenderer>();
        RightWallRenderer = rightWall.GetComponent<MeshRenderer>();
        FrontWallRenderer = frontWall.GetComponent<MeshRenderer>();
        BackWallRenderer = backWall.GetComponent<MeshRenderer>();
        
        wallRenderers = new MeshRenderer[] {UpWallRenderer, BottomWallRenderer, LeftWallRenderer, RightWallRenderer, FrontWallRenderer, BackWallRenderer};
        SetWallsVisibility(false);  // Start with all walls invisible
    }

    void FixedUpdate()
    {
        CheckBoudaries();
    }

    private void CheckBoudaries()
    {
        Vector3 pos = target.transform.localPosition;
        // Check each boundary
        if (pos.y < upWall.transform.localPosition.y) {
            UpWallRenderer.enabled = true;
            return;
        }
        
        if (pos.y > bottomWall.transform.localPosition.y) {
            BottomWallRenderer.enabled = true;
            return;
        }
        
        if (pos.x < leftWall.transform.localPosition.x) {
            LeftWallRenderer.enabled = true;
            return;
        }
        
        if (pos.x > rightWall.transform.localPosition.x) {
            RightWallRenderer.enabled = true;
            return;
        }
        
        if (pos.z > frontWall.transform.localPosition.z) {
            FrontWallRenderer.enabled = true;
            return;
        }
        
        if (pos.z < backWall.transform.localPosition.z) {
            BackWallRenderer.enabled = true;
            return;
        }
        
        SetWallsVisibility(false);
    }

    private void SetWallsVisibility(bool visible)
    {
        foreach (var renderer in wallRenderers)
        {
            renderer.enabled = visible;
        }
    }

}
