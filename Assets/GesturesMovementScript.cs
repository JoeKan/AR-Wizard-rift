using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GesturesMovementScript : MonoBehaviour
{
    public GameObject objectToMove;
    public GameObject projectileOriginal;
    public GameObject enemy;
    GameObject projectile;

    //movement values
    float xPositionMouse, yPositionMouse, zPositionKeyboard;

    //gesture
    bool startGesture, completeGesture;
    float startPositionX, oldPositionX;

    //shooting the projectile
    bool shooting;
    float startingPointTimer, timer;
    bool snapshot;
    float projectilePositionX, projectilePositionY, projectilePositionZ, deltaScaling;

    //enemyInteraction
    int hitCounter;

    void Start () {
        snapshot = false;
        shooting = false;

        //initialize gameObject center of screen
        xPositionMouse = 0.0f;
        yPositionMouse = 0.0f;
        zPositionKeyboard = 5.0f;

        //initialize gesture
        startGesture = true;
        completeGesture = false;
        oldPositionX = 0.0f;

        //initialize timer for projectile
        startingPointTimer = Time.time;
        timer = 0.0f;

        //enemy initialize base stats
        hitCounter = 0;
        
    }
	
	// Update is called once per frame
	void Update () {
        //update z position if w or s is pressed (w for farther back, s for closer to the screen)
        /*if (Input.GetKey("w"))
            zPositionKeyboard += 0.1f;
        else if (Input.GetKey("s"))
            zPositionKeyboard -= 0.1f;

        //update game object position via movement of mouse cursor and z
        objectToMove.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zPositionKeyboard));*/
        
        //check for gestures
        if (startGesture)
        {
            startPositionX = objectToMove.transform.position.x;
            startGesture = false;
        }
        //gesture only recognized as attempt if left mouse button is pressed
        else //if (Input.GetMouseButton(0))
            CheckGesture(objectToMove.transform.position.x);
        /*else
        {
            startPositionX = objectToMove.transform.position.x;
        }*/

        //if we did a gesture successfully, we shoot a one time projectile in z direction, starting from the game object
        if (shooting)
        {
            //take a snapshot of the initial position of the gameObject
            if (snapshot && GameObject.FindGameObjectsWithTag("bullet").Length < 2)
            {
                snapshot = false;
                //getPosition of Child object of objectToMove
                Transform childObj = objectToMove.transform.GetChild(0);
                
                projectilePositionX = childObj.transform.position.x;
                projectilePositionY = childObj.transform.position.y;
                projectilePositionZ = childObj.transform.position.z;

                Debug.Log(GameObject.FindGameObjectsWithTag("bullet").Length);
                //initialize
                projectile = Instantiate(projectileOriginal);
                projectile.transform.position = new Vector3(projectilePositionX, projectilePositionY,
                    projectilePositionZ);
                //set initiale rotation
                projectile.transform.rotation = Quaternion.LookRotation(enemy.transform.position);
            }

            if (GameObject.FindGameObjectsWithTag("bullet").Length <= 2)
            {
                //move projectile continuously back
                projectile.transform.LookAt(enemy.transform);
                projectile.transform.position += projectile.transform.TransformDirection(transform.forward)
                    * 8.0f * Time.deltaTime;
            }
            

            //we want to stop if the projectile hits something (is done in separate script) or after 10 seconds
            timer += Time.deltaTime;
            if (timer >= 5.0f)
            {
                DestroyImmediate(projectile, true);
                shooting = false;
                timer = 0;
            }
        }

    }

    void CheckGesture(float continousX)
    {
        //if cursor was moved to the right: cancel, restart gesture tracking
        if(continousX > oldPositionX)
        {
            startGesture = true;
            oldPositionX = continousX;
        }
        //if we continuously moved to the left
        else
        {
            //if we moved more than 2.0f to the left: gesture successful
            if (continousX - startPositionX <= -0.5f)
            {
                startGesture = true;
                oldPositionX = continousX;
                //Gesture successful - we fire the projectile
                shooting = true;
                snapshot = true;
            }
            //if we didnt complete the gesture yet we continue checking
            else
            {
                oldPositionX = continousX;
            }
        }
    }
    
    public void DestroyProjectile()
    {
        shooting = false;
        timer = 0;
        Destroy(projectile, 1.0f);
    }
}
