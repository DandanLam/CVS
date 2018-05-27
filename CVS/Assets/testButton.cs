using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testButton : MonoBehaviour {

    [SerializeField]
    private Animator myAnim;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == StringConstants.playerTag)
        {
            myAnim.SetBool("Pushed", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == StringConstants.playerTag)
        {
            myAnim.SetBool("Pushed", false);
        }

    }
}
