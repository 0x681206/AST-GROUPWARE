$(function () {
    let url = baseUrl;
    $.ajax({
        type: "POST",
        url: url + "Base/GetNoticeHeader",
        success: function (ret) {
            if (ret['message'] == null && ret['files'].length == 0) {
                $("#layout_div_notice").addClass("d-none");
            }
            if (ret['message'] == null) {
                $("#layout_notice").addClass("d-none");
            } else {
                $("#layout_notice").text(ret['message']);
            }
            if (ret['files'].length == 0) {
                $("#layout_div_notice_download").addClass("d-none");
            } else {
                for (var i = 0; i < ret['files'].length; i++) {
                    $('#layout_div_notice_download').append('<div class="px-1"><a class="layout_download_file btn btn-info" data-file_name="' + ret['files'][i] + '">' + ret['files'][i] +'</a></div>');
                }
            }
        },
        error: function (e) {
            $("#layout_notice").text("社内通知を取得できませんでした。システム管理者に問い合わせ下さい。");
        },
    });
    // #region ダウンロードボタン
    $(document).on("click", ".layout_download_file", function () {
        var file_name = $(this).data('file_name');
        let url = baseUrl + "Notice/DownloadFile?" + "file_name=" + file_name;

        //指定したURLからファイルをダウンロードする
        funcFileDownload(url, file_name);
    });
    // #endregion

    GetBukkenCommentReadCount(url);
    GetMemoReadCount(url);

    $.ajax({
        type: "POST",
        url: url + "Base/GetReportCount",
        success: function (ret) {
            $("#layout_report_count").text(ret);
        },
        error: function (e) {
            $("#layout_report_count").text("件数取得失敗");
        },
    });
    $.ajax({
        type: "POST",
        url: url + "Base/GetGroupItems",
        success: function (ret) {
            var totalCount = 0;
            var html = '<ul class="nav-second-level">';
            for (var i = 0; i < ret.length; i++) {
                var group = ret[i];
                html += '<li>';

                // Manually construct the URL with the group_cd parameter
                var url = `/groupware/EmployeeGroup/GetDetails/${group.group_cd}`;

                html += `<a href="${url}">${group.group_name}`+` (${group.user_count})</a>`;
                html += '</li>';
                totalCount += group.user_count;
            }
            html += '</ul>';
            $('#total-count').text('(' + totalCount + ')');
            $('#menu-container').html(html);
        },
        error: function (e) {
            console.log('error here?', e);
        },
    });


    // 確認ボタンをクリックするとイベント発動
    $('.kakuninDialog').on('click', function (e) {
        //$("#frm").validate()
        //var form = $(this).closest('form');
        if ($(this).closest('form').valid()) {
            // もしキャンセルをクリックしたら
            if (!confirm('登録してもよろしいですか？')) {

                // submitボタンの効果をキャンセルし、クリックしても何も起きない
                return false;

                // 「OK」をクリックした際の処理を記述
            } else {

                $(this).closest('form').trigger("submit");

            }
        }
    });
    $('input').on('change', function (e) {
        $(this).closest('form').find(".validation-summary-errors").addClass('d-none');
    });

    //datatable
    $(".datatables").DataTable({
        "language": {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/ja.json",
        },
        //"columnDefs": [
        //    { "searchable": false, "targets": 1 },
        //    { "searchable": false, "targets": 2 }
        //],
        searching: false,
        "order": []
    });
    $(".datatables-search").DataTable({
        "language": {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/ja.json",
        },
        searching: true,
        "order": []
    });
    $('.site_iframe').colorbox(
        {
            iframe: true,
            width: "80%", height: "80%",
            opacity: 0.5
        });
    $(".site_iframe_close").on('click',function () {
        parent.$.fn.colorbox.close();
        return false;
    });


});

/**
 * 非同期通信　共通関数　PDFファイルのダウンロード
 *
 * */
function funcFileDownload(url, filename, messageForFailure) {
    //show_loading();
    var xhr = new XMLHttpRequest();
    xhr.open("GET", url, true);
    xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
    xhr.responseType = "blob";
    xhr.onloadend = function () {
        hide_loading();

    };

    xhr.onloadstart = function () {
        show_loading();
    };

    xhr.onload = function () {
        console.log(this.status);
        if (xhr.readyState === 4 && xhr.status === 200) {
            var blob = new Blob([xhr.response]);
            const url = window.URL.createObjectURL(blob);
            var a = document.createElement('A');
            a.href = url;
            a.download = filename;
            a.click();
            setTimeout(function () {
                window.URL.revokeObjectURL(blob)
                    , 100
            });

            //メッセージ表示
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-bottom-left",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
            toastr["success"]("ダウンロードに成功しました");
        } else {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-bottom-left",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
            toastr["error"](messageForFailure == null ? "ダウンロードに失敗しました" : messageForFailure);

        }

    };

    xhr.send();
}
function GetBukkenCommentReadCount(url) {
    $.ajax({
        type: "POST",
        url: url + "Base/GetBukkenCommentReadCount",
        success: function (ret) {
            $("#layout_bukken_memo_count").text(ret['count']);
        },
        error: function (e) {
            $("#layout_bukken_memo_count").text("件数取得失敗");
        },
    });
}

function GetMemoReadCount(url) {
    $.ajax({
        type: "POST",
        url: url + "Memo/GetMemoReadCount",
        success: function (ret) {
            var count = ret['count'];
            console.log('------------', count)
            if (count > 0)
                $(".memo_unread_count").text(count);
        },
        error: function (e) {
        },
    });
}