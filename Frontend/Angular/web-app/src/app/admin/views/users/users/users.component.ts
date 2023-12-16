import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/services/models/user.model';
import { UserRoleService } from 'src/app/services/user-role.service';
import { UserService } from 'src/app/services/user.service';
import { ColumnType, Option, TableColumn } from 'src/app/theme/models/table-model';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {
  users!: User[];
  cols!: TableColumn[];
  isEditable: boolean = false;
  userRoleOptions!: Option[];
  constructor(private userService: UserService,
    private userRoleService: UserRoleService
  ) { }

  ngOnInit(): void {
    this.userRoleService.getUserRolesOptions().subscribe((options) => {
      this.userRoleOptions = options;
      this.cols.push({
        field: 'role', header: 'Role', sortable: false, filtrable: false, type: ColumnType.DropDown,
        options: this.userRoleOptions
      });
    });
    this.cols = [
      { field: 'id', header: 'User Id', sortable: true, filtrable: true, type: ColumnType.Link },
      { field: 'name', header: 'User Full Name', sortable: true, filtrable: true },
      { field: 'phone', header: 'Phone No.', sortable: true, filtrable: true },
      { field: 'username', header: 'Username', sortable: true, filtrable: true },
      { field: 'isActive', header: 'isActive', sortable: false, filtrable: false }
    ] as TableColumn[];
    this.userService.getUsers().subscribe((users) => {
      this.users = users;
    });
  }

}
