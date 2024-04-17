using UnityEngine;
using UnityEditor;

public class TagFranka : MonoBehaviour
{
    [MenuItem("GameObject/Tag Franka", false, 10)]
    static void TagWithChildren(MenuCommand menuCommand)
    {
        GameObject parentObject = menuCommand.context as GameObject;
        string tagToApply = "Franka"; // Set this to your desired tag

        if (parentObject != null)
        {
            SetTagRecursive(parentObject, tagToApply);
        }
    }

    static void SetTagRecursive(GameObject obj, string tag)
    {
        obj.tag = tag;
        foreach (Transform child in obj.transform)
        {
            SetTagRecursive(child.gameObject, tag);
        }
    }
}
