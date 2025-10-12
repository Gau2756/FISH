using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    private Transform bg1, bg2;
    public int pixelsPerFrame = 20;
    // Using the fact that img size = res / 100
    int xRes = 1920;
    private int effectiveScrollSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bg1 = transform.GetChild(0);
        bg2 = transform.GetChild(1);
        print(bg1 + "" + bg2);
    }

    // Update is called once per frame
    void Update()
    {
        int numFrames = (int)Mathf.Floor(xRes / pixelsPerFrame);
        float effectiveScrollSpeed = xRes / numFrames / 100f;
        print(effectiveScrollSpeed);

        bg1.position = new Vector3(bg1.position.x - effectiveScrollSpeed, bg1.position.y, bg1.position.z);
        bg2.position = new Vector3(bg2.position.x - effectiveScrollSpeed, bg2.position.y, bg2.position.z);

        if (bg1.position.x <= -19.2f)
        {
            bg1.position = new Vector3(19.2f, bg1.position.y, bg1.position.z);
        }
        if (bg2.position.x <= -19.2f)
        {
            bg2.position = new Vector3(19.2f, bg2.position.y, bg2.position.z);
        }
    }
}
