using UnityEngine;
using System.Collections;

public class Clouds : MonoBehaviour
{
    public float Speed;
    public float BorderX;
    private Transform _myTransform;
	// Use this for initialization
	void OnEnable ()
	{
	    _myTransform = transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _myTransform.localPosition += new Vector3(Speed*Time.deltaTime, 0, 0);
	    if (_myTransform.localPosition.x > BorderX)
	    {
            _myTransform.localPosition = new Vector3(-12, 3.8f, 18);
	    }
	}
}
