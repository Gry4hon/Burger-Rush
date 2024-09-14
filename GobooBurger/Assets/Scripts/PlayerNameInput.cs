using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


namespace GoblinEats.SuperFriends.Com.PNInput
{
    //This script will only work for a gameobject that has the component of inputField
    [RequireComponent(typeof(InputField))]

    public class PlayerNameInput : MonoBehaviour
    {
        #region Private Constant Values
        const string playerName = "Player Name";
        #endregion

        #region MonoBehaivor CallBacks

        void Start()
        {

            string defaultPlayerName = string.Empty;
            InputField playerInputField = this.GetComponent<InputField>();

            if(playerInputField != null)
            {
                if (PlayerPrefs.HasKey(playerName))
                {
                    defaultPlayerName = PlayerPrefs.GetString(playerName);
                    playerInputField.text = defaultPlayerName;
                }
            }
            PhotonNetwork.NickName = defaultPlayerName;
            
        }

        #endregion

        #region Public Method

        //This method is pretty cool, so essentially it allows the "saving" of the choosen players name for future games :D
        public void SetPlayerName(string cilentChoice)
        {
            if (string.IsNullOrEmpty(cilentChoice))
            {
                Debug.LogAssertionFormat("The Name Field is empty, please enter a name");
                return;
            }

            PhotonNetwork.NickName = cilentChoice;

            PlayerPrefs.SetString(playerName, cilentChoice);
        }

        #endregion


    }
}


