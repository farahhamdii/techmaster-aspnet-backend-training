create database LibraryManagementDB;
go

use LibraryManagementDB;
go

-- authors table

create table Authors
(
    AuthorId int identity(1,1) primary key,
    FullName nvarchar(150) not null,
    BirthDate date null,
    Country nvarchar(100) null
);


-- categories table

create table Categories
(
    CategoryId int identity(1,1) primary key,
    Name nvarchar(100) not null,
    Description nvarchar(500) null
);


-- members table

create table Members
(
    MemberId int identity(1,1) primary key,
    FullName nvarchar(150) not null,
    Email nvarchar(150) not null,
    PhoneNumber nvarchar(30) null,
    JoinDate date not null,
    IsActive bit not null default 1
);


-- books table

create table Books
(
    BookId int identity(1,1) primary key,
    Title nvarchar(200) not null,
    ISBN nvarchar(20) not null,
    PublishedYear int null,
    AvailableCopies int not null default 0,
    AuthorId int not null,
    CategoryId int not null,

    constraint FK_Books_Authors foreign key (AuthorId)
        references Authors(AuthorId),
    constraint FK_Books_Categories foreign key (CategoryId)
        references Categories(CategoryId)
);


-- borrowrecords table

create table BorrowRecords
(
    BorrowRecordId int identity(1,1) primary key,
    BookId int not null,
    MemberId int not null,
    BorrowDate date not null,
    DueDate date not null,
    ReturnDate date null,
    Status nvarchar(30) not null,

    constraint FK_BorrowRecords_Books foreign key (BookId)
        references Books(BookId),
    constraint FK_BorrowRecords_Members foreign key (MemberId)
        references Members(MemberId)
);


-- insert sample authors

insert into Authors (FullName, BirthDate, Country)
values
('Robert C. Martin', '1952-12-05', 'USA'),
('Martin Fowler', '1963-12-18', 'UK'),
('Andrew Hunt', '1964-08-07', 'USA'),
('Eric Evans', '1961-01-01', 'USA');


-- insert sample categories

insert into Categories (Name, Description)
values
('Programming', 'Books about programming and software development'),
('Database', 'Books about databases and data management'),
('Software Engineering', 'Books about software engineering concepts'),
('Computer Science', 'General computer science books');


-- insert sample members

insert into Members (FullName, Email, PhoneNumber, JoinDate, IsActive)
values
('Ahmed Ali', 'ahmed@example.com', '01011111111', '2026-01-10', 1),
('Sara Mohamed', 'sara@example.com', '01022222222', '2026-02-15', 1),
('Omar Hassan', 'omar@example.com', '01033333333', '2026-03-20', 1),
('Mariam Adel', 'mariam@example.com', '01044444444', '2026-04-05', 0);


-- insert sample books

insert into Books (Title, ISBN, PublishedYear, AvailableCopies, AuthorId, CategoryId)
values
('Clean Code', '9780132350884', 2008, 3, 1, 1),
('Refactoring', '9780134757599', 2018, 2, 2, 3),
('The Pragmatic Programmer', '9780135957059', 2019, 4, 3, 1),
('Domain-Driven Design', '9780321125217', 2003, 1, 4, 3),
('Clean Architecture', '9780134494166', 2017, 2, 1, 3);


-- insert sample borrow records

insert into BorrowRecords (BookId, MemberId, BorrowDate, DueDate, ReturnDate, Status)
values
(1, 1, '2026-08-01', '2026-08-10', null, 'Overdue'),
(1, 2, '2026-08-05', '2026-08-20', null, 'Borrowed'),
(2, 1, '2026-07-20', '2026-07-30', '2026-07-28', 'Returned'),
(3, 3, '2026-08-10', '2026-08-20', null, 'Borrowed'),
(4, 2, '2026-07-01', '2026-07-10', '2026-07-09', 'Returned'),
(5, 1, '2026-08-08', '2026-08-18', null, 'Borrowed');


-- queries

-- 1. select all books

select *
from Books;


-- 2. select all active members

select *
from Members
where IsActive = 1;


-- 3. select books by category

select b.BookId, b.Title, b.ISBN, b.PublishedYear, b.AvailableCopies,
       c.Name as CategoryName
from Books b
inner join Categories c on b.CategoryId = c.CategoryId
where c.Name = 'Programming';


-- 4. count books per category

select c.CategoryId, c.Name as CategoryName, count(b.BookId) as BookCount
from Categories c
left join Books b on c.CategoryId = b.CategoryId
group by c.CategoryId, c.Name;


-- 5. select borrow records with member name and book title

select br.BorrowRecordId, m.FullName as MemberName, b.Title as BookTitle,
       br.BorrowDate, br.DueDate, br.ReturnDate, br.Status
from BorrowRecords br
inner join Members m on br.MemberId = m.MemberId
inner join Books b on br.BookId = b.BookId;


-- 6. select overdue books

select b.BookId, b.Title, m.FullName as MemberName, br.DueDate, br.Status
from BorrowRecords br
inner join Books b on br.BookId = b.BookId
inner join Members m on br.MemberId = m.MemberId
where br.DueDate < cast(getdate() as date)
  and br.ReturnDate is null;


-- 7. select borrowing history for one member

select br.BorrowRecordId, b.Title as BookTitle, br.BorrowDate,
       br.DueDate, br.ReturnDate, br.Status
from BorrowRecords br
inner join Books b on br.BookId = b.BookId
where br.MemberId = 1;


-- 8. select available books

select BookId, Title, AvailableCopies
from Books
where AvailableCopies > 0;


-- 9. count how many books each author has

select a.AuthorId, a.FullName as AuthorName, count(b.BookId) as BookCount
from Authors a
left join Books b on a.AuthorId = b.AuthorId
group by a.AuthorId, a.FullName;


-- 10. select top 5 most borrowed books

select top 5 b.BookId, b.Title, count(br.BorrowRecordId) as BorrowCount
from Books b
inner join BorrowRecords br on b.BookId = br.BookId
group by b.BookId, b.Title
order by BorrowCount desc;