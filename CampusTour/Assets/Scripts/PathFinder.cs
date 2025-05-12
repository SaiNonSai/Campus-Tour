using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PathFinder : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool hasReached = false;
    public CameraController cameraController;

    public TimetableLinkOpener timetableButton;

    public Text roomDescriptionText;
    public GameObject descriptionPanel;

    private string currentRoomCode;

    private Dictionary<string, string> roomDescriptions = new Dictionary<string, string>
{
    //Teaching and Learning Spaces
    {"2Q048", "Type: Standard Teaching Room\nDepartment: Engineering, Design, and Mathematics\nSeats: Estimated 40-60\nPurpose: Used for regular classes, lectures, and seminars."},
    {"2Q049", "Type: Large Lecture Hall\nDepartment: Central Exams and Teaching Timetabling Services\nSeats: 100+ tiered seating\nPurpose: Used for large lectures, exams, and events.\nView Room Timetable:(go.uwe.ac.uk/rm_2Q049_FR)"},
    {"2Q050", "Type: Large Teaching Room\nDepartment: Faculty of Environment and Technology\nSeats: 80+\nPurpose: Large lecture-based classes and interactive teaching.\nView Room Timetable:(go.uwe.ac.uk/rm_2Q050_FR)"},
    {"2Q047", "Type: Medium-Sized Teaching Room\nDepartment: Geography and Environmental Management\nSeats: 15-25\nPurpose: Small group lectures and discussions."},
    {"2Q046", "Type: Seminar Room\nDepartment: Geography and Environmental Management\nSeats: 15-25\nPurpose: Tutorials, project discussions, and small-group teaching."},
    {"2Q043", "Type: Standard Teaching Room\nDepartment: Computer Science and Creative Technology\nSeats: 30-40\nPurpose: Regular IT-focused lectures.\nView Room Timetable:(go.uwe.ac.uk/rm_2Q043_FR)"},
    {"2Q042", "Type: Standard Classroom\nDepartment: Geography and Environmental Management\nSeats: 50+\nPurpose: Used for geography/environmental sciences lectures.\nView Room Timetable:(go.uwe.ac.uk/rm_2Q042_FR)"},

    //Computer Labs
    {"2Q052", "Type: IT Lab\nDepartment: Computer Science and Creative Technology\nSeats: 30+ workstations\nPurpose: Programming, software training, and hands-on IT learning."},
    {"2Q053", "Type: IT Lab\nDepartment: Computer Science and Creative Technology\nSeats: 25+ workstations\nPurpose: Practical IT-related sessions.\nView Room Timetable:(go.uwe.ac.uk/rm_2Q053_FR)"},
    {"2Q028", "Type: Computer Lab\nSeats: 35+\nPurpose: Software and IT courses.\nView Room Timetable:(go.uwe.ac.uk/rm_2Q028_FR)"},

    //Administrative Offices
    {"2Q045", "Type: Faculty Office\nDepartment: Computer Science and Creative Technology\nSeats: Office desks for faculty members\nPurpose: Faculty workspace and administrative tasks."},
    {"2Q035", "Type: IT Services Office\nDepartment: IT Services\nSeats: Several desks for IT staff\nPurpose: IT support, troubleshooting, and administration."},
    {"2Q034", "Type: IT Technicians’ Workspace\nDepartment: IT Services Technicians\nSeats: Office desks for IT staff\nPurpose: Administrative work and tech support."},
    {"2Q033", "Type: Student Information Desk\nDepartment: IT Services\nSeats: Service desk area\nPurpose: Student queries, IT support, and information assistance."},

    //Facilities and Utility Spaces
    {"2Q032", "Type: Utility Room\nDepartment: Estates Balance\nPurpose: Housing technical and mechanical systems."},
    
    //Learning and Social Spaces
    {"2Q008", "Type: Study Area\nDepartment: Library Services\nSeats: 50+ desks and study pods\nPurpose: Individual and group study."},
    {"2Q030", "Type: Library Study Space\nDepartment: Library Services\nSeats: 60+ seating areas\nPurpose: Research and independent study."},

    //Other Spaces
    {"2Q029", "Type: Laboratory\nPurpose: Geography and Environmental research.\nView Room Timetable:(go.uwe.ac.uk/rm_2Q029_FR)"}
};

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        descriptionPanel.SetActive(false);
    }

    void Update()
    {
        if (!hasReached && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            hasReached = true;
            ShowRoomDescription();
            UnlockCursor();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && descriptionPanel.activeSelf)
        {
            descriptionPanel.SetActive(false);
        }
    }

    public void SetNewDestination(Vector3 newTarget, string roomCode)
    {
        if (agent != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(newTarget, out hit, 2.0f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                Debug.Log($"Destination set for {roomCode}: {hit.position}");
            }
            else
            {
                Debug.LogError($"Destination for {roomCode} is not on the NavMesh!");
            }

            hasReached = false;
            descriptionPanel.SetActive(false);
            currentRoomCode = roomCode;
        }
    }

    void ShowRoomDescription()
    {
        if (string.IsNullOrEmpty(currentRoomCode))
        {
            return;
        }

        if (roomDescriptions.ContainsKey(currentRoomCode))
        {
            string description = roomDescriptions[currentRoomCode];
            string timetableURL = ExtractTimetableLink(description);

            if (!string.IsNullOrEmpty(timetableURL))
            {
                description = description.Replace($"\nView Room Timetable:({timetableURL})", ""); 
            }

            roomDescriptionText.text = description;

            if (timetableButton != null)
            {
                timetableButton.SetTimetableURL(timetableURL);
            }
        }
        else
        {
            roomDescriptionText.text = "No description available for this room.";
        }

        descriptionPanel.SetActive(true);
    }


    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (cameraController != null)
        {
            cameraController.UnlockCursor();
        }
    }

    private string ExtractTimetableLink(string description)
    {
        int startIndex = description.IndexOf("(go.uwe.ac.uk/");
        if (startIndex == -1)
        {
            return null;
        }

        startIndex += 1; // Move past '('

        int endIndex = description.IndexOf(")", startIndex);
        if (endIndex == -1)
        {
            return null;
        }

        string extractedURL = description.Substring(startIndex, endIndex - startIndex);
        return extractedURL;
    }
}
