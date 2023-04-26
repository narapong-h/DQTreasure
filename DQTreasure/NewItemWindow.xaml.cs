using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DQTreasure
{
    /// <summary>
    /// NewItemWindow.xaml
    /// </summary>
    public partial class NewItemWindow : Window
	{
		public uint ID { get; set; }

		public enum eType
		{
			eItem,
			eMonster,
			eTreasure,
			eCoin,
			eCharacteristic
		}

		public eType Type { get; set; } = eType.eItem;

        public NewItemWindow()
		{
			InitializeComponent();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			CreateItemList("");
			TextBoxFilter.Focus();
		}

		private void TextBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
		{
			CreateItemList(TextBoxFilter.Text);
		}

		private void ListBoxItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ButtonDecision.IsEnabled = ListBoxItem.SelectedIndex >= 0;
		}

		private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			ButtonDecision_Click(sender, null);
		}

		private void ButtonDecision_Click(object sender, RoutedEventArgs e)
		{
			var Listed = (List<NameValueInfo>)ListBoxItem.ItemsSource;
            var selectedIds = Listed.Where(item => item.IsSelected).Select(item => item.Name).ToArray();

			Close();
		}

		private void CreateItemList(String filter)
		{
			List<NameValueInfo> filtered = new();

			var Targets = new Dictionary<eType, List<NameValueInfo>>()
			{
				{eType.eItem, Info.Instance().Item },
				{eType.eMonster, Info.Instance().Monster },
				{eType.eTreasure, Info.Instance().Treasure },
                {eType.eCoin, Info.Instance().Coin },
                {eType.eCharacteristic, Info.Instance().Characteristic },
            };

            var items = Targets[Type];

			foreach (var item in items)
			{
				if (String.IsNullOrEmpty(filter) || item.Name.IndexOf(filter) >= 0)
				{
                    filtered.Add(item);
				}
			}

			ListBoxItem.ItemsSource = filtered;
        }
	}
}
