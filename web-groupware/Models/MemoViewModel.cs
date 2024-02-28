#pragma warning disable CS8600,CS8601,CS8602,CS8604,CS8618,CS8629

namespace web_groupware.Models
{
    public class MemoModel
    {
        public int memo_no { get; set; }
        public string? create_date { get; set; }
        public int state { get; set; }
        public int receiver_type { get; set; }
        public int receiver_cd { get; set; }
        public string? receiver_name { get; set; }
        public string? comment_no { get; set; }
        public string? phone { get; set; }
        public string? content { get; set; }
        public string? sender_name { get; set; }
        public bool is_editable { get; set; }
        public string? readers { get; set; }
        public string? working_msg { get; set; }
        public string? finish_msg { get; set; }
    }
    public class MemoComment
    {
        public string comment_no { get; set; }
        public string? comment { get; set; }
    }
    public class MemoViewModelStaff
    {
        public int staff_cd { get; set; }
        public string? staff_name { get; set; }
    }
    public class MemoViewModelGroup
    {
        public int group_cd { get; set; }
        public string? group_name { get; set; }
    }

    public class MemoViewModel
    {
        public List<MemoModel>? memoList = new List<MemoModel>();
        public List<MemoViewModelStaff>? staffList = new List<MemoViewModelStaff>();
        public List<MemoViewModelGroup>? groupList = new List<MemoViewModelGroup>();
        public List<MemoComment>? commentList = new List<MemoComment>();
        public int selectedState = 0;
        public int selectedUser = 0;
        public bool isSent = true;
    }

    public class CreateUpdateMemoRequest
    {
        public int memo_no { get; set; }
        public int receiver_type { get; set; }
        public int receiver_cd { get; set; }
        public string comment_no { get; set; }
        public string phone { get; set; }
        public string content { get; set; }
        public int working { get; set; }
        public int finish { get; set; }
    }
    public class UpdateMemoStateRequest
    {
        public int memo_no { get; set; }
        public int state { get; set; }
    }
}