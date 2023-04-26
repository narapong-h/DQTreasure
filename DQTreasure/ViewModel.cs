using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DQTreasure
{
    internal class ViewModel
    {
        public Info Info { get; private set; } = Info.Instance();

        public CommandAction FileSaveCommand { get; private set; }
        public CommandAction FileSaveAsCommand { get; private set; }
        public CommandAction ImportCommand { get; private set; }
        public CommandAction ExportCommand { get; private set; }

        public CommandAction ChoiceItemCommand { get; private set; }
        public CommandAction ChoiceMonsterCommand { get; private set; }
        public CommandAction ChoiceTreasureCommand { get; private set; }
        public CommandAction SetMinCommand { get; private set; }
        public CommandAction SetMaxCommand { get; private set; }
        public CommandAction MaxOutCommand { get; private set; }
        public CommandAction MonsterUpgradeCommand { get; private set; }
        public CommandAction CoinUpgradeCommand { get; private set; }

        public General General { get; private set; }
        public Player Player { get; private set; }
        public ObservableCollection<Item> Items { get; private set; } = new ObservableCollection<Item>();
        public ObservableCollection<Treasure> Treasures { get; private set; } = new ObservableCollection<Treasure>();
        public ObservableCollection<Monster> Monsters { get; private set; } = new ObservableCollection<Monster>();
        public ObservableCollection<Coin> Coins { get; private set; } = new ObservableCollection<Coin>();

        public ViewModel()
        {
            FileSaveCommand = new CommandAction(FileSave);
            FileSaveAsCommand = new CommandAction(FileSaveAs);
            ImportCommand = new CommandAction(Import);
            ExportCommand = new CommandAction(Export);

            ChoiceItemCommand = new CommandAction(ChoiceItem);
            ChoiceMonsterCommand = new CommandAction(ChoiceMonster);
            ChoiceTreasureCommand = new CommandAction(ChoiceTreasure);
            //NewItemCommand = new CommandAction(NewItem);

            SetMinCommand = new CommandAction(SetMin);
            SetMaxCommand = new CommandAction(SetMax);
            MaxOutCommand = new CommandAction(MaxOut);
            MonsterUpgradeCommand = new CommandAction(MonsterUpgrade);
            CoinUpgradeCommand = new CommandAction(CoinUpgrade);

            var json = SaveData.Instance().Json;
            if (json == null) return;

            General = new General();
            Player = new Player();

            var items = new ObservableCollection<Item>();

            foreach (var obj in json["SaveData"]["BelongingsItem"]["ItemList"])
            {
                items.Add(new Item((JObject)obj));
            }

            Items = new ObservableCollection<Item>(items.OrderBy(x => x.GetOrder));

            foreach (var obj in json["SaveData"]["History"]["TreasureQuality"])
            {
                Treasures.Add(new Treasure((JObject)obj));
            }

            foreach (var obj in json["SaveData"]["StockMonster"]["StockMonsters"])
            {
                Monsters.Add(new Monster((JObject)obj));
            }

            foreach (var obj in json["SaveData"]["BelongingsCoin"]["Coins"])
            {
                var param = obj["Param"];
                Coins.Add(new Coin((JObject)param));
            }
        }

        private void FileSave(object? parameter)
        {
            SaveData.Instance().Save();
        }

        private void FileSaveAs(object? parameter)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            if (dlg.ShowDialog() == false) return;

            SaveData.Instance().SaveAs(dlg.FileName);
        }

        private void MaxOut(object? parameter)
        {
            foreach (var item in Items)
            {
                var min = Info.Instance().Search(Info.Instance().Item, item.ID)?.MinValue;

                if (min != null && min > 1)
                {
                    var max = Info.Instance().Search(Info.Instance().Item, item.ID)?.MaxValue;

                    item.Count = (uint)max;
                }
            }
        }

        private void CoinUpgrade(object? parameter)
        {
            foreach (var coin in Coins)
            {
                coin.Rarity = 3;

                var features = new List<CoinFeature>();

                foreach (var f in coin.FeatureList)
                {
                    features.Add(new CoinFeature()
                    {
                         featureId = f.featureId,
                         step = 9
                    });
                }

                coin.FeatureList = features.ToArray();
            }
        }

        private void MonsterUpgrade(object? parameter)
        {
            foreach (var mon in Monsters)
            {
                mon.Exp = 999999999;
                mon.HP = 9999;
                mon.MP = 9999;

                mon.mIndividualStatus = new MonsterStatus()
                {
                    MaxHP = 9999,
                    MaxMP = 9999,
                    Attack = 999,
                    Defense = 999,
                    Dexterity = 999,
                    Magic = 999
                };
            }
        }

        private void Import(object? parameter)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == false) return;

            SaveData.Instance().Import(dlg.FileName);
        }

        private void Export(object? parameter)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = "json";
            dlg.Filter = "json|*json";
            if (dlg.ShowDialog() == false) return;

            SaveData.Instance().Export(dlg.FileName);
        }

        private void ChoiceItem(object? parameter)
        {
            var item = parameter as Item;
            if (item == null) return;

            item.ID = CreateChoiceWindow(item.ID, ChoiceWindow.eType.eItem);
        }

        private void ChoiceMonster(object? parameter)
        {
            var item = parameter as Monster;
            if (item == null) return;

            item.ID = CreateChoiceWindow(item.ID, ChoiceWindow.eType.eMonster);
        }

        private void ChoiceTreasure(object? parameter)
        {
            var item = parameter as Treasure;
            if (item == null) return;

            item.ID = CreateChoiceWindow(item.ID, ChoiceWindow.eType.eTreasure);
        }

        private void SetMin(object? parameter)
        {
            var item = parameter as Item;
            if (item == null) return;

            item.Count = 1;
        }

        private void SetMax(object? parameter)
        {
            var item = parameter as Item;
            if (item == null) return;

            item.Count = 999;
        }

        private uint CreateChoiceWindow(uint id, ChoiceWindow.eType type)
        {
            var window = new ChoiceWindow();
            window.ID = id;
            window.Type = type;
            window.ShowDialog();
            return window.ID;
        }
    }
}
