/*******************************************************
 * Hace que el GameObject no pueda salir de los limites
 * establecidos.
 * ******************************************************/
using UnityEngine;

public class LimieDeMovimiento : MonoBehaviour {
    //Límites de movimiento
    public float limiteXIzqdo = 0;
    public float limiteXDcho = 0;
    public float limiteYSup = 0;
    public float limiteYInf = 0;
    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        ComprobarLimites();
	}

    /**
     * Evita que el GameObject salga de los límites
     * */
     private void ComprobarLimites() {
        if (transform.position.x < limiteXIzqdo) {
            transform.Translate(Vector3.right);
        }
        if (transform.position.x > limiteXDcho) {
            transform.Translate(Vector3.left);
        }
        if (transform.position.y > limiteYSup) {
            transform.Translate(Vector3.down);
        }
        if (transform.position.y < limiteYInf) {
            transform.Translate(Vector3.up);
        }
    }

}
