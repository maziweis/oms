/**
 * @功能描述：封装各种系统消息框基于Bootstrap3封装
 * @前置插件：Bootstrap3
 * @开 发 者：魏巍
 * @开发日期：2015-04-30
 * @version 1.0
 */
var fz = {};
fz.Msgbox = function () {
    var $msgbox, offTimer;

    var popup = function (_msg) {//自动关闭
        var _html
        = ''
        + '<div class="dialog modal fade">'
        + '<div class="modal-dialog" style="width:200px; text-align:center;">'
        + '<div class="modal-content">'
        + '<div class="modal-body"></div>'
        + '</div>'
        + '</div>'
        + '</div>'
        ;
        var $dlg = $(_html);
        $('.modal-body', $dlg).append(_msg);

        $dlg.modal({
            backdrop: 'static',
            keyboard: false
        });

        $dlg.on('hidden.bs.modal', function () {
            $dlg.remove();
        });

        clearTimeout(offTimer);
        offTimer = setTimeout(function () {
            $dlg.modal("hide");
        }, 1500);
    }

    var succeed = function () {//操作成功
        popup('操作成功');
    }

    var alert = function (_msg, _callback) {
        var _html
        = ''
        + '<div class="dialog modal fade">'
        + '<div class="modal-dialog" style="width:300px;">'
        + '<div class="modal-content">'
        + '<div class="modal-body" style="height:100px;"></div>'
        + '<div class="modal-footer"><button type="button" class="btn btn-default btn-sm" data-dismiss="modal"><span class="glyphicon glyphicon-ok"></span> 确&nbsp;&nbsp;&nbsp;&nbsp;定</button></div>'
        + '</div>'
        + '</div>'
        + '</div>'
        ;
        var $dlg = $(_html);
        $('.modal-body', $dlg).append(_msg);

        $('.modal-footer button:first', $dlg).click(function () {
            _callback();
        });

        $dlg.modal({
            backdrop: 'static',
            keyboard: false
        });

        $dlg.on('hidden.bs.modal', function () {
            $dlg.remove();
        });
    }

    var confirm = function (_msg, _callback) {
        var _html
        = ''
        + '<div class="dialog modal fade">'
        + '<div class="modal-dialog" style="width:300px;">'
        + '<div class="modal-content">'
        + '<div class="modal-header"><button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button><h4 class="modal-title">提示消息</h4></div>'
        + '<div class="modal-body" style="height:100px;"></div>'
        + '<div class="modal-footer">'
        + '<button type="button" class="btn btn-default btn-sm" data-dismiss="modal"><span class="glyphicon glyphicon-ok"></span> 确&nbsp;&nbsp;&nbsp;&nbsp;定</button>'
        + '<button type="button" class="btn btn-default btn-sm" data-dismiss="modal"><span class="glyphicon glyphicon-remove"></span> 取&nbsp;&nbsp;&nbsp;&nbsp;消</button>'
        + '</div>'
        + '</div>'
        + '</div>'
        + '</div>'
        ;
        var $dlg = $(_html);
        $('.modal-body', $dlg).append(_msg);

        $('.modal-footer button:first', $dlg).click(function () {
            _callback();
        });

        $dlg.modal({
            backdrop: 'static',
            keyboard: false
        });

        $dlg.on('hidden.bs.modal', function () {
            $dlg.remove();
        });
    }

    return {
        popup: popup,
        succeed: succeed,
        alert: alert,
        confirm: confirm
    }
}();