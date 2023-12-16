import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { GeoRegionService } from 'src/app/services/geo-region.service';
import { Address, CreateAddress, GeoRegionOptions } from 'src/app/services/models/address.model';
import { Shop } from 'src/app/services/models/shop.model';
import { ShopService } from 'src/app/services/shop.service';
import { UserService } from 'src/app/services/user.service';
import { Action, RouteAction } from 'src/app/shared/models/app-constants';
import { Option } from 'src/app/theme/models/table-model';

@Component({
  selector: 'app-shop-detail',
  templateUrl: './shop-detail.component.html',
  styleUrls: ['./shop-detail.component.css']
})
export class ShopDetailComponent implements OnInit {
  currentAddress?:Address;
  permanentAddress?:Address;
  shopFormGroup: FormGroup = this.formBuilder.group({
    shopName: [null],
    phone: [null],
    userId: [null],
    isActive: [true]
  });

  usersOptions: Option[] = [];
  showUpdateBtn: boolean = false;
  showDeleteBtn: boolean = false;
  shopOldValue!:Shop;
  shopId!: string;

  constructor(private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private shopService:ShopService,
    private userService:UserService,
    private messageService:MessageService,
    private confirmationService: ConfirmationService,
    private location: Location,
    private router:Router
    ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.shopId = params['shopId'];
      this.userService.getUsersOptions().subscribe((options)=>{
        this.usersOptions = options;
      });
      if (this.shopId != RouteAction.ADD) {
        this.showUpdateBtn = true;
        this.showDeleteBtn = true;
        this.getShopDetail(this.shopId);
      } else {
        // add case
        this.shopFormGroup.reset();
        this.shopFormGroup.get('isActive')?.setValue(true);
        this.currentAddress = undefined;
        this.permanentAddress = undefined;
        this.showUpdateBtn = false;
        this.showDeleteBtn = false;
      }
    });

  }
  private getShopDetail(id: string) {
    this.shopService.getShop(id).subscribe((shop: Shop) => {
      this.shopOldValue = shop;
      this.setFormOnGetShop(shop);
    });
  }
  private setFormOnGetShop(shop: Shop) {
    this.shopFormGroup.controls['shopName'].setValue(shop.name);
    this.shopFormGroup.controls['phone'].setValue(shop.phone);
    this.shopFormGroup.controls['userId'].setValue(shop.userId);
    this.shopFormGroup.controls['isActive'].setValue(shop.isActive);
    this.setAddressForm(shop.addresses);
  }
  setAddressForm(addresses:Address[]){
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
  onSave(){
    let shop = this.formData();
    this.shopService.createShop(shop).subscribe((shop)=>{
      this.messageService.add({ severity: 'success', summary: 'Shop Created Successfully', detail: shop.name, life: 3000 });
      this.router.navigate(['/admin/views/shops']);
    });

  }
  onUpdate(){
    let shopToUpdate = this.formData(Action.UPDATE);
    this.shopService.updateShop(shopToUpdate).subscribe((shop)=>{
      this.messageService.add({ severity: 'success', summary: 'Shop Updated Successfully', detail: shop.name, life: 3000 });
    });
  }
  onDelete(){
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the shop?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.shopService.deleteShop(this.shopId).subscribe((shop) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: `${shop.name} Deleted`, life: 3000 });
          this.location.back();
        });
      }
    });
  }
  formData(action?:Action):Shop{
    let shop = {} as Shop;
    let currentAddressId!:number;
    let permanentAddressId!:number;
    if(action==Action.UPDATE){
      shop.id =  +this.shopId;
      this.shopOldValue.addresses.forEach(item =>{
        if(item.isPermanent){
          permanentAddressId = item.id;
        }else{
          currentAddressId = item.id;
        }
      });
    }
    shop.name = this.shopFormGroup.controls['shopName'].value;
    shop.phone = this.shopFormGroup.controls['phone'].value;
    shop.userId = this.shopFormGroup.controls['userId'].value;
    shop.isActive = this.shopFormGroup.controls['isActive'].value;

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
    shop.addresses = addresses;
    return shop;
  }
}

