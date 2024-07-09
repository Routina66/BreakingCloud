/***************************************************
 * Obtiene el record de saltos conseguido y lo muestra
 * en la pantalla.
 * ****************************************************/
using UnityEngine;
using UnityEngine.UI;

public class MostrarRecord : MonoBehaviour {
    //Record actual
    private int recordActual;
    //Texto que muestra el record
    private Text recordText;
    //Nombre de la variable que guarda el record
    private const string varRecord = "record";
    //Nos dice el número de saltos conseguidos
    //private int numSaltos;

	// Use this for initialization
	void Start () {
        recordText = GetComponent<Text>();
        recordActual = PlayerPrefs.GetInt(varRecord);
        recordText.text = recordActual.ToString() + "  " + PlayerPrefs.GetString("Nombre");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
