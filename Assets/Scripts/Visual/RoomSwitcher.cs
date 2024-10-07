using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomSwitcher : Singleton<RoomSwitcher>
{
    public string[] rooms;
    public int currentRoomIndex = -1;
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
    }

    public void BackToUI()
    {
        SceneManager.LoadScene("StartUI", LoadSceneMode.Single);
    }
}
