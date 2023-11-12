create database use Dapper_Basic
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

