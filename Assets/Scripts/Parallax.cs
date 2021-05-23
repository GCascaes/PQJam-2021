using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform[] backgrounds;

    private float[] parallaxScales;

    public float smoothing = 1f;

    private Transform camera;

    private Vector3 previousCamPos;


    private void Awake()
    {
        camera = Camera.main.transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        previousCamPos = camera.position;

        parallaxScales = new float[backgrounds.Length];

        for(int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i =0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - camera.position.x) * parallaxScales[i];

            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }

        previousCamPos = camera.position;
    }
}
