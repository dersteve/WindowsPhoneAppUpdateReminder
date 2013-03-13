using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Hedgehog.UpdateReminder.UpdateValidator
{
    public class FileUpdateValidator : IUpdateValidator
    {
        public string VersionFileUrl { get; set; }
        public string CurrentVersion { get; set; }

        public FileUpdateValidator(string versionFileUrl)
        {
            this.VersionFileUrl = versionFileUrl;
        }

        public void IsUpdateAvailableAsync()
        {
            if (string.IsNullOrWhiteSpace(VersionFileUrl))
                throw new ArgumentNullException("VersionFileIUrl");

            if (string.IsNullOrWhiteSpace(CurrentVersion))
                throw new ArgumentNullException("CurrentVersion");

            var client = new WebClient();
            client.DownloadStringCompleted += client_DownloadStringCompleted;
            client.DownloadStringAsync(new Uri(this.VersionFileUrl));
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            var args = new IsUpdateAvailableCompletedEventArgs { IsUpdateAvailable = false };

            if (e.Error == null)
            {
                var result = e.Result;
                args.IsUpdateAvailable = result != this.CurrentVersion;
            }
            else
            {
                // log error
            }

            OnIsUpdateAvailableCompleted(args);
        }

        public event EventHandler<IsUpdateAvailableCompletedEventArgs> IsUpdateAvailableCompleted;
        public void OnIsUpdateAvailableCompleted(IsUpdateAvailableCompletedEventArgs args)
        {
            if (IsUpdateAvailableCompleted != null)
            {
                IsUpdateAvailableCompleted(this, args);
            }
        }

    }
}
