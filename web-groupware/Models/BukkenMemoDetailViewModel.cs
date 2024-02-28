using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Common.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
#pragma warning disable CS8600,CS8602,CS8604,CS8618
namespace web_groupware.Models
{
    public class BukkenMemoDetailViewModel
    {
        public int bukken_cd { get; set; }
        public string? bukken_name { get; set; }
        public string? bukken_nameWithCode { get; set; }
        public string? zip { get; set; }
        public string? address1 { get; set; }
        public string? address2 { get; set; }
        public List<BukkenMemoDetail> list_detail = new List<BukkenMemoDetail>();
        [DisplayName("ƒRƒƒ“ƒg")]
        [Required(ErrorMessage = Messages.REQUIRED)]
        [MaxLength(1000, ErrorMessage = Messages.MAXLENGTH)]
        public string? message_new { get; set; }
        [DisplayName("’Ê’mÒ")]
        [Required(ErrorMessage = Messages.REQUIRED)]
        public List<string> list_selected_staf_cd { get; set; } 
        public List<SelectListItem> list_staff { get; set; } = new List<SelectListItem>();

    }
    public class BukkenMemoDetail
    {
        public string update_user { get; set; }
        public string update_date { get; set; }
        public string? message { get; set; }
    }

}