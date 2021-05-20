using System;
using System.Threading;
using System.Windows.Forms;

namespace NTS_Reader_CS
{
    public partial class LoadingForm : Form
    {
       
        public Action Function { get; set; }

        public LoadingForm()

        {
            InitializeComponent();
            this.Shown += new EventHandler(Form_Loaded);
        }

        private void Form_Loaded(object sender, EventArgs e)
        {
            var thread = new Thread(
                () =>
                {
                    Function?.Invoke();
                    this.Invoke(
                        (Action)(() =>
                        {
                            this.Close();
                        }));
                });
            thread.Start();
        }
    }
}
