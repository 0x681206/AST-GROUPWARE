using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using web_groupware.Models;
using web_groupware.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Azure.Core;

#pragma warning disable CS8600, CS8601, CS8602, CS8604, CS8618, CS8629

namespace web_groupware.Controllers
{
    [Authorize]
    public class MemoController : BaseController
    {
        public const int BUKKEN_COMMENT_CD = 2;

        public const int MEMO_STATE_ALL = 0;
        public const int MEMO_STATE_UNREAD = 0;
        public const int MEMO_STATE_READ = 1;
        public const int MEMO_STATE_WORKING = 2;
        public const int MEMO_STATE_FINISH = 3;

        private const int MaxContentLength = 255;

        public MemoController(IConfiguration configuration, ILogger<BaseController> logger, web_groupwareContext context, IWebHostEnvironment hostingEnvironment, IHttpContextAccessor httpContextAccessor)
            : base(configuration, logger, context, hostingEnvironment, httpContextAccessor)
        {
        }

        [HttpGet]
        public IActionResult Memo_sent(int state = 0, int user = 0)
        {
            MemoViewModel model = CreateMemoViewModel(state, user, true);
            TempData["is_sent"] = true;
            return View(model);
        }

        [HttpGet]
        public IActionResult Memo_received(int state = 0, int user = 0)
        {
            MemoViewModel model = CreateMemoViewModel(state, user, false);
            TempData["is_sent"] = false;
            return View(model);
        }

        [HttpPost]
        public IActionResult Filter(bool is_sent, int filter_state, int filter_user)
        {
            TempData["filter_state"] = filter_state;
            TempData["filter_user"] = filter_user;
            TempData["is_sent"] = is_sent;

            var routeValues = new { state = filter_state, user = filter_user };
            if (is_sent)
                return RedirectToAction("Memo_sent", routeValues);
            else
                return RedirectToAction("Memo_received", routeValues);
        }

        public MemoViewModel CreateMemoViewModel(int selected_state = 0, int selected_user = 0, bool is_sent = true)
        {
            var model = new MemoViewModel
            {
                selectedState = selected_state,
                selectedUser = selected_user,
                isSent = is_sent,

                staffList = _context.T_STAFFM
                    .Select(u => new MemoViewModelStaff
                    {
                        staff_cd = u.staf_cd,
                        staff_name = u.staf_name
                    })
                    .ToList(),
                groupList = _context.T_GROUPM
                    .Select(g => new MemoViewModelGroup
                    {
                        group_cd = g.group_cd,
                        group_name = g.group_name
                    })
                    .ToList()
            };
            var comments = _context.T_DIC
                .Where(m => m.dic_kb == 710)
                .ToList();
            foreach (var item in comments)
            {
                model.commentList.Add(new MemoComment
                {
                    comment_no = item.dic_cd,
                    comment = item.content
                });
            }

            int user_id = Convert.ToInt32(@User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value);
            var memoList = _context.T_MEMO.ToList();
            for (var i = memoList.Count - 1; i >= 0; i--)
            {
                var memo = memoList[i];

                if (selected_state == 0 || memo.state == selected_state - 1)
                {
                    bool is_show = false;
                    if (is_sent)
                    {
                        if (memo.sender_cd == user_id)
                        {
                            if (selected_user == 0) is_show = true;
                            else if (selected_user == 1 && memo.receiver_type == 0 && memo.receiver_cd == user_id) is_show = true;
                            else if (selected_user > 1 && memo.receiver_type == 1)
                            {
                                if (memo.receiver_cd == selected_user - 2) is_show = true;
                            }
                        }

                    } else
                    {
                        if ((selected_user == 0 || selected_user == 1) && memo.receiver_type == 0 && memo.receiver_cd == user_id) is_show = true;
                        else if (memo.receiver_type == 1 && (selected_user == 0 || selected_user > 1 && memo.receiver_cd == selected_user - 2))
                        {
                            var memoReader = _context.T_MEMO_READ
                                .Where(m => m.memo_no == memo.memo_no && m.staff_cd == user_id)
                                .FirstOrDefault();
                            // グループの対象社員は作成者が宛先を登録・変更したタイミングでグループに属していた社員
                            if (memoReader != null)
                            {
                                is_show = model.groupList
                                    .Where(m => m.group_cd == memo.receiver_cd)
                                    .ToList().Any();
                            }
                        }                        
                    }
                    if (is_show)
                    {
                        var receiver_name = "";
                        if (memo.receiver_type == 0)
                        {
                            var user = model.staffList.FirstOrDefault(u => u.staff_cd == memo.receiver_cd);
                            receiver_name = user.staff_name;
                        }
                        else
                        {
                            var group = model.groupList.FirstOrDefault(u => u.group_cd == memo.receiver_cd);
                            receiver_name = group.group_name;
                        }
                        var sender = model.staffList.FirstOrDefault(u => u.staff_cd == memo.sender_cd);
                        var sender_name = sender.staff_name;

                        var memoReaders = _context.T_MEMO_READ
                            .Include(m => m.staff)
                            .Where(m => m.memo_no == memo.memo_no && m.read_flag)
                            .ToList();
                        var readerNames = "";
                        foreach (var memoReader in memoReaders)
                        {
                            if (readerNames.Length != 0) readerNames += "、";
                            readerNames += memoReader.staff.staf_name;
                        }
                        var working_msg = "";
                        if (memo.working_cd > 0)
                        {
                            var working = model.staffList.FirstOrDefault(u => u.staff_cd == memo.working_cd);
                            working_msg = memo.working_date.ToString("yyyy年MM月dd日 HH時mm分  ") + working.staff_name;
                        }
                        var finish_msg = "";
                        if (memo.finish_cd > 0)
                        {
                            var working = model.staffList.FirstOrDefault(u => u.staff_cd == memo.finish_cd);
                            finish_msg = memo.finish_date.ToString("yyyy年MM月dd日 HH時mm分  ") + working.staff_name;
                        }

                        model.memoList.Add(new MemoModel
                        {
                            memo_no = memo.memo_no,
                            create_date = memo.create_date.ToString("yyyy/MM/dd HH:mm"),
                            state = memo.state,
                            receiver_type = memo.receiver_type,
                            receiver_cd = memo.receiver_cd,
                            receiver_name = receiver_name,
                            comment_no = memo.comment_no,
                            phone = memo.phone,
                            content = memo.content,
                            sender_name = sender_name,
                            is_editable = memo.sender_cd == user_id,
                            readers = readerNames,
                            working_msg = working_msg,
                            finish_msg = finish_msg
                        });
                    }
                }
            }
            return model;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUpdateMemoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Common.Constants.Message_register.FAILURE_001);
            }
            if (request.receiver_cd == 0)
            {
                return BadRequest("宛先は必修項目です。");
            }
            if (!IsValidMemoRecord(request, out var message))
            {
                return BadRequest(message);
            }
            if (_context.T_MEMO == null)
            {
                return Problem("Entity set 'web_groupwareContext.Memo'  is null.");
            }

            using (IDbContextTransaction tran = _context.Database.BeginTransaction())
            {
                var user_id = @User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value;
                try
                {
                    var record_new = new T_MEMO
                    {
                        memo_no = GetNextNo(Utilities.DataTypes.MEMO_NO),
                        state = MEMO_STATE_UNREAD,
                        receiver_type = request.receiver_type,
                        receiver_cd = request.receiver_cd,
                        comment_no = request.comment_no,
                        phone = request.phone,
                        content = request.content,
                        sender_cd = Convert.ToInt32(user_id),
                        working_cd = 0,
                        finish_cd = 0,
                        create_date = DateTime.Now,
                        update_user = user_id,
                        update_date = DateTime.Now
                    };
                    var tracked = _context.T_MEMO.Add(record_new);

                    if (request.receiver_type == 0)
                    {
                        var memo_read = new T_MEMO_READ
                        {
                            memo_no = tracked.Entity.memo_no,
                            staff_cd = request.receiver_cd,
                            read_flag = false,
                            update_user = user_id,
                            update_date = DateTime.Now
                        };
                        _context.T_MEMO_READ.Add(memo_read);
                    }
                    else
                    {
                        var staf_cds = _context.T_GROUPSTAFF
                            .Where(m => m.group_cd == request.receiver_cd)
                            .Select(m => m.staf_cd)
                            .ToList();
                        foreach (var staf_cd in staf_cds)
                        {
                            var memo_read = new T_MEMO_READ
                            {
                                memo_no = tracked.Entity.memo_no,
                                staff_cd = staf_cd,
                                read_flag = false,
                                update_user = user_id,
                                update_date = DateTime.Now
                            };
                            _context.T_MEMO_READ.Add(memo_read);
                        }
                    }

                    await _context.SaveChangesAsync();

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            
            return ProperRedirect();
        }

        public async Task<IActionResult> UpdateReadState(int memo_no)
        {
            if (_context.T_MEMO == null)
            {
                return Problem("Entity set 'web_groupwareContext.Memo'  is null.");
            }

            int user_id = Convert.ToInt32(@User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value);
            try
            {
                var memoRead = await _context.T_MEMO_READ
                    .Where(m => m.memo_no == memo_no && m.staff_cd == user_id)
                    .FirstOrDefaultAsync();
                if (memoRead != null && !memoRead.read_flag)
                {
                    using (IDbContextTransaction tran = _context.Database.BeginTransaction())
                    {
                        memoRead.read_flag = true;

                        _context.T_MEMO_READ.Update(memoRead);
                        await _context.SaveChangesAsync();

                        var memoItem = await _context.T_MEMO.FindAsync(memo_no);
                        if (memoItem.state == MEMO_STATE_UNREAD && CheckAllReadMemo(memoItem))
                        {
                            memoItem.state = MEMO_STATE_READ;
                            _context.T_MEMO.Update(memoItem);
                            await _context.SaveChangesAsync();
                        }

                        tran.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return ProperRedirect();
        }

        public async Task<IActionResult> Update([FromBody] CreateUpdateMemoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(Common.Constants.Message_change.FAILURE_001);
            }
            if (!IsValidMemoRecord(request, out var message))
            {
                return BadRequest(message);
            }
            if (_context.T_MEMO == null)
            {
                return Problem("Entity set 'web_groupwareContext.Memo'  is null.");
            }

            var memoItem = await _context.T_MEMO.FindAsync(request.memo_no);
            if (memoItem != null)
            {
                using (IDbContextTransaction tran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (request.receiver_type != memoItem.receiver_type || request.receiver_cd != memoItem.receiver_cd)
                        {
                            var itemsRemove = _context.T_MEMO_READ.Where(m => m.memo_no == request.memo_no);
                            _context.T_MEMO_READ.RemoveRange(itemsRemove);

                            if (request.receiver_type == 0)
                            {
                                var memo_read = new T_MEMO_READ
                                {
                                    memo_no = request.memo_no,
                                    staff_cd = request.receiver_cd,
                                    read_flag = false,
                                    update_user = @User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value,
                                    update_date = DateTime.Now
                                };
                                _context.T_MEMO_READ.Add(memo_read);
                            }
                            else
                            {
                                var staf_cds = _context.T_GROUPSTAFF
                                    .Where(m => m.group_cd == request.receiver_cd)
                                    .Select(m => m.staf_cd)
                                    .ToList();
                                foreach (var staf_cd in staf_cds)
                                {
                                    var memo_read = new T_MEMO_READ
                                    {
                                        memo_no = request.memo_no,
                                        staff_cd = staf_cd,
                                        read_flag = false,
                                        update_user = @User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value,
                                        update_date = DateTime.Now
                                    };
                                    _context.T_MEMO_READ.Add(memo_read);
                                }
                            }
                            memoItem.receiver_type = request.receiver_type;
                            memoItem.receiver_cd = request.receiver_cd;
                        }
                        memoItem.comment_no = request.comment_no;
                        memoItem.phone = request.phone;
                        memoItem.content = request.content;

                        var user_id = Convert.ToInt32(@User.FindFirst(Utilities.ClaimTypes.STAF_CD).Value);
                        if (request.working == 1)
                        {
                            if (memoItem.working_cd == 0)
                            {
                                memoItem.working_cd = user_id;
                                memoItem.working_date = DateTime.Now;
                            }
                        }
                        else
                        {
                            memoItem.working_cd = 0;
                        }
                        if (request.finish == 1)
                        {
                            if (memoItem.finish_cd == 0)
                            {
                                memoItem.finish_cd = user_id;
                                memoItem.finish_date = DateTime.Now;
                            }
                        }
                        else
                        {
                            memoItem.finish_cd = 0;
                        }
                        if (request.finish == 1)
                        {
                            memoItem.state = MEMO_STATE_FINISH;
                        }
                        else if (request.working == 1)
                        {
                            memoItem.state = MEMO_STATE_WORKING;
                        }
                        else
                        {
                            memoItem.state = CheckAllReadMemo(memoItem) ? MEMO_STATE_READ : MEMO_STATE_UNREAD;
                        }
                        memoItem.update_date = DateTime.Now;

                        _context.T_MEMO.Update(memoItem);
                        await _context.SaveChangesAsync();

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
            }
  
            return ProperRedirect();
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.T_MEMO == null)
            {
                return Problem("Entity set 'web_groupwareContext.memoItem'  is null.");
            }
            var memoDetail = await _context.T_MEMO.FindAsync(id);
            if (memoDetail != null)
            {
                try
                {
                    var itemsRemove = _context.T_MEMO_READ.Where(m => m.memo_no == id);
                    _context.T_MEMO_READ.RemoveRange(itemsRemove);

                    _context.T_MEMO.Remove(memoDetail);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            return ProperRedirect();
        }

        [Authorize]
        public async Task<IActionResult> GetMemoReadCount()
        {
            int count;

            var claim = HttpContext.User.FindFirst(Utilities.ClaimTypes.STAF_CD);
            if (claim != null)
            {
                int staf_cd = int.Parse(claim.Value);
                count = await Task.Run(() => _context.T_MEMO_READ
                    .Where(m => m.staff_cd == staf_cd && !m.read_flag)
                    .Count());
            }
            else
            {
                count = 0;
            }

            var ret = new
            {
                count
            };
            return new JsonResult(ret);
        }

        private IActionResult ProperRedirect()
        {
            bool is_sent = true;
            if (TempData.TryGetValue("is_sent", out var res))
            {
                is_sent = Convert.ToBoolean(res);
            }
            var filter_state = 0;
            if (TempData.TryGetValue("filter_state", out res))
            {
                filter_state = Convert.ToInt32(res);
            }
            var filter_user = 0;
            if (TempData.TryGetValue("filter_user", out res))
            {
                filter_user = Convert.ToInt32(res);
            }

            var routeValues = new { state = filter_state, user = filter_user };
            if (is_sent)
                return RedirectToAction("Memo_sent", routeValues);
            else
                return RedirectToAction("Memo_received", routeValues);
        }

        private bool IsValidMemoRecord(CreateUpdateMemoRequest request, out string validationMessage)
        {
            validationMessage = string.Empty;

            Regex regex = new Regex(Common.Constants.RegularExpression.TEL);
            if (!string.IsNullOrWhiteSpace(request.phone) && !regex.IsMatch(request.phone))
            {
                validationMessage = "電話番号は半角数字と半角ハイフンのみ入力可能です。";
            }
            else if (request.comment_no == "")
            {
                validationMessage = "用件は必修項目です。";
            }
            else if (string.IsNullOrWhiteSpace(request.content))
            {
                validationMessage = "伝言は必修項目です。";
            }
            else if (request.content.Length > MaxContentLength)
            {
                validationMessage = $"伝言は{MaxContentLength}文字以内で入力してください。";
            }

            return string.IsNullOrEmpty(validationMessage);
        }
        private bool CheckAllReadMemo(T_MEMO? memoItem)
        {
            if (memoItem == null) return false;

            if (memoItem.receiver_type == 0)
            {
                var memoRead = _context.T_MEMO_READ
                    .Where(m => m.memo_no == memoItem.memo_no && m.staff_cd == memoItem.receiver_cd && m.read_flag)
                    .FirstOrDefault();
                return (memoRead != null);
            }
            else
            {
                var staff_cds = _context.T_GROUPSTAFF
                    .Where(m => m.group_cd == memoItem.receiver_cd)
                    .Select(m => m.staf_cd)
                    .ToList();
                foreach(var staff in staff_cds)
                {
                    var memoRead = _context.T_MEMO_READ
                        .Where(m => m.memo_no == memoItem.memo_no && m.staff_cd == staff && m.read_flag)
                        .FirstOrDefault();
                    if (memoRead == null)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}