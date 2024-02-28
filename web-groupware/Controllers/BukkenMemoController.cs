using System.Data;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using web_groupware.Data;
using web_groupware.Models;
#pragma warning disable CS8600,CS8601,CS8602,CS8604,CS8618
namespace web_groupware.Controllers
{
    [Authorize]
    public class BukkenMemoController : BaseController
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="httpContextAccessor"></param>
        public BukkenMemoController(IConfiguration configuration, ILogger<BaseController> logger, web_groupwareContext context, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor) : base(configuration, logger, context, hostingEnvironment, httpContextAccessor) { }

        // GET: Notice
        [HttpGet]
        public IActionResult Index()
        {
            BukkenMemoViewModel model = createViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(BukkenMemoViewModel model)
        {
            model = createViewModel(model.cond_bukken_name);
            return View(model);
        }

        public BukkenMemoViewModel createViewModel(string bukken_name = "")
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT ");
            sql.AppendLine("  	B.bukken_cd ");
            sql.AppendLine("  	,B.bukken_name ");
            sql.AppendLine("  	,S2.staf_name ");
            sql.AppendLine("  	,S1.update_date  ");
            sql.AppendLine(" FROM T_BUKKEN B ");
            sql.AppendLine(" LEFT JOIN ");
            sql.AppendLine(" ( select bukken_cd,update_user,update_date,row_number() over(PARTITION BY bukken_cd ORDER BY update_date DESC) as num from T_BUKKENCOMMENT ) S1 ");
            sql.AppendLine(" ON B.bukken_cd = S1.bukken_cd ");
            sql.AppendLine(" LEFT JOIN T_STAFFM S2 ");
            sql.AppendLine(" ON S1.update_user = S2.staf_cd ");
            sql.AppendLine(" WHERE 1=1 ");
            sql.AppendLine(" AND (num =1 OR num IS NULL ) ");
            if (bukken_name != "")
            {
                sql.AppendFormat(" AND B.bukken_name LIKE '%{0}%' ", bukken_name);
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

            BukkenMemoViewModel model = new BukkenMemoViewModel();
            foreach (DataRow dr in dt.Rows)
            {
                string message = "";
                if (dr["update_date"] == DBNull.Value)
                {
                    message = "コメント未作成";
                }
                else
                {
                    message = GetWhenFromNow((DateTime)dr["update_date"]);
                }

                model.list_bukken.Add(new BukkenMemo
                {
                    bukken_cd = dr.Field<int>("bukken_cd"),
                    bukken_name = dr.Field<string>("bukken_name"),
                    update_user = dr.Field<string>("staf_name") ?? "コメント未作成",
                    update_date = message
                });

            }
            return model;
        }

        [HttpGet]
        public async Task<IActionResult> BukkenMemoDetail(int bukken_cd)
        {
            using (IDbContextTransaction tran = _context.Database.BeginTransaction())
            {
                try
                {
                    var user= HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                    var now = DateTime.Now;
                    var list_read = _context.T_BUKKENCOMMENT_READ.Where(x => x.bukken_cd == bukken_cd&&x.staf_cd== int.Parse(user)).ToList();
                    for (int i = 0; i < list_read.Count; i++)
                    {
                        list_read[i].alreadyread_flg = true;
                        list_read[i].update_user = user;
                        list_read[i].update_date = DateTime.Now;
                    }
                    await _context.SaveChangesAsync();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    _logger.LogError(ex.Message);
                    throw;
                }
            }
            BukkenMemoDetailViewModel model = createDetailViewModel(bukken_cd);
            return View(model);
        }
        public BukkenMemoDetailViewModel createDetailViewModel(int bukken_cd)
        {
            BukkenMemoDetailViewModel model = new BukkenMemoDetailViewModel();
            DataTable dt = new DataTable();
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT ");
            sql.AppendLine("  	B.bukken_cd ");
            sql.AppendLine("  	,B.bukken_name ");
            sql.AppendLine("  	,B.zip ");
            sql.AppendLine("  	,B.address1 ");
            sql.AppendLine("  	,B.address2 ");
            sql.AppendLine(" FROM T_BUKKEN B ");
            sql.AppendLine(" WHERE 1=1 ");
            sql.AppendFormat(" AND B.bukken_cd = {0} ", bukken_cd);

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
            model.bukken_name = dt.Rows[0].Field<string>("bukken_name");
            model.bukken_nameWithCode = dt.Rows[0].Field<string>("bukken_name") + "(" + dt.Rows[0].Field<int>("bukken_cd") + ")";
            model.zip = dt.Rows[0].Field<string>("zip") ?? "";
            model.address1 = dt.Rows[0].Field<string>("address1") ?? "";
            model.address2 = dt.Rows[0].Field<string>("address2") ?? "";

            //dt = new DataTable();
            //sql.Clear();
            //sql.AppendLine(" SELECT ");
            //sql.AppendLine("  	S1.staf_name ");
            //sql.AppendLine("  	,B.update_date  ");
            //sql.AppendLine("  	,B.message  ");
            //sql.AppendLine(" FROM T_BUKKENCOMMENT B ");
            //sql.AppendLine(" LEFT JOIN T_STAFFM S1 ");
            //sql.AppendLine(" ON B.update_user = S1.staf_cd ");
            //sql.AppendLine(" WHERE 1=1 ");
            //sql.AppendFormat(" AND B.bukken_cd = {0} ", bukken_cd);

            //using (SqlConnection con = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            //{
            //    con.Open();
            //    using (SqlCommand cmd = con.CreateCommand())
            //    {
            //        cmd.CommandText = sql.ToString();
            //        var dataAdapter = new SqlDataAdapter(cmd);
            //        dataAdapter.Fill(dt);
            //    }
            //}
            //foreach (DataRow dr in dt.Rows)
            //{
            //    model.list_detail.Add(new BukkenMemoDetail
            //    {
            //        update_user = dr.Field<string>("staf_name"),
            //        update_date = dr.Field<DateTime>("update_date").ToString("yyyy/M/d HH:mm"),
            //        message = dr.Field<string>("message")
            //    });

            //}

            //dt = new DataTable();
            //sql.Clear();
            //sql.AppendLine(" SELECT ");
            //sql.AppendLine("  	B.staf_cd ");
            //sql.AppendLine("  	,B.staf_name ");
            //sql.AppendLine(" FROM T_STAFFM B ");

            //using (SqlConnection con = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            //{
            //    con.Open();
            //    using (SqlCommand cmd = con.CreateCommand())
            //    {
            //        cmd.CommandText = sql.ToString();
            //        var dataAdapter = new SqlDataAdapter(cmd);
            //        dataAdapter.Fill(dt);
            //    }
            //}

            //foreach (DataRow dr in dt.Rows)
            //{
            //    model.list_staff.Add(new SelectListItem
            //    {
            //        Value = dr.Field<int>("staf_cd").ToString(),
            //        Text = dr.Field<string>("staf_name"),
            //    });
            //}
            createList(model, bukken_cd);
            return model;
        }
        public BukkenMemoDetailViewModel createList(BukkenMemoDetailViewModel model, int bukken_cd)
        {
            //BukkenMemoDetailViewModel model = new BukkenMemoDetailViewModel();
            //DataTable dt = new DataTable();
            //StringBuilder sql = new StringBuilder();
            //sql.AppendLine(" SELECT ");
            //sql.AppendLine("  	B.bukken_cd ");
            //sql.AppendLine("  	,B.bukken_name ");
            //sql.AppendLine("  	,B.zip ");
            //sql.AppendLine("  	,B.address1 ");
            //sql.AppendLine("  	,B.address2 ");
            //sql.AppendLine(" FROM T_BUKKEN B ");
            //sql.AppendLine(" WHERE 1=1 ");
            //sql.AppendFormat(" AND B.bukken_cd = {0} ", bukken_cd);

            //using (SqlConnection con = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            //{
            //    con.Open();
            //    using (SqlCommand cmd = con.CreateCommand())
            //    {
            //        cmd.CommandText = sql.ToString();
            //        var dataAdapter = new SqlDataAdapter(cmd);
            //        dataAdapter.Fill(dt);
            //    }
            //}
            //model.bukken_name = dt.Rows[0].Field<string>("bukken_name");
            //model.bukken_nameWithCode = dt.Rows[0].Field<string>("bukken_name") + "(" + dt.Rows[0].Field<int>("bukken_cd") + ")";
            //model.zip = dt.Rows[0].Field<string>("zip") ?? "";
            //model.address1 = dt.Rows[0].Field<string>("address1") ?? "";
            //model.address2 = dt.Rows[0].Field<string>("address2") ?? "";

            DataTable dt = new DataTable();
            StringBuilder sql = new StringBuilder();
            sql.Clear();
            sql.AppendLine(" SELECT ");
            sql.AppendLine("  	S1.staf_name ");
            sql.AppendLine("  	,B.update_date  ");
            sql.AppendLine("  	,B.message  ");
            sql.AppendLine(" FROM T_BUKKENCOMMENT B ");
            sql.AppendLine(" LEFT JOIN T_STAFFM S1 ");
            sql.AppendLine(" ON B.update_user = S1.staf_cd ");
            sql.AppendLine(" WHERE 1=1 ");
            sql.AppendFormat(" AND B.bukken_cd = {0} ", bukken_cd);
            sql.AppendLine(" ORDER BY B.update_date ");

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
            foreach (DataRow dr in dt.Rows)
            {
                model.list_detail.Add(new BukkenMemoDetail
                {
                    update_user = dr.Field<string>("staf_name"),
                    update_date = dr.Field<DateTime>("update_date").ToString("yyyy/M/d HH:mm"),
                    message = dr.Field<string>("message")
                });

            }

            dt = new DataTable();
            sql.Clear();
            sql.AppendLine(" SELECT ");
            sql.AppendLine("  	B.staf_cd ");
            sql.AppendLine("  	,B.staf_name ");
            sql.AppendLine(" FROM T_STAFFM B ");

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

            foreach (DataRow dr in dt.Rows)
            {
                model.list_staff.Add(new SelectListItem
                {
                    Value = "1_" + dr.Field<int>("staf_cd").ToString(),
                    Text = dr.Field<string>("staf_name"),
                });
            }

            dt = new DataTable();
            sql.Clear();
            sql.AppendLine(" SELECT ");
            sql.AppendLine("  	B.group_cd ");
            sql.AppendLine("  	,B.group_name ");
            sql.AppendLine(" FROM T_GROUPM B ");

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

            foreach (DataRow dr in dt.Rows)
            {
                model.list_staff.Add(new SelectListItem
                {
                    Value = "2_" + dr.Field<int>("group_cd").ToString(),
                    Text = dr.Field<string>("group_name"),
                });
            }

            return model;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BukkenMemoDetailViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", Common.Constants.Message_register.FAILURE_001);
                return View(model);
            }
            createList(model, model.bukken_cd);
            using (IDbContextTransaction tran = _context.Database.BeginTransaction())
            {
                try
                {
                    //var recoard_no = _context.T_NO.FirstOrDefault(x => x.data_type == 2);
                    //var nextNo = recoard_no.no;
                    //recoard_no.no = recoard_no.no + 1;
                    //_context.SaveChanges();
                    //tran.Commit();
                    //return RedirectToAction("Index", "BukkenMemo");

                    var comment_no = GetNextNo(Utilities.DataTypes.BUKKEN_COMMENT_CD);
                    var now = DateTime.Now;
                    var user = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                    var recoard_new = new T_BUKKENCOMMENT();
                    recoard_new.bukken_cd = model.bukken_cd;
                    //T_BUKKENCOMMENT recoard = _context.T_BUKKENCOMMENT.OrderByDescending(e => e.comment_cd).FirstOrDefault(x => x.bukken_cd == model.bukken_cd);
                    //T_BUKKENCOMMENT recoard = _context.T_BUKKENCOMMENT.OrderBy(e => e.comment_cd).FirstOrDefault(x => x.bukken_cd == model.bukken_cd);
                    //int nextNumber = recoard ==null ? 1: recoard.comment_cd+1;
                    recoard_new.comment_cd = comment_no;
                    recoard_new.message = model.message_new;
                    recoard_new.update_user = user;
                    recoard_new.update_date = now;
                    _context.T_BUKKENCOMMENT.Add(recoard_new);
                    _context.SaveChanges();

                    //登録すべきstaf_cのリスト作成d
                    var list_all_staf_cd= new List<int>();
                    for (int i = 0; i < model.list_selected_staf_cd.Count; i++)
                    {
                        if (model.list_selected_staf_cd[i] == null) continue;
                        var arr = model.list_selected_staf_cd[i].Split('_');
                        var staff1_Group2 = arr[0];
                        var code = int.Parse(arr[1]);
                        if (staff1_Group2 == "1")
                        {
                            list_all_staf_cd.Add(code);
                        }
                        else if (staff1_Group2 == "2")
                        {
                            var list_staf_cd = _context.T_GROUPSTAFF.Where(x => x.group_cd == code).ToList();
                            for (var g = 0; g < list_staf_cd.Count; g++)
                            {
                                list_all_staf_cd.Add(list_staf_cd[g].staf_cd);
                            }
                        }
                    }
                    var list_distinct_staf_cd=list_all_staf_cd.Distinct();
                    for (var i = 0;i< list_distinct_staf_cd.Count(); i++)
                    {
                        var recoard_read_new = new T_BUKKENCOMMENT_READ();
                        recoard_read_new.bukken_cd = model.bukken_cd;
                        recoard_read_new.comment_no = comment_no;
                        recoard_read_new.staf_cd = list_distinct_staf_cd.ElementAt(i);
                        recoard_read_new.alreadyread_flg = list_distinct_staf_cd.ElementAt(i) == int.Parse(user) ? true : false;//ログインユーザーは既読として登録
                        recoard_read_new.update_user = user;
                        recoard_read_new.update_date = now;
                        _context.T_BUKKENCOMMENT_READ.Add(recoard_read_new);
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
                    return View("BukkenMemoDetail", model);
                }
            }
            return RedirectToAction("Back_to_parent","Home");

        }
    }
}
