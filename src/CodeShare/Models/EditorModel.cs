using System.ComponentModel.DataAnnotations;

namespace CodeShare.Models
{
    public class EditorModel
    {
        public string Lang { get; set; } = "abap";
        public string Theme { get; set; } = "vs-dark";
        [DataType (DataType.MultilineText)]
        public string Code { get; set; }
    }
}