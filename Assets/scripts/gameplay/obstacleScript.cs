﻿using UnityEngine;
using System.Collections;

public class obstacleScript : MonoBehaviour {

    public int actType;
    public int obstacleType;

    float shake;

    bool removing;

    Vector3 originalPosition;

	void Start () {
        shake = 0;
        removing = false;
        originalPosition = transform.localPosition;
	}

    public void interact() {
        if (actType == 1) {
            shake += 0.4f;
            if (obstacleType == 1) {
                GetComponent<Animator>().SetBool("Hit", true);
                if (!GetComponent<AudioSource>().isPlaying)
                    GetComponent<AudioSource>().Play();
            }
        } else if (actType == 2) {
            GetComponent<Animator>().SetBool("Destroyed", true);
            removing = true;
            StartCoroutine(die());
        } else if (actType == 3) {
            GetComponent<carSpawnerScript>().active = false;
        } else if (actType == 4) { 
            if (obstacleType == 0) {
                GetComponent<Animator>().SetBool("Hit", true);
                if (!GetComponent<AudioSource>().isPlaying)
                    GetComponent<AudioSource>().Play();
                removing = true;
                StartCoroutine(die());
            } else { 
                for (int i = 0; i < GetComponentsInChildren<obstacleScript>().Length; i++) {
                    if (GetComponentsInChildren<obstacleScript>()[i].GetComponent<Animator>() != null) {
                        GetComponentsInChildren<obstacleScript>()[i].GetComponent<Animator>().SetBool("Hit", true);
                        GetComponentsInChildren<obstacleScript>()[i].removing = true;
                    }
                    StartCoroutine(GetComponentsInChildren<obstacleScript>()[i].die());
                }
            }
        } else if (actType == 5) { 
            if (obstacleType == 1) {
                transform.localPosition = Vector3.zero;
            } else if (obstacleType == 0) {
                shake++;
            }
        } else if (actType == 6) {
            if (playerScript.instance.wrongInput) {
                shake = 10.0f;
            }
            else
                GetComponent<Animator>().SetBool("DoorOpened", true);
            
        }
    }
    public void remove() {
        removing = true;
        StartCoroutine(die());
    }
    IEnumerator die() {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    IEnumerator die(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        if (actType == 1) {
            if (removing) {
                if (obstacleType == 1) {
                    transform.localPosition += new Vector3(0, 0.1f, 0.3f);
                    transform.RotateAround(transform.position, Vector3.right, 20 * Time.deltaTime);
                    return;
                } else if (obstacleType == 2) {
                    transform.localPosition += new Vector3(0, 0.04f, 0.5f);
                    GetComponent<Animator>().SetBool("Wrecked", true);
                    return;
                }
                transform.localPosition += new Vector3(0, 0.04f, 0.5f);
                transform.Rotate(new Vector3(0f, 0f, 20f));
                return;
            }
            if (shake > 10)
                shake = 10;
            if (shake > 0) {
                transform.localPosition = originalPosition + Random.insideUnitSphere * shake * 0.02f;
                shake -= 0.02f;
            } else {
                shake = 0f;
            }
        }
        else if (actType == 4) { 
            if (removing) {
                transform.localPosition += new Vector3(0, 0.1f, 0.3f);
                transform.RotateAround(Vector3.zero, Vector3.right, 20 * Time.deltaTime);
                return;
            }
        }
        else if (actType == 5) {
            if (obstacleType == 0)
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y,
                    -minigameOverheadScript.instance.components[6].transform.localEulerAngles.z);
        }
        else if (actType == 6) {
            if(shake > 0.0f) {
                transform.localPosition = originalPosition + Random.insideUnitSphere * shake * 0.02f;
                shake -= 0.4f;
                if(shake <= 0.0f) {
                    shake = 0.0f;
                    playerScript.instance.wrongInput = false;
                }
            }
        }
    }
}
