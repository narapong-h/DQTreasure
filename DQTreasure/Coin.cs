using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQTreasure
{
    internal class Coin : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly JObject mObject;
        public Coin(JObject obj)
        {
            mObject = obj;
        }

        public uint ID
        {
            get { return (uint)mObject["MonsterKindId"]; }
            set
            {
                mObject["MonsterKindId"] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
            }
        }

        public uint Rarity
        {
            get { return (uint)mObject["Rarity"]; }
            set
            {
                mObject["Rarity"] = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Rarity)));
            }
        }

        public CoinFeature[] FeatureList
        {
            get
            {
                return ((JArray)mObject["FeatureList"]).Select(f => new CoinFeature
                {
                    featureId = (string)f["featureId"],
                    step = (uint)f["step"]
                }).ToArray();
            }
            set
            {
                mObject["FeatureList"] = JArray.FromObject(value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FeatureList)));
            }
        }
    }
}
