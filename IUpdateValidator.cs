using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hedgehog.UpdateReminder
{
    public interface IUpdateValidator
    {
        string CurrentVersion { get; set; }

        void IsUpdateAvailableAsync();
        event EventHandler<IsUpdateAvailableCompletedEventArgs> IsUpdateAvailableCompleted;
    }
}
