using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Typical Structure - Coding Best Practice
    // PARAMETERS - for tuning, typically set in the editor
    // CACHE - e.g. references for readibility or speed
    // STATE - private instance (member) variables

    // PARAMETERS
    [SerializeField] float rotationThrust = 1000f;
    [SerializeField] float mainThrust = 1000f;

    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainBooster;
    [SerializeField] ParticleSystem leftBooster;
    [SerializeField] ParticleSystem rightBooster;

    // CACHE
    Rigidbody rb; 
    AudioSource audioSource;

    // STATE (Don't have any at the moment)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            audioSource.Stop();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            mainBooster.Stop();
        }
    }

void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rightBooster.Play();
            ApplyRotation(rotationThrust);

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            leftBooster.Play();
            ApplyRotation(-rotationThrust);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            rightBooster.Stop();
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            leftBooster.Stop();
        }
    }

     void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; //unfreezing rotation so physics system can take over
    }

    void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }    
            mainBooster.Play();
    }

}