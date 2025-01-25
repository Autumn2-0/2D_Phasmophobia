using System.Collections;
using UnityEngine;

public class Footprints : MonoBehaviour
{
    public SpriteRenderer render;
    public float visibility = 0f;
    public float duration = 8f;
    public float depletionRate = 0.1f;
    public void Start()
    {
        UV.footprints.Add(this);
        render = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(Expire());
        InteractionMarking.Instantiate(gameObject, 2, duration);
    }

    public void Update()
    {
        visibility -= Time.deltaTime * depletionRate;
        visibility = Mathf.Clamp(visibility, 0f, 0.6f);
        render.color = new Color(render.color.r, render.color.g, render.color.b, visibility);
    }

    public IEnumerator Expire()
    {
        yield return new WaitForSeconds(duration);
        UV.footprints.Remove(this);
        Destroy(this.gameObject);
    }


}