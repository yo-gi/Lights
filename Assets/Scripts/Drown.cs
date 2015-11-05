using UnityEngine;

public class Drown : MonoBehaviour {

	public float timeToDrown = 5;

	private float drowningStartTime;

	public void Start() {
		this.drowningStartTime = 0;
	}

	public void Update() {
		if (Player.S.color == LightColor.Blue || this.CurrentWaterTile == null) {
			this.drowningStartTime = 0;
			return;
		}

		if (this.drowningStartTime == 0) {
			// TODO: Raise light intensity if it isn't at max.
			if (this.IsSubmergedInWater()) {
				this.drowningStartTime = Time.time;
			}
		}
		else {
			if (this.IsSubmergedInWater() == false) {
				Debug.Log("Not drowning no more.");

				// TODO: Start increasing light intensity.
				this.drowningStartTime = 0;
			}
		}

		if (this.drowningStartTime != 0) {
			// TODO: Decrease light intensity.
			var drownTime = (Time.time - this.drowningStartTime);

			if (drownTime >= this.timeToDrown) {
				this.drowningStartTime = 0;

				// TODO: Die.
				Debug.Log("Drowned");
				Door.switchLevels(MainCam.level == 1 ? 5 : MainCam.level - 1);
			}
		}
    }

    private GameObject CurrentWaterTile
    {
        get
        {
            return Player.S.water;
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
