using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class audioController : MonoBehaviour {
    public AudioSource ball;
    public AudioSource crowd;
    public Slider adjust;

    private float volume;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "board" || collision.gameObject.tag == "court") {
            Debug.Log("Hit");
            ball.Play();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "target") {
            crowd.Play();
        }
    }


    public void adjustVolume() {
        Debug.Log(adjust.value);
        ball.volume = adjust.value;
    }

}
