using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;

public class NTest : MonoBehaviour
{
    public DatabaseReference DBreference;
    
    // Start is called before the first frame update
    void Start()
    {
        
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(UpdatePath());
        //User = GameManager.instance.user;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator UpdatePath() 
    {
        while(true)
        {
            StartCoroutine(UpdateXp((int)GameManager.instance.userExp));
            StartCoroutine(UpdateLevel((int)GameManager.instance.userLevel));
            StartCoroutine(UpdatePosition(GameManager.instance.userPositionX, GameManager.instance.userPositionY, GameManager.instance.userPositionZ));
            yield return new WaitForSeconds(5);
        }
        
    }

    private IEnumerator UpdateXp(int _xp)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("users").Child(GameManager.instance.user.UserId).Child("xp").SetValueAsync(_xp);
        Debug.Log("SaveDate");
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
        }
    }

    private IEnumerator UpdateLevel(int _level)
    {
        //Set the currently logged in user xp
        var DBTask = DBreference.Child("users").Child(GameManager.instance.user.UserId).Child("level").SetValueAsync(_level);
        Debug.Log("SaveDate");
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
        }
    }

    private IEnumerator UpdatePosition(float _positionX, float _positionY, float _positionZ)
    {
        float[] pos = {_positionX, _positionY, _positionZ};
        string[] position = {"posX", "posY", "posZ"};
        for(int i=0; i<3; i++)
        {
            var DBTask = DBreference.Child("users").Child(GameManager.instance.user.UserId).Child(position[i]).SetValueAsync(pos[i]);
        
            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                //Xp is now updated
            }
        }
        
        
    }
    
}
