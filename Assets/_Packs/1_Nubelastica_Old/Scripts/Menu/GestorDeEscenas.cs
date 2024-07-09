/********************************************
 * Gestion los cambios entre escenas.
 * *************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GestorDeEscenas : MonoBehaviour {
    //Indica si se debe cargar una escena al inicializar el GameObject
    public bool cargarAlInicio = false;
    //Índice, en Scenes in build, de la escena que se cargará al inicio, si procede
    public int escena = 0;
    //Modo (single o additive) en el que se carga la escena al inicio, 
    public bool single = true;
    

    // Use this for initialization
    void Start () {
        if (cargarAlInicio) {
            cargarAlInicio = false;
            if (single) {
                CargarEscenaSimple(escena);
            }
            else {
                CargarEscenaAdditive(escena);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
     * Carga la escena con la posición i en Scenes in Build
     * en modo simple
     * */
     public void CargarEscenaSimple(int i) {
        SceneManager.LoadScene(i, LoadSceneMode.Single);
    }

    /**
     * Carga la escena con la posición i en Scenes in Build
     * en modo aditvo
     * */
    public void CargarEscenaAdditive(int i) {
        SceneManager.LoadScene(i, LoadSceneMode.Additive);
    }

    /**
     * Sale del juego y manda a analitics los puntos obtenidos.
     * */
     public void Salir() {
        int puntosObtenidos = 100;

        Analytics.CustomEvent("FIN DE JUEGO", new Dictionary<string, object> {
            {"puntos", puntosObtenidos }
        });

        Application.Quit();
    }

    /**
     * Manda a analitics los puntos obtenidos.
     * */
     public void enviarAnaliticas() {
        int puntosObtenidos = Int32.Parse(GameObject.Find("NumSaltos").GetComponent<Text>().text);

        Analytics.CustomEvent("FIN DE JUEGO", new Dictionary<string, object> {
            {"puntos", puntosObtenidos }
        });

        Debug.Log("Fin de jugego. Puntos: " + puntosObtenidos);
     }
}
