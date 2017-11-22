using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;

namespace GestionStockInformatique.Logger
{
    public class logger
    {
        public System.Windows.Forms.RichTextBox rTextbox { get; set; }
        public logger(System.Windows.Forms.RichTextBox _box)
        {
            rTextbox = _box;
        }

        public void addLog(LogType type, string content)
        {
            string log = "[" + type.ToString() + "](" + DateTime.Now.ToString() + ") " + content + Environment.NewLine;
            rTextbox.AppendText(log);
        }

    }

    public enum LogType {
        Erreur,
        Succes,
        Information
    }
}
