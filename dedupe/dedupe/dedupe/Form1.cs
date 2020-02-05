using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using GUKV.ImportToolUtils;
using System.Data.SqlClient;

namespace dedupe
{
    public partial class Form1 : Form
    {
        
        private StreetGroups streetDupes;
        private StreetGroups savedGroups;

        private BuildGroups buildDupes;
        private BuildGroups savedBuildGroups;

        public Form1()
        {
            savedGroups = new StreetGroups();
            savedBuildGroups = new BuildGroups();
            InitializeComponent();
        }

        private void LoadStreetDupesFromBase() {
            streetDupes = new StreetGroups();            
            var data = new Context().Streets;

            var dataMaterial = data.OrderBy(s => s.Name).ToList();

            //разпределяем улицы по группам
            var i = 1;
            foreach (var item in dataMaterial)
            {
                var streetGroup = streetDupes.FindByName(item.Name, (int)maxDistance.Value);
                if (streetGroup.IsEmpty())
                {
                    streetGroup.GroupName = item.Name;
                    streetGroup.GroupId = i++;
                    streetDupes.Items.Add(streetGroup);
                }
                streetGroup.StreetsInGroup.Add(item);

            }
        }

        private void LoadBuildingDupesFromBase()
        {
            buildDupes = new BuildGroups();
            var data = new Context().BuildingsFull;
            
            var dataMaterial = data.OrderBy(b => b.Addr_Street_Name).ThenBy(b => b.Addr_Nomer).Where(b => b.Master_Building_Id == null).ToList();

            //разпределяем дома по группам
            var i = 1;
            foreach (var item in dataMaterial)
            {
                var address = new GUKV.ImportToolUtils.ObjectFinder.UnifiedAddress();
                address.streetName = new ObjectFinder.StreetName(item.addr_street_id.ToString());
                ObjectFinder.ParseAddressNumbers(item.Addr_Nomer, address);
                item.normalizedAddress = address.FormatFullAddress();
                var buildGroup = buildDupes.FindByName(item.addr_street_id, item.Addr_Nomer, item.normalizedAddress, (int)maxDistanceBuildings.Value);
                if (buildGroup.IsEmpty())
                {
                    buildGroup.StreetName = item.Addr_Street_Name;
                    buildGroup.NormalizedBuild = item.normalizedAddress;
                    buildGroup.StreetId = item.addr_street_id;
                    buildGroup.BuildName = item.Addr_Nomer;
                    buildGroup.LastModifiedDate = item.modify_date == null ? new DateTime(1700, 1, 1) : item.modify_date;
                    buildGroup.GroupId = i++;
                    buildDupes.Items.Add(buildGroup);
                }
                buildGroup.BuildsInGroup.Add(item);
            }
        }

        private void ShowStreetDupes() {
            lvStreets.Items.Clear();
            //строим визуальный список дублей для групп содержащих более 1й улицы
            foreach (var item in streetDupes.Items)
            {
                if (item.StreetsInGroup.Count > 1)
                {
                    ListViewItem lvItem = new ListViewItem();
                    lvItem.Text = item.GroupId.ToString();
                    lvItem.SubItems.Add(item.GroupName);
                    lvItem.SubItems.Add(item.StreetsInGroup.Count.ToString());
                    lvStreets.Items.Add(lvItem);
                }
            }
        }

        private void ShowBuildDupes()
        {
            lvBuildings.Items.Clear();
            int cnt = 0;
            //строим визуальный список дублей для групп содержащих более 1го здания
            foreach (var item in buildDupes.Items)
            {
                if (item.BuildsInGroup.Count > 1)
                {
                    ListViewItem lvItem = new ListViewItem();
                    lvItem.Text = item.GroupId.ToString();
                    lvItem.SubItems.Add(item.StreetName);
                    lvItem.SubItems.Add(item.BuildName);
                    lvItem.SubItems.Add(item.BuildsInGroup.Count.ToString());
                    lvBuildings.Items.Add(lvItem);
                    cnt++;
                }
            }
            label7.Text = cnt.ToString();
        }    

        private void btnLoadStreetGroups(object sender, EventArgs e)
        {
            LoadStreetDupesFromBase();
            ShowStreetDupes();           
        }        

        private void lvStreets_Click(object sender, EventArgs e)
        {
            ClearNewStreetGroupArea();
            lvStreetsDuplicates.Items.Clear();
            //найдем группу и выведем ее содержимое
            int selectedGroupId = int.Parse(lvStreets.FocusedItem.Text);
            foreach (var item in streetDupes.Items)
            {
                if (item.GroupId == selectedGroupId) {
                    foreach (var street in item.StreetsInGroup)
                    {
                        ListViewItem lvItem = new ListViewItem();
                        lvItem.Text = street.Id.ToString();
                        lvItem.SubItems.Add(street.Name);
                        lvStreetsDuplicates.Items.Add(lvItem);
                    }
                    break;
                }                
            }
        }
        
        private void lvStreetsDuplicates_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mainStreetNode = lvStreetsDuplicates.FocusedItem;
            if (mainStreetNode != null)
            {
                label3.Text = mainStreetNode.Text + " = " + mainStreetNode.SubItems[1].Text;
            }
            else {
                label3.Text = "";
            }

            var checkedDuplicates = lvStreetsDuplicates.CheckedItems.Cast<ListViewItem>().Where(i => i.Text != mainStreetNode.Text).ToList();
            lvChildGroups.Items.Clear();
            
            foreach (var node in checkedDuplicates) {
                 ListViewItem lvItem = new ListViewItem();
                 lvItem.Text = node.Text;
                 lvItem.SubItems.Add(node.SubItems[1]);
                 lvChildGroups.Items.Add(lvItem);
            }
            btnSaveGroup.Enabled = mainStreetNode != null && checkedDuplicates.Any();
        }

        private void ClearNewStreetGroupArea() {
            label3.Text = "";
            btnSaveGroup.Enabled = false;
            lvChildGroups.Items.Clear();
        }


        private void ClearNewBuildGroupArea()
        {
            label4.Text = "";
            btnSaveBuildingsGroup.Enabled = false;
            lvBuildChildGroups.Items.Clear();
        }

               

        private void btnSaveStreetGroup_Click(object sender, EventArgs e)
        {
            var newDuplicate = new StreetGroup();
            var mainStreetNode = lvStreetsDuplicates.FocusedItem;           
                
            newDuplicate.GroupId = int.Parse(mainStreetNode.Text);
            newDuplicate.GroupName = mainStreetNode.SubItems[1].Text;

            var checkedDuplicates = lvStreetsDuplicates.CheckedItems.Cast<ListViewItem>().Where(i => i.Text != mainStreetNode.Text).ToList();
            
            foreach (var node in checkedDuplicates)
            {
                newDuplicate.StreetsInGroup.Add(
                    new Street() { 
                        Id = int.Parse(node.Text),
                        Name = node.SubItems[1].Text
                    }
                );                
            }
            savedGroups.Items.Add(newDuplicate);

            ClearNewStreetGroupArea();

            //удалим из основного списка потенциальных групп отобранную группу дублей
            var groupToDelete = int.Parse(lvStreets.FocusedItem.Text);


            foreach (var item in streetDupes.Items)
            {
                if (item.GroupId == groupToDelete) {
                    streetDupes.Items.Remove(item);
                    ShowStreetDupes();           
                    break;
                }
            }
            lvStreetsDuplicates.Items.Clear();
        }


        private void btnSaveBuildingGroup_Click(object sender, EventArgs e)
        {
            var newDuplicate = new BuildGroup();
            var mainBuildNode = lvBuildingsDuplicates.FocusedItem;

            newDuplicate.GroupId = int.Parse(mainBuildNode.Text);
            newDuplicate.StreetName = mainBuildNode.SubItems[1].Text;
            newDuplicate.BuildName = mainBuildNode.SubItems[2].Text;

            var checkedDuplicates = lvBuildingsDuplicates.CheckedItems.Cast<ListViewItem>().Where(i => i.Text != mainBuildNode.Text).ToList();

            foreach (var node in checkedDuplicates)
            {
                newDuplicate.BuildsInGroup.Add(
                    new BuildingFull()
                    {
                        Id = int.Parse(node.Text),
                        Addr_Street_Name = node.SubItems[1].Text,
                        Addr_Nomer = node.SubItems[2].Text
                    }
                );
            }
            savedBuildGroups.Items.Add(newDuplicate);

            ClearNewBuildGroupArea();

            //удалим из основного списка потенциальных групп отобранную группу дублей
            var groupToDelete = int.Parse(lvBuildings.FocusedItem.Text);


            foreach (var item in buildDupes.Items)
            {
                if (item.GroupId == groupToDelete)
                {
                    buildDupes.Items.Remove(item);
                    ShowBuildDupes();
                    break;
                }
            }
            lvBuildingsDuplicates.Items.Clear();
        }

        private string BuildStreetScriptHeader() {
            var header = new StringBuilder("/*\n");
            header.Append("Этот скрипт объединяет дубликаты улиц. Г - главная улица которая остается Д - дубликат, будет удален после объединения\n");
            foreach (var group in savedGroups.Items)
            {
                header.Append(String.Format("Г {0} - {1}\n", group.GroupId, group.GroupName));
                if (!group.IsEmpty())
                {
                    foreach (var dupe in group.StreetsInGroup)
                    {
                        header.Append(String.Format("    Д {0} - {1}\n", dupe.Id, dupe.Name));
                    }
                }
            }
            header.Append("*/\n");
            return header.ToString();
        }

        private string BuildingsScriptHeader()
        {
            var header = new StringBuilder("/*\n");
            header.Append("Этот скрипт объединяет дубликаты зданий. Г - главное здание которое остается, Д - дубликат, будет удален после объединения\n");
            foreach (var group in savedBuildGroups.Items)
            {
                header.Append(String.Format("Г {0} - {1} {2}\n", group.GroupId, group.StreetName, group.BuildName));
                if (!group.IsEmpty())
                {
                    foreach (var dupe in group.BuildsInGroup)
                    {
                        header.Append(String.Format("    Д {0} - {1} {2}\n", dupe.Id, dupe.Addr_Street_Name, dupe.Addr_Nomer));
                    }
                }
            }
            header.Append("*/\n");
            return header.ToString();
        }

        private void UploadStreetdedupeScript_Click(object sender, EventArgs e)
        {
            var result = new StringBuilder(BuildStreetScriptHeader());
            var currentDir = Application.StartupPath;
            string dedupeTpl = File.ReadAllText(currentDir + "\\StreetDedupeTemplate.sql");
            string delTpl = File.ReadAllText(currentDir + "\\StreetDelDupesTemplate.sql");

            foreach (var group in savedGroups.Items) {
                if (!group.IsEmpty()) {
                    foreach (var dupe in group.StreetsInGroup)
                    {
                        if (result.Length > 0) {
                            result.Append("\nGO\n");
                        }
                        result.Append(String.Format(dedupeTpl, group.GroupId, dupe.Id));
                        result.Append(String.Format(delTpl, dupe.Id));
                        
                    }                    
                }
            }
            Clipboard.SetText(result.ToString());
            MessageBox.Show("Скрипт скопирован в буфер обмена");
        }
        private void UploadBuilddedupeAllScripts(object sender, EventArgs e)
        {   
            //заполним бувер отобранных групп автоматически по всем группам дубликатов, количество дублей в которых > 1
            foreach (var dupe in buildDupes.Items) {
                if (dupe.BuildsInGroup.Count > 1) {
                    var sortedDupeArray = dupe.BuildsInGroup.OrderByDescending(db => db.modify_date).ThenByDescending(db => db.Id).ToArray();
                    var newDuplicate = new BuildGroup();

                    newDuplicate.GroupId = sortedDupeArray[0].Id;
                    newDuplicate.StreetName = sortedDupeArray[0].Addr_Street_Name;
                    newDuplicate.BuildName = sortedDupeArray[0].Addr_Nomer;

                    for (var i = 1; i < sortedDupeArray.Length; i++) {
                        newDuplicate.BuildsInGroup.Add(
                            new BuildingFull()
                            {
                                Id = sortedDupeArray[i].Id,
                                Addr_Street_Name = sortedDupeArray[i].Addr_Street_Name,
                                Addr_Nomer = sortedDupeArray[i].Addr_Nomer
                            }
                        );
                    }
                    savedBuildGroups.Items.Add(newDuplicate);
                } 
            }

            //вызовем стандартную процедуру построения скрипта
            UploadBuilddedupeScript_Click(null, null);
        }

        private void UploadBuilddedupeScript_Click(object sender, EventArgs e)
        {
            var result = new StringBuilder(BuildingsScriptHeader());
            var currentDir = Application.StartupPath;
            string dedupeTpl = File.ReadAllText(currentDir + "\\BuildDedupeTemplate.sql");
            string delTpl = File.ReadAllText(currentDir + "\\BuildDelDupesTemplate.sql");

            foreach (var group in savedBuildGroups.Items)
            {
                if (!group.IsEmpty())
                {
                    foreach (var dupe in group.BuildsInGroup)
                    {
                        if (result.Length > 0)
                        {
                            result.Append("\nGO\n");
                        }
                        result.Append(String.Format(dedupeTpl, group.GroupId, dupe.Id));
                        result.Append(String.Format(delTpl, dupe.Id));
                    }
                }
            }
            Clipboard.SetText(result.ToString());
            MessageBox.Show("Скрипт скопирован в буфер обмена");
        }

        private void btnLoadBuildingsGroups(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            LoadBuildingDupesFromBase();
            ShowBuildDupes();
            Cursor.Current = Cursors.Default;
        }

        private void lvBuildings_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lvBuildings_Click(object sender, EventArgs e)
        {
            ClearNewBuildGroupArea();
            lvBuildingsDuplicates.Items.Clear();
            //найдем группу и выведем ее содержимое
            int selectedGroupId = int.Parse(lvBuildings.FocusedItem.Text);
            foreach (var item in buildDupes.Items)
            {
                if (item.GroupId == selectedGroupId)
                {
                    foreach (var build in item.BuildsInGroup)
                    {
                        ListViewItem lvItem = new ListViewItem();
                        lvItem.Text = build.Id.ToString();
                        lvItem.SubItems.Add(build.Addr_Street_Name);
                        lvItem.SubItems.Add(build.Addr_Nomer);
                        lvItem.SubItems.Add(build.normalizedAddress);
                        lvBuildingsDuplicates.Items.Add(lvItem);
                    }
                    break;
                }
            }
        }

        private void lvBuildingsDuplicates_SelectedIndexChanged(object sender, EventArgs e)
        {
            var mainBuildNode = lvBuildingsDuplicates.FocusedItem;
            if (mainBuildNode != null)
            {
                label4.Text = mainBuildNode.Text + " = " + mainBuildNode.SubItems[1].Text + " " +  mainBuildNode.SubItems[2].Text;
            }
            else
            {
                label4.Text = "";
            }

            var checkedDuplicates = lvBuildingsDuplicates.CheckedItems.Cast<ListViewItem>().Where(i => i.Text != mainBuildNode.Text).ToList();
            lvBuildChildGroups.Items.Clear();

            foreach (var node in checkedDuplicates)
            {
                ListViewItem lvItem = new ListViewItem();
                lvItem.Text = node.Text;
                lvItem.SubItems.Add(node.SubItems[1]);
                lvItem.SubItems.Add(node.SubItems[2]);
                lvBuildChildGroups.Items.Add(lvItem);
            }
            btnSaveBuildingsGroup.Enabled = mainBuildNode != null && checkedDuplicates.Any();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var selectedBuildIds = lvBuildingsDuplicates.Items.Cast<ListViewItem>().Select(b => int.Parse(b.Text)).ToList();
            if (selectedBuildIds.Any()) {
                var dlg = new Form2();
                dlg.LoadBuildingsGroup(selectedBuildIds);
                dlg.Show();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var selectedBuildIds = lvBuildingsDuplicates.CheckedItems.Cast<ListViewItem>().Select(b => int.Parse(b.Text)).ToList();
            if (selectedBuildIds.Any())
            {
                var dlg = new Form2();
                dlg.LoadBuildingsGroup(selectedBuildIds);
                dlg.Show();
            }
        }

        private void lvBuildings_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            buildDupes = new BuildGroups();
            var ctx = new Context();
            var data = ctx.BuildingsFull;

            var dataMaterial = data.OrderBy(b => b.Addr_Street_Name).ThenBy(b => b.Addr_Nomer).Where(b => b.Master_Building_Id == null).ToList();

                       
            foreach (var item in dataMaterial)
            {
                var address = new GUKV.ImportToolUtils.ObjectFinder.UnifiedAddress();
                address.streetName = new ObjectFinder.StreetName(item.Addr_Street_Name.ToString());
                ObjectFinder.ParseAddressNumbers(item.Addr_Nomer, address);
                ctx.Database.ExecuteSqlCommand("update buildings set normalized_number = @p_normalized_number where id = @p_id", 
                    new SqlParameter("@p_normalized_number", address.FormatFullAddress()),
                    new SqlParameter("@p_id", item.Id));                   
                    
            }
            Cursor.Current = Cursors.Default;
            MessageBox.Show("Нормализованные №№ вычислены");
        }          
         
                
    }
}
