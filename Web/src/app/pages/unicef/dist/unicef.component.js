"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
exports.__esModule = true;
exports.UnicefComponent = void 0;
var core_1 = require("@angular/core");
var change_password_component_1 = require("src/app/shared/components/change-password/change-password.component");
var MessageConstant_1 = require("src/app/utility/MessageConstant");
var UnicefComponent = /** @class */ (function () {
    function UnicefComponent(authService, modalService, toastrService) {
        this.authService = authService;
        this.modalService = modalService;
        this.toastrService = toastrService;
        this.sidebarCollapsed = false;
    }
    UnicefComponent.prototype.ngOnInit = function () {
        this.user = this.authService.userObject;
    };
    UnicefComponent.prototype.togleSidebar = function () {
        var sidebar = document.getElementById('sidebar');
        var content = document.getElementById('content');
        if (!this.sidebarCollapsed) {
            sidebar.style.width = "0px";
            content.style.width = "100%";
        }
        else {
            sidebar.style.width = "260px";
            content.style.width = "calc(100% - 260px)";
        }
        this.sidebarCollapsed = !this.sidebarCollapsed;
    };
    UnicefComponent.prototype.changePassword = function () {
        var _this = this;
        this.modalService.open(change_password_component_1.ChangePasswordComponent, {})
            .then(function (model) {
            if (model) {
                _this.authService.changePassword(model)
                    .then(function (x) {
                    _this.toastrService.success(MessageConstant_1.MessageConstant.SaveSuccessful);
                });
            }
        })["catch"](function (x) { return x; });
    };
    __decorate([
        core_1.ViewChild('sidebarToggle')
    ], UnicefComponent.prototype, "sidebarToggle");
    UnicefComponent = __decorate([
        core_1.Component({
            selector: 'app-unicef',
            templateUrl: './unicef.component.html',
            styleUrls: ['./unicef.component.scss']
        })
    ], UnicefComponent);
    return UnicefComponent;
}());
exports.UnicefComponent = UnicefComponent;
