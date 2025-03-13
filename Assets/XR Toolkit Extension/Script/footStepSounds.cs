using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class footStepSounds : MonoBehaviour
{
    [System.Serializable]
    public class footstepSound
    {
        public string soundName;
        public AudioClip[] sounds;
        public string[] tags;

        public float minVolume;
        public float maxVolume;

        public float minPitch;
        public float maxPitch;

    }

    [Header("FootStep Settings")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float speedTreshold = 0.1f;
    [SerializeField] private float timeBetweenStep = 1f;
    [SerializeField] private float footstepSpeed = 1f;
    [Header("FootStep Sounds")]
    [SerializeField] private footstepSound[] footstepSounds;
    private string currentGround = "default";

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (characterController.isGrounded && characterController.velocity.magnitude > speedTreshold)
        {
            if (Time.time >= timeBetweenStep)
            {
                
                currentGround = hit.collider.tag;
                Debug.Log("Current ground: " + currentGround);
                PlayFootstepSound();
                timeBetweenStep = Time.time + 1 / footstepSpeed;
            }
        }
    }

    void PlayFootstepSound()
    {
        foreach (footstepSound footstepSound in footstepSounds)
        {
            foreach (string tag in footstepSound.tags)
            {
                if (tag == currentGround)
                {
                    audioSource.volume = Random.Range(footstepSound.minVolume, footstepSound.maxVolume);
                    audioSource.pitch = Random.Range(footstepSound.minPitch, footstepSound.maxPitch);
                    audioSource.PlayOneShot(footstepSound.sounds[Random.Range(0, footstepSound.sounds.Length)]);
                    Debug.Log("Playing sound: " + footstepSound.soundName);
                }
            }
        }
    }
}