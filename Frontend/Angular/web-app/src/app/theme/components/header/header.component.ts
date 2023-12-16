import { AppConstants } from 'src/app/shared/models/app-constants';
import { LocalStorageService } from './../../../core/services/storages/local-storage.service';
import { RouteAction } from './../../../shared/models/app-constants';
import { AuthenticationService } from './../../../core/services/auth/authentication.service';
import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { faCoffee } from '@fortawesome/free-solid-svg-icons';
import { UserService } from 'src/app/services/user.service';
import { Option } from '../../models/table-model';
import { DialogService } from 'primeng/dynamicdialog';
import { ShopListComponent } from 'src/app/pages/views/shop-list/shop-list.component';
import { ShopService } from 'src/app/services/shop.service';
import { Shop } from 'src/app/services/models/shop.model';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {
  faCoffee = faCoffee;
  isEnLang: boolean = true;
  autoDisplay:boolean = false;
  baseZIndex:number = 10;
  items: MenuItem[] = [];
  languageMenu: MenuItem[] = [];
  isSuperAdmin:boolean = true;
  selectedShopId!:number;
  shopOptions!:Option[];
  shopName!:string;
  loggedInUser!:string;
  constructor(private authenticationService: AuthenticationService,
    private translateService: TranslateService,
    private localstoraceService:LocalStorageService,
    private userService:UserService,
    private dialogService: DialogService,
    private authenticationSrvice:AuthenticationService,
    private shopService:ShopService
  ) { }

  languageSubscription: any;
  initMenuSubsctiption: any;
  ngOnInit(): void {
    this.loggedInUser = this.authenticationService.getLoggedInUser();
    this.shopName = this.authenticationService.getUserShopName();
    this.userService.isSuperAdmin().subscribe((isSuperAdmin)=>{
      this.isSuperAdmin = isSuperAdmin;
      if(!isSuperAdmin){
        let shopId = this.authenticationSrvice.getUserSelectedShopId()
        if(shopId == null || +shopId == 0){
          // user has multiple shop, open shop list model. to select shop.
          this.showShopListDialog(false);
        }else{
          // user has only one shop so set the shop name, in header logo.
          this.shopService.getShop(shopId).subscribe((shop:Shop)=>{
            this.authenticationService.setSelectedShopName(shop.name);
            this.shopName = shop.name;
          });
        }
      }
      this.initMenuSubsctiption = this.translateService.get('title').subscribe((translated: string) => {
        this.languageMenu = this.buildLanguageMenu();
        this.items = this.buildNavigationMenu();
      });
      this.languageSubscription = this.translateService.onLangChange.subscribe(
        (event: LangChangeEvent) => {
          this.languageMenu = this.buildLanguageMenu();
          this.items = this.buildNavigationMenu();
        },
      );
    });
  }
  ngOnDestroy() {
    if (this.languageSubscription) {
      this.languageSubscription.unsubscribe();
    }
    if (this.initMenuSubsctiption) {
      this.initMenuSubsctiption.unsubscribe();
    }
  }
  onLogout() {
    this.authenticationService.logout();
  }
  onShopMenuClick(){
    this.showShopListDialog(true);
  }
  private showShopListDialog(isClosable:boolean) {
    const ref = this.dialogService.open(ShopListComponent, {
      header: 'Select Shop',
      width: '70%',
      closable:isClosable
    });
    ref.onClose.subscribe((shopName) => {
      this.shopName = shopName;
    });
  }
  onLanguageMenuItemClick(language: string) {
    this.translateService.use(language);
    this.localstoraceService.set(AppConstants.LANG_KEY,language);
  }
  buildLanguageMenu(): MenuItem[] {
    const retVal: MenuItem[] = [
      {
        label: this.translateService.instant("languageLabel"),
        items: [
          {
            label: this.translateService.instant("Menu.Lang.English"),
            icon: 'pi pi-fw pi-language',
            command: () => {
              this.onLanguageMenuItemClick('en-US');
            }
          },
          {
            label: this.translateService.instant("Menu.Lang.Hindi"),
            icon: 'pi pi-fw pi-language',
            command: () => {
              this.onLanguageMenuItemClick('hi-IN');
            }
          }
        ]
      }
    ];

    return retVal;
  }

  buildNavigationMenu(): MenuItem[] {
    let navMenu:MenuItem[] = [];
    if(this.isSuperAdmin){
      navMenu = this.buildAdminNavMenu();
    }else{
      navMenu = this.buildUserNavMenu();
    }
    return navMenu;
  }
  buildUserNavMenu():MenuItem[]{
    const retVal: MenuItem[] = [
      {
        id:'home',
        label: this.translateService.instant("Menu.Nav.Home"),
        icon: 'pi pi-fw pi-home',
        routerLink: 'views/home'
      },
      {
        label: this.translateService.instant("Menu.Nav.Orders"),
        icon: 'pi pi-fw pi-shopping-cart',
        items: [
          {
            label: this.translateService.instant("Menu.Nav.Order"),
            icon: 'pi pi-fw pi-cart-plus',
            routerLink: 'views/orders/order'
          },
          {
            label: this.translateService.instant("Menu.Nav.Orders"),
            icon: 'pi pi-fw pi-bars',
            routerLink: 'views/orders'
          }
        ]
      },
      {
        label: this.translateService.instant("Menu.Nav.Customers"),
        icon: 'pi pi-fw pi-user',
        items: [
          {
            label: this.translateService.instant("Menu.Nav.Customers"),
            icon: 'pi pi-fw pi-bars',
            routerLink: 'views/customers'
          },
          {
            label: this.translateService.instant("Menu.Nav.Add_Customer"),
            icon: 'pi pi-fw pi-plus',
            routerLink: 'views/customers/'+RouteAction.ADD
          },
          {
            label: this.translateService.instant("Menu.Nav.Invoices"),
            icon: 'pi pi-fw pi-trash',
            routerLink: 'views/invoices'
          },
          {
            separator: true
          },
          {
            label: 'Export',
            icon: 'pi pi-fw pi-external-link'
          }
        ]
      },
      {
        label: this.translateService.instant("Menu.Nav.Products"),
        icon: 'pi pi-fw pi-box',
        items: [
          {
            label: this.translateService.instant("Menu.Nav.Products"),
            icon: 'pi pi-fw pi-bars',
            routerLink: 'views/products'
          },
          {
            label: this.translateService.instant("Menu.Nav.Add_Product"),
            icon: 'pi pi-fw pi-plus',
            routerLink: 'views/products/'+RouteAction.ADD
          },
          {
            label: this.translateService.instant("Menu.Nav.Units"),
            icon: 'pi pi-fw pi-box',
            routerLink: 'views/units'
          },
          {
            label: this.translateService.instant("Menu.Nav.Add_Unit"),
            icon: 'pi pi-fw pi-box',
            routerLink: 'views/units/'+RouteAction.ADD
          }
        ]
      },
      {
        label: this.translateService.instant("Menu.Nav.Payments"),
        icon: 'pi pi-fw pi-dollar ',
        routerLink: 'views/payments'
      }
    ];
    return retVal;
  }
  buildAdminNavMenu():MenuItem[]{
    const retVal : MenuItem[] = [
      {
        id:'home',
        label: this.translateService.instant("Menu.Nav.Home"),
        icon: 'pi pi-fw pi-home',
        routerLink: 'views/home'
      },
      {
        label: this.translateService.instant("Menu.Nav.Shops"),
        icon: 'pi pi-fw pi-shopping-bag',
        items: [
          {
            label: this.translateService.instant("Menu.Nav.Shops"),
            icon: 'pi pi-fw pi-bars',
            routerLink: 'views/shops'
          },
          {
            label: this.translateService.instant("Menu.Nav.Add_Shop"),
            icon: 'pi pi-fw pi-plus',
            routerLink: 'views/shops/'+RouteAction.ADD
          }
        ]
      },
      {
        label: this.translateService.instant("Menu.Nav.Users"),
        icon: 'pi pi-fw pi-user',
        items: [
          {
            label: this.translateService.instant("Menu.Nav.Users"),
            icon: 'pi pi-fw pi-users',
            routerLink: 'views/users'
          },
          {
            label: this.translateService.instant("Menu.Nav.Add_User"),
            icon: 'pi pi-fw pi-user-plus',
            routerLink: 'views/users/'+RouteAction.ADD
          }
        ]
      },
      {
        label: this.translateService.instant("Menu.Nav.GeoRegion"),
        icon: 'pi pi-fw pi-map-marker',
        items: [
          {
            label: this.translateService.instant("Menu.Nav.Countries"),
            icon: 'pi pi-fw pi-bars',
            routerLink: 'views/countries'
          },
          {
            label: this.translateService.instant("Menu.Nav.Add_Country"),
            icon: 'pi pi-fw pi-plus',
            routerLink: 'views/countries/'+RouteAction.ADD
          },
          {
            label: this.translateService.instant("Menu.Nav.States"),
            icon: 'pi pi-fw pi-bars',
            routerLink: 'views/states'
          },
          {
            label: this.translateService.instant("Menu.Nav.Add_State"),
            icon: 'pi pi-fw pi-plus',
            routerLink: 'views/states/'+RouteAction.ADD
          },
          {
            label: this.translateService.instant("Menu.Nav.Cities"),
            icon: 'pi pi-fw pi-bars',
            routerLink: 'views/cities'
          },
          {
            label: this.translateService.instant("Menu.Nav.Add_City"),
            icon: 'pi pi-fw pi-plus',
            routerLink: 'views/cities/'+RouteAction.ADD
          }
        ]
      }
    ];
    return retVal;
  }
}
