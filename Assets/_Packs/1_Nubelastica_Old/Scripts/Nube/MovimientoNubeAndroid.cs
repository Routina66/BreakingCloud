/**********************************************************
 * Mueve la nube a la coordenada x del toque en la pantalla,
 * mantiene constante la coordenada y de la nube.
 * *****************************************************************/
using UnityEngine;

public class MovimientoNubeAndroid : MonoBehaviour {
    //Toque del dedo en la pantalla
    //private Touch toque;
    
    /**
     * Update is called once per frame
     * */
    private void Update() {
        MoverPersonaje();
    }

    /**
	 * Genera el movimiento del personaje.
	 */
    private void MoverPersonaje() {
        //toque = Input.GetTouch(0);

        transform.position = new Vector2(Input.GetTouch(0).position.x, transform.position.y);
    }
}
