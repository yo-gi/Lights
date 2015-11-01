using UnityEngine;

public class Drown : MonoBehaviour {

	public float timeToDrown = 2;

	private float drowningStartTime;

	private GameObject CurrentWaterTile {
		get {
			// TODO: Get current water tile.
			return null;
		}
	}

	public void Start() {
		this.drowningStartTime = 0;
	}

	public void Update() {
		if (this.drowningStartTime == 0) {
			// TODO: Raise light intensity if it isn't at max.
			if (this.IsSubmergedInWater()) {
				this.drowningStartTime = Time.time;
			}
		}
		else {
			if (this.IsSubmergedInWater() == false) {
				// TODO: Start increasing light intensity.
				this.drowningStartTime = 0;
			}
		}

		if (this.drowningStartTime != 0) {
			// TODO: Decrease light intensity.
			var drownTime = (Time.time - this.drowningStartTime);

			if (drownTime >= this.timeToDrown) {
				// TODO: Die.
				Debug.Log("Drowned coach");
			}
		}
	}

	private bool IsSubmergedInWater() {
		var objectHeight = this.GetHeightFromTransform(this.transform);
		var waterHeight = this.GetHeightFromTransform(this.CurrentWaterTile.transform);

		return objectHeight < waterHeight;
	}

	private float GetHeightFromTransform(Transform transform) {
		return transform.position.y + transform.lossyScale.y / 2;
	}
}
