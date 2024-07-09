/***************************************************************
 * El GameObject se activa si se ha superado el record.
 * *************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class AutoActivadoSiGana : MonoBehaviour {
    //Saltos conseguidos
    private int saltos;
    //Record Actual
    private int record;

	/**
     * Use this for initialization.
     * Si se ha superado el record, se activa el objeto.
     * **/
	void Start () {
        GameObject.Find("FondoText").GetComponent<SpriteRenderer>().enabled = false;
        GameObject.Find("CanvasWin").GetComponent<Canvas>().enabled = false;
        if (GameObject.Find("NumSaltos").GetComponent<ActualizarPuntos>().RecordSuperado()) {
            Activar();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
     * Se activa el objeto.
     * */
     private void Activar() {
        GameObject.Find("CanvasWin").GetComponent<Canvas>().enabled = true;
        GameObject.Find("FondoText").GetComponent<SpriteRenderer>().enabled = true;
    }
}
