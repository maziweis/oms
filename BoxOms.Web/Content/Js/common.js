$(document).ready(function () {

    //隐藏modal时清除数据
    $("#myModal").on("hidden.bs.modal", function () {
        $(this).removeData("bs.modal");
    });

});

function formSuccess(result) {
    if (result.success) {
        $('#myModal').modal('hide');
        location.reload();
    } else {
        $('.modal-content').html(result);
    }
}

function search() {
    $('#Grid_Pager_PageIndex').val(1);
    $("#form1").submit();
}

function changePageIndex(pIndex) {
    $('#Grid_Pager_PageIndex').val(pIndex);
    $("#form1").submit();
}

function changePageSize(_this) {
    $('#Grid_Pager_PageIndex').val(1);
    $('#Grid_Pager_PageSize').val($(_this).val());
    $("#form1").submit();
}

function exit() {
    fz.Msgbox.confirm("您确定要退出系统吗？", function () {
        $.ajax({
            type: 'post',
            url: '/SyPassport/Exit',
            dataType: 'json',
            success: function (result) {
                switch (result.flag) {
                    case 1:
                        window.location = "/SyPassport/Login";
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
}