/***************************************************************************
 * Cambia de escena cuando un game object sale del trigger
 * ***************************************************************************/
using UnityEngine;

public class CambioDeEscenaAlSalirDeTrigger : MonoBehaviour {
    //Posición en Scenes in build de la escena que se carga
    public int posScene = 0;
    //Gerstor de escenas del objeto
    private GestorDeEscenas gestorEscenas;
    //Cámara de la escena actual
    private Camera camara;

	// Use this for initialization
	void Start () {
        gestorEscenas = GetComponent<GestorDeEscenas>();
        camara = GameObject.FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    /**
     * Cambia la escena cuando un GameObject sale del trigger.
     * */
    private void OnTriggerExit2D(Collider2D collision) {
        camara.enabled = false;
        gestorEscenas.CargarEscenaAdditive(posScene);
    }
}
