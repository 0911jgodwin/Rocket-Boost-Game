
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rThrust = 100f;
    [SerializeField] float mThrust = 750f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive) 
        {
            RespondToRotateInput();
            RespondToThrustInput();
        }
    }

    private void RespondToRotateInput()
    {

        rigidBody.freezeRotation = true; // Grabs manual control of rotation

        float rotationSpeed = rThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
        rigidBody.freezeRotation = false; // Resumes normal physics control

    }

    private void RespondToThrustInput()
    {

        if (Input.GetKey(KeyCode.Space)) // Able to thrust whilst rotating
        {
            ApplyThrust();

        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * mThrust);
        if (!audioSource.isPlaying) // Prevents audio start from being looped repeatedly
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) //Ignores collisions when dead
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // Do nothing.
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();         // Stop thrust sound before playing death sound
        audioSource.PlayOneShot(death);
        Invoke("PlayerDeath", 1f);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        Invoke("LoadNextLevel", 1f);
    }

    private void PlayerDeath()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); //allow for more levels at some point
    }
}
