using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
   [SerializeField] AudioClip crashAudio;
   [SerializeField] AudioClip successAudio;
   [SerializeField] float loadLevelDelay = 3f;

   [SerializeField] ParticleSystem crashParticles;
   [SerializeField] ParticleSystem successParticles;
   
   Movement movement; 
   AudioSource audioSource;

   bool isTransitioning = false;
   bool collisionDisabled = false;
   
   void Start() 
   {
       audioSource = GetComponent<AudioSource>();
       movement = GetComponent<Movement>();
   }

   void Update()
   {
       DebugKeys();
   }

   void DebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // toggle collision
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            LoadPreviousLevel();
        }
    }

   void OnCollisionEnter(Collision other) 
   {
       if (isTransitioning || collisionDisabled) { return; }
       
       switch (other.gameObject.tag)
       {
            case "Friendly":
                Debug.Log("Your mission is active.");
                break;

            case "Finish":
                Debug.Log("Level Complete!");
                StartLevelUpSequence();
                break;

            default:
                Debug.Log("Womp Womp");
                StartCrashSequence();
                break;
       }
   }

    void StartLevelUpSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(successAudio);
        successParticles.Play();
        movement.enabled = false;
        Invoke ("LoadNextLevel" , loadLevelDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crashAudio);
        crashParticles.Play();
        movement.enabled = false;
        Invoke ("ReloadLevel" , loadLevelDelay);
    }

    void LoadNextLevel()
   {
       Debug.Log("Your mission is active");
       
       int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
       int nextSceneIndex = currentSceneIndex + 1;
       
       if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
       {
           nextSceneIndex = 0;
       }
       SceneManager.LoadScene(nextSceneIndex);
   }
   
   void LoadPreviousLevel()
   {
       int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
       int nextSceneIndex = currentSceneIndex - 1;
       
       if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
       {
           nextSceneIndex = 0;
       }
       SceneManager.LoadScene(nextSceneIndex);

       if (currentSceneIndex == 0 && Input.GetKeyDown(KeyCode.K))
       {
           Debug.Log("No levels left to load");
       }
       else if (currentSceneIndex != 0 && Input.GetKeyDown(KeyCode.L))
       {
           Debug.Log("Your mission is active");
       }
   }

   void ReloadLevel()
   {
       
       int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
       SceneManager.LoadScene(currentSceneIndex);
   }    

}
