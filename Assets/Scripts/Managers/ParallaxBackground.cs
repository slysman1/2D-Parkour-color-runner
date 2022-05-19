using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length, startPos;
    private GameObject cam;

    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");

        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    // Update is called once per frame
    void Update()
    {
        float temp = (cam.transform.position.x) * (1 - parallaxEffect); // how far we moved relativly to camera

        float distance = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (temp > startPos + length)
        {
            startPos = startPos + length;
        }
        else if (temp < startPos - length)
        {
            startPos = startPos - length;
        }

    }
}
