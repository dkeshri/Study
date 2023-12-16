import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Address } from 'src/app/services/models/address.model';
import { User } from 'src/app/services/models/user.model';
import { UserRoleService } from 'src/app/services/user-role.service';
import { UserService } from 'src/app/services/user.service';
import { Action, RouteAction } from 'src/app/shared/models/app-constants';
import { Option } from 'src/app/theme/models/table-model';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {
  currentAddress?:Address;
  permanentAddress?:Address;
  resetPermanentAddressForm:boolean = false;
  userFormGroup: FormGroup =  this.formBuilder.group({
    name: [null],
    phone: [null],
    userName: [null],
    isActive: [true],
    role: [null],
    userRoleId:[null],
    password: [null]
  });
  userRolesOptions: Option[] = [];
  showUpdateBtn: boolean = false;
  showDeleteBtn: boolean = false;
  userId!: string;
  userOldValue!:User;

  constructor(private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private userService: UserService,
    private messageService: MessageService,
    private confirmationService: ConfirmationService,
    private location: Location,
    private userRoleService:UserRoleService,
    private router:Router
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.userId = params['userId'];
      this.userRoleService.getUserRolesOptions().subscribe((options) => {
        this.userRolesOptions = options;
      });
      if (this.userId != RouteAction.ADD) {
        this.showUpdateBtn = true;
        this.showDeleteBtn = true;
        this.getUserDetail(this.userId);
      } else {
        // add case
        this.userFormGroup.reset();
        this.userFormGroup.get('isActive')?.setValue(true);
        this.currentAddress = undefined;
        this.permanentAddress = undefined;
        this.showUpdateBtn = false;
        this.showDeleteBtn = false;
      }
    })
  }
  private getUserDetail(id: string) {
    this.userService.getUser(id).subscribe((user: User) => {
      this.userOldValue = user;
      this.setFormOnGetUser(user);
    });
  }
  private setFormOnGetUser(user: User) {
    this.userFormGroup.controls['name'].setValue(user.name);
    this.userFormGroup.controls['phone'].setValue(user.phone);
    this.userFormGroup.controls['userName'].setValue(user.username);
    this.userFormGroup.controls['userRoleId'].setValue(user.role);
    this.userFormGroup.controls['isActive'].setValue(user.isActive);
    this.userFormGroup.controls['password'].setValue(user.password);
    this.setAddressForm(user.addresses);
  }
  setAddressForm(addresses: Address[]) {
    addresses.forEach((address) => {
      if (address.isPermanent) {
        this.permanentAddress = address;
      } else {
        this.currentAddress = address;
      }
    });
  }
  onPermanentAddChkChange(event: any) {
    if (event.checked) {
      // fill the permanent address form same as current
      if(this.permanentAddress && this.currentAddress){
        let permanentAddressId = this.permanentAddress.id;
        this.permanentAddress = {
          ...this.currentAddress,
          id:permanentAddressId,
          isPermanent:true
        }
      }else if(this.currentAddress){
        this.permanentAddress = {
          ...this.currentAddress,
          isPermanent:true
        }
      }
    } else {
      this.permanentAddress = undefined;
    }
  }

  onSave() {
    let user = this.formData();
    this.userService.createUser(user).subscribe((user) => {
      this.messageService.add({ severity: 'success', summary: 'User Created Successfully', detail: user.username, life: 3000 });
      this.router.navigate(['/admin/views/users']);
    });
  }
  onUpdate() {
    let userUpdateData = this.formData(Action.UPDATE);
    this.userService.updateUser(userUpdateData).subscribe((user) => {
      this.messageService.add({ severity: 'success', summary: 'User Update Successfully', detail: user.username, life: 3000 });
    });
  }
  onDelete() {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the user?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.userService.deleteUser(this.userId).subscribe((user) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: `${user.username} Deleted`, life: 3000 });
          this.location.back();
        });
      }
    });
  }

  formData(action?:Action):User{
    let user = {} as User;
    let currentAddressId!:number;
    let permanentAddressId!:number;
    if(action==Action.UPDATE) {
      user.id = this.userOldValue.id;
      this.userOldValue.addresses.forEach(item =>{
        if(item.isPermanent){
          permanentAddressId = item.id;
        }else{
          currentAddressId = item.id;
        }
      });
    }
    user.name = this.userFormGroup.controls['name'].value;
    user.phone = this.userFormGroup.controls['phone'].value;
    user.username = this.userFormGroup.controls['userName'].value;
    user.isActive = this.userFormGroup.controls['isActive'].value;
    user.role = this.userFormGroup.controls['userRoleId'].value;
    user.password = this.userFormGroup.controls['password'].value;
    let addresses = [] as Address[];
    if(this.currentAddress){
      this.currentAddress.id = currentAddressId;
      addresses.push(this.currentAddress);
    }
    if(this.permanentAddress){
      this.permanentAddress.id = permanentAddressId;
      this.permanentAddress.isPermanent = true;
      addresses.push(this.permanentAddress);
    }

    user.addresses = addresses;
    return user;
  }
}
