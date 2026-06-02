using System.Collections.Generic;

namespace KooliProjekt.PublicAPI
{
    public class Result
    {
        public string? Error { get; set; }
        public bool HasError => !string.IsNullOrEmpty(Error) || (Errors != null && Errors.Count > 0);

        // UUS: Serveri valideerimise vigade loend (Väli -> Veateated)
        public Dictionary<string, List<string>> Errors { get; set; } = new Dictionary<string, List<string>>();
    }

    public class Result<T> : Result
    {
        public T? Value { get; set; }
    }
}