using Microsoft.Phone.Tasks;
using System;
using System.Windows;

namespace Hedgehog.UpdateReminder
{
    public class AppUpdateReminder
    {
        /// <summary>
        /// Caption of the message box shwon, when reminded of available update
        /// </summary>
        public string MessageBoxCaption { get; set; }
        /// <summary>
        /// Body of the message box shown, when reminded of available update
        /// </summary>
        public string MessageBoxMessage { get; set; }
        /// <summary>
        /// Specifies how often the app checks for available updates;
        /// E.g. by setting RecurrencePerUsageCount = 5 every 5 times a user opens the app it'll check for an update
        /// </summary>
        public int? RecurrencePerUsageCount { get; set; }
        /// <summary>
        /// Specifies the current version of the running app
        /// </summary>
        public string CurrentVersion { get; set; }
        /// <summary>
        /// Implemention of the actual validator
        /// </summary>
        public IUpdateValidator UpdateValidator { get; set; }

        /// <summary>
        /// Provider to persist the data, e.g. custom xml, database
        /// </summary>
        protected ReminderDataPersistenceFileProvider<AppUpdateReminderData> PersistenceProvider { get; set; }

        int? _numberOfUsagesAfterLastReminder;
        protected int NumberOfUsagesAfterLastReminder
        {
            get
            {
                if (_numberOfUsagesAfterLastReminder == null)
                {
                    var data = PersistenceProvider.LoadData();
                    _numberOfUsagesAfterLastReminder = data.NumberOfAppRunsAfterLastReminder;
                }
                return _numberOfUsagesAfterLastReminder.GetValueOrDefault(0);
            }
            set
            {
                this._numberOfUsagesAfterLastReminder = value;
                PersistenceProvider.SaveData(new AppUpdateReminderData { NumberOfAppRunsAfterLastReminder = value });
            }
        }

        public AppUpdateReminder()
        {
            PersistenceProvider = new ReminderDataPersistenceFileProvider<AppUpdateReminderData>()
            {
                FilePath = "AppUpdateReminderData.xml"
            };
            PersistenceProvider.EnsureFileExistence(new AppUpdateReminderData { NumberOfAppRunsAfterLastReminder = _numberOfUsagesAfterLastReminder.GetValueOrDefault(0) });

            this.MessageBoxCaption = "Update available";
            this.MessageBoxMessage = "Good news: A new version of the app is available to download. Do you want to update straight away?";
        }

        public void Init()
        {
            if (this.UpdateValidator == null)
                throw new ArgumentNullException("UpdateValidator");

            NumberOfUsagesAfterLastReminder++;

            if (NumberOfUsagesAfterLastReminder >= RecurrencePerUsageCount.GetValueOrDefault(0))
            {
                NumberOfUsagesAfterLastReminder = 0;
                UpdateValidator.CurrentVersion = this.CurrentVersion;
                UpdateValidator.IsUpdateAvailableCompleted += UpdateValidator_IsUpdateAvailableCompleted;
                UpdateValidator.IsUpdateAvailableAsync();
            }
        }

        void UpdateValidator_IsUpdateAvailableCompleted(object sender, IsUpdateAvailableCompletedEventArgs e)
        {
            if (e.IsUpdateAvailable)
            {
                var result = MessageBox.Show(MessageBoxMessage, MessageBoxCaption, MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    MarketplaceDetailTask task = new MarketplaceDetailTask();
                    task.Show();
                }
            }
        }
    }
}
