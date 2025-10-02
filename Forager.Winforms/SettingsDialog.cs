using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forager {
    public partial class SettingsDialog : Form {
        public int NumShrooms => (int)numShroomsNUD.Value;
        public int FieldSize => (int)fieldSizeNUD.Value;

        public SettingsDialog(int curNumShrooms, int curFieldSize) {
            InitializeComponent();
            numShroomsNUD.Value = curNumShrooms;
            fieldSizeNUD.Value = curFieldSize;
        }

        private void button1_Click(object sender, EventArgs e) => DialogResult = DialogResult.OK;

        private void button2_Click(object sender, EventArgs e) => DialogResult = DialogResult.Cancel;
    }
}
