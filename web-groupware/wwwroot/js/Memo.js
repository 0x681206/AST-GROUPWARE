var type_dlg = 0;
var selected_id = -1;

$(document).ready(function () {
    $("#form-create").validate({
        rules: {
            receiver: "required",
            phone: {
                regex: /^[-0-9]+$/
            },
            content: {
                required: true,
                maxlength: 255
            },
        },
        messages: {
            receiver: "宛先は必須項目です。",
            phone: {
                regex: "電話番号は半角数字と半角ハイフンのみ入力可能です。",
            },
            content: {
                required: "伝言は必須項目です。",
                maxlength: "255文字以下で入力してください。",
            },
        },
    });
});
function createMemoDlg() {
    $('label.error').hide();

    type_dlg = 0;
    $("#receiver").val(1);
    $("#phone").val("");
    $("#comment").val("1");
    $("#content").val("");
    $("#readers").text("");

    $("#phone").attr("disabled", false);
    $("#comment").attr("disabled", false);
    $("#content").attr("disabled", false);
    $("#receiver").attr("disabled", false);
    $("#checkWorking").attr("checked", false);
    $("#checkFinish").attr("checked", false);

    $("#checkbox_div").hide();
    $("#title").text("新規登録");
    var myModal = new bootstrap.Modal(document.getElementById('memo-modal'), {});
    myModal.toggle();
}

function editMemoDlg(memo_no, state, receiver, phone, comment, readers, working_msg, finish_msg, is_edit) {
    $('label.error').hide();
    is_edit = is_edit == 'True' ? true : false;

    type_dlg = 1;

    selected_id = memo_no;
    var content = $("#table_content" + memo_no).text();

    $("#checkWorking").attr("checked", false);
    $("#checkFinish").attr("checked", false);
    $("#working_msg").text("");
    $("#finish_msg").text("");
    if (working_msg && working_msg.length > 0) {
        $("#checkWorking").attr("checked", true);
        $("#working_msg").text('対応します　済　　' + working_msg);
    }
    if (state == 3) {
        $("#checkFinish").attr("checked", true);
        $("#finish_msg").text('処理済　　　済　　' + finish_msg);
    }

    $("#receiver").val(receiver);
    $("#phone").val(phone);
    $("#comment").val(comment);
    $("#content").val(content);
    $("#readers").text(readers);

    $("#receiver").attr("disabled", !is_edit);
    $("#phone").attr("disabled", !is_edit);
    $("#comment").attr("disabled", !is_edit);
    $("#content").attr("disabled", !is_edit);

    $("#checkbox_div").show();
    $("#title").text("編集");

    var myModal = new bootstrap.Modal(document.getElementById('memo-modal'), {});
    myModal.toggle();

    var data = {
        memo_no: selected_id,
        // state: 1
    };
    var url = $("#url_update_read").val();

    $.ajax({
        method: "get",
        url: url,
        data: data,

        success: function (result) {
            //window.location.reload();
        },
        error: function (xhr) {
            console.log("Error: " + xhr.responseText);
        }
    });
}

function checkWorking_Clicked() {
}

function checkFinish_Clicked() {
}

function FilterChanged() {
    $("#selectionForm").submit();
}

$("#form-create").on('submit', function (e) {
    e.preventDefault();

    var receiver_str = $('#receiver').val();
    if (receiver_str == null || receiver_str == "") {
        // alert("宛先は必修項目です。");
        return;
    }

    var receiver_type = receiver_str[0] == 's' ? 0 : 1;
    var receiver_cd = 0;
    receiver_cd = receiver_str.slice(5);

    // create memo
    if (type_dlg == 0) {
        var form = $("#form-create")[0];
        var url = $("#url_create").val();
        if (form.checkValidity()) {
            var data = {
                receiver_type: receiver_type,
                receiver_cd: receiver_cd,
                comment_no: $('#comment').val(),
                phone: $('#phone').val(),
                content: $('#content').val(),
                working: 0,
                finish: 0
            };
            $.ajax({
                method: "post",
                url: url,
                contentType: 'application/json',
                data: JSON.stringify(data),

                success: function (result) {
                    window.location.reload();
                },
                error: function (xhr) {
                    console.log(xhr.responseText);
                }
            });
            // $('#memo-modal').modal('hide');
        } else {
            e.stopPropagation();
        }
    }
    else {
        if (selected_id == -1) return;

        var working = $("#checkWorking").is(":checked") ? 1 : 0;
        var finish = $("#checkFinish").is(":checked") ? 1 : 0;
        var url = $("#url_update").val();
        var data = {
            memo_no: selected_id,
            receiver_type: receiver_type,
            receiver_cd: receiver_cd,
            comment_no: $('#comment').val(),
            phone: $('#phone').val(),
            content: $('#content').val(),
            working: working,
            finish: finish
        };
        $.ajax({
            method: "post",
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(data),

            success: function (result) {
                window.location.reload();
            },
            error: function (xhr) {
                console.log(xhr.responseText);
            }
        });
    }
});