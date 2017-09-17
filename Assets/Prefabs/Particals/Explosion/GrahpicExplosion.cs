using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrahpicExplosion : MonoBehaviour {

    public float loopduration;
    private float rampTime = 0f;
    private float alphaTime = 1f;
    public float lifeTime = 1f;

    void Update () {
        rampTime += Time.deltaTime * 1.5f;
        rampTime = Mathf.Clamp(rampTime, 0, 1);
        alphaTime -= Time.deltaTime * 1.5f;
        alphaTime = Mathf.Clamp(alphaTime, 0, 1);

        float r = Mathf.Sin((Time.time / loopduration) * (2 * Mathf.PI)) * 0.5f + 0.25f;
        float g = Mathf.Sin((Time.time / loopduration + 0.33333333f) * 2 * Mathf.PI) * 0.5f + 0.25f;
        float b = Mathf.Sin((Time.time / loopduration + 0.66666667f) * 2 * Mathf.PI) * 0.5f + 0.25f;
        float correction = 1 / (r + g + b);
        r *= correction;
        g *= correction;
        b *= correction;
        transform.gameObject.GetComponent<Renderer>().material.SetVector("_ChannelFactor", new Vector4(r, g, b, 0));
        transform.gameObject.GetComponent<Renderer>().material.SetVector("_Range", new Vector4(rampTime, 0, 0, 0));
        transform.gameObject.GetComponent<Renderer>().material.SetFloat("_ClipRange", alphaTime);

        lifeTime -= Time.deltaTime;

        if(lifeTime <= 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
