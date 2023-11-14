create database use Dapper_Basic

--==============================================
-- 1. Query Base CRUD
--==============================================
--_______________________ Company __________________
create table tblCompany
(
	Id int primary key identity,
	Name varchar(25),
	Address varchar(25),
	City varchar(25),
	State varchar(25),
	PostalCode varchar(25)
)
select * from tblCompany

use Dapper_Basic
--_______________________ Employee __________________
create table tblEmployee
(
	Id int primary key identity,
	Name varchar(25),
	Email varchar(25),
	Phone varchar(25),
	Title varchar(25),
	 CompanyId int REFERENCES tblCompany(Id) ON DELETE SET NULL ON UPDATE SET NULL
)

--===================================================
-- 1. Query Base CRUD
--===================================================

insert into tblCompany(Name,Address,City,State,PostalCode) values (@name,@address,@city,@state,@postalCode)
Select Cast(Scope_Identity() as int)

update tblCompany set Name = @name,Address = @address,City = @City,state=@state, PostalCode = @postalCode where Id = @id

SELECT * from tblCompany

delete tblCompany where Id = @Id


--===================================================
-- 2. Procedure Base
--===================================================
--____________ Company _________________
create proc spSelectCompanyById
@id int
as
begin
	select * from tblCompany
	where Id = @id
end


alter proc spSelectCompanys
as
begin
	select * from tblCompany
end

alter proc spInsertCompany
@id int output,
@name varchar(25),
@address varchar(25),
@City varchar(25),
@state varchar(25),
@PostalCode varchar(25)
as
begin
	insert into tblCompany (name,Address,City,State,PostalCode)
	values (@name,@address,@City,@state,@PostalCode)
	--Select Cast(Scope_Identity() as int)
	select @id = SCOPE_IDENTITY()
end


alter proc spUpdateCompany
@id int,
@name varchar(25),
@address varchar(25),
@City varchar(25),
@state varchar(25),
@PostalCode varchar(25)
as
begin
	update tblCompany
	set 
	name = @name,
	Address = @address,
	City = @City,
	State = @state,
	PostalCode  = @PostalCode
	where Id = @id
end

create proc spDeleteCompanyById
@id int
as
begin
	delete from tblCompany
	where Id = @id
end




--____________ Employee _________________
create proc spSelectEmployeeById
@id int
as
begin
	select * from tblEmployee
	where Id = @id
end


create proc spSelectEmployees
as
begin
	select * from tblEmployee
end

alter proc spInsertEmployee
@id int output,
@name varchar(25),
@title varchar(25),
@email varchar(25),
@phone varchar(25),
@companyId varchar(25)
as
begin
	insert into tblEmployee(Name,Title,Email,Phone,CompanyId)
	values (@name,@title,@email,@phone,@companyId)
	--Select Cast(Scope_Identity() as int)
	select @id = SCOPE_IDENTITY()
end


create proc spUpdateEmployee
@id int ,
@name varchar(25),
@title varchar(25),
@email varchar(25),
@phone varchar(25),
@companyId varchar(25)
as
begin
	update tblEmployee
	set 
	name = @name,
	Title = @title,
	Email = @email,
	Phone = @phone,
	CompanyId  = @companyId
	where Id = @id
end

create proc spDeleteEmployeeById
@id int
as
begin
	delete from tblEmployee
	where Id = @id
end


