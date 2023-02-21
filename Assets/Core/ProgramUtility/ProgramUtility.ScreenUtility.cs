using UnityEngine;

public static partial class ProgramUtility
{
    public static void AdjustView(Camera mainCam)
    {
        // calculate aspect ratio
        float aspectRatio = (float)Screen.width / Screen.height;
        
        // cache maincam rect
        Rect mainCamRect = mainCam.rect;
        
        // rescale
        Vector2 screenDimensions = new Vector2(Screen.width, Screen.height);
        Vector2 scale = aspectRatio > 1
                ? new Vector2(
                    screenDimensions.x * mainCamRect.width * (1/aspectRatio), 
                    screenDimensions.y * mainCamRect.height)
                : new Vector2(
                    screenDimensions.x * mainCamRect.width, 
                    screenDimensions.y * mainCamRect.height * aspectRatio)
            ;

        // reposition
        Vector2 pos = new Vector2(
            (mainCamRect.x + mainCamRect.width / 2) * Screen.width,
            (mainCamRect.y + mainCamRect.height / 2) * Screen.height
        );
        pos.x -= scale.x / 2;
        pos.y -= scale.y / 2;

        // update maincam pixel rect
        mainCam.pixelRect = new Rect(pos.x, pos.y, scale.x, scale.y);
    }

    public static Vector4 GetScreenBounds(Camera mainCam)
    {
        // (x min, x max, y min, y max)
        Vector4 bounds = new Vector4();

        Rect mainCamRect = mainCam.rect;

        bounds.x = Screen.width * mainCamRect.x;
        bounds.y = bounds.x + Screen.width * mainCamRect.width;
        bounds.z = Screen.height * mainCamRect.y;
        bounds.w = bounds.z + Screen.height * mainCamRect.height;

        return bounds;
    }

    public static bool CheckBounds(Camera mainCam)
    {
        Vector4 bounds = GetScreenBounds(mainCam);

        return
            Input.mousePosition.x > bounds.x &&
            Input.mousePosition.x < bounds.y &&
            Input.mousePosition.y > bounds.z &&
            Input.mousePosition.y < bounds.w;
    }
}
