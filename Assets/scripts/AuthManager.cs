using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Collections.Generic;
using Unity.Services.CloudSave;
using System;
public class AuthManager : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveData("Key_Test", "(object)gameData");
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            LoadData(key: "Key_Test");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private async void Start()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Player signed in anonymously.");
        }
    }
    private async void SaveData(string key, string value)
    {
        Dictionary<string, object> data = new Dictionary<string, object>
    {
        { key, value }
    };

        await CloudSaveService.Instance.Data.ForceSaveAsync(data);
        Debug.Log("Data saved successfully!");
    }

    private async void LoadData(string key)
    {
        try
        {
            var data = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { key });
            if (data.TryGetValue(key, out var value))
            {
                Debug.Log($"Loaded value: {value}");
            }
            else
            {
                Debug.Log("Key not found.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading data: {ex.Message}");
        }
    }
    
}
