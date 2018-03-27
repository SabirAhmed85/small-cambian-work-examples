angular.module('app').component('registrationSubmissionReceivedModalComponent', {
    templateUrl: 'app/components/shared/modals/registration-submission-received/registration-submission-received-modal-dialog.view.html',
    bindings: {
        resolve: '<',
        close: '&'
    },
    controller: function () {
        var $ctrl = this;

        $ctrl.$onInit = function () {
            $ctrl.translatedModalMessages = {
                'Title': $ctrl.resolve.messages.Title,
                'Head': $ctrl.resolve.messages.Head,
                'Message': $ctrl.resolve.messages.Message,
                'SubHead': $ctrl.resolve.messages.SubHead,
                'Message2': $ctrl.resolve.messages.Message2
            }
            $ctrl.showClose = $ctrl.resolve.messages.ShowClose;
        };

        $ctrl.ok = function () {
            $ctrl.close();
        }
    }
});
