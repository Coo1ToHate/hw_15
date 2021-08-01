using System.IO;

namespace hw_15.ViewModel
{
    class LogViewModel : ApplicationViewModel
    {
        private string log;

        public string Log
        {
            get => log;
            set => log = value;
        }

        public LogViewModel(string path)
        {
            if (File.Exists(path))
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    Log = sr.ReadToEnd();
                }
            }
        }
    }
}
