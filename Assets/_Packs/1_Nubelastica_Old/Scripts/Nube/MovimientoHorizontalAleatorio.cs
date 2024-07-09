/**********************************************************
 * El GameObject se mueve en el eje horinzontal
 * con una velocidad y una dirección aleatorias.
 * ********************************************************/
using UnityEngine;

public class MovimientoHorizontalAleatorio : MonoBehaviour {
    //RigidBody2D del GameObject
    private Rigidbody2D rigidBody;

	/**
     * Use this for initialization
     * */
	void Start () {
        rigidBody = GetComponent<Rigidbody2D>();
        moverHorizontal();
	}

    /**
     * Mueve al objeto en el eje horizontal.
     * */
     private void moverHorizontal() {;
        //Velocidad del objeto
        float velocidad = Random.Range(0.0f, 1.0f);
        //Si derecha = 1 se moverá hacia la derecha
        int derecha = Random.Range(0, 2);

        if (derecha == 1) {
            rigidBody.velocity = Vector2.right * velocidad;
        }
        else {
            rigidBody.velocity = Vector2.left * velocidad;
        }
    }
}
