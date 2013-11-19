using UnityEngine;
using System.Collections;

public class PandaElectricution : MonoBehaviour {

    [SerializeField] GameObject electricityGObj;
    [SerializeField] GameObject pandaGObj;

    protected Vector3 initOffset = new Vector3(0, .8f, -0.1f);

	// Use this for initialization
	void Start () {
        Vector3 initPos = transform.position + initOffset;
        transform.position = initPos;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
