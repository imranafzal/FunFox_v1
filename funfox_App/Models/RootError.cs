using static System.Runtime.InteropServices.JavaScript.JSType;

namespace funfox_App.Models
{
    public class RootError
    {
        public string type { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public string detail { get; set; }
        public Errors errors { get; set; }
    }
}
