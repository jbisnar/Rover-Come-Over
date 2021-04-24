using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ping : MonoBehaviour
{
    public SpriteRenderer inner;
    public Transform innertrans;
    public SpriteRenderer outer;
    public Transform outertrans;
    public Transform mask;
    float lifetime = 1f;
    float faderate = 1f;
    float curalpha = 1f;
    float pointShrinkRate = 1f;
    float pointCurSize = 1f;
    float ringGrowRate = .5f;
    float maskGrowRate = 3f;
    float ringCurSize = 1f;
    float maskCurSize = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Delete", lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        inner.color = new Color(1, 1, 1, curalpha);
        outer.color = new Color(1, 1, 1, curalpha);
        curalpha -= faderate * Time.deltaTime;
        innertrans.localScale = new Vector3(pointCurSize, pointCurSize, pointCurSize);
        pointCurSize -= pointShrinkRate * Time.deltaTime;
        outertrans.localScale = new Vector3(ringCurSize, ringCurSize, ringCurSize);
        ringCurSize += ringGrowRate * Time.deltaTime;
        mask.localScale = new Vector3(maskCurSize, maskCurSize, maskCurSize);
        maskCurSize += maskGrowRate * Time.deltaTime;
    }

    IEnumerator Delete(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(gameObject);
    }
}
