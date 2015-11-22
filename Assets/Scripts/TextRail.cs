using UnityEngine;
using System.Collections.Generic;

public class TextRail : MonoBehaviour {

    public bool visible = false;
    public int currentSegment = 0;

    public GameObject text;
    public List<Vector3> path = new List<Vector3>();

    void Awake() {
        text = gameObject.transform.Find("Text").gameObject;

        foreach (Transform tranform in gameObject.transform.Find("Path")) {
            path.Add(tranform.position);
        }

        enabled = false;
        text.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        bool hasReachedEndOfRail = currentSegment == path.Count;
        bool colliderIsPlayer = other.gameObject == Player.S.gameObject;

        if (hasReachedEndOfRail == false && colliderIsPlayer) {
            enabled = true;

            if (visible == false) {
                visible = true;
                text.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == Player.S.gameObject) {
            enabled = false;
        }
    }

    void Update() {
        var current = path[currentSegment];
        var next = path[currentSegment + 1];

        bool rightWards = true;
        var leftBound = current.x;
        var rightBound = next.x;
        var playerPos = Player.S.transform.position;

        if (rightBound < leftBound) {
            var temp = leftBound;

            rightWards = false;
            leftBound = rightBound;
            rightBound = temp;
        }

        // Don't do anything if the player isn't within the bounds of the path.
        if (playerPos.x < leftBound || playerPos.x > rightBound) {
            return;
        }

        var position = transform.position;

        bool playerIsToRight = (position.x <= playerPos.x);
        bool shouldUpdatePos = (rightWards == playerIsToRight);

        if (shouldUpdatePos) {
            // TODO: Update position.y as well. We'll need to calculate the y
            // value from the two bounds.
            position.x = playerPos.x;

            transform.position = position;
        }
    }
}
