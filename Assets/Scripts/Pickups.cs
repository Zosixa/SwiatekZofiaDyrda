using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickups : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1;
    private int Coin = 0;



    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "coin")
        {
            Coin++;
            src.clip = sfx1;
            src.Play();
            Destroy(other.gameObject);
            print("Masz ju¿ " + Coin + " pieniêdzy");
            
                
        }
    }

}