using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class RoomSelection : MonoBehaviour
{
    public Dropdown dropdown;
    public Transform[] roomTargets;
    public PathFinder AIPathFinder;
    public CameraController cameraController;

    private Dictionary<string, Transform> roomMap = new Dictionary<string, Transform>();

    private List<string> roomCodes = new List<string>
    {
        "2Q048", "2Q049", "2Q050", "2Q047", "2Q052", "2Q046", "2Q043", "2Q042",
        "2Q053", "2Q028", "2Q045", "2Q035", "2Q034",
        "2Q033", "2Q032", "Subway", "2Q005 (Toilet)",
        "2Q008", "2Q030", "2Q004 (Toilet)", "2Q029"
    };

    void Start()
    {
        AutoBuildRoomMap();
        OpenDropdown();
    }

    void AutoBuildRoomMap()
    {
        foreach (Transform t in roomTargets)
        {
            string roomCode = t.gameObject.name;
            roomMap[roomCode] = t;
        }
    }


    void OpenDropdown()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(roomCodes);
    }

    public void SetDestination()
    {
        string selectedRoomCode = roomCodes[dropdown.value];

        if (roomMap.TryGetValue(selectedRoomCode, out Transform targetTransform))
        {
            Debug.Log($"Setting destination for room: {selectedRoomCode}");
            AIPathFinder.SetNewDestination(targetTransform.position, selectedRoomCode);
            cameraController.LockCursor();
        }
        else
        {
            Debug.LogError($"SetDestination: No target found for room code {selectedRoomCode}");
        }
    }

}
