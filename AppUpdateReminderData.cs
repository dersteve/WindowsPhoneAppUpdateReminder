using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Hedgehog.UpdateReminder
{
    [XmlRoot]
    public class AppUpdateReminderData
    {
        [XmlElement]
        public int NumberOfAppRunsAfterLastReminder { get; set; }
    }
}
