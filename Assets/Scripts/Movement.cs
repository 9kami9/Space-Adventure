using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainSpeed = 1000f;
    [SerializeField] float rotationSpeed = 100f;

    [SerializeField] AudioClip rocketSound;

    [SerializeField] ParticleSystem rocketJetParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;

    Rigidbody rb;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(rocketSound);
            }
            rb.AddRelativeForce(Vector3.up * Time.deltaTime * mainSpeed);
            if (!rocketJetParticles.isPlaying)
            {
                rocketJetParticles.Play();
            }
        }
        else
        {
            audioSource.Stop();
            rocketJetParticles.Stop();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if (!rightThrusterParticles.isPlaying)
            {
                rightThrusterParticles.Play();
            }
            ApplyRotation(rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!leftThrusterParticles.isPlaying)
            {
                leftThrusterParticles.Play();
            }
            ApplyRotation(-rotationSpeed);
        }
        else
        {
            leftThrusterParticles.Stop();
            rightThrusterParticles.Stop();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; //freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationThisFrame);
        rb.freezeRotation = false; //unfreezing rotation so the physics system can take over
    }
}
