using System.Collections;
using UnityEngine;

public class Footprints : MonoBehaviour
{
    private SpriteRenderer render;
    public float visibility = 0f;
    private void Start()
    {
        UV.footprints.Add(this);
        gameObject.AddComponent<Interaction>().Initiate(2);
        render = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(Expire());
    }

    private void Update()
    {
        visibility = Mathf.Clamp01(visibility);
        render.color = new Color(render.color.r, render.color.g, render.color.b, visibility * 255f);
    }

    private IEnumerator Expire()
    {
        yield return new WaitForSeconds(5);
        UV.footprints.Remove(this);
        Destroy(this.gameObject);
    }


}