using Common.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web_groupware.Models
{
    public class PlaceDetailModel
    {
        [DisplayName("施設コード")]
        [Required(ErrorMessage = Messages.REQUIRED)]
        [Range(1, 1000000000, ErrorMessage = Messages.RANGE)]
        public int place_cd { get; set; }

        [DisplayName("施設名")]
        [MaxLength(10, ErrorMessage = Messages.MAXLENGTH)]
        public string? place_name { get; set; }

        [DisplayName("並び順")]
        [Required(ErrorMessage = Messages.REQUIRED)]
        [Range(1, 1000000000, ErrorMessage = Messages.RANGE)]
        public int sort { get; set; }
    }
    public class PlaceViewModel
    {
        public List<PlaceDetailModel> placeList = new();
    }
}