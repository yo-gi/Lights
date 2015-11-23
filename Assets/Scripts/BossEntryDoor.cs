using UnityEngine;

public class BossEntryDoor : MonoBehaviour {
	
	public void Awake()
	{
		Events.Register<OnTorchGroupLitEvent>((e) => {
			if(e.group == TorchGroup.AlphaLevel)
			{
				Destroy(gameObject);
			}
		});
	}
}
