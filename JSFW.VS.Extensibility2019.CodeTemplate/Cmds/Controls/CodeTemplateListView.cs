using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace JSFW.VS.Extensibility2019.CodeTemplate.Cmds.Controls
{
    public partial class CodeTemplateListView : UserControl
    {
        readonly string ___KEY_______ = "$CHANGE_CODE$";
        readonly string ___KEY_DAY___ = "$CHANGE_DAY$";
        readonly string ___KEY_TIME__ = "$CHANGE_TIME$";
        
        public event Action<string> ExportToString = null;
        public event Func<string> ImportToString = null;

        public CodeTemplateListView()
        {
            InitializeComponent();
        }

        internal void ListRefresh()
        {
            for (int loop = listView1.Items.Count - 1; loop >= 0; loop--)
            {
                listView1.Items[loop].SubItems.Clear();
            }
            listView1.Items.Clear();
            listView1.Groups.Clear();
            listView1.ShowGroups = true;

            string root = CodeConvertPackage._DIR_JSFW + "CodeTemplate\\";

            if (Directory.Exists(root) == false)
            {
                Directory.CreateDirectory(root);
            }

            string[] dirs = Directory.GetDirectories(root);

            foreach (string dir in dirs)
            {
                string groupName = dir.Substring(root.Length);

                ListViewGroup group =  listView1.Groups.Add("G_" + groupName, groupName);
                string[] files = Directory.GetFiles(dir);
                foreach (string file in files)
                {
                    string templateName = file.Substring((dir+"\\").Length);
                    string ext = Path.GetExtension(templateName);

                    if (string.IsNullOrWhiteSpace(ext) == false)
                    {
                        templateName = templateName.Substring(0, templateName.Length - ext.Length);
                    }

                    var item = listView1.Items.Add(templateName);
                    item.Group = group;
                    item.SubItems.Add(file);
                }
            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems == null || listView1.SelectedItems.Count < 0) return;

            ListViewItem codeTemplateSelectItem = listView1.SelectedItems[0];

            string codeTemplateFileName = codeTemplateSelectItem.SubItems[1].Text;

            if (File.Exists(codeTemplateFileName))
            {
                string content = File.ReadAllText( codeTemplateFileName, Encoding.UTF8 );

                string word = "";
                if (ImportToString != null) word = ImportToString();
                if (ExportToString != null)
                    ExportToString( content.Replace(___KEY_______, word)
                                           .Replace(___KEY_DAY___, DateTime.Now.ToShortDateString())
                                           .Replace(___KEY_TIME__, DateTime.Now.ToString("HH:mm:ss")));
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (CreateCodeTmpForm cct = new CreateCodeTmpForm())
            {
                cct.ShowDialog();
                ListRefresh();
            }
        }
    }
}
