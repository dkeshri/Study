import { CreateUnitConversionComponent } from './../../units/create-unit-conversion/create-unit-conversion.component';
import { CreateUnitComponent } from './../../units/create-unit/create-unit.component';
import { DialogService } from 'primeng/dynamicdialog';
import { UnitService } from './../../../../services/unit.service';
import { Product } from './../Models/product.model';
import { ProductService } from './../service/product.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Option } from 'src/app/theme/models/table-model';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Action, Format, RouteAction } from 'src/app/shared/models/app-constants';
import { faPlusCircle } from '@fortawesome/free-solid-svg-icons';
import { UnitConversionParam } from '../../units/Models/unit-conversion.model';
import { UnitConversionService } from '../../units/services/unit-conversion.service';
import { Location } from '@angular/common';
import { forkJoin } from 'rxjs';
import { ValidationError } from 'src/app/core/model/validation-error.model';
@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.css']
})
export class ProductDetailsComponent implements OnInit {
  productFormGroup: FormGroup = this.formBuilder.group({
    barcode: ['', [Validators.required]],
    name: ['', [Validators.required]],
    costPrice: ['', [Validators.required]],
    price: ['', [Validators.required]],
    expireOn: [new Date()],
    unit: ['', [Validators.required]],
    secUnit: [{ value: '', disabled: true }],
    secUnitChk: [false],
    isActive: [true],
    unitConversion: [{ value: '', disabled: true }]
  });
  dateValue!: Date;
  unitOptions: Option[] = [];
  secUnitOptions: Option[] = [];
  unitConversionOptions: Option[] = [];
  showUpdateBtn: boolean = false;
  showDeleteBtn: boolean = false;
  faPlusCircle = faPlusCircle;
  isUnitConversionBtnDisabled = true;
  faPlusCircleStyle = {
    color: 'rgb(63 81 181)'
  };
  productOldValue!: Product;
  dateformat = Format.PRIMENG_DATE;
  get barcodeControl() {
    return this.productFormGroup.get('barcode') as FormControl;
  }
  get nameControl() {
    return this.productFormGroup.get('name') as FormControl;
  }
  get costPriceControl() {
    return this.productFormGroup.get('costPrice') as FormControl;
  }
  get priceControl() {
    return this.productFormGroup.get('price') as FormControl;
  }
  constructor(private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private productService: ProductService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private unitService: UnitService,
    private dialogService: DialogService,
    private unitConversionService: UnitConversionService,
    private location: Location,
    private router: Router
  ) {
  }

  productId: any;
  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.productId = params['productId'];
      if (this.productId != RouteAction.ADD) {
        this.showUpdateBtn = true;
        this.showDeleteBtn = true;
        this.getProductDetail(this.productId);
      } else {
        // add case
        this.productFormGroup.reset();
        this.productFormGroup.controls['expireOn'].setValue(new Date());
        this.productFormGroup.controls['isActive'].setValue(true);
        this.initPriSecUnits();
        this.initUnitConversionOptions();
        this.showUpdateBtn = false;
        this.showDeleteBtn = false;
        this.onSecUnitChkChange();
      }
    });
  }

  private getProductDetail(productId: string) {
    forkJoin(
      [
        this.productService.getProduct(productId),
        this.unitService.getUnitOptions(),
        this.unitConversionService.getUnitConversionOptions()
      ]
    ).subscribe(([product, unitOptions, unitConversionOptions]) => {
      this.setPriSecUnits(unitOptions);
      this.setUnitConversionOptions(unitConversionOptions);
      this.productOldValue = product;
      this.setFormOnGetProduct(product);
    });
  }
  private initPriSecUnits() {
    this.unitService.getUnitOptions().subscribe((options) => {
      this.setPriSecUnits(options);
    });
  }

  private setPriSecUnits(options: Option[]) {
    this.unitOptions = options;
    this.secUnitOptions = options;
  }

  private initUnitConversionOptions() {
    this.unitConversionService.getUnitConversionOptions().subscribe((options) => {
      this.setUnitConversionOptions(options);
    });
  }

  private setUnitConversionOptions(options: Option[]) {
    this.unitConversionOptions = options;
  }

  private setFormOnGetProduct(product: Product) {
    let date = new Date(product.expireOn);
    this.productFormGroup.controls['barcode'].setValue(product.barcode);
    this.productFormGroup.controls['name'].setValue(product.name);
    this.productFormGroup.controls['costPrice'].setValue(product.costPrice);
    this.productFormGroup.controls['price'].setValue(product.price);
    this.productFormGroup.controls['expireOn'].setValue(date);
    this.productFormGroup.controls['unit'].setValue(product.unitId);
    this.productFormGroup.controls['secUnit'].setValue(product.secUnitId);
    this.productFormGroup.controls['unitConversion'].setValue(product.unitConversionId);
    this.productFormGroup.controls['isActive'].setValue(product.isActive);
    if (product.unitConversionId) {
      this.productFormGroup.controls['secUnitChk'].setValue(true);
      this.onSecUnitChkChange();
    }
  }
  formData(action?: Action): Product {
    let product = {} as Product;
    if (action == Action.UPDATE) {
      product.id = this.productOldValue.id;
    }
    product.barcode = this.productFormGroup.controls['barcode'].value;
    product.name = this.productFormGroup.controls['name'].value;
    product.costPrice = this.productFormGroup.controls['costPrice'].value;
    product.price = this.productFormGroup.controls['price'].value;
    product.expireOn = this.productFormGroup.controls['expireOn'].value;
    product.unitId = this.productFormGroup.controls['unit'].value;
    product.secUnitId = this.productFormGroup.controls['secUnit'].value;
    product.isActive = this.productFormGroup.controls['isActive'].value;
    product.unitConversionId = this.productFormGroup.controls['unitConversion'].value;
    return product;
  }
  onSave() {
    let product = this.formData();
    let validationError = this.validate(product);
    if (validationError.isValidationError) {
      this.displayErrorMessage(validationError);
    } else {
      this.productService.createProduct(product).subscribe((data) => {
        this.messageService.add({ severity: 'success', summary: 'Product Created Successfully', detail: data.name, life: 3000 });
        this.router.navigate(['/pages/views/products']);
      });
    }

  }
  onUpdate() {
    let updatedProductData = this.formData(Action.UPDATE);
    let validationError = this.validate(updatedProductData);
    if (validationError.isValidationError) {
      this.displayErrorMessage(validationError);
    } else {
      this.productService.updateProduct(updatedProductData).subscribe((user) => {
        this.messageService.add({ severity: 'success', summary: 'Product Update Successfully', detail: updatedProductData.name, life: 3000 });
      });
    }
  }
  onDelete() {
    this.confirmationService.confirm({
      message: 'Are you sure you want to delete the Product?',
      header: 'Confirm',
      icon: 'pi pi-exclamation-triangle',
      accept: () => {
        this.productService.deleteProduct(this.productId).subscribe((product) => {
          this.messageService.add({ severity: 'success', summary: 'Successful', detail: `${product.name} Deleted`, life: 3000 });
          this.location.back();
        });
      }
    });
  }

  onShowUnitDialog() {
    const ref = this.dialogService.open(CreateUnitComponent, {
      header: 'Add Unit',
      width: '70%'
    });
    ref.onClose.subscribe((unitId: number) => {
      this.initPriSecUnits();
    });
  }
  onConversionRateAdd() {

    let primaryId = this.productFormGroup.controls['unit'].value;
    let secId = this.productFormGroup.controls['secUnit'].value;
    let primaryUnitText = ''
    let secUnitText = '';
    this.unitOptions.forEach(item => {
      if (item.value == primaryId) {
        primaryUnitText = item.label;
      }
    });
    this.secUnitOptions.forEach(item => {
      if (item.value == secId) {
        secUnitText = item.label;
      }
    });

    let data = {
      primaryId: primaryId,
      primaryUnitText: primaryUnitText,
      secId: secId,
      secUnitText: secUnitText
    } as UnitConversionParam;
    const ref = this.dialogService.open(CreateUnitConversionComponent, {
      header: 'Add Unit Conversion',
      width: '65%',
      data
    });
    ref.onClose.subscribe((data) => {
      this.initUnitConversionOptions();
    });
  }
  onPrimaryUnitChange() {
    if (this.productFormGroup.controls['secUnit'].disabled && this.productFormGroup.controls['secUnitChk'].value) {
      this.productFormGroup.controls['secUnit'].enable();
    }
    if (this.productFormGroup.controls['secUnit'].enabled) {
      this.disableSecUnitOption(this.productFormGroup.controls['unit'].value);
    }

  }
  private disableSecUnitOption(value:string|number){
    this.secUnitOptions.forEach(option => {
      if(option.disabled===true){
        option.disabled = false;
      }
      if (value == option.value) {
        option.disabled = true;
      }
    });
  }
  onSecUnitChange() {
    if (this.productFormGroup.controls['unitConversion'].disabled) {
      this.productFormGroup.controls['unitConversion'].enable();
      this.isUnitConversionBtnDisabled = false;
    }
  }
  onSecUnitChkChange() {
    if (this.productFormGroup.controls['secUnitChk'].value) {
      if (this.productFormGroup.controls['secUnit'].disabled && this.productFormGroup.controls['unit'].value) {
        this.productFormGroup.controls['secUnit'].enable();
        this.disableSecUnitOption(this.productFormGroup.controls['unit'].value);
        if (this.productOldValue?.secUnitId) {
          this.productFormGroup.controls['secUnit'].setValue(this.productOldValue.secUnitId);
        }
      }
      if (this.productFormGroup.controls['unitConversion'].disabled && this.productFormGroup.controls['secUnit'].value) {
        this.productFormGroup.controls['unitConversion'].enable();
        this.isUnitConversionBtnDisabled = false;
        if (this.productOldValue?.unitConversionId) {
          this.productFormGroup.controls['unitConversion'].setValue(this.productOldValue.unitConversionId);
        }
      }
    } else {
      this.productFormGroup.controls['secUnit'].reset();
      this.productFormGroup.controls['unitConversion'].reset();
      this.productFormGroup.controls['secUnit'].disable();
      this.productFormGroup.controls['unitConversion'].disable();
      this.isUnitConversionBtnDisabled = true;
    }
  }
  private displayErrorMessage(error: ValidationError) {
    this.messageService.add({ severity: 'error', summary: error.field, detail: `${error.errorMessage}`, life: 3000 });
  }
  private validate(product: Product): ValidationError {
    let error = { isValidationError: false } as ValidationError;
    if (this.productFormGroup.controls['secUnitChk'].value) {
      if (!product.secUnitId) {
        return this.setValidationError('Secondary Unit', 'Please Select Secondary unit.');
      }
      if (product.secUnitId) {
        if (!product.unitConversionId) {
          return this.setValidationError('Unit Conversion', 'Please Select unit conversion.');
        }
      }
    }
    return error;
  }
  private setValidationError(field: string, errorMessage: string): ValidationError {
    let error = {
      isValidationError: true,
      field: field,
      errorMessage: errorMessage
    } as ValidationError;
    return error;
  }
}
