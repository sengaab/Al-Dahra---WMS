/* =========================================================
   COMMON
========================================================= */

export interface ApiResponse<T> {
    success?: boolean;
    message?: string;
    data?: T;
}


/* =========================================================
   AUTH
========================================================= */

export interface AuthUser {
    user_Id: string;
    user_Name: string;
    user_Email: string;
    role_Id: number;
    createAt: string;
    updateAt: string;
    loginAt?: string | null;
    role?: Role;
}


/* =========================================================
   DEPARTMENT
========================================================= */

export interface Department {
    department_Id: number;
    department_Name: string;
    description?: string | null;
    isActive: boolean;
    createAt: string;
    updateAt?: string | null;
}

export interface CreateDepartmentRequest {
    department_Name: string;
    description?: string;
    isActive: boolean;
}

export interface UpdateDepartmentRequest {
    department_Name: string;
    description?: string;
    isActive: boolean;
}


/* =========================================================
   CATEGORY
========================================================= */

export interface Category {
    category_Id: number;
    category_Name: string;
    description?: string | null;
    isActive: boolean;
    createdAt: string;
    updatedAt?: string | null;
    department_Id: number;
    department?: Department;
}

export interface CreateCategoryRequest {
    category_Name: string;
    description?: string;
    isActive: boolean;
    department_Id: number;
}

export interface UpdateCategoryRequest {
    category_Name: string;
    description?: string;
    isActive: boolean;
    department_Id: number;
}


/* =========================================================
   SUB CATEGORY
========================================================= */

export interface SubCategory {
    subCategoryId: number;
    subCategory_Name: string;
    subCategory_Description?: string | null;
    categoryId: number;
    isActive: boolean;
    createdAt: string;
    updatedAt?: string | null;
}


/* =========================================================
   UNIT
========================================================= */

export interface Unit {
    unit_Id: number;
    unit_Name: string;
    unit_Symbol: string;
    isActive: boolean;
}

export interface CreateUnitRequest {
    unit_Name: string;
    unit_Symbol: string;
    isActive: boolean;
}

export interface UpdateUnitRequest {
    unit_Name: string;
    unit_Symbol: string;
    isActive: boolean;
}


/* =========================================================
   ROLE
========================================================= */

export interface Role {
    role_Id: number;
    role_Name: string;
    role_Description: string;
    isActive: boolean;
    createAt: string;
    updateAt: string;
}

export interface CreateRoleRequest {
    role_Name: string;
    role_Description: string;
    isActive: boolean;
}

export interface UpdateRoleRequest {
    role_Name: string;
    role_Description: string;
    isActive: boolean;
}


/* =========================================================
   SITE
========================================================= */

export interface Site {
    site_Id: number;
    site_Name: string;
    site_Code?: string | null;
    site_Description?: string | null;
    isActive: boolean;
}

export interface CreateSiteRequest {
    site_Name: string;
    site_Code?: string;
    site_Description?: string;
    isActive: boolean;
}

export interface UpdateSiteRequest {
    site_Name: string;
    site_Code?: string;
    site_Description?: string;
    isActive: boolean;
}


/* =========================================================
   WAREHOUSE
========================================================= */

export interface Warehouse {
    warehouse_Id: number;
    warehouse_Name: string;
    warehouse_Code?: string | null;
    warehouse_Description?: string | null;
    isActive: boolean;
    site_Id: number;
    site?: Site;
}

export interface CreateWarehouseRequest {
    warehouse_Name: string;
    warehouse_Code?: string;
    warehouse_Description?: string;
    isActive?: boolean;
    site_Id: number;
}

export interface UpdateWarehouseRequest {
    warehouse_Name: string;
    warehouse_Code?: string;
    warehouse_Description?: string;
    isActive: boolean;
    site_Id: number;
}


/* =========================================================
   ROOM
========================================================= */

export interface Room {
    room_Id: number;
    room_Name: string;
    room_Code?: string | null;
    room_Description?: string | null;
    isActive: boolean;
    warehouse_Id: number;
    warehouse?: Warehouse;
}

export interface CreateRoomRequest {
    room_Name: string;
    room_Code?: string;
    room_Description?: string;
    isActive: boolean;
    warehouse_Id: number;
}

export interface UpdateRoomRequest {
    room_Name: string;
    room_Code?: string;
    room_Description?: string;
    isActive: boolean;
    warehouse_Id: number;
}


/* =========================================================
   ROW
========================================================= */

export interface Row {
    row_Id: number;
    row_Name: string;
    row_Code?: string | null;
    row_Description?: string | null;
    isActive: boolean;
    room_Id: number;
    room?: Room;
}

export interface CreateRowRequest {
    row_Name: string;
    row_Code?: string;
    row_Description?: string;
    isActive: boolean;
    room_Id: number;
}

export interface UpdateRowRequest {
    row_Name: string;
    row_Code?: string;
    row_Description?: string;
    isActive: boolean;
    room_Id: number;
}


/* =========================================================
   SHELF
========================================================= */

export interface Shelf {
    shelf_Id: number;
    shelf_Name: string;
    shelf_Code?: string | null;
    shelf_Description?: string | null;
    isActive: boolean;
    row_Id: number;
    row?: Row;
}

export interface CreateShelfRequest {
    shelf_Name: string;
    shelf_Code?: string;
    shelf_Description?: string;
    isActive: boolean;
    row_Id: number;
}

export interface UpdateShelfRequest {
    shelf_Name: string;
    shelf_Code?: string;
    shelf_Description?: string;
    isActive: boolean;
    row_Id: number;
}


/* =========================================================
   BIN
========================================================= */

export interface Bin {
    bin_Id: number;
    bin_Name: string;
    bin_Code?: string | null;
    bin_Description?: string | null;
    isActive: boolean;
    shelf_Id: number;
    shelf?: Shelf;
}

export interface CreateBinRequest {
    bin_Name: string;
    bin_Code?: string;
    bin_Description?: string;
    isActive: boolean;
    shelf_Id: number;
}

export interface UpdateBinRequest {
    bin_Name: string;
    bin_Code?: string;
    bin_Description?: string;
    isActive: boolean;
    shelf_Id: number;
}


/* =========================================================
   PRODUCT
========================================================= */

export interface Product {
    productId: number;
    productName: string;
    sku: string;
    barcode?: string | null;
    qrValue: string;
    categoryId: number;
    unitPrice: number;
    minimumStock: number;
    status: string;
    createdAt: string;
    updatedAt?: string | null;
    unitId: number;
    subCategoryId?: number | null;

    category?: Category;
    unit?: Unit;
    subCategory?: SubCategory;
}

export interface CreateProductRequest {
    productName: string;
    sku: string;
    barcode?: string;
    qrValue: string;
    categoryId: number;
    unitPrice: number;
    minimumStock: number;
    status: string;
    unitId: number;
    subCategoryId?: number | null;
}

export interface UpdateProductRequest {
    productName: string;
    sku: string;
    barcode?: string;
    qrValue: string;
    categoryId: number;
    unitPrice: number;
    minimumStock: number;
    status: string;
    unitId: number;
    subCategoryId?: number | null;
}

export interface UpdateProductStatusRequest {
    status: string;
}


/* =========================================================
   STOCK
========================================================= */

export interface Stock {
    stock_Id: number;
    quantity: number;
    isActive: boolean;
    createAt: string;
    lastUpdatedAt: string;
    productId: number;
    bin_Id: number;
    stockStatue: string;

    product?: Product;
    bin?: Bin;
}

export interface CreateStockRequest {
    quantity: number;
    isActive: boolean;
    productId: number;
    bin_Id: number;
    stockStatue: string;
}

export interface UpdateStockRequest {
    quantity: number;
    isActive: boolean;
    productId: number;
    bin_Id: number;
    stockStatue: string;
}

export interface UpdateStockStatusRequest {
    stockStatue: string;
}


/* =========================================================
   TRANSACTION
========================================================= */

export interface Transaction {
    transaction_Id: number;
    transactionType: string;
    product_Id: number;
    quantity: number;
    unit_Id: number;
    fromBinId?: number | null;
    toBinId?: number | null;
    user_Id: string;
    notes?: string | null;
    createAt: string;

    product?: Product;
    unit?: Unit;
    fromBin?: Bin;
    toBin?: Bin;
    user?: AuthUser;
}

export interface CreateTransactionRequest {
    transactionType: string;
    product_Id: number;
    quantity: number;
    unit_Id: number;
    fromBinId?: number | null;
    toBinId?: number | null;
    notes?: string;
}

export interface TransactionFilter {
    transactionType?: string;
    productId?: number;
    userId?: string;
    binId?: number;
    fromDate?: string;
    toDate?: string;
}


/* =========================================================
   USER
========================================================= */

export interface User {
    user_Id: string;
    user_Name: string;
    user_Email: string;
    createAt: string;
    updateAt: string;
    loginAt?: string | null;
    role_Id: number;

    role?: Role;
}

export interface UpdateUserRequest {
    user_Name: string;
    user_Email: string;
}

export interface ChangeUserRoleRequest {
    user_Id: string;
    role_Id: number;
}


/* =========================================================
   REPORT
========================================================= */

export interface Report {
    report_Id: number;
    reportType: string;
    fromDate: string;
    toDate: string;
    craeteByUserId: string;
    createAt: string;
    warehouse_Id?: number | null;
    product_Id?: number | null;

    warehouse?: Warehouse;
    product?: Product;
    createdByUser?: User;
}


/* =========================================================
   REPORT SCHEDULE
========================================================= */

export interface ReportSchedule {
    reportSchedule_Id: number;
    reportType: number;
    frequency: string;
    runAt: string;
    isActive: boolean;
    craeteByUserId: string;
    warehouse_Id?: number | null;
    product_Id?: number | null;
    lastRunAt?: string | null;
    nextRunAt?: string | null;
    updateAt?: string | null;
    createAt: string;
}