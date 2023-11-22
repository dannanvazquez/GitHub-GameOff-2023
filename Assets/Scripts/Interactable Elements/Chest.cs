    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Chest : MonoBehaviour
    {
        [SerializeField] private AudioSource AudioSource_opening;
        [SerializeField] private AudioSource AudioSource_closing;
        [Header("Audioclips")] 
        [Tooltip("opening")]  
        [SerializeField] private AudioClip[] chest_opening_sfx;
        [Tooltip("closing")]       
        [SerializeField] private AudioClip[] chest_closing_sfx;    

        private bool hasOpened = false;
        private LootManager lootManager;

        private Animator anim;
        // Start is called before the first frame update
        void Start()
        {
            anim = this.gameObject.GetComponent<Animator>();
            lootManager = this.gameObject.GetComponent<LootManager>();
        }

        private void PlayRandomClip(AudioClip[] clips, AudioSource audioSource)
        {
                AudioClip clip;
                clip = clips[UnityEngine.Random.Range(0, clips.Length)];
                audioSource.clip = clip;
                audioSource.pitch = UnityEngine.Random.Range(.95f, 1.15f);
                audioSource.Play();
                
            }


        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                anim.SetBool("OpenChest", true);
                Debug.Log("Test<color=red> chest should open");
                PlayRandomClip(chest_opening_sfx, AudioSource_opening);
                // Instantiate loot on the first opening

                lootManager.InstantiateRandomLoot(transform.position);
                
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                anim.SetBool("OpenChest", false);
                PlayRandomClip(chest_closing_sfx, AudioSource_closing);
                // Destroy the loot when closing the chest

                lootManager.DestroyLoot();
            }
        }
    }
