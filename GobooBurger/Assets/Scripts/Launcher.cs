using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

using Photon.Realtime;

namespace GoblinEats.SuperFriends.Com.Launcher
{
    //Monobehaviour as a class allows the script to become a UNITY Componets making it interactivable with the Unity game engine.
    public class Launcher : MonoBehaviourPunCallbacks
    {
        //every #region must have an #endregion after it
        #region Private Serializable Fields
        [SerializeField]
        private byte maxPlayersPerRoom = 6;

        [Tooltip("The UI that allows user to enter a name and play the game")]
        [SerializeField]
        private GameObject controlPanel;

        [Tooltip("The UI that shows the user is connecting to the game")]
        [SerializeField]
        private GameObject progressText;

        #endregion

        #region Private Fields

        /// <summary>
        ///This is the players version number which allows them to be separated from other players
        /// </summary>
        string gameVersion = "1";
        bool isConnecting;
        #endregion

        #region MonoBehavior CallBacks

        private void Awake()
        {
            //This allows PhotonNetwork.LoadLevel(); to make sure all other players make it into the same level as the master client
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            progressText.SetActive(false);
            controlPanel.SetActive(true);  
        }

        #endregion

        #region Public Methods

        //This will run as soon as this script is called
        /// <summary>
        /// This will check for two things when it is called:
        /// 1- If the client is connected to the game, then have them join one of the random rooms that have been created
        /// 2- If the client is not connected the game, then connect this game to the Proton server that is being hosted by Proton Cloud Network
        /// </summary>
        public void Connect()
        {
            progressText.SetActive(true);
            controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public void GameQuit()
        {
            Debug.Log("Game has been Quit");
            Application.Quit();
        }

        #endregion

        #region MonoBehaviorPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("Goblin Eats: OnConnectedToMaster() was called by PUN");

            //This will insure that upon connection to master we will automatically be connected to a room, and if not then we will create a room to join using OnJoinRandomFailed()
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false; 
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.LogWarningFormat("Goblin Eats Doesn't have a room to join, so we'll create one for ya!");

            //This will create a room in the even that there are no rooms OR even if there are rooms but they are currently full. It will also create a room with a limited amount of players that can join.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom});
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Goblin Eats now has an avaliable room and the client has joined");

            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for one' ");

                PhotonNetwork.LoadLevel("Room for 1");
            }

        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            progressText.SetActive(false);
            controlPanel.SetActive(true);
            Debug.LogErrorFormat("Goblin Eats: OnDisconnected() was called by for reason {0}: ", cause);
        }

        #endregion

    }
}