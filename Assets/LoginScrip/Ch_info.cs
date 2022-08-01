using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Ch_info : MonoBehaviour
{
    FirebaseManager firebaseManager;
    public string userName;


    // Start is called before the first frame update
    void Awake()
    {
        
    }
    void Start()
    {
        firebaseManager = GetComponent<FirebaseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CharacterInfoUpdate()
    {
        //userName = firebaseManager.userName;
        SceneManager.LoadScene("Scene1");
        

    }
}
