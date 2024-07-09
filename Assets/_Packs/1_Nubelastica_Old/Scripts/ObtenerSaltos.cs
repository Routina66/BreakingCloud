/*****************************************************************
 * Obtiene los saltos conseguidos al final del juego y
 * los muestra en el componente texto.
 * *************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class ObtenerSaltos : MonoBehaviour {
    //Objeto que tiene el número de saltos
    private GameObject contadorSaltos;
    //Nombre del objeto que tiene el número de saltos
    public string nameSaltos = "";

	// Use this for initialization
	void Start () {
        if (!nameSaltos.Equals("")) {
            contadorSaltos = GameObject.Find(nameSaltos);
            if (contadorSaltos != null) {
                GetComponent<Text>().text = contadorSaltos.GetComponent<Text>().text;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    /**
     * Devuelve el número de saltos conseguidos.
     * */
    public int GetSaltos() {
        return System.Convert.ToInt32(GetComponent<Text>().text);
    }

    /**
     * Actualiza el record a los puntos obtenidos
     * */
    public void GuardarRecord() {
        //Nombre de quien ha conseguido el record
        string nombreRecord = GameObject.Find("NombreRecord").GetComponent<Text>().text;

        PlayerPrefs.SetInt("record", GetSaltos());
        PlayerPrefs.SetString("Nombre", nombreRecord);
    }
}
