using System.ComponentModel.DataAnnotations;
using Common.Constants;
using Newtonsoft.Json;
#pragma warning disable CS8600,CS8602,CS8604,CS8618
namespace web_groupware.Models
{
    public class BukkenMemoViewModel
    {
        [MaxLength(60, ErrorMessage = Messages.MAXLENGTH)]
        public string? cond_bukken_name { get; set; }

        public List<BukkenMemo> list_bukken = new List<BukkenMemo>();
    }
    public class BukkenMemo
    {
        public decimal bukken_cd { get; set; }

        public string? bukken_name { get; set; }
        public string update_user { get; set; }
        public string update_date { get; set; }
    }

}