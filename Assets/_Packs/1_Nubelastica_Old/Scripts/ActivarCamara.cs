/************************************************
 * Activa el componente Camera al inicarse
 * ***************************************************/
using UnityEngine;

public class ActivarCamara : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Camera>().enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
