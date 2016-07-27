using System;
using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using Docker.PowerShell.Objects;
using Newtonsoft.Json;

namespace Docker.PowerShell.Cmdlets
{
    internal class JsonMessageWriter
    {
        public JsonMessageWriter(PSCmdlet cmdlet)
        {
            _cmdlet = cmdlet;
        }

        public async Task WriteJsonMessages(Stream stream)
        {
            using (var progressReader = new StreamReader(stream, new UTF8Encoding(false)))
            {
                string line;
                while ((line = await progressReader.ReadLineAsync()) != null)
                {
                    var message = JsonConvert.DeserializeObject<JsonMessage>(line);
                    WriteJsonMessage(message);
                }
            }
        }

        public void WriteJsonMessage(JsonMessage message)
        {
            if (message.Error != null)
            {
                var error = new ErrorRecord(new Exception(message.Error.Message), null, ErrorCategory.OperationStopped, null);
                _cmdlet.WriteError(error);
            }
            else if (message.Progress != null)
            {
                var id = message.ID ?? "";
                int activity;
                if (!_idToActivity.TryGetValue(id, out activity))
                {
                    activity = _nextActivity;
                    _nextActivity++;
                    _idToActivity.Add(id, activity);
                }

                var activityName = new StringBuilder(id);
                if (activityName.Length == 0)
                {
                    activityName.Append("Operation");
                }

                if (message.From != null)
                {
                    activityName.AppendFormat("(from {0})", message.From);
                }

                var record = new ProgressRecord(activity, activityName.ToString(), message.Status ?? "Processing");

                var progress = message.Progress;
                if (progress.Total > 0 && progress.Current <= progress.Total)
                {
                    record.PercentComplete = (int)(progress.Current * 100 / progress.Total);
                }

                if (progress.Current > 0)
                {
                    record.CurrentOperation = string.Format(" ({0} bytes)", progress.Current);
                }

                _cmdlet.WriteProgress(record);
            }
            else
            {
                var info = new StringBuilder();
                if (message.ID != null)
                {
                    info.Append(message.ID);
                    info.Append(": ");
                }

                if (message.From != null)
                {
                    info.AppendFormat("(from {0})", message.From);
                }

                var infoRecord = new HostInformationMessage();
                if (message.Stream != null)
                {
                    info.Append(message.Stream);
                    infoRecord.NoNewLine = true;
                }
                else
                {
                    info.Append(message.Status);
                }

                infoRecord.Message = info.ToString();
                _cmdlet.WriteInformation(infoRecord, new string[] { "PSHOST" });
            }
        }

        public void ClearProgress()
        {
            foreach (var activity in _idToActivity)
            {
                var record = new ProgressRecord(activity.Value, "Operation", "Processing");
                record.RecordType = ProgressRecordType.Completed;
                _cmdlet.WriteProgress(record);
            }
        }

        private Dictionary<string, int> _idToActivity = new Dictionary<string, int>();
        private int _nextActivity = 0x10000;
        private PSCmdlet _cmdlet;
    }
}