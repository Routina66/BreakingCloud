/***********************************************************************
 * Mueve al personaje hacia los lados con la flechas del teclado.
 * *********************************************************************/
using UnityEngine;

public class MovimientoNube : MonoBehaviour {
	//Componentes	
	private Rigidbody2D componenteRigidbody;
		
	//Controles del personaje
	private KeyCode derecha = KeyCode.RightArrow;
	private KeyCode izquierda = KeyCode.LeftArrow;
	
	//velocidad
	public float speed = 1;

	// Use this for initialization
	void Start () {
		componenteRigidbody = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    private void Update () { 
		Mover();
	}
	
	/**
	 * Genera el movimiento del personaje.
	 */
	 private void Mover() {
		 if (Input.GetKeyDown(izquierda) || Input.GetKey(izquierda)) {
			componenteRigidbody.velocity = Vector2.left * speed;
			//Debug.Log("Corriendo");			
		}
		
		if (Input.GetKeyDown(derecha) || Input.GetKey(derecha)) {
			componenteRigidbody.velocity = Vector2.right * speed;
		}
		
		if (Input.GetKeyUp(izquierda) || Input.GetKeyUp(derecha)) {
            componenteRigidbody.velocity = Vector2.zero;
            //Debug.Log("Parado");
        }
	 }
	 
	 
}
