using UnityEngine;
using System.Collections;

public class Clouds : MonoBehaviour
{
    public float Speed;
    public float BorderX;
    private Transform _myTransform;
    public int Layer;
    public float StartHeight;
	// Use this for initialization
	void OnEnable ()
	{
	    _myTransform = transform;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _myTransform.localPosition += new Vector3(Speed*Time.deltaTime, 0, 0);
	    if (Speed > 0)
	    {
	        if (_myTransform.localPosition.x > BorderX)
	        {
                _myTransform.localPosition = new Vector3(-12, StartHeight, Layer);
	        }
	    }
	    else
	    {
	        if (_myTransform.localPosition.x < BorderX)
	        {
                _myTransform.localPosition = new Vector3(12, StartHeight, Layer);
	        }
	    }
	}
}
