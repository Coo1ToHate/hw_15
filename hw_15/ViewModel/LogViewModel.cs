using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace hw_15.ViewModel
{
    class LogViewModel : ApplicationViewModel
    {
        private string title;
        private DataTable logDataSet;

        public DataTable LogDataSet
        {
            get => logDataSet;
            set
            {
                logDataSet = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        public LogViewModel()
        {
            Title = "История действий";
            Task t2 = Task.Factory.StartNew(LoadLog);
        }

        private void LoadLog()
        {
            Title = "История действий - загрузка данных...";

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            StringBuilder sb = new StringBuilder();

            string[] files = Directory.GetFiles("logs");
            foreach (var f in files)
            {
                using (StreamReader sr = new StreamReader(f))
                {
                    sb.Append(sr.ReadToEnd());
                }
            }

            string[] qwe = sb.ToString().Split(new char[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);
            DataTable table = new DataTable();
            DataColumn column;
            DataRow row;
            column = new DataColumn();
            column.ColumnName = "msg";
            table.Columns.Add(column);
            foreach (var s in qwe)
            {
                row = table.NewRow();
                row["msg"] = s;
                table.Rows.Add(row);
            }
            LogDataSet = table;

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";

            Title = $"История действий - загрузка прошла за {elapsedTime}";
        }
    }
}