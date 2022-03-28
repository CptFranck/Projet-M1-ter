using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapControl : MonoBehaviour
{
    private int id;
    private int contacts;
    private List<int> contactId;
    // Start is called before the first frame update
    void Start()
    {
        contacts = 0;
        contactId = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider collision){
        if (collision.GetType() == typeof(CapsuleCollider)){
            var type = collision.gameObject.GetComponent<CapsuleColliderbis>().type;
            if (collision.gameObject.tag == "Agent" && collision.GetType() == typeof(CapsuleCollider)){
                var idbis = collision.gameObject.GetComponentInParent<AgentControl>().id; 
                if (type == "Caps"){
                    if (!contactId.Contains(idbis)){
                        contactId.Add(idbis);
                        contacts++;
                    }
                }
            }
       } 
    }

    void OnTriggerExit(Collider collision){
        if (collision.GetType() == typeof(CapsuleCollider)){
            var type = collision.gameObject.GetComponent<CapsuleColliderbis>().type;
            if (collision.gameObject.tag == "Agent" && collision.GetType() == typeof(CapsuleCollider)){
                var idbis = collision.gameObject.GetComponentInParent<AgentControl>().id; 
                if (type == "Caps"){
                    if (contactId.Contains(idbis)){
                        contactId.RemoveAt(contactId.IndexOf(idbis));
                        contacts--;
                    }
                }
            }
        }
    }

    public void SetId(int newId){
        id = newId;
    } 
    public void SetContacts(int newContact){
        contacts = newContact;
    } 
    public int  GetId(){
        return id;
    }   
    public int  GetContacts(){
        return contacts;
    }
}
