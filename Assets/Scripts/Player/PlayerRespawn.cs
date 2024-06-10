using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uIManager;


    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uIManager = FindObjectOfType<UIManager>();
    }

    public void CheckRespawn()
    {
        if(currentCheckpoint == null)
        {
            //game over
            uIManager.GameOver();

            return;
        }

        transform.position = currentCheckpoint.position;
        playerHealth.Respawn();
        gameObject.layer = LayerMask.NameToLayer("Player");



        // TODO: room camera aspect, gotta think about it later
        //Camera.main.GetComponent<CameraController>().MoveToNewRoom(currentCheckpoint.parent); 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Checkpoint")
        {
            //Activating previous checkpoint
            if(currentCheckpoint != null)
                currentCheckpoint.gameObject.GetComponent<Collider2D>().enabled = true;

            currentCheckpoint = collision.transform; //Store activated checkpoint

            SoundManager.instance.PlaySound(checkpointSound);

            collision.GetComponent<Collider2D>().enabled = false;
            collision.GetComponent<Animator>().SetTrigger("Activated");
        }
    }
}
