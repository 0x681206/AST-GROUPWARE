using System.Data;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json.Linq;
using web_groupware.Data;
using web_groupware.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
#pragma warning disable CS8600,CS8601,CS8602,CS8604,CS8618,CS8629
namespace web_groupware.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="httpContextAccessor"></param>
        public ReportController(IConfiguration configuration, ILogger<BaseController> logger, web_groupwareContext context, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor) : base(configuration, logger, context, hostingEnvironment, httpContextAccessor) { }
        enum enum_mode
        {
            Create = 1,
            Update,
            Delete,
            Reffer,
            Comment
        }
        #region "日報"

        [HttpGet]
        public IActionResult Index(DateTime? date)
        {
            date = date??DateTime.Now;
            ReportViewModel model = createViewModel(date);
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(ReportViewModel model)
        {
            model = createViewModel(model.cond_date);
            return View(model);
        }

        public ReportViewModel createViewModel(DateTime? date = null)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT ");
            sql.AppendLine("  	B.report_no ");
            sql.AppendLine("  	,B.update_user ");
            sql.AppendLine("  	,S2.staf_name ");
            sql.AppendLine("  	,B.report_date ");
            sql.AppendLine("  	,B.update_date as update_date_1 ");
            sql.AppendLine("  	,S1.update_date as update_date_2 ");
            sql.AppendLine("  	,B.message  ");
            sql.AppendLine(" FROM T_REPORT B ");
            sql.AppendLine(" LEFT JOIN ");
            sql.AppendLine(" ( select report_no,update_user,update_date,row_number() over(PARTITION BY report_no ORDER BY update_date DESC) as num from T_REPORTCOMMENT ) S1 ");
            sql.AppendLine(" ON B.report_no = S1.report_no ");
            sql.AppendLine(" LEFT JOIN T_STAFFM S2 ");
            sql.AppendLine(" ON B.update_user = S2.staf_cd ");
            sql.AppendLine(" WHERE 1=1 ");
            sql.AppendLine(" AND (num =1 OR num IS NULL ) ");
            if (date != null)
            {
                sql.AppendFormat(" AND B.report_date = '{0}' ", date);
            }

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

            ReportViewModel model = new ReportViewModel();
            model.cond_date = date;
            foreach (DataRow dr in dt.Rows)
            {
                var staf_cd = int.Parse(HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value);
                var records = _context.T_REPORTCOMMENT_READ.Where(x => x.staf_cd == staf_cd && x.alreadyread_flg == false && x.update_user != staf_cd.ToString() && x.report_no== dr.Field<int>("report_no"));
                string count = records.Count() > 0 ? records.Count().ToString() : ""; ;

                bool isMe= dr.Field<string>("staf_name")==staf_cd.ToString()?true:false;
                
                    string update_date = "";
                if (dr["update_date_2"] == DBNull.Value)
                {
                    update_date = dr.Field<DateTime>("update_date_1").ToString("yyyy/M/d HH:mm");
                }
                else
                {
                    update_date = dr.Field<DateTime>("update_date_2").ToString("yyyy/M/d HH:mm");
                }
                model.list_report.Add(new Report
                {
                    report_no = dr.Field<int>("report_no"),
                    name = dr.Field<string>("staf_name"),
                    report_date = dr.Field<DateTime>("report_date").ToString("D"),
                    update_date = update_date ,
                    message = dr.Field<string>("message"),
                    count=count,
                    isMe=isMe
                });

            }
            return model;
        }
        #endregion
        #region "日報詳細"
        [HttpGet]
        public IActionResult ReportDetail(int mode,int report_no,DateTime cond_date)//cond_dateは登録後に日報画面に戻った時のために日付保持
        {
            ReportDetailViewModel model = new ReportDetailViewModel();
            try {
                if (mode == 1)
                {
                    model.mode = (int)enum_mode.Create;
                    model.report_date = DateTime.Now;
                }
                else
                {
                    model = createDetailViewModel(mode, report_no);
                    Update_alreadyread_flg(report_no);
                }
                model.cond_date= cond_date;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
            }
            return View("ReportDetail", model);
        }

        public ReportDetailViewModel createDetailViewModel(int mode, int report_no)
        {
            ReportDetailViewModel model = new ReportDetailViewModel();
            model.mode = mode;
            DataTable dt = new DataTable();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT ");
            sql.AppendLine("  	B.report_no ");
            sql.AppendLine("  	,B.report_date ");
            sql.AppendLine("  	,B.message ");
            sql.AppendLine("  	,B.update_user ");
            sql.AppendLine(" FROM T_REPORT B ");
            sql.AppendLine(" WHERE 1=1 ");
            sql.AppendFormat(" AND B.report_no = {0} ", report_no);

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
            model.report_no = dt.Rows[0].Field<int>("report_no");
            model.report_date = dt.Rows[0].Field<DateTime>("report_date");
            model.message = dt.Rows[0].Field<string>("message") ?? "";
            model.update_user = dt.Rows[0].Field<string>("update_user");

            //コメント表示欄
            if (mode == (int)enum_mode.Update || mode == (int)enum_mode.Delete || mode == (int)enum_mode.Reffer || mode == (int)enum_mode.Comment)
            {
                dt = new DataTable();
                sql.Clear();
                sql.AppendLine(" SELECT ");
                sql.AppendLine("  	B.report_no ");
                sql.AppendLine("  	,S1.comment_no ");
                sql.AppendLine("  	,S1.update_user ");
                sql.AppendLine("  	,S1.update_date ");
                sql.AppendLine("  	,S1.message ");
                sql.AppendLine(" FROM T_REPORT B ");
                sql.AppendLine(" INNER JOIN T_REPORTCOMMENT S1 ");
                sql.AppendLine(" ON B.report_no = S1.report_no ");
                sql.AppendLine(" WHERE 1=1 ");
                sql.AppendFormat(" AND B.report_no = {0} ", report_no);

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
                model.list_report=new List<ReportDetail>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    model.list_report.Add(new ReportDetail()
                    {
                        comment_no = dt.Rows[i].Field<int>("comment_no"),
                        update_user = dt.Rows[i].Field<string>("update_user"),
                        update_date= dt.Rows[i].Field<DateTime>("update_date").ToString("yyyy/M/d H:m"),
                        message= dt.Rows[i].Field<string>("message")
                    }
                        );

                }
            }
            //コメント入力欄
            if (mode == (int)enum_mode.Comment)
            {
                model.report = new ReportDetail();
            }
                return model;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReportDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", Common.Constants.Message_register.FAILURE_001);
                return View("ReportDetail", model);
            }
            using (IDbContextTransaction tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var recoard_new = new T_REPORT();
                    recoard_new.report_no = GetNextNo(Utilities.DataTypes.REPORT_NO);
                    recoard_new.report_date = model.report_date;
                    recoard_new.message = model.message;
                    recoard_new.update_user = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                    recoard_new.update_date = DateTime.Now;
                    _context.T_REPORT.Add(recoard_new);
                    _context.SaveChanges();

                    tran.Commit();

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                    tran.Dispose();
                    ModelState.AddModelError("", Common.Constants.Message_register.FAILURE_001);
                    return View("ReportDetail", model);
                }
            }
            return RedirectToAction("Index", "Report",model.cond_date);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ReportDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", Common.Constants.Message_register.FAILURE_001);
                return View("ReportDetail", model);
            }
            using (IDbContextTransaction tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var recoard = _context.T_REPORT.FirstOrDefault(x => x.report_no == model.report_no);
                    recoard.report_date = model.report_date;
                    recoard.message = model.message;
                    recoard.update_user = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                    recoard.update_date = DateTime.Now;
                    _context.SaveChanges();

                    tran.Commit();

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                    tran.Dispose();
                    ModelState.AddModelError("", Common.Constants.Message_register.FAILURE_001);
                    return View("ReportDetail", model);
                }
            }
            return RedirectToAction("Index", "Report", model.cond_date);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ReportDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", Common.Constants.Message_register.FAILURE_001);
                return View("ReportDetail",model);
            }
            using (IDbContextTransaction tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var recoard = _context.T_REPORT.FirstOrDefault(x => x.report_no == model.report_no);
                    _context.T_REPORT.Remove(recoard);
                    _context.SaveChanges();
                    var recoard_detail = _context.T_REPORTCOMMENT.Where(x => x.report_no == model.report_no);
                    if (recoard_detail.Count()>0)
                    {
                        _context.T_REPORTCOMMENT.RemoveRange(recoard_detail);
                        _context.SaveChanges();
                    }
                    var recoard_read = _context.T_REPORTCOMMENT_READ.Where(x => x.report_no == model.report_no);
                    if (recoard_read.Count() > 0)
                    {
                        _context.T_REPORTCOMMENT_READ.RemoveRange(recoard_read);
                        _context.SaveChanges();
                    }

                    tran.Commit();

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                    tran.Dispose();
                    ModelState.AddModelError("", Common.Constants.Message_register.FAILURE_001);
                    return View("ReportDetail", model);
                }
            }
            return RedirectToAction("Index", "Report", model.cond_date);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create_Comment(ReportDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", Common.Constants.Message_register.FAILURE_001);
                return View("ReportDetail", model);
            }
            using (IDbContextTransaction tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var now = DateTime.Now;
                    int nextComment_no= GetNextNo(Utilities.DataTypes.REPORT_COMMENT_NO);

                    var record = new T_REPORTCOMMENT();
                    record.report_no = (int)model.report_no;
                    record.comment_no = nextComment_no;
                    record.message = model.report.message;
                    record.alreadyread_flg = false;
                    record.update_user = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                    record.update_date = now;
                    _context.T_REPORTCOMMENT.Add(record);
                    _context.SaveChanges();

                    var record_read = new T_REPORTCOMMENT_READ();
                    record_read.report_no = (int)model.report_no;
                    record_read.comment_no = nextComment_no;
                    record_read.staf_cd = int.Parse(model.update_user);
                    record_read.alreadyread_flg = false;
                    record_read.update_user = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                    record_read.update_date = now;
                    _context.T_REPORTCOMMENT_READ.Add(record_read);
                    _context.SaveChanges();

                    tran.Commit();

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    _logger.LogError(ex.Message);
                    _logger.LogError(ex.StackTrace);
                    tran.Dispose();
                    ModelState.AddModelError("", Common.Constants.Message_register.FAILURE_001);
                    return View("ReportDetail", model);
                }
            }
            return RedirectToAction("Index", "Report", model.cond_date);
        }
        /// <summary>
        /// alreadyread_flg更新
        /// </summary>
        /// <param name="report_no"></param>
        /// <returns></returns
        public void Update_alreadyread_flg(int report_no)
        {
            try
            {
                using (IDbContextTransaction tran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        StringBuilder sql = new StringBuilder();
                        sql.AppendLine(" UPDATE ");
                        sql.AppendLine(" T_REPORTCOMMENT ");
                        sql.AppendLine(" SET alreadyread_flg=1");
                        sql.AppendLine(" WHERE 1=1 ");
                        sql.AppendFormat(" AND report_no = {0} ", report_no);

                        using (SqlConnection con = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = con.CreateCommand())
                            {
                                cmd.CommandText = sql.ToString();
                                cmd.ExecuteNonQuery();
                            }
                        }

                        sql.Clear();
                        sql.AppendLine(" UPDATE ");
                        sql.AppendLine(" T_REPORTCOMMENT_READ ");
                        sql.AppendLine(" SET alreadyread_flg=1");
                        sql.AppendLine(" WHERE 1=1 ");
                        sql.AppendFormat(" AND report_no = {0} ", report_no);

                        using (SqlConnection con = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                        {
                            con.Open();
                            using (SqlCommand cmd = con.CreateCommand())
                            {
                                cmd.CommandText = sql.ToString();
                                cmd.ExecuteNonQuery();
                            }
                        }



                        tran.Commit();

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        tran.Rollback();
                        throw;
                    }                 
                }

                //var recoard_detail = _context.T_REPORTCOMMENT.Where(x => x.report_no == report_no);
                //var data_1 = _context.T_REPORTCOMMENT.Where(x => x.report_no == report_no).ToList();
                //for(int i = 0; i < data_1.Count; i++)
                //{
                //    data_1[i].alreadyread_flg = true;
                //    _context.SaveChanges();

                //}
                ////foreach (var item_1 in data_1)
                ////{
                ////    item_1.alreadyread_flg = true;
                ////    _context.SaveChanges();

                ////}

                //var data_2 = _context.T_REPORTCOMMENT_READ.Where(x => x.report_no == report_no).ToList();
                //foreach (var item_2 in data_2)
                //{
                //    item_2.alreadyread_flg = true;
                //    _context.SaveChanges();
                //}
                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        #endregion


    }
}
