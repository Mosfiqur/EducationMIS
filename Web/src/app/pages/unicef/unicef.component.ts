import { Component, Inject, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { AppUser } from 'src/app/models/auth/AppUser';
import { ModalService } from 'src/app/services/modal.service';
import { IPasswordChangeModel } from 'src/app/models/user/passwordResetModel';
import { ChangePasswordComponent } from 'src/app/shared/components/change-password/change-password.component';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { User } from 'src/app/models/user/user.model';
import { Router } from '@angular/router';
import { NotificationService } from 'src/app/services/notification.service';
import { jsonpCallbackContext } from '@angular/common/http/src/module';
import { Observable } from 'rxjs/Observable';
import { PaginationInstance } from 'ngx-pagination';
import { JQ_TOKEN } from 'src/app/services/jQuery.service';


@Component({
  selector: 'app-unicef',
  templateUrl: './unicef.component.html',
  styleUrls: ['./unicef.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class UnicefComponent implements OnInit {

  @ViewChild('sidebarToggle') sidebarToggle: HTMLButtonElement;

  user: any;
  public sidebarCollapsed: boolean = false;
  public notActedTotal: number = 0;
  public lstNotification: Notification[] = [];

  paginationConfig: PaginationInstance = {
    id: 'notification_list',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }

  constructor(
    @Inject(JQ_TOKEN) private $: any,
    private authService: AuthService,
    private modalService: ModalService,
    private toastrService: ToastrService,
    private router: Router,
    private notification: NotificationService

  ) {

  }

  ngOnInit() {
    this.user = this.authService.userObject;

    this.getNotification(1);
    setInterval(a => {
      this.getNotification(this.paginationConfig.currentPage);
    }, 30000);

    this.$('.notification-bottom').on('click', function (event) {
        setTimeout(() => {
          document.getElementById('notificationContainer').classList.add("show")
          document.getElementById('notificationBody').classList.add("show")
          document.getElementById('notificationContainer').setAttribute("aria-expanded", "true");
        });
    });

  }

  logout() {
    this.authService.logout();
  }

  togleSidebar() {
    let sidebar = document.getElementById('sidebar');
    let content = document.getElementById('content');
    if (!this.sidebarCollapsed) {
      sidebar.style.width = "0px";
      content.style.width = "100%";
    } else {
      sidebar.style.width = "260px";
      content.style.width = "calc(100% - 260px)";
    }
    this.sidebarCollapsed = !this.sidebarCollapsed;
  }

  profileInfoUpdate() {
    this.router.navigate(["/unicef/users/my-profile"]);
  }

  changePassword() {
    this.modalService.open<ChangePasswordComponent, IPasswordChangeModel>(ChangePasswordComponent, {})
      .then((model: IPasswordChangeModel) => {
        if (model) {
          this.authService.changePassword(model)
            .then(x => {
              this.toastrService.success(MessageConstant.SaveSuccessful);
            })
        }
      })
      .catch(x => x);
  }
  getPage(pageNo: number) {
    if (!pageNo)
      return;
    this.getNotification(pageNo);
  }
  getNotification(pageNo) {
    let baseQueryModel = {
      pageNo: pageNo,
      pageSize: this.paginationConfig.itemsPerPage,

    }

    this.notification.getAll(baseQueryModel).subscribe(a => {

      let getData: any = a;
      let data = JSON.parse(getData._body)

      this.notActedTotal = data.notActedTotal;
      this.lstNotification = data.data;

      this.paginationConfig.currentPage = pageNo;
      this.paginationConfig.totalItems = data.notActedTotal;
    });
  }

  readAndRedirectToUri(id, uri, data, notificationTypeId) {
    this.notification.readNotification(id).then(a => {
      if (uri == '') {
        return;
      }
      var url = "/" + uri;

      if (notificationTypeId == 1 || notificationTypeId == 2) {
        var jsonData = data;
        url = url + "/" + JSON.parse(jsonData).InstanceId;
      }
      this.getNotification(1);
      this.router.navigate([url]);
    })

  }
  clearAll() {
    this.notification.clearNotification().then(a => {
      this.getNotification(1);
    })
  }
}
