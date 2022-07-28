using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delayTime = 0.5f;

    [SerializeField] AudioClip explosionSound;
    [SerializeField] AudioClip winSound;

    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // toggle collision
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning) { return; }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                ReachFinish();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartCrashSequence()
    {
        if (collisionDisabled) { return; }
        isTransitioning = true;

        explosionParticles.Play();

        audioSource.Stop();
        audioSource.PlayOneShot(explosionSound);

        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", delayTime);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void ReachFinish()
    {
        isTransitioning = true;

        successParticles.Play();

        audioSource.Stop();
        audioSource.PlayOneShot(winSound);

        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", delayTime);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
