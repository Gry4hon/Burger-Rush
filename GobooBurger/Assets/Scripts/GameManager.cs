using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

namespace GoblinEats.SuperFriends.Com.GameManager 
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Public Variables
            [Tooltip("The Player Prefab")]
            public GameObject thePlayerPrefab;
            
            

        #endregion

        #region Photon Callbacks

        /// <summary>
        /// Essentially this will be called when a user leaves the room, in which the title screen will be reloaded
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player thePlayer)
        {
            Debug.LogFormat("This cunt is joining",thePlayer.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("Oi this cunts the Master Client", PhotonNetwork.IsMasterClient);

                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player thePlayer)
        {
            Debug.LogFormat("Yo this dude left", thePlayer.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("This mf was the master client", PhotonNetwork.IsMasterClient);

                LoadArena();
            }
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region Private Methods

        private void Start()
        {
            if(thePlayerPrefab != null)
            {
                Debug.LogErrorFormat("<color = green><a>MISSING GOBLINB ADD PLZ</a></color> Just add the player prefab to thePlayerPrefab");
            }
            else
            {
                if(PlayerManager.PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("The player is being instantiated", Application.loadedLevelName);
                    PhotonNetwork.Instantiate(this.thePlayerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogFormat("Ignoring Scene Load", SceneManagerHelper.ActiveSceneName);
                }
                
            }
        }

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Trying to load the game but we aren't cool enough (Not the master client)");
                return;
            }
            Debug.LogFormat("Loading the level (FUck yea)", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room for: " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        #endregion

    }

}


