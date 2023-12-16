export class AppConstants {
    static readonly AUTH_USER_KEY = 'AUTH';
    static readonly LOGIN_URL = '/auth/login';
    static readonly LANG_KEY = 'lang';
    static readonly UserState = 'userState';
    static readonly SHOP_NAME = 'shop_name';
    static readonly HOME_URL = '/';
}
export class CustomHeader {
    static readonly SHOP_ID = 'shop_id';
}
export class RouteAction{
    static readonly ADD = "add";
    static readonly ALL = "All";
}
export class Role{
    static readonly SUPER_ADMIN:string = 'SuperAdmin';
    static readonly ADMIN = 'Admin';
}
export class Action{
    static readonly ADD = "ADD";
    static readonly UPDATE = "UPDATE";
    static readonly DELETE = "DELETE";
}

export class Format{
    static readonly DATE = 'dd-MM-yyyy';
    static readonly DATETIME = 'dd-MM-yy hh:mm:ss';
    static readonly PRIMENG_DATE = 'dd-mm-yy';
}

export class TableProperty {
    static readonly SORT_ASC:number = 1;
    static readonly SORT_DESC:number = -1;
}
