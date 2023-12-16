import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Address, CreateAddress } from 'src/app/services/models/address.model';
import { ShopService } from 'src/app/services/shop.service';
import { CustomerService } from '../services/customer.service';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Option } from 'src/app/theme/models/table-model';
import { CreateCustomer, CreateGovDocument, Customer, GovDocument } from '../models/customer.model';
import { ActivatedRoute, Router } from '@angular/router';
import { Action, RouteAction } from 'src/app/shared/models/app-constants';
import { Location } from '@angular/common';
import { Barcode, BarcodeCreate } from 'src/app/core/model/barcode-model';
import { BarcodeService } from 'src/app/services/barcode.service';
import { DocumentService } from 'src/app/services/document.service';
@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer-detail.component.html',
  styleUrls: ['./customer-detail.component.css']
})
export class CustomerDetailComponent implements OnInit {

  currentAddress?:Address;
  permanentAddress?:Address;
  customerId!:string;
  customerOldValue!:Customer;
  documentId!:number;
  barcodeImage!: string;
  customerForm: FormGroup = this.formBuilder.group({
    customerName: [null],
    phoneNumber: [null],
    shopId: [{ value: null, disabled: true }],
    isActive: [true],
    documents: this.documentForm()
  });
  shopOptions: Option[] = [];
  showUpdateBtn: boolean = false;
  showDeleteBtn: boolean = false;
  constructor(private formBuilder: FormBuilder,
    private shopService: ShopService,
    private customerService: CustomerService,
    private messageService: MessageService,
    private route: ActivatedRoute,
    private confirmationService: ConfirmationService,
    private barcodeService: BarcodeService,
    private location: Location,
    private router:Router,
    private documentService: DocumentService
  ) { }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.customerId = params['customerId'];
      // this.documentService.getDocumentTypes().subscribe((documentTypes)=>{
      //   console.log(documentTypes);
      // })
      if (this.customerId != RouteAction.ADD) {
        this.showUpdateBtn = true;
        this.showDeleteBtn = true;
        this.shopService.getShopOptions().subscribe((options) => {
          this.shopOptions = options;
        });
        this.getCustomerDetails(this.customerId);
      } else {
        // add case
        this.customerForm.reset();
        this.customerForm.get('isActive')?.setValue(true);
        this.shopOptions = [] as Option[];
        this.customerForm.controls['shopId'].disable();
        this.currentAddress = undefined;
        this.permanentAddress = undefined;
        this.showUpdateBtn = false;
        this.showDeleteBtn = false;
        this.barcodeImage = '';
      }
    });
  }
  private getCustomerDetails(id: string) {
    this.customerService.getCustomerById(id).subscribe((customer: Customer) => {
      this.customerOldValue = customer;
      this.setFormOnGetCustomer(customer);
    });
  }
  private setFormOnGetCustomer(customer: Customer) {
    this.setBarcodeImage(customer.barcode);
    this.customerForm.controls['customerName'].setValue(customer.name);
    this.customerForm.controls['phoneNumber'].setValue(customer.phone);
    this.customerForm.controls['shopId'].setValue(customer.shopId);
    this.customerForm.controls['isActive'].setValue(customer.isActive);
    this.setAddressForm(customer.addresses);
    this.setDocumentForm(customer.documents);
  }
  private setBarcodeImage(barcode:string){
    let barcodeReq = {
      code: barcode,
      width: 220,
      height: 50,
      includeLabel: false
    } as BarcodeCreate;
    this.barcodeService.getBarcode(barcodeReq).subscribe((barcode: Barcode) => {
      this.barcodeImage = "data:image/png;base64," + barcode.image;
    });
  }
  private setAddressForm(addresses:Address[]){
    addresses.forEach((address) => {
      if (address.isPermanent) {
        this.permanentAddress = address;
      } else {
        this.currentAddress = address;
      }
    });
  }
  private setDocumentForm(documents:GovDocument[]){
    documents.forEach((item)=>{
      this.documentId = item.id;
      this.customerForm.patchValue({
        documents : item
      });
    })

  }
  private documentForm(): FormGroup {
    return this.formBuilder.group({
      type: [null],
      docId: [null],
      isVerified: [false]
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
  private formData(action?:Action):Customer{
    let customer = {} as Customer;
    let currentAddressId!:number;
    let permanentAddressId!:number;
    if(action==Action.UPDATE){
      customer.id =  +this.customerId;
      this.customerOldValue.addresses.forEach(item =>{
        if(item.isPermanent){
          permanentAddressId = item.id;
        }else{
          currentAddressId = item.id;
        }
      });
    }
    customer.name = this.customerForm.controls['customerName'].value;
    customer.shopId = this.customerForm.controls['shopId'].value;
    customer.phone = this.customerForm.controls['phoneNumber'].value;
    customer.isActive = this.customerForm.controls['isActive'].value;
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
    customer.addresses = addresses;
    let documents = [] as GovDocument[];
    let document = {
      id:this.documentId,
      docId: this.customerForm.controls['documents'].value.docId,
      type: this.customerForm.controls['documents'].value.type,
      isVerified: this.customerForm.controls['documents'].value.isVerified,
    } as GovDocument;
    documents.push(document);
    customer.documents = documents;
    return customer;
  }
  onSave() {
    let customer = this.formData();
    this.customerService.createCustomer(customer).subscribe((customer) => {
      this.messageService.add({ severity: 'success', summary: 'Customer Created Successfully', detail: customer.name, life: 3000 });
      this.router.navigate(['/pages/views/customers']);
    });
  }
  onUpdate(){
    let customerToUpdate = this.formData(Action.UPDATE);
    this.customerService.updateCustomer(customerToUpdate).subscribe((customer)=>{
      this.messageService.add({ severity: 'success', summary: 'Customer Updated Successfully', detail: customer.name, life: 3000 });
    });
  }
  onDelete(){
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the customer?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.customerService.deleteCustomer(this.customerId).subscribe((customer) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: `${customer.name} Deleted`, life: 3000 });
          this.location.back();
        });
      }
    });
  }
}
