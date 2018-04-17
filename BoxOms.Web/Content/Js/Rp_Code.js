$(document).ready(function () {

    $(document).on('click', '.btnDelete', function (e) {//删除
        var _id = $(this).attr("dataid");
        fz.Msgbox.confirm("您确定要删除该记录吗？", function () {
            $.ajax({
                type: 'post',
                url: '/RpCode/Delete',
                data: { id: _id },
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
});