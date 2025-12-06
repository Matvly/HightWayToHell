using System.Collections.Generic;
using System;
using Unity.Services.CloudSave;
using UnityEngine;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine.Playables;
using UnityEditor.Overlays;


public class JsonSaveSystem
{

    //public async void OnLoadBtnClicked()
    //{
    //    _gameData = await _saveSystem.Load<GameData>();
    //    _view.Display(_gameData);
    //}
    //private async void Start()
    //{
    //    await UnityServices.InitializeAsync();

    //    if (!AuthenticationService.Instance.IsSignedIn)
    //    {
    //        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    //        Debug.Log("Player signed in anonymously.");
    //    }

    //    _gameData = await _saveSystem.Load<GameData>();
    //    _view.Display(_gameData);
    //}
    private readonly string _cloudKey = "save_data";
    public async void SaveT<T>(T data)
    {
        try
        {
            var jsonString = JsonUtility.ToJson(data);

            Dictionary<string, object> saveData = new Dictionary<string, object>
        {
            { _cloudKey, jsonString }
        };

            await CloudSaveService.Instance.Data.ForceSaveAsync(saveData);
            Debug.Log("Data saved to cloud successfully.");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error saving data to cloud: {ex.Message}");
        }
    }

    public async Task<T> Load<T>()
    {
        try
        {
            var data = await CloudSaveService.Instance.Data.LoadAsync(new HashSet<string> { _cloudKey });

            if (data.TryGetValue(_cloudKey, out var jsonString))
            {
                Debug.Log("Data loaded from cloud successfully.");
                return JsonUtility.FromJson<T>(jsonString.ToString());
            }
            else
            {
                Debug.LogWarning("No data found in cloud. Returning default instance.");
                return Activator.CreateInstance<T>();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading data from cloud: {ex.Message}");
            return Activator.CreateInstance<T>();
        }
    }
}
