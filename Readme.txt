-- add Db, Members table, EFModels, RegisterVm
[V] add BookStore.Site mvc project
[V]Create BookStore db(bookstoreLogin, 123), Members table
	Id, int, PK, not null, identity
	Account, NVarchar(30), not null, unique
	EncryptedPassword, varchar(70), not null
	Email, nvarchar(256), not null
	Name, nvarchar(30), not null
	Mobile, varchar(10), allow null
	IsConfirmed, bit, allow null
	ConfirmCode, varchar(50), allow null
[V]add EFModels, AppDbContext, connection string='AppDbContext'
[V]add ViewModels/RegisterVM, DTOs/RegisterDto class
	add 擴充方法 RegisterDto ToRequestDto(this RegisterVM source)
[V]add MembersController, Register action, View Page: Register.cshtml

-- 實作 MemberService.CreateNewMember
目前功能只會新增新會員,但不會發確認信
[V]add /Models/Infrastructures/HashUtility.cs 用來將密碼加密
[V]modify RegisterDto
	add EncryptedPassword readonly property,存放加密後的密碼
	add ConfirmCode property,用來生成新會員確認信url時使用
[V] add /Models/Services/Interfaces/IMemberRepository.cs
[V] add /Models/Infrastructures/Repositories/MemberRepository.cs
[V]add /Models/Services/MemberService 
	method : (bool IsSuccess, string ErrorMessage) CreateNewMember(RegisterDto dto)
[V] add /Views/Members/RegisterConfirm.cshtml (empty 範本)
[v]modify MembersController
	add HttpPost Register action
[V]modify _Layout page
	加入'註冊新會員'連結

-- 實作 新會員 Email確認功能
正式啟用會員資格 , url template /members/activeRegister?memberId=99&confirmCode=xxx
[V] add /Models/DTOs/MemberDto.cs
[V]modify IMemberRepository
	add MemberDto Load(int memberId);
	add void ActiveRegister(int memberId);
[V]modify MemberRepository
	add MemberDto Load(int memberId);
	add void ActiveRegister(int memberId);
[V]modify MembersController
	add ActiveRegister action
[V]add /Views/Members/ActiveRegister.csthml

-- 實作 登入/登出網站
		只有帳密正確而且已正式開通的會員才允許登入, 實作之前, 請先各別建立一個已/未開通的會員記錄,方便測試
[V] modify web.config, add Authenthcation node
[V] 登入功能
	add /Models/ViewModels/LoginVM.cs
	modify MembersController add Login() action
	add  "Login" view page(使用 Create 範本)
	add /Models/Infrastructures/ExtensionMethods/MemberExts.cs (static class), add 擴充方法 MemberDto ToDto(this Member entity) 
	modify IMemberRepository , add GetByAccount(string account)
	modify MemberRepository, add GetByAccount(string account)
	modify RegisterDto, 將SALT 宣告成常數
	modify MemberService, add Login method
	modify MembersController, add HttpPost Login action(使用表單認證,寫入 cookie), private ProcessLogin method
[V] modify MemberController.About, add "Authorize" attribute
	若沒登入過,會自動導向到/Members/Login
	modify MembersController, add HttpPost Logout action
[V] modify _Layout page, add "Login/Logout" links


-- 實作 修改個人基本資料
[V]建立會員中心頁
	modify MembersController, add Index action
	add Views/Members/Index.cshtml(空白範本), 填入二個超連結 : ""修改個人基本資料", "重設密碼"
	改 web.config, form節點 defaultUrl="/Members/Index/"
[V]實作 修改個人基本資料
	modify MembersController,在class 加入Field :MemberService service,供各method共用
	add DTOs/UpdateProfileDto.cs
	add EditProfileVM.cs(作為稍後 EditProfile.cshtml 的model,由於不允許修改帳號,所以這類別裡沒有Account), 在此cs加入add MemberDtoExts class

	modify IMemberRepository, add void Update(MemberDto entity);
		modify MemberRepository, add void Update(MemberDto entity)
	modify MemberService, add UpdateProfile() method
	modify MembersController, add EditProfile actions(HttpGet, HttpPost二個 action)
	add "EditProfile" view page(使用 Edit 範本)

-- 實作 變更密碼
[V] add EditPasswordVM.cs
[V]add Models/DTOs/ChangePasswordRequest.cs
[V] add 擴充方法class EditPasswordVMExts,寫在EditPasswordVM.cs
[V]modify MembersController, add EditPassword action(HttpGet action)
[V] add EditPassword view page(用create 範本)
[V]modify MembersController, add HttpPost EditPassword action

[V]modify IMemberRepository, add void UpdatePassword(int memberId, string newEncryptedPassword);
		modify MemberRepository add UpdatePassword method
[V]modify MemberService, add ChangePassword()
[V]modify /Views/Members/Index, => <a href="/Members/EditPassword/">修改密碼</a>

-- 實作 忘記密碼/重設密碼
[V]modify MemberExts.ToDto() 如果是null,就傳回null

[V]modify Views/Members/Login.cshtml, 加入 '忘記密碼'超連結
[V]add Views/Members/ConfirmForgetPassword.cshtml, 用空白範本
[V]add /Models/ViewModels/ForgetPasswordVM.cs

[V]add Models/Infrastructures/EmailHelper class, 撰寫寄信的功能
		建立 ~/files/ folder,用來放寄信的測試內容

[V]modify MemberService add RequestResetPassword method

[V]modify MembersController , add ForgetPassword action(HttpGet action)
[V]add ForgetPassword view page(用Create 範本)
[V]modify MembersController , add ForgetPassword action(HttpPost action)
[V]modify MemerService add ResetPassword method

[V]add Models/ViewModels/ResetPasswordVM
[V]modify MembersController , add ResetPassword action(httpget action)
		add /Views/Members/ResetPassword.cshtml(create 範本)
		modify MembersController , add ResetPassword action(httpPost action)
		add ResetPasswordConfirm.csthml(空白範本)

**** 前台購物車功能 ****
-- 新增資料表(加入商品範例記錄)及重建EFModels
Categories
	Id, Name(NVarchar 30, ,Unique, NN), DisplayOrder(int, NN)

Products
	Id, CategoryId(int, FK, ref Categories.Id, NN), Name(NVarchar 50, NN, Unique), Description(Nvarchar 3000, NN), 
	Price(int, NN), Status(bit ,NN,default value=1,1=可購買;0=已下架), ProductImage(NVarchar 70, NN), Stock(int, NN)

Carts(購物車,與會員呈現一對一關係, 結帳後,就將記錄刪除)
	Id(int, NN), MemberAccount(Nvarchar 30, NN, unique, ref Members.Account, 每一個會員最多只有一個購物車, 需確認Members.Account有設為unique index)

CartItems
	Id(int, NN), CartId(int ,NN, FK ,ref Carts.Id, 設為串接刪除), ProductId(int, NN, ref Products.Id), Qty(int, NN)

Orders
	Id(int, NN), MemberId(int,NN, ref Members.Id), Total(int, NN), 
	CreatedTime(DateTime, NN, default=getdate()), Status(int ,NN,1=訂單成立, 2=已結案, -1=已退訂),
	RequestFund(bit, NN,def=0,客戶是否提出退訂要求), RequestFundTime(DateTime, Allow Null),
	收件人資訊: Receiver(Nvarchar 30, NN), Address(NVarchar 200, NN), CellPhone(varchar 10, NN)

OrderItems
	Id(int, NN), OrderId(int, NN,FK, ref Orders.Id), ProductId(int, NN, FK, ref Products.Id), 
	ProductName(NVarchar 50, NN), Price(int, NN), Qty(int , NN), SubTotal(int, NN)

[V] 在前台網站重新建立 /Models/EFModels/, 重新建置專案
[V] /Files/ 用來放商品照片

-- add ProductService
[V]add CategoryDto, ProductDto
[V]add IProductRepository
[V]add ProductService

-- add 庫存服務
[V]add IStockRepository
[V]add DTOs/DeductStockInfo, DTOs/ReviseStickInfo
[V]add StockService

-- add CustomerService
[V] add CustomerDto
[V] add ICustomerRepository
[V] add CustomerService

-- add CartService
[V]add CartProductEntity / CartItemEntity/ CartEntity / 
[V]add ICartRepository
[V]add DTOs/ ShippingInfo / CreateOrderItem / CreateOrderRequest classes
[V]add CartService

-- add OrderService
[V] add DTOs/OrderProductEntity, OrderItemEntity , OrderEntity
[V] add IOrderRepository
[V] add OrderService
[V] add CartMediator

-- add ProductRepository
[V]add ProductExts class in ProductDto.cs
[V]add CategoryExts class in Category.cs
[V]modify IProductRepository
	from IEnumerable<ProductDto> Search(int categoryId, string productName, bool? status);
	to IEnumerable<ProductDto> Search(int? categoryId, string productName, bool? status);

[V]add ProductRepository.cs

-- add CustomerRepository
[V]add CustomerExts in CustomerDto.cs
[V]add CustomerRepository

-- add StockRepository,CartRepository
[V]StockRepository

[V]modify ProductDto.cs
	將ProductExts 改為partial class
	在 CartProductEntity.cs再加入另一個 partial class ProductExts

[V]modify CartItemEntity
	add CartItemExts in CartItemEntity.cs
[V]modify CartEntity
	add CartExts class in CartEntity.cs
[V]add CartRepository

--add OrderRepository
[V]修改Orders table二個欄位
	RequestRefund(bit, NN,def=0,客戶是否提出退訂要求), RequestRefundTime(DateTime, Allow Null),
	手動修改EFModels/Order class的屬性名稱
[V]修改OrderItem table 一個欄位
	SubTotal, 拼錯字
	一併修改EFModels/OrderItem class property name

[V]重建CreateOrderItem.cs, add CreateOrderItemExts partial class in CreateOrderItem.cs
[V]重建CreateOrderRequest.cs

[V]add ProductExts partial class in OrderProductEntity.cs
[V]add OrderItemExts partial calss in OrderItemEntity.cs

[V]modify IOrderRepository
	add Search method
	將 Refund method 變更為 RefundByCustomer
[V]modify OrderService.cs
	line 48, 叫用 RefundByCustomer()

[V]重做 ORderEntity,add OrderExts partial class in OrderEntity.cs

[V]add OrderRepository
	最上方要加using System.Data.Entity; 才能使用 Include

-- add ProductsController, 修改網站首頁為 products/index
INSERT INTO Categories(Name, DisplayOrder)
VALUES
('3C', 10),
('家電', 20),
('生活用品', 30)

INSERT INTO Products(CategoryId, Name, Price, Description, Status, ProductImage, Stock)
VALUES
(1, 'HP Printer', 3000, 'description', 1, 'printer.jpg', 100),
(1, '華碩A320 桌機', 20000, 'description', 1, 'acerPC.jpg', 100),
(1, '華碩 Zenbook 14 UX3402ZA', 34900, 'description', 1, 'zenbook.jpg', 100),
(2, 'Dyson Cyclone V10 Fluffy SV12 無線吸塵器', 12900, 'description', 1, 'a.jpg', 100),
(2, 'SANSUI 山水 1.8L大容量304不銹鋼電茶壺二入組', 788, 'description', 1, 'a.jpg', 100),
(2, '大日 Dainichi 煤油暖氣機', 10900, 'description', 1, 'a.jpg', 100),
(2, 'PHILIPS 飛利浦  智能護齦音波震動牙刷 HX6803/02', 2490, 'description', 1, 'a.jpg', 100),
(3, 'Boden-奧洛4尺多功能收納升降大茶几', 5599, 'description', 1, 'a.jpg', 100),
(3, 'Flexispot 單螢幕懸浮旋臂支架 - 黑色', 1912, 'description', 1, 'a.jpg', 100),
(3, 'Flexispot 簡易式氣壓升降工作台', 3390, 'description', 1, 'a.jpg', 100),
(3, 'TOTO TCF23710ATW C2 溫水洗淨便座', 8990, 'description', 1, 'a.jpg', 100),
(3, '象印*0.82L*SLiT不鏽鋼真空保溫瓶', 690, 'description', 1, 'a.jpg', 100)
======================

[V]modify Productservice.Search的參數
[V]add ProductVM
	add PorductDtoExts class in ProductVM.cs
[V]add ProductsController, add Index action
	add Products/Index.cshtml, 修改button, 加入 js code, 移除 AddNew hyperlink
		只有在已登入狀況下,才顯示 'add to cart' button
[V]add CartController, 實作將商品加入購物車的功能
[V]modify app_start/RouteConfig.cs , 預設為 Products/Index
	modify _layout ,修改home的hyperlink

-- add Cart/Info, 顯示購物車資訊, 實作增減數量的功能
[V]modify CartController
	add CustomerAccount property
	add Info action
	add UpdateItem action
	add Cart/Info view page(用details 範本, model是 CartEntity,內容自己寫)
[V]modify _layout view page
	add 購物車 navbar
[V]modify CartEntity
		add Total, ShippingInfo properties
		modify UpdateQty method
[V]modify CartRepository.Save()
	
-- 實作 結帳作業 Cart/Checkout/
[working on]modify CartEntity
	add AllowCheckout property
[working on]add CheckoutVM
[working on]modify CartService
	add shippingInfo field, 
	modify Checkout
[working on]modify CartController
	add orderService, stockService fields
	modify constructor
	add 2 Checkout actions
	add Checkout view page(edit template, model=CheckoutVM),完成後再修改
	add CheckoutConfirm view page(空白template),完成後再修改
[working on]modify CartService
	modify ToCreateOrderRequest,在轉換成 CreateOrderRequest時, 加入ShippingInfo

[working on]modify Cart/info view page
	add total, checkout css style
	add 結帳 button