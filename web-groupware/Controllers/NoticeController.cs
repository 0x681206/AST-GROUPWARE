using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata;
using System;
using web_groupware.Data;
using web_groupware.Models;
using System.IO;
#pragma warning disable CS8600,CS8601,CS8602,CS8604,CS8618,CS8629
namespace web_groupware.Controllers
{
    [Authorize]
    public class NoticeController : BaseController
    {
        const int info_cd = 2;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="httpContextAccessor"></param>
        public NoticeController(IConfiguration configuration, ILogger<BaseController> logger, web_groupwareContext context, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor) : base(configuration, logger, context, hostingEnvironment, httpContextAccessor) { }

        // GET: Notice
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                //workディレクトリ設定
                var dir_root = _context.T_DIC.FirstOrDefault(x => x.dic_kb == 700 && x.dic_cd == "1")?.content;
                string dir_work = Path.Combine(dir_root, "work", info_cd.ToString(), HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value, DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
                ////workディレクトリの削除
                //Directory.Delete(dir_work, true);
                //workディレクトリの再作成
                //if (!Directory.Exists(dir_work))
                //{
                Directory.CreateDirectory(dir_work);
                //}

                var recoard = _context.T_INFO.FirstOrDefault(x => x.info_cd == info_cd);
                var recoard_file = GetRecoard_file();
                var model = new NoticeViewModel();
                model.Message = recoard.message;
                model.Work_dir = dir_work;
                model.List_T_INFO_FILE = recoard_file;


                ////ファイルコピー・削除
                //string? dir_main = Path.Combine(dir_root, "2");
                //for (int i = 0; i < model.List_T_INFO_FILE.Count; i++)
                //{
                //    using (var fileStream = new FileStream(Path.Combine(dir_main, model.List_T_INFO_FILE[i].fullPath), FileMode.Create))
                //    {
                //        model.File[i].CopyTo(fileStream);
                //    }

                //}
                return View(model);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return RedirectToAction("Index", "Login");
            }



        }

        //[HttpPost]
        //public IActionResult Index(NoticeViewModel model)
        //{
        //    return View(model);
        //}

        // POST: T_INFO/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NoticeViewModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", Common.Constants.Message_register.FAILURE_001);
                    //ModelState.Clear();
                    model.List_T_INFO_FILE = GetRecoard_file();
                    return View("Index", model);
                }

                using (IDbContextTransaction tran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        //for (int i = 0; i < model.List_T_INFO_FILE.fileName.Count; i++)
                        //{

                        //    for (int f = 0; f < model.File.Count; f++)
                        //    {
                        //        //ファイルとファイル名がある場合は実ファイルからファイル名
                        //        if (model.List_T_INFO_FILE.fileName[i] == model.File[f].FileName)
                        //        {
                        //            fileName.Add(model.File[f].FileName);
                        //            path_uploadFile.Add(Path.Combine(dir_main, model.File[f].FileName));
                        //        }
                        //    }
                        //    //ファイル名のみの場合はDBのファイル名
                        //    if (fileName.Count != i + 1)
                        //    {
                        //        fileName.Add(model.List_T_INFO_FILE.fileName[i]);
                        //        path_uploadFile.Add(Path.Combine(dir_main, model.List_T_INFO_FILE.fileName[i]));
                        //    }
                        //}

                        //ファイル名とフルパス設定
                        //if (model.File != null && model.List_T_INFO_FILE.fileName != null)
                        //{
                        //}

                        //if (model.File == null && model.List_T_INFO_FILE.fileName != null)
                        //{
                        //    fileName = model.List_T_INFO_FILE.fileName;
                        //    path_uploadFile = Path.Combine(dir_main, fileName);
                        //}
                        //T_INFO　登録・変更
                        var recoard = _context.T_INFO.FirstOrDefault(x => x.info_cd == info_cd);

                        if (recoard == null)
                        {
                            var recoard_new = new T_INFO();
                            recoard_new.info_cd = info_cd;
                            recoard_new.message = model.Message;
                            recoard_new.update_user = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                            recoard_new.update_date = DateTime.Now;
                            _context.T_INFO.Add(recoard_new);
                            _context.SaveChanges();

                        }
                        else
                        {
                            recoard.message = model.Message;
                            recoard.update_user = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                            recoard.update_date = DateTime.Now;
                            _context.SaveChanges();
                        }

                        //ファイルに関する処理
                        List<string> path_uploadFile = new List<string>();
                        //ディレクトリ設定
                        var dir_root = _context.T_DIC.FirstOrDefault(x => x.dic_kb == 700 && x.dic_cd == "1").content;
                        string dir_main = Path.Combine(dir_root, "main", info_cd.ToString());
                        if (!Directory.Exists(dir_main))
                        {
                            Directory.CreateDirectory(dir_main);
                        }


                        //対象info_cdのレコード全削除
                        Dictionary<string, DateTime?> dic_name_and_date = new Dictionary<string, DateTime?>();
                        Dictionary<string, string> dic_name_and_user = new Dictionary<string, string>();
                        List<T_INFO_FILE> list_recoard_file = _context.T_INFO_FILE.Where(x => x.info_cd == info_cd).ToList();
                        foreach (T_INFO_FILE record_file in list_recoard_file)
                        {
                            dic_name_and_date.Add(record_file.fileName, record_file.update_date);
                            dic_name_and_user.Add(record_file.fileName, record_file.update_user);
                            _context.T_INFO_FILE.RemoveRange(record_file);
                        }
                        _context.SaveChanges();

                        //レコード登録　mainディレクトリ
                        foreach (string path_file in Directory.EnumerateFiles(dir_main))
                        {
                            var file_name = Path.GetFileName(path_file);
                            T_INFO_FILE recoard_file = null;
                            recoard_file = new T_INFO_FILE();
                            recoard_file.file_no = GetNextNo(Utilities.DataTypes.INFO_FILE_NO);
                            recoard_file.info_cd = info_cd;
                            recoard_file.fileName = file_name;
                            recoard_file.fullPath = path_file;
                            recoard_file.update_user = dic_name_and_user[file_name];
                            recoard_file.update_date = dic_name_and_date[file_name];
                            _context.T_INFO_FILE.Add(recoard_file);
                            _context.SaveChanges();
                        }
                        //レコード登録　workディレクトリ
                        var work_dir_files = Directory.GetFiles(model.Work_dir);
                        for(int i = 0;i< work_dir_files.Count();i++) 
                        {
                            //同名ファイルが存在していたら名前変更
                            if (System.IO.File.Exists(Path.Combine(dir_main, Path.GetFileName(work_dir_files[i]))))
                            {
                                var count = 1;
                                while (true)
                                {
                                    var arr_work = work_dir_files[i].Split(".");
                                    var kandidat = "";
                                    for (var w = 0; w < arr_work.Length - 1; w++)
                                    {
                                        kandidat = kandidat + arr_work[w] + ".";
                                    }
                                    kandidat = kandidat.Substring(0, kandidat.Length - 1);
                                    kandidat = kandidat + '（' + count + '）';
                                    // ファイルの拡張子を取得
                                    string fileExtention = Path.GetExtension(work_dir_files[i]);
                                    kandidat = kandidat + "." + fileExtention;
                                    if (!System.IO.File.Exists(work_dir_files[i]))
                                    {
                                        work_dir_files[i] = Path.Combine(model.Work_dir , kandidat);
                                        break;
                                    }
                                    count++;
                                }
                            }

                            var file_name = Path.GetFileName(work_dir_files[i]);
                            System.IO.File.Copy(work_dir_files[i], Path.Combine(dir_main, file_name));

                            T_INFO_FILE recoard_file = null;
                            recoard_file = new T_INFO_FILE();
                            recoard_file.file_no = GetNextNo(Utilities.DataTypes.INFO_FILE_NO);
                            recoard_file.info_cd = info_cd;
                            recoard_file.fileName = file_name;
                            recoard_file.fullPath = Path.Combine(dir_main, file_name);
                            recoard_file.update_user = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                            recoard_file.update_date = DateTime.Now;
                            _context.T_INFO_FILE.Add(recoard_file);
                            _context.SaveChanges();
                        }

                        //for (int i = 0; i < model.List_T_INFO_FILE.Count; i++)
                        //{
                        //    T_INFO_FILE recoard_file = null;
                        //    for (int f = 0; f < model.File.Count; f++)
                        //    {
                        //        //ファイルとファイル名がある場合は登録
                        //        if (model.List_T_INFO_FILE[i].fileName == model.File[f].FileName)
                        //        {
                        //            recoard_file = new T_INFO_FILE();
                        //            recoard_file.file_no = GetNextNo(9);
                        //            recoard_file.info_cd = 2;
                        //            recoard_file.fileName = model.List_T_INFO_FILE[i].fileName;
                        //            recoard_file.fullPath = Path.Combine(dir_main, model.File[f].FileName);
                        //            recoard_file.update_user = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                        //            recoard_file.update_date = DateTime.Now;
                        //            _context.T_INFO_FILE.Add(recoard_file);
                        //            _context.SaveChanges();
                        //            path_uploadFile.Add(recoard_file.fullPath);
                        //            break;
                        //        }
                        //    }
                        //    //実ファイルがなくファイル名のみの場合は変更
                        //    if (recoard_file == null)
                        //    {
                        //        recoard_file = _context.T_INFO_FILE.FirstOrDefault(x => x.info_cd == 2 && x.fileName == model.List_T_INFO_FILE[i].fileName);
                        //        recoard_file.fileName = model.List_T_INFO_FILE[i].fileName;
                        //        recoard_file.fullPath = Path.Combine(dir_main, model.List_T_INFO_FILE[i].fileName);
                        //        recoard_file.update_user = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                        //        recoard_file.update_date = DateTime.Now;
                        //        _context.SaveChanges();
                        //        path_uploadFile.Add(recoard_file.fullPath);
                        //    }
                        //}
                        ////ファイルコピー・削除
                        //for (int f = 0; f < model.File.Count; f++)
                        //{
                        //    using (var fileStream = new FileStream(Path.Combine(dir_main, model.File[f].FileName), FileMode.Create))
                        //    {
                        //        model.File[f].CopyTo(fileStream);
                        //    }
                        //}
                        //if (model.File != null && fileName != null)
                        //{

                        //    using (var fileStream = new FileStream(path_uploadFile, FileMode.Create))
                        //    {
                        //        model.File[0].CopyTo(fileStream);
                        //    }
                        //}
                        //foreach (string path_file in Directory.EnumerateFiles(dir_main))
                        //{
                        //    if (!path_uploadFile.Contains(path_file))
                        //    {
                        //        System.IO.File.Delete(path_file);
                        //    }
                        //}
                        Directory.Delete(model.Work_dir, true);
                        tran.Commit();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        _logger.LogError(ex.Message);
                        _logger.LogError(ex.StackTrace);
                        tran.Dispose();
                        ModelState.AddModelError("", Common.Constants.Message_register.FAILURE_001);
                        model.List_T_INFO_FILE = GetRecoard_file();
                        return View("Index", model);
                    }
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                ModelState.AddModelError("", Common.Constants.Message_register.FAILURE_001);
                model.List_T_INFO_FILE = GetRecoard_file();
                return View("Index", model);
            }

        }

        [HttpGet]
        public IActionResult DownloadFile(string file_name)
        {
            try
            {
                var fullPath = _context.T_INFO_FILE.FirstOrDefault(x => x.info_cd == info_cd&& x.fileName == file_name).fullPath;
                var fileName = Path.GetFileName(fullPath);
                var content = System.IO.File.ReadAllBytes(fullPath);
                new FileExtensionContentTypeProvider()
                                .TryGetContentType(fullPath, out string contentType);
                if (contentType == null) contentType = System.Net.Mime.MediaTypeNames.Application.Octet;

                try
                {
                    //1年以上前のwrokフォルダ削除
                    var dir_root = _context.T_DIC.FirstOrDefault(x => x.dic_kb == 700 && x.dic_cd == "1")?.content;
                    string dir_work = Path.Combine(dir_root, "work");
                    var list_dir_1 = Directory.GetDirectories(dir_work);
                    //var oneYearAgo=DateTime.Now.AddYears(-1);
                    DateTime oneYearAgo = DateTime.Now.AddYears(-1);
                    foreach (string dir_1 in list_dir_1)
                    {
                        var list_dir_2 = Directory.GetDirectories(dir_1); ;
                        foreach (string dir_2 in list_dir_2)
                        {
                            var list_dir_3 = Directory.GetDirectories(dir_2); ;
                            foreach (string dir_3 in list_dir_3)
                            {
                                DateTime updateDate = Directory.GetLastWriteTime(dir_3);
                                if (updateDate < oneYearAgo)
                                {
                                    Directory.Delete(dir_3, true);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("フォルダ削除に失敗しました。");
                    _logger.LogError(ex.Message);
                    return File(content, contentType, fileName);
                }
                return File(content, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        public List<T_INFO_FILE> GetRecoard_file()
        {
            List<T_INFO_FILE> recoard_file = _context.T_INFO_FILE.Where(x => x.info_cd == info_cd).OrderBy(o => o.file_no).ToList();
            return recoard_file;
        }
        /// <summary>
        /// ファイル削除
        /// </summary>
        /// <param name="work_dir"></param>
        /// <param name="dir_kind">1：main　2：work</param>
        /// <param name="file_name"></param>
        public IActionResult DeleteFile(string work_dir, int dir_kind, string file_name)
        {
            try
            {
                using (IDbContextTransaction tran = _context.Database.BeginTransaction())
                {
                    try
                    {

                        if (dir_kind == 1)
                        {
                            //データ削除
                            var recoard = _context.T_INFO_FILE.FirstOrDefault(x => x.info_cd == info_cd && x.fileName == file_name);
                            _context.T_INFO_FILE.Remove(recoard);
                            _context.SaveChanges();
                            //ファイル削除
                            System.IO.File.Delete(recoard.fullPath);
                        }
                        else if (dir_kind == 2)
                        {
                            System.IO.File.Delete(Path.Combine(work_dir, file_name));
                        }
                        tran.Commit();
                        return Ok();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        _logger.LogError(ex.Message);
                        tran.Dispose();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public IActionResult UploadFile(string work_dir, IFormFile file)
        {
            Console.WriteLine("ファイルアップロード："+file.Name);
            try
            {
                using (IDbContextTransaction tran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        string path_file = Path.Combine(work_dir, file.FileName);
                        //T_INFO_FILE recoard_file = null;
                        //recoard_file = new T_INFO_FILE();
                        //recoard_file.file_no = GetNextNo(Utilities.DataTypes.INFO_FILE_NO);
                        //recoard_file.info_cd = info_cd;
                        //recoard_file.fileName = file.FileName;
                        //recoard_file.fullPath = path_file;
                        //recoard_file.update_user = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                        //recoard_file.update_date = DateTime.Now;
                        //_context.T_INFO_FILE.Add(recoard_file);
                        //_context.SaveChanges();

                        //ファイルコピー
                        using (var fileStream = new FileStream(path_file, FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        tran.Commit();
                        return Ok();

                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        _logger.LogError(ex.Message);
                        tran.Dispose();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        //private bool T_INFOExists(int id)
        //{
        //  return (_context.T_INFO?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
        /// <summary>
        /// 確認ダイアログ
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        //public IActionResult ConfirmDialog(int kind=1, string title = "", string message = "")
        //{
        //    _logger.LogInformation($"確認ダイアログを表示します。");

        //    var capsule = new ConfirmDialogViewModel();
        //    if (kind == 1)
        //    {
        //        capsule.Kind = "登録";
        //    }
        //    else if (kind == 2)
        //    {
        //        capsule.Kind = "変更";
        //    }
        //    else if (kind == 3)
        //    {
        //        capsule.Kind = "削除";
        //    }
        //    capsule.Title = title;
        //    capsule.Message = message;
        //    return View(capsule);
        //}

    }
}
