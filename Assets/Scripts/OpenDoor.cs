using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour {
    public TorchGroup requiredGroup;
    public float openSpeed;
    public Vector3 endPos;

    // Use this for initialization
    void Start () {
        endPos += transform.position - transform.localPosition;
        Events.Register<OnTorchGroupLitEvent>(e => {
            if (e.group == requiredGroup) StartOpening();
        });
	}
	
	private void StartOpening()
    {
        StartCoroutine(Open());
    }

    IEnumerator Open()
    {
        Vector3 startPos = transform.position;
        Vector3 travelVec = endPos - startPos;
        Vector3 direction = travelVec.normalized;
        float targetDistance = travelVec.magnitude;
        float time = Time.time;
        yield return null;
        while((transform.position - startPos).magnitude < targetDistance)
        {
            float delta = Time.time - time;
            time = Time.time;
            transform.position += direction * openSpeed * delta;
            yield return null;
        }
        Destroy(gameObject);
    }
}
