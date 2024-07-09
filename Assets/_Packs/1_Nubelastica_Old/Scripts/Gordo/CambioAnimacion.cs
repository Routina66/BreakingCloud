using UnityEngine;

public class CambioAnimacion : MonoBehaviour {
    private const int cero = 0;

    //Animaciones a las que se cambia.
    public string animRun = "Run_02";
    public string animStill = "Still_01";
    public string animIdle = "Idle_01";

    //Componentes del GameObject.
    private Animator animatorGordo;
    private Rigidbody2D rigidGordo;

	// Use this for initialization
	void Start () {
        animatorGordo = GetComponent<Animator>();
        rigidGordo = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        CambiarASalto();
	}

    /**
     * Activa el trigger para que gordo cambie
     * a la animación de salto.
     **/
     private void CambiarASalto()
    {
        //Cambia a parado
        if (rigidGordo.velocity.y == cero) {
            animatorGordo.SetTrigger(animIdle);
        }
        //Cambia a saltar
        if (rigidGordo.velocity.y > cero) {
            animatorGordo.SetTrigger(animRun);
        }
        //Cambia a still
        else if (rigidGordo.velocity.y < cero) {
            animatorGordo.SetTrigger(animStill);
        }
    }
}
