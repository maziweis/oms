$(document).ready(function () {

    $(document).on('click', '.btnExport', function (e) {
        $.ajax({
            type: 'post',
            url: '/BoxGood/ExportRes',
            dataType: 'json',
            success: function (result) {
                switch (result.flag) {
                    case 1:
                        alert("操作成功！");
                        window.location.reload();
                        break;
                    case 0:
                        alert(result.msg);
                        break;
                    case -1:
                        break;
                    case -2:
                        break;
                    case -3:
                        break;
                }
            }
        });
    });
});