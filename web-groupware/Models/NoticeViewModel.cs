using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
#pragma warning disable CS8600,CS8601,CS8602,CS8604,CS8618,CS8629
namespace web_groupware.Models
{
    public class NoticeViewModel
    {
        public string? Title { get; set; }
        [DisplayName("コメント")]
        //[Required(ErrorMessage = Common.Constants.Messages.REQUIRED)]
        [StringLength(200, ErrorMessage = Common.Constants.Messages.MAXLENGTH)]
        public string? Message { get; set; }
        [DisplayName("ファイル")]
        public List<IFormFile> File { get; set; }=new List<IFormFile>();
        public List<T_INFO_FILE> List_T_INFO_FILE { get; set; }=new List<T_INFO_FILE>();
        public string Work_dir { get; set; }
} 
}