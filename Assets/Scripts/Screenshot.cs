using UnityEngine;

public class ScreenshotTaker : MonoBehaviour
{
    // Define the screenshot file name and folder
    // Screenshots will be saved in the project's root folder in the "Screenshots" directory
    private string screenshotFolder = "Screenshots";
    private string screenshotFileName = "Screenshot";
    private int screenshotCount = 0;

    // Method to be called when the screenshot button is clicked
    public void TakeScreenshot()
    {
        // Ensure the directory exists
        System.IO.Directory.CreateDirectory(screenshotFolder);

        // Construct the full path for the new screenshot file
        string filePath = System.IO.Path.Combine(screenshotFolder, screenshotFileName + "_" + screenshotCount + ".png");

        // Capture the screenshot and save it to the file
        ScreenCapture.CaptureScreenshot(filePath);

        // Increment the screenshot counter for next screenshot's filename
        screenshotCount++;

        Debug.Log("Screenshot saved to: " + filePath);
    }
}
