const _dataTransfer = new DataTransfer()
// #region 選択画像反映(プレビュー)
/**
 * @param {any} obj ファイルリスト
 */

function previewImage(obj) {
    var arr_fileName = [];
    //アップロード済みファイル名を追加
    console.log("保存済↓")
    document.getElementsByClassName('main_files').forEach(main_file => {
        arr_fileName.push($(main_file).text());
        console.log( $(main_file).text());
    });
    //前回選択・ドロップファイル名を追加
    console.log("前回選択・ドロップ↓")
    _dataTransfer.files.forEach(file => {
        console.log(file.name);
        if (!arr_fileName.includes(file.name)) {
            arr_fileName.push(file.name)
        } else {
            var count = 1
            while (true) {
                var kandidat = file.name + '（' + count + '）';
                if (!arr_fileName.includes(kandidat)) {
                    file.name = kandidat;
                    arr_fileName.push(file.name);
                    console.log( file.name+"　ファイル名変更後");
                    break;
                }
                count++;
            }
        }
    });
    //今回選択・ドロップファイル名を検査・ファイル作り替え
    console.log("今回選択・ドロップ↓")
    const _dataTransfer_work = new DataTransfer()
    for (var i = 0; i < obj.files.length; i++) {
        console.log(obj.files[i]);
        if (!arr_fileName.includes(obj.files[i].name)) {
            arr_fileName.push(obj.files[i].name)
            _dataTransfer_work.items.add(obj.files[i])
        } else {
            var count = 1;
            while (true) {
                var arr_work = obj.files[i].name.split('.');
                var kandidat = "";
                for (var w = 0; w < arr_work.length - 1; w++) {
                    kandidat = kandidat + arr_work[w] + ".";
                }
                kandidat = kandidat.slice(0, -1);
                kandidat = kandidat + '（' + count + '）';
                // ファイルの拡張子を取得
                const fileExtention = obj.files[i].name.substring(obj.files[i].name.lastIndexOf(".") + 1);
                kandidat = kandidat + "." + fileExtention;

                if (!arr_fileName.includes(kandidat)) {
                    const blob = obj.files[i].slice(0, obj.files[i].size, obj.files[i].type);
                    // ファイル名称変更後のファイルオブジェクト
                    const renamedFile = new File([blob], kandidat, { type: obj.files[i].type });

                    //obj.files[i] = renamedFile;
                    arr_fileName.push(kandidat);
                    _dataTransfer_work.items.add(renamedFile);
                    console.log(renamedFile.name + "　ファイル名変更後");
                    break;
                }
                count++;
            }
        }
    }
    //今回選択・ドロップファイル　HTML追加
    console.log("HTML追加");
    _dataTransfer_work.files.forEach(file => {
        _dataTransfer.items.add(file)
        var count = $('#div_icon').find('.div_icon_child').length;
        var svg = file.name.split('.').pop() + ".svg";
        $('#div_icon').append('<div id="div_icon_' + count + '" class="div_icon_child dropdown fileAreaHeitWidth">' +
            '<input type="hidden" id="List_T_INFO_FILE_' + count + '__fileName" name="List_T_INFO_FILE[' + count + '].fileName" value="' + file.name + '">' +
            '<input type="hidden" id="List_T_INFO_FILE_' + count + '__file_no" name="List_T_INFO_FILE[' + count + '].file_no" value="">' +
            '<button class="bg-white border-0 p-0 dropdown-toggle btn_file fileAreaInnerWidth" type="button" data-bs-toggle="dropdown" aria-expanded="false">' +
            '<div class="div_tooltip" data-toggle="tooltip" data-placement="top" title="' + file.name + '">' +
            '<div class="div_img_file bg-light p-2">' +
            '<img src="' + baseUrl + 'images/file-icons/' + svg + '" alt="icon" class="">' +
            '</div>' +
            '<div class="text-wrap">' + file.name + '</div>' +
            '<div class="">' +
            '<progress class="fileAreaInnerWidth" value="0" id="prog_' + file.name + '" max="100"></progress>' +
            '</div>' +
            '</div>' +
            '</button>' +
            '<ul class="dropdown-menu fileAreaInnerWidth text-center">' +
            '<button class="dropdown-item delete_file" type="button" role="button" id="delete_' + count + '" data-dir_kind="2"  data-file_name="' + file.name + '">削除</button>' +
            '</ul>' +
            '</div>');
    });
    $('.div_tooltip').tooltip();
    //file置き換え
    console.log("file置き換え");
    document.getElementById('File').files = _dataTransfer.files;

    //今回選択・ドロップファイル　ファイルアップロード追加
    console.log("ファイルアップロード↓");
    _dataTransfer_work.files.forEach(file => {
        console.log("アップロード開始　" + file.name);
        let url = baseUrl;
        var formData = new FormData();
        formData.append("file", file);
        formData.append("work_dir", $("#Work_dir").val());
        var progressBar = document.getElementById('prog_' + file.name);
        //var progressValue = document.getElementById('pv');
        $.ajax({
            url: url + "Notice/UploadFile",
            type: 'POST',
            processData: false,
            contentType: false,
            async: true,
            data: formData,
            xhr: function () {
                XHR = $.ajaxSettings.xhr();
                if (XHR.upload) {
                    XHR.upload.addEventListener('progress', function (e) {
                        var progVal = parseInt(e.loaded / e.total * 10000) / 100;
                        progressBar.value = progVal;
                        //progressValue.innerHTML = progVal + '%';
                    }, false);
                }
                return XHR;
            },
            success: function (data) {
                console.log(data);
                console.log("アップロード完了　" + file.name);
            },
            error: function () {
                console.log("アップロード失敗　" + file.name);
            }
        });
    });

}
// #endregion

$(function () {
    var list_btn_file = $('.div_tooltip');
    for (var i = 0; i < list_btn_file.length; i++) {
        $(list_btn_file[i]).tooltip();
    }
    $('.btn_file').on('mouseenter', function (e) {
        $(this).find('.div_img_file').addClass('border');
        $(this).find('.div_img_file').addClass('border-info');
    });
    $('.btn_file').on('mouseleave', function (e) {
                $(this).find('.div_img_file').removeClass('border');
                $(this).find('.div_img_file').removeClass('border-info');
    });


    //$('.btn_file').mouseover(function (e) {
    //    console.log('mouseover:' + e.target.id);
    //})
    //        //マウスカーソルが重なった時の処理
    //        $(this).find('.div_img_file').addClass('border');
    //        $(this).find('.div_img_file').addClass('border-info');


    //        //マウスカーソルが離れた時の処理
    //        $(this).find('.div_img_file').removeClass('border');
    //        $(this).find('.div_img_file').removeClass('border-info');
    //);
    // #region ファイル追加（クリップボード・選択ボタン・ドロップ）
    //クリップボードの貼り付け
    document.addEventListener("paste", function (e) {
        const _dataTransfer_clip = new DataTransfer()
        // event からクリップボードのアイテムを取り出す
        var items = e.clipboardData.items; // ここがミソ
        for (var i = 0; i < items.length; i++) {
            var item = items[i];
            if (item.type.indexOf("image") != -1) {
                // 画像のみサーバへ送信する
                var image = item.getAsFile();
                _dataTransfer_clip.items.add(image);
            }
        }
        previewImage(_dataTransfer_clip);
    });
    //選択ボタンでファイルが選択さえｒた時
    $('#File').on('change', function (e) {
        previewImage(this);
    });
    // ファイルドラッグアンドドロップ
    // ドラッグしている要素がドロップされたとき
    $('.dropArea').on('drop', function (event) {
        event.preventDefault();

        //$('#input_file')[0].files = event.originalEvent.dataTransfer.files;
        //$(this).find('input[type="file"]')[0].files = event.originalEvent.dataTransfer.files;
        //var obj = $(this).find('input[type="file"]')[0];
        previewImage(event.originalEvent.dataTransfer);
    });
    // #endregion

    // #region ドロップ対象外のオブジェクト抑制(ファイルがダウンロードされるため)
    $(document).on('dragenter', function (e) {
        e.stopPropagation();
        e.preventDefault();
    });
    $(document).on('dragover', function (e) {
        e.stopPropagation();
        e.preventDefault();
    });
    $(document).on('drop', function (e) {
        e.stopPropagation();
        e.preventDefault();
    });
    // #endregion

    // #region 削除ボタン
    $(document).on("click", ".delete_file", function () {

        var work_dir = $('#Work_dir').val();
        var dir_kind = $(this).data('dir_kind');
        var file_name = $(this).data('file_name');
        if (dir_kind==1&&!confirm('「' + file_name + '」を削除しても宜しいですか？この操作は元に戻せません。')) {

            // submitボタンの効果をキャンセルし、クリックしても何も起きない
            return false;

            // 「OK」をクリックした際の処理を記述
        } else {
            let url = baseUrl;
            var btn_delete = $(this);
            $.ajax({
                type: "POST",
                url: url + "Notice/DeleteFile?work_dir=" + work_dir + "&" + "dir_kind=" + dir_kind + "&" + "file_name=" + file_name,
                async: false,
                success: function (ret, status, xhr) {
                    if (xhr.status === 200) {
                        // ステータスコードが 200 の場合
                        div_icon_empty(btn_delete);
                    } else if (xhr.status === 202) {
                        // ステータスコードが 200 以外の場合
                        div_icon_empty(btn_delete);
                    } else {

                    }
                    removeFileFromDataTransfer(file_name)
                },
                error: function (e) {
                    //レスポンスが返って来ない場合
                },
            });
        }
    });
    // #endregion
    // #region ダウンロードボタン
    $(document).on("click", ".download_file", function () {
        var file_name = $(this).data('file_name');
        let url = baseUrl + "Notice/DownloadFile?" + "file_name=" + file_name;

        //指定したURLからファイルをダウンロードする
        funcFileDownload(url, file_name);
    });
    // #endregion
    // #region ファイルドラッグアンドドロップ(イベントキャンセル)
    //$('.dropArea').on('dragenter', function (e) {
    //    e.stopPropagation();
    //    e.preventDefault();
    //});

    //$('.dropArea').on('dragover', function (e) {
    //    e.stopPropagation();
    //    e.preventDefault();
    //});
    // #endregion
});

function div_icon_empty(btn_delete) {
    var arr_no = btn_delete.attr('id').split('_');
    var no = arr_no[arr_no.length - 1];
    $('#div_icon_' + no).empty();
    $('#div_icon_' + no).addClass('d-none');

}
function removeFileFromDataTransfer(file_name) {
    for (var i = 0; i < _dataTransfer.items.length; i++) {
        console.log("前回選択ドロップからファイル削除開始");
        if (_dataTransfer.files[i].name == file_name) {
            console.log(_dataTransfer.files[i].name);
            _dataTransfer.items.remove(i);
        }
    }
}
