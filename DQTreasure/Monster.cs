using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQTreasure
{
    internal class Monster : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly JObject mObject;
        public Monster(JObject obj)
        {
            mObject = obj;
        }

        public uint ID
        {
            get { return (uint)mObject["mKindId"]; }
            set
            {
                mObject["mKindId"] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
            }
        }

        public int Level
        {
            get { return (int)mObject["mLevel"]; }
            set { mObject["mLevel"] = value; }
        }

        public int Exp
        {
            get { return (int)mObject["mTotalExp"]; }
            set { mObject["mTotalExp"] = value; }
        }

        public int HP
        {
            get { return (int)mObject["mHp"]; }
            set { mObject["mHp"] = value; }
        }

        public int MP
        {
            get { return (int)mObject["mMp"]; }
            set { mObject["mMp"] = value; }
        }

        public int VoiceJP
        {
            get { return (int)mObject["mVoiceDataType"]; }
            set { mObject["mVoiceDataType"] = value; }
        }

        public int VoiceEN
        {
            get { return (int)mObject["mVoiceDataTypeEN"]; }
            set { mObject["mVoiceDataTypeEN"] = value; }
        }

        public MonsterStatus mIndividualStatus
        {
            get
            {
                var status = new MonsterStatus()
                {
                    MaxHP = (uint)mObject["mIndividualStatus"]["MaxHP"],
                    MaxMP = (uint)mObject["mIndividualStatus"]["MaxMP"],
                    Attack = (uint)mObject["mIndividualStatus"]["Attack"],
                    Defense = (uint)mObject["mIndividualStatus"]["Defense"],
                    Dexterity = (uint)mObject["mIndividualStatus"]["Dexterity"],
                    Magic = (uint)mObject["mIndividualStatus"]["Magic"],
                };

                return status;
            }
            set
            {
                mObject["mIndividualStatus"] = JObject.FromObject(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(mIndividualStatus)));
            }
        }
    }
}
