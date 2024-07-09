using UnityEngine;

public class ReboteNube : MonoBehaviour {
    private Animator nubeAnimator;


    // Use this for initialization
    void Start()
    {
        nubeAnimator = GetComponent<Animator>();
    }
    /**
	 * Avisa cuando otro gameObject colisiona con este.
	 */
    void OnCollisionEnter2D(Collision2D otroCollider) {
        nubeAnimator.SetBool("bote", true);
		//Debug.Log("El GameObject " + otroCollider.collider.name + " ha colisionado con " + transform.name);
	}
	
	/**
	 * Avisa cuando otro gameObject continúa colisioando con este.
	 */
	void OnCollisionStay2D(Collision2D otroCollider) {
        nubeAnimator.SetBool("bote", true);
        //Debug.Log("El GameObject " + otroCollider.collider.name + " continua colisionando con " + transform.name);
    }
	
	/**
	 *	Avisa cuando otro gameObject deja de colisionar con este.
	 **/
	void OnCollisionExit2D(Collision2D otroCollider) {
        nubeAnimator.SetBool("bote", false);
        //Debug.Log("El GameObject " + otroCollider.collider.name + " ha dejado de colisionar con " + transform.name);
	}
}
