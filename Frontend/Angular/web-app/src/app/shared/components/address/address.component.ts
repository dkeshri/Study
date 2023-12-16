import { Component, EventEmitter, Input, OnInit, Output, SimpleChange, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { forkJoin } from 'rxjs';
import { GeoRegionService } from 'src/app/services/geo-region.service';
import { Address, GeoRegionOptions } from 'src/app/services/models/address.model';
import { Option } from "src/app/theme/models/table-model";
@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.css']
})
export class AddressComponent implements OnInit {

  @Input()
  address?: Address;
  @Input()
  isPermanent : boolean = false;
  @Output()
  addressChange: EventEmitter<Address> = new EventEmitter<Address>();
  addressFormGroup: FormGroup =  this.formBuilder.group({
    address1: [null],
    address2: [null],
    landmark: [null],
    city: [null],
    pincode: [null],
    state: [null],
    country: [null]
  });
  addressGeoRegoinOption = {
    city : [] as Option[],
    state : [] as Option[],
    country: [] as Option[]
  } as GeoRegionOptions;
  constructor(private formBuilder: FormBuilder,
    private geoRegionService:GeoRegionService
    ) { }


  ngOnChanges(changes: SimpleChanges):void{
    //console.log(changes['address'].currentValue);
    if(this.address){
      forkJoin(
        [
        this.geoRegionService.getCountriesOptions(),
        this.geoRegionService.getStatesOptionsByCountryCode(this.address.country),
        this.geoRegionService.getCitiesOptionsByCountryAndStateCode(this.address.country,this.address.state)
        ]
      ).subscribe(([countries,states,cities])=>{
        this.addressGeoRegoinOption.country = countries;
        this.addressGeoRegoinOption.state = states;
        this.addressGeoRegoinOption.city = cities;
        if(this.address){
          this.addressFormGroup.patchValue(this.address);
        }
      });
    }else{
      // add case
      this.addressFormGroup.reset();
      this.geoRegionService.getCountriesOptions().subscribe((options)=>{
        this.addressGeoRegoinOption.country = options;
        this.addressGeoRegoinOption.state = [] as Option[];
        this.addressGeoRegoinOption.city = [] as Option[];
      });
    }
  }
  ngOnInit(): void {

  }
  getCurrentAddStates(){
    let countryCode = this.addressFormGroup.controls['country'].value;
    this.geoRegionService.getStatesOptionsByCountryCode(countryCode).subscribe((options)=>{
      this.addressGeoRegoinOption.state = options;
      this.addressFormGroup.controls['state'].reset();
      this.addressFormGroup.controls['city'].reset();
      this.onAddressChange();
    });

  }
  getCurrentAddCities(){
    let countryCode = this.addressFormGroup.controls['country'].value;
    let stateCode = this.addressFormGroup.controls['state'].value;
    this.geoRegionService.getCitiesOptionsByCountryAndStateCode(countryCode,stateCode).subscribe((options)=>{
      this.addressGeoRegoinOption.city = options;
      this.addressFormGroup.controls['city'].reset();
      this.onAddressChange();
    });
  }
  private getAddress():Address|undefined{
    if(this.address){
      this.setAddress(this.address);
    }else{
      this.address = {} as Address;
      this.setAddress(this.address);
    }
    return this.address;
  }
  onAddressChange(){
    this.addressChange.emit(this.getAddress());
  }
  setAddress(address:Address){
      address.isPermanent = this.isPermanent;
      address.address1 = this.addressFormGroup.controls['address1'].value;
      address.address2 = this.addressFormGroup.controls['address2'].value;
      address.landmark = this.addressFormGroup.controls['landmark'].value;
      address.city= this.addressFormGroup.controls['city'].value;
      address.pincode = this.addressFormGroup.controls["pincode"].value;
      address.state = this.addressFormGroup.controls['state'].value;
      address.country= this.addressFormGroup.controls['country'].value;
  }
}
