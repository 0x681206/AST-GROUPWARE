using System.Data;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using web_groupware.Data;
using web_groupware.Models;
#pragma warning disable CS8600,CS8601,CS8602,CS8604,CS8618
namespace web_groupware.Controllers
{
    [Authorize]
    public class RestrationReportController : BaseController
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="httpContextAccessor"></param>
        public RestrationReportController(IConfiguration configuration, ILogger<BaseController> logger, web_groupwareContext context, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor) : base(configuration, logger, context, hostingEnvironment, httpContextAccessor) { }

        // GET: Notice
        [HttpGet]
        public IActionResult Index()
        {
            RestrationReportViewModel model=createViewModel(DateTime.Now.Date.AddMonths(-1),DateTime.Now.Date,null);
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(RestrationReportViewModel model)
        {
            model = createViewModel(model.cond_leaving_date_from,model.cond_leaving_date_to,model.cond_staf_cd);
            return View(model);
        }

        public RestrationReportViewModel createViewModel(DateTime? from,DateTime? to,string? staf_cd)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(" SELECT ");
            sql.AppendLine("  	B.* ");
            sql.AppendLine("  	,S1.staf_name  ");
            sql.AppendLine(" FROM R_RESTORATION_REPORT B ");
            sql.AppendLine(" LEFT JOIN T_STAFFM S1 ");
            sql.AppendLine(" ON B.leaving_staffcd = S1.staf_cd ");
            sql.AppendLine(" WHERE 1=1 ");
            if (from != null)
            {
                sql.AppendFormat(" AND B.leaving_date >= '{0}' ", from);
            }
            if (to != null)
            {
                sql.AppendFormat(" AND B.leaving_date < '{0}' ", ((DateTime)to).AddDays(1));
            }
            if (staf_cd != null)
            {
                sql.AppendFormat(" AND B.leaving_staffcd = '{0}' ", staf_cd);
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

            RestrationReportViewModel model = new RestrationReportViewModel();
            foreach (DataRow dr in dt.Rows)
            {
                model.list_report.Add(new RestrationReport
                {
                    hachu_no = dr.Field<int>("hachu_no"),
                    bukken_name = dr.Field<string>("bukken_name"),
                    room_no = dr.Field<int>("room_no"),
                    leaving_date = dr.Field<DateTime>("leaving_date").ToString("yyyy年M月d日"),
                    leaving_staffcd= dr.Field<int>("leaving_staffcd"),
                    restoration_date= dr.Field<DateTime>("restoration_date").ToString("yyyy年M月d日"),
                    staf_name=  dr.Field<string>("staf_name")
                });

            }
            return model;
        }

    }
}
