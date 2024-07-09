/*********************************
 * Actualiza el marcador de puntos.
 * *************************************/
using UnityEngine;
using UnityEngine.UI;

public class ActualizarPuntos : MonoBehaviour {
    //Puntos obtenidos
    private int puntos;
    //Texto que muestra los puntos
    private Text texto;
    //Record de puntos obtenidos
    private int recordPuntos;
    //Nombre con el que se guarda record
    private const string record = "record";
    
    // Use this for initialization
	void Start () {
        puntos = 0;
        texto = GetComponent<Text>();
        recordPuntos = GetRecord();

	}

    /**
     * Incrementa en uno la puntuación y 
     * muestra el resultado en la pantalla.
     * */
    public void IncPuntuacion() {
        puntos++;
        texto.text = puntos.ToString();
    }

    /**
     * Devuelve true si se ha superdo el record
     * */
     public bool RecordSuperado() {
        if (puntos > recordPuntos) {
            return true;
        }
        else {
            return false;
        }
    }

    /**
    * Devuelve el record de puntos. Si aún no
    * hay record, pone el record a cero
    * */
    private int GetRecord() {
        if (!PlayerPrefs.HasKey(record)) {
            PlayerPrefs.SetInt(record, 0);
        }

        return PlayerPrefs.GetInt(record);
    }   
}
