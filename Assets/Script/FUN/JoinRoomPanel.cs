using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomPanel : MonoBehaviourPunCallbacks
{
    [SerializeField] private Button m_ButtonJoinRoom;
    private string roomName = "MainRoom";

    void Start()
    {
        m_ButtonJoinRoom.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        m_ButtonJoinRoom.interactable = true;
        Debug.Log("Đã connect Master Server!");
    }

    public void OnClickJoinRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
        }
        else
        {
            Debug.Log("Chưa kết nối xong tới Master Server!");
        }
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Join room: " + PhotonNetwork.CurrentRoom.Name);
        float randomZ = Random.Range(4.5f, 5.5f);
        PhotonNetwork.Instantiate("PlayerPrefab", new Vector3(7.77f, -3.4f, randomZ), Quaternion.identity);
        if (PhotonNetwork.IsMasterClient)
        {
            MazeGenerator.Instance.CreateNewMaze();
            MazeGenerator.Instance.SyncMazeWithOthers();
        }
        else Debug.Log("Notttttttttt master");
        gameObject.SetActive(false);
    }
}
