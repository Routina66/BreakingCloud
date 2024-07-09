/***********************************************************
 * Reproduce un sonido cuando hay una colisión
 * **********************************************************/
using UnityEngine;

public class PlayOnCollision : MonoBehaviour {
    //Audio source del GameObject
    private AudioSource sonido;

    // Use this for initialization
    void Start() {
        sonido = GetComponent<AudioSource>();
    }

    /**
     * Reproduce el sondido en la colisión
     * */
    private void OnCollisionEnter2D(Collision2D collision) {
        sonido.Play();
    }
}
