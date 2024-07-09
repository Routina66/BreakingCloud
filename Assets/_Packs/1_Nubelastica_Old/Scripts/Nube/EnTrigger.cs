/*****************************************************************
 * Comportamiento cuando un GameObject entra en el trigger:
 *     - Incrementa en 1 el númro de saltos.
 *     - Reproduce el sonido de audioSource
 * ************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class EnTrigger : MonoBehaviour {
    //Contador de saltos
    private ActualizarPuntos puntos;
    //Audio source del gameObject
    private AudioSource sonido;

    // Use this for initialization
    void Start()
    {
        puntos = GameObject.Find("NumSaltos").GetComponent<ActualizarPuntos>();
        sonido = GetComponent<AudioSource>();
    }

    /**
	 * Avisa de que otro gameObject ha entrado en el trigger.
	 */
    void OnTriggerEnter2D(Collider2D otroCollider) {
        //Incrementa la puntuacion
        puntos.IncPuntuacion();
        //Reproduce un sonido
        sonido.Play();
	}
}
