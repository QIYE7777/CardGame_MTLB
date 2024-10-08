using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class RoomSwitcher : Singleton<RoomSwitcher>
{
    public string[] rooms;
    public int currentRoomIndex = -1;

    public event Action OpenandCloseGlowImageEvent;
    private void Start()
    {
        currentRoomIndex = 0;
        SwitchToNextLevel();
    }
    public void SwitchToNextLevel()
    {
        if (currentRoomIndex > rooms.Length-1)
        {
            Debug.LogError("no enough room! currentIndex is " + currentRoomIndex);
            // Win();
            return;
        }
        SceneManager.LoadScene(rooms[currentRoomIndex], LoadSceneMode.Single);
        currentRoomIndex++;
        if (currentRoomIndex == rooms.Length)
        {
            OpenandCloseGlowImageEvent?.Invoke();
            Debug.Log("454545");
        }

    }

    public void BackToUI()
    {
        SceneManager.LoadScene("StartUI", LoadSceneMode.Single);
    }
}
