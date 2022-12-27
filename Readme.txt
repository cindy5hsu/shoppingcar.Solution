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
	add �X�R��k RegisterDto ToRequestDto(this RegisterVM source)
[V]add MembersController, Register action, View Page: Register.cshtml

-- ��@ MemberService.CreateNewMember
�ثe�\��u�|�s�W�s�|��,�����|�o�T�{�H
[V]add /Models/Infrastructures/HashUtility.cs �ΨӱN�K�X�[�K
[V]modify RegisterDto
	add EncryptedPassword readonly property,�s��[�K�᪺�K�X
	add ConfirmCode property,�Ψӥͦ��s�|���T�{�Hurl�ɨϥ�
[V] add /Models/Services/Interfaces/IMemberRepository.cs
[V] add /Models/Infrastructures/Repositories/MemberRepository.cs
[V]add /Models/Services/MemberService 
	method : (bool IsSuccess, string ErrorMessage) CreateNewMember(RegisterDto dto)
[V] add /Views/Members/RegisterConfirm.cshtml (empty �d��)
[v]modify MembersController
	add HttpPost Register action
[V]modify _Layout page
	�[�J'���U�s�|��'�s��

-- ��@ �s�|�� Email�T�{�\��
�����ҥη|����� , url template /members/activeRegister?memberId=99&confirmCode=xxx
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

-- ��@ �n�J/�n�X����
		�u���b�K���T�ӥB�w�����}�q���|���~���\�n�J, ��@���e, �Х��U�O�إߤ@�Ӥw/���}�q���|���O��,��K����
[V] modify web.config, add Authenthcation node
[V] �n�J�\��
	add /Models/ViewModels/LoginVM.cs
	modify MembersController add Login() action
	add  "Login" view page(�ϥ� Create �d��)
	add /Models/Infrastructures/ExtensionMethods/MemberExts.cs (static class), add �X�R��k MemberDto ToDto(this Member entity) 
	modify IMemberRepository , add GetByAccount(string account)
	modify MemberRepository, add GetByAccount(string account)
	modify RegisterDto, �NSALT �ŧi���`��
	modify MemberService, add Login method
	modify MembersController, add HttpPost Login action(�ϥΪ��{��,�g�J cookie), private ProcessLogin method
[V] modify MemberController.About, add "Authorize" attribute
	�Y�S�n�J�L,�|�۰ʾɦV��/Members/Login
	modify MembersController, add HttpPost Logout action
[V] modify _Layout page, add "Login/Logout" links


-- ��@ �ק�ӤH�򥻸��
[V]�إ߷|�����߭�
	modify MembersController, add Index action
	add Views/Members/Index.cshtml(�ťսd��), ��J�G�ӶW�s�� : ""�ק�ӤH�򥻸��", "���]�K�X"
	�� web.config, form�`�I defaultUrl="/Members/Index/"
[V]��@ �ק�ӤH�򥻸��
	modify MembersController,�bclass �[�JField :MemberService service,�ѦUmethod�@��
	add DTOs/UpdateProfileDto.cs
	add EditProfileVM.cs(�@���y�� EditProfile.cshtml ��model,�ѩ󤣤��\�ק�b��,�ҥH�o���O�̨S��Account), �b��cs�[�Jadd MemberDtoExts class

	modify IMemberRepository, add void Update(MemberDto entity);
		modify MemberRepository, add void Update(MemberDto entity)
	modify MemberService, add UpdateProfile() method
	modify MembersController, add EditProfile actions(HttpGet, HttpPost�G�� action)
	add "EditProfile" view page(�ϥ� Edit �d��)

-- ��@ �ܧ�K�X
[V] add EditPasswordVM.cs
[V]add Models/DTOs/ChangePasswordRequest.cs
[V] add �X�R��kclass EditPasswordVMExts,�g�bEditPasswordVM.cs
[V]modify MembersController, add EditPassword action(HttpGet action)
[V] add EditPassword view page(��create �d��)
[V]modify MembersController, add HttpPost EditPassword action

[V]modify IMemberRepository, add void UpdatePassword(int memberId, string newEncryptedPassword);
		modify MemberRepository add UpdatePassword method
[V]modify MemberService, add ChangePassword()
[V]modify /Views/Members/Index, => <a href="/Members/EditPassword/">�ק�K�X</a>

-- ��@ �ѰO�K�X/���]�K�X
[V]modify MemberExts.ToDto() �p�G�Onull,�N�Ǧ^null

[V]modify Views/Members/Login.cshtml, �[�J '�ѰO�K�X'�W�s��
[V]add Views/Members/ConfirmForgetPassword.cshtml, �Ϊťսd��
[V]add /Models/ViewModels/ForgetPasswordVM.cs

[V]add Models/Infrastructures/EmailHelper class, ���g�H�H���\��
		�إ� ~/files/ folder,�Ψө�H�H�����դ��e

[V]modify MemberService add RequestResetPassword method

[V]modify MembersController , add ForgetPassword action(HttpGet action)
[V]add ForgetPassword view page(��Create �d��)
[V]modify MembersController , add ForgetPassword action(HttpPost action)
[V]modify MemerService add ResetPassword method

[V]add Models/ViewModels/ResetPasswordVM
[V]modify MembersController , add ResetPassword action(httpget action)
		add /Views/Members/ResetPassword.cshtml(create �d��)
		modify MembersController , add ResetPassword action(httpPost action)
		add ResetPasswordConfirm.csthml(�ťսd��)

**** �e�x�ʪ����\�� ****
-- �s�W��ƪ�(�[�J�ӫ~�d�ҰO��)�έ���EFModels
Categories
	Id, Name(NVarchar 30, ,Unique, NN), DisplayOrder(int, NN)

Products
	Id, CategoryId(int, FK, ref Categories.Id, NN), Name(NVarchar 50, NN, Unique), Description(Nvarchar 3000, NN), 
	Price(int, NN), Status(bit ,NN,default value=1,1=�i�ʶR;0=�w�U�[), ProductImage(NVarchar 70, NN), Stock(int, NN)

Carts(�ʪ���,�P�|���e�{�@��@���Y, ���b��,�N�N�O���R��)
	Id(int, NN), MemberAccount(Nvarchar 30, NN, unique, ref Members.Account, �C�@�ӷ|���̦h�u���@���ʪ���, �ݽT�{Members.Account���]��unique index)

CartItems
	Id(int, NN), CartId(int ,NN, FK ,ref Carts.Id, �]���걵�R��), ProductId(int, NN, ref Products.Id), Qty(int, NN)

Orders
	Id(int, NN), MemberId(int,NN, ref Members.Id), Total(int, NN), 
	CreatedTime(DateTime, NN, default=getdate()), Status(int ,NN,1=�q�榨��, 2=�w����, -1=�w�h�q),
	RequestFund(bit, NN,def=0,�Ȥ�O�_���X�h�q�n�D), RequestFundTime(DateTime, Allow Null),
	����H��T: Receiver(Nvarchar 30, NN), Address(NVarchar 200, NN), CellPhone(varchar 10, NN)

OrderItems
	Id(int, NN), OrderId(int, NN,FK, ref Orders.Id), ProductId(int, NN, FK, ref Products.Id), 
	ProductName(NVarchar 50, NN), Price(int, NN), Qty(int , NN), SubTotal(int, NN)

[V] �b�e�x�������s�إ� /Models/EFModels/, ���s�ظm�M��
[V] /Files/ �Ψө�ӫ~�Ӥ�

-- add ProductService
[V]add CategoryDto, ProductDto
[V]add IProductRepository
[V]add ProductService

-- add �w�s�A��
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
	�NProductExts �אּpartial class
	�b CartProductEntity.cs�A�[�J�t�@�� partial class ProductExts

[V]modify CartItemEntity
	add CartItemExts in CartItemEntity.cs
[V]modify CartEntity
	add CartExts class in CartEntity.cs
[V]add CartRepository

--add OrderRepository
[V]�ק�Orders table�G�����
	RequestRefund(bit, NN,def=0,�Ȥ�O�_���X�h�q�n�D), RequestRefundTime(DateTime, Allow Null),
	��ʭק�EFModels/Order class���ݩʦW��
[V]�ק�OrderItem table �@�����
	SubTotal, �����r
	�@�֭ק�EFModels/OrderItem class property name

[V]����CreateOrderItem.cs, add CreateOrderItemExts partial class in CreateOrderItem.cs
[V]����CreateOrderRequest.cs

[V]add ProductExts partial class in OrderProductEntity.cs
[V]add OrderItemExts partial calss in OrderItemEntity.cs

[V]modify IOrderRepository
	add Search method
	�N Refund method �ܧ� RefundByCustomer
[V]modify OrderService.cs
	line 48, �s�� RefundByCustomer()

[V]���� ORderEntity,add OrderExts partial class in OrderEntity.cs

[V]add OrderRepository
	�̤W��n�[using System.Data.Entity; �~��ϥ� Include

-- add ProductsController, �ק���������� products/index
INSERT INTO Categories(Name, DisplayOrder)
VALUES
('3C', 10),
('�a�q', 20),
('�ͬ��Ϋ~', 30)

INSERT INTO Products(CategoryId, Name, Price, Description, Status, ProductImage, Stock)
VALUES
(1, 'HP Printer', 3000, 'description', 1, 'printer.jpg', 100),
(1, '�غ�A320 ���', 20000, 'description', 1, 'acerPC.jpg', 100),
(1, '�غ� Zenbook 14 UX3402ZA', 34900, 'description', 1, 'zenbook.jpg', 100),
(2, 'Dyson Cyclone V10 Fluffy SV12 �L�u�l�о�', 12900, 'description', 1, 'a.jpg', 100),
(2, 'SANSUI �s�� 1.8L�j�e�q304���׿��q�����G�J��', 788, 'description', 1, 'a.jpg', 100),
(2, '�j�� Dainichi �Ѫo�x���', 10900, 'description', 1, 'a.jpg', 100),
(2, 'PHILIPS ���Q��  �����@�i���i�_�ʤ��� HX6803/02', 2490, 'description', 1, 'a.jpg', 100),
(3, 'Boden-����4�ئh�\�ব�Ǥɭ��j���L', 5599, 'description', 1, 'a.jpg', 100),
(3, 'Flexispot ��ù��a�B���u��[ - �¦�', 1912, 'description', 1, 'a.jpg', 100),
(3, 'Flexispot ²���������ɭ��u�@�x', 3390, 'description', 1, 'a.jpg', 100),
(3, 'TOTO TCF23710ATW C2 �Ť��~�b�K�y', 8990, 'description', 1, 'a.jpg', 100),
(3, '�H�L*0.82L*SLiT���ÿ��u�ūO�Ų~', 690, 'description', 1, 'a.jpg', 100)
======================

[V]modify Productservice.Search���Ѽ�
[V]add ProductVM
	add PorductDtoExts class in ProductVM.cs
[V]add ProductsController, add Index action
	add Products/Index.cshtml, �ק�button, �[�J js code, ���� AddNew hyperlink
		�u���b�w�n�J���p�U,�~��� 'add to cart' button
[V]add CartController, ��@�N�ӫ~�[�J�ʪ������\��
[V]modify app_start/RouteConfig.cs , �w�]�� Products/Index
	modify _layout ,�ק�home��hyperlink

-- add Cart/Info, ����ʪ�����T, ��@�W��ƶq���\��
[V]modify CartController
	add CustomerAccount property
	add Info action
	add UpdateItem action
	add Cart/Info view page(��details �d��, model�O CartEntity,���e�ۤv�g)
[V]modify _layout view page
	add �ʪ��� navbar
[V]modify CartEntity
		add Total, ShippingInfo properties
		modify UpdateQty method
[V]modify CartRepository.Save()
	
-- ��@ ���b�@�~ Cart/Checkout/
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
	add Checkout view page(edit template, model=CheckoutVM),������A�ק�
	add CheckoutConfirm view page(�ť�template),������A�ק�
[working on]modify CartService
	modify ToCreateOrderRequest,�b�ഫ�� CreateOrderRequest��, �[�JShippingInfo

[working on]modify Cart/info view page
	add total, checkout css style
	add ���b button