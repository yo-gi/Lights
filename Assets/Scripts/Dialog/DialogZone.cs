using UnityEngine;

public class DialogZone : MonoBehaviour {

    public static DialogZone currentZone = null;

    /// 
    /// The speech Aura will say once the dialog zone has been triggered.
    /// 
    public string dialog;

    /// 
    /// If true this dialog will can only be triggered once.
    /// 
    public bool showOnce = false;

    /// 
    /// If true this dialog will override whatever Aura is currently saying.
    /// 
    public bool overrideDialog = false;

    /// 
    /// The minimum amount of time that this dialog will be displayed for.
    /// 
    public float minSeconds = 2f;

    public virtual void Awake() {
        this.dialog = this.dialog.Replace("\\n", "\n");
    }

    public virtual void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == Player.S.gameObject) {
            Navi.S.dialog.Queue(new Speech {
                dialog = this.dialog,
                duration = this.minSeconds
            }, force: this.overrideDialog);

            if (this.showOnce) {
                Destroy(this.gameObject);
            }
        }
    }
}
