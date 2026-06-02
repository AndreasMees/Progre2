namespace KooliProjekt.PublicAPI;
    public class Result
    {
        public string Error { get; set; }

        public bool HasError
        {
            get
            {
                return !string.IsNullOrEmpty(Error);
            }
        }
    }

