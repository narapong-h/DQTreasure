using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQTreasure
{
    internal class NameValueInfo : IComparable, INotifyPropertyChanged
    {
        public uint Value { get; private set; }
        public String Name { get; private set; } = String.Empty;
        public uint MinValue { get; private set; }
        public uint MaxValue { get; private set; }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public int CompareTo(object obj)
        {
            var dist = obj as NameValueInfo;
            if (dist == null) return 0;

            if (Value < dist.Value) return -1;
            else if (Value > dist.Value) return 1;
            else return 0;
        }

        public bool Line(String[] oneLine)
        {
            if (oneLine[0].Length > 1 && oneLine[0][1] == 'x')
                Value = Convert.ToUInt32(oneLine[0], 16);
            else 
                Value = Convert.ToUInt32(oneLine[0]);
            
            Name = oneLine[1];
            
            if(oneLine.Length > 2)
            {
                _ = uint.TryParse(oneLine[2], out uint min);
                MinValue = min;
            }

            if (oneLine.Length > 3)
            {
                _ = uint.TryParse(oneLine[3], out uint max);
                MaxValue = max;
            }

            return true;
        }
    }
}
