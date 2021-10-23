using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject target;
    public AudioSource musicSource;
    public AudioClip levelMusic;
    public AudioClip winMusic;
    public AudioClip loseMusic;


    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = levelMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (PlayerScript.level == 1){
            this.transform.position = new Vector3(target.transform.position.x, this.transform.position.y, this.transform.position.z);
        }
        else if(PlayerScript.level == 2){
            this.transform.position = new Vector3(35.6f, target.transform.position.y, this.transform.position.z); 
        }
        else if(PlayerScript.level == 3){
            musicSource.clip = winMusic;
            musicSource.loop = false;
            musicSource.volume = 1;
            musicSource.Play();
            PlayerScript.level++;
            PlayerScript.isAlive = false;
            //Destroy(target.gameObject); 
        }
        if(PlayerScript.isAlive == false  && PlayerScript.level != 4){
            PlayerScript.level = 4;
            musicSource.clip = loseMusic;
            musicSource.loop = false;
            musicSource.volume = 1;
            musicSource.Play();
            //Destroy(target.gameObject);
        }
        
    }
}
