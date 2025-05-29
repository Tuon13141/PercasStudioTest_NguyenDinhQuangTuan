using Project_Data.Scripts;
using System;
using System.Collections.Generic;


using UnityEngine;

namespace Data
{
    [Serializable]
    public class UserData : SavePlayerPrefs
    {
        public bool firstLoad = false;
        public bool sound = true;
        public bool music = true;
        public void Init()
        {
            if (firstLoad)
            {
                sound = true;
                music = true;
                firstLoad = false;
            }

            SoundManager.Instance.Init(sound, music);
        }
    }

}