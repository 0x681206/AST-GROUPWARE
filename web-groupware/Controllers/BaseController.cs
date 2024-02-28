using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Microsoft.Data.SqlClient;
using web_groupware.Data;
using web_groupware.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;

#pragma warning disable CS8600, CS8601, CS8602, CS8604, CS8618, CS8629
namespace web_groupware.Controllers
{
    public class BaseController : Controller
    {
        #region "member"
        /// <summary>
        /// データベース接続　サービス
        /// </summary>
        protected readonly web_groupwareContext _context;

        /// <summary>
        /// クライアントHTTP　サービス
        /// </summary>
        protected IHttpContextAccessor _httpContextAccessor;

        /// <summary>設定値</summary>
        protected IConfiguration _config;
        /// <summary>ログ出力</summary>
        protected readonly ILogger<BaseController> _logger;

        /// <summary>
        /// 実行環境
        /// </summary>
        IWebHostEnvironment _hostingEnvironment;

        #endregion

        /// <summary>
        /// 本来のコンストラクタ
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        /// <param name="loginInfoRepository"></param>
        /// <param name="httpContextAccessor"></param>
        public BaseController(
            IConfiguration configuration,
            ILogger<BaseController> logger,
            web_groupwareContext context,
            IWebHostEnvironment hostingEnvironment,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _config = configuration;
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }
        /// <summary>
        /// API ログイン認証されているか確認
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult LoginState()
        {
            //認証情報から氏名を取得する
            var claim = HttpContext.User.FindFirst(ClaimTypes.Name);

            if (claim == null)
            {
                return Json(new { success = false, message = "認証されていません" });
            }

            return Json(new { success = true, message = "認証されています。" });

        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// T_NOから最新の番号を取得し、+1で更新
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public int GetNextNo(int data_type)
        {
            T_NO recoard_no = _context.T_NO.FirstOrDefault(x => x.data_type == data_type);
            if (recoard_no == null)
            {
                var item = new T_NO
                {
                    data_type = data_type,
                    no = 2,
                };

                _context.T_NO.Add(item);
                _context.SaveChanges();

                return 1;
            }
            var nextNo = recoard_no.no;
            recoard_no.no = recoard_no.no + 1;
            _context.SaveChanges();
            return nextNo;

        }


        /// <summary>
        /// 社内通知取得
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> GetNoticeHeader()
        {

            var notice = await _context.T_INFO.FirstOrDefaultAsync(x => x.info_cd == 2);
            var list_file = new List<string>();
            var files = _context.T_INFO_FILE.Where(x => x.info_cd == 2).ToList();
            for (int i = 0; i < files.Count; i++)
            {
                list_file.Add(files[i].fileName);
            }
            var ret = new { 
                message = notice == null ? "" : notice.message
                , files = list_file
            };
            return new JsonResult(ret);

        }
        [Authorize]
        public async Task<IActionResult> GetGroupItems()
        {
            var groups = await _context.T_GROUPM.ToListAsync();
            var result = new List<object>();

            foreach (var group in groups)
            {
                // Count the number of users in the group
                var userCount = await _context.T_GROUPSTAFF
                    .Where(gs => gs.group_cd == group.group_cd)
                    .Select(gs => gs.staf_cd) // Select the user IDs
                    .Distinct() // Ensure unique user IDs
                    .CountAsync(); // Count the distinct users

                result.Add(new
                {
                    group.group_cd,
                    group.group_name,
                    user_count = userCount
                });
            }

            return Json(result);
        }

        /// <summary>
        /// 社内通知取得
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> GetReportCount()
        {
            var staf_cd = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
            var records = await Task.Run(() => _context.T_REPORTCOMMENT_READ.Where(x => x.staf_cd == int.Parse(staf_cd) && x.alreadyread_flg == false && x.update_user != staf_cd.ToString())); 
                //_context.T_REPORTCOMMENT_READ.Where(x => x.staf_cd == staf_cd && x.alreadyread_flg ==false && x.update_user!=staf_cd.ToString());
            string count = records.Count() == 0 ? "":records.Count().ToString();
            var arrMessage = new string[1]
        {
                            count
        };
            return new JsonResult(arrMessage);

        }
        /// <summary>
        /// 物件メモ未読
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> GetBukkenCommentReadCount()
        {
            //var staf_cd= int.Parse(HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value);
            var staf_cd= HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
            var records = await Task.Run(() => _context.T_BUKKENCOMMENT_READ.Where(x => x.staf_cd == int.Parse(staf_cd) && x.alreadyread_flg == false && x.update_user != staf_cd.ToString()));
            //_context.T_REPORTCOMMENT_READ.Where(x => x.staf_cd == staf_cd && x.alreadyread_flg ==false && x.update_user!=staf_cd.ToString());
            string count = records.Count() == 0 ? "": records.Count().ToString();
            var ret = new
            {
                count = count
            };
            return new JsonResult(ret);
        }


        /// <summary>
        /// 差分メッセージ取得
        /// </summary>
        /// <param name="ymd"></param>
        /// <returns></returns
        public string GetWhenFromNow(DateTime ymd)
        {
            string message;
            DateTime now = DateTime.Now;

            DateTime lastYear1st = new DateTime(now.AddYears(-1).Year, 1, 1);
            DateTime thisYear1st = new DateTime(now.Year, 1, 1);
            DateTime thisMonth1st = new DateTime(now.Year, now.Month, 1);
            DateTime today0am = new DateTime(now.Year, now.Month, now.Day);
            //TimeSpan ts1 = now - ymd;
            //int difHours = ts1.Hours;
            //int difMinutes = ts1.Minutes;
            if (ymd < lastYear1st)
            {
                message = "去年以前";
            }
            else if (ymd < thisYear1st)
            {
                message = "去年";
            }
            else if (GetDATEDIFF("month", ymd, now) > 1)
            {
                message = GetDATEDIFF("month", ymd, now) + "か月前";
            }
            else if (ymd < thisMonth1st)
            {
                message = "先月";
            }
            else if (GetDATEDIFF("week", ymd, now) > 0)
            {
                message = GetDATEDIFF("week", ymd, now) + "週間前";
            }
            else if (GetDATEDIFF("day", ymd, now) > 1)
            {
                message = GetDATEDIFF("day", ymd, now) + "日前";
            }
            else if (ymd < today0am)
            {
                message = "昨日";
            }
            else if (GetDATEDIFF("hour", ymd, now) > 0)
            {
                message = GetDATEDIFF("hour", ymd, now) + "時間前";
            }
            else
            {
                message = GetDATEDIFF("minute", ymd, now) + "分前";
            }
            return message;

        }

        /// <summary>
        /// 月差分取得（C#にDATEDIFFが存在しないのでSQLで代用）
        /// </summary>
        /// <param name="span">day,month,year</param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns
        public int GetDATEDIFF(string span,DateTime start,DateTime end)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT ");
            sql.AppendFormat(" DATEDIFF({0}, '{1}', '{2}') as diff ",span,start,end);
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandText = sql.ToString();
                    var dataAdapter = new SqlDataAdapter(cmd);
                    dataAdapter.Fill(dt);
                }
            }
            return dt.Rows[0].Field<int>("diff");

        }


    }
}