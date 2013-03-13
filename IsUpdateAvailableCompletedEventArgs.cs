using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Hedgehog.UpdateReminder
{
    public class IsUpdateAvailableCompletedEventArgs : AsyncCompletedEventArgs
    {
        public bool IsUpdateAvailable { get; set; }
    }
}
