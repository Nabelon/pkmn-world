using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	 
    public Transform target;
float distance = 30.0f;
 
float xSpeed = 250.0f;
float ySpeed = 120.0f;
 
 
private float x = 0.0f;
private float y = 0.0f;
public Quaternion rotation = Quaternion.AngleAxis(-30.0f, Vector3.left);
int xsign =1;
 
 
void Start () {
    var angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;
   
    var rotation = Quaternion.Euler(y, x, 0);
    var position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
       
        transform.rotation = rotation;
        transform.position = position;
 
   
}
 
void LateUpdate () {
    if (target == null)
    {
        target = Player.player.transform;
    }
    var position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
    position.y = Mathf.Max(position.y, 1);
    transform.position = position;
    transform.rotation = rotation;
   
    //get the rotationsigns
   
    var forward = transform.TransformDirection(Vector3.up);
    var forward2 = target.transform.TransformDirection(Vector3.up);
     if (Vector3.Dot(forward,forward2) < 0)
            xsign = -1;
            else
            xsign =1;
   
   
    foreach (Touch touch  in Input.touches) {
    if (touch.phase == TouchPhase.Moved) {
        x += xsign * touch.deltaPosition.x * xSpeed *0.02f;
        y -= touch.deltaPosition.y * ySpeed *0.02f;
       
       
               
        rotation = Quaternion.Euler(y, x, 0);
        position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;
        position.y = Mathf.Max(position.y, 1);
        transform.position = position;
        transform.rotation = rotation;
    }
    }
}

}
