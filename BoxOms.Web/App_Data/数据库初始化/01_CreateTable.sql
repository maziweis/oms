if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('box_good_online') and o.name = 'FK_BOX_GOODONLINE_REFERENCE_BOX_GOOD')
alter table box_good_online
   drop constraint FK_BOX_GOODONLINE_REFERENCE_BOX_GOOD
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('box_subject_edition') and o.name = 'FK_BOX_SUBJ_REFERENCE_BOX_GOOD')
alter table box_subject_edition
   drop constraint FK_BOX_SUBJ_REFERENCE_BOX_GOOD
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('box_update_sys_log') and o.name = 'FK_OMS_BOX_UPDATESYS_REFERENCE_OMS_BOX_UPDATESYSLOG')
alter table box_update_sys_log
   drop constraint FK_OMS_BOX_UPDATESYS_REFERENCE_OMS_BOX_UPDATESYSLOG
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('box_update_sys_log') and o.name = 'FK_BOX_UPDATESYSLOG_REFERENCE_BOX_GOOD')
alter table box_update_sys_log
   drop constraint FK_BOX_UPDATESYSLOG_REFERENCE_BOX_GOOD
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('sys_role_and_auth') and o.name = 'FK_SYS_ROLE_AND_AUTH_REFERENCE_SYS_ROLE')
alter table sys_role_and_auth
   drop constraint FK_SYS_ROLE_AND_AUTH_REFERENCE_SYS_ROLE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('sys_user_and_role') and o.name = 'FK_SYS_USER_AND_ROLE_REFERENCE_SYS_USER')
alter table sys_user_and_role
   drop constraint FK_SYS_USER_AND_ROLE_REFERENCE_SYS_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('sys_user_and_role') and o.name = 'FK_SYS_USER_AND_ROLE_REFERENCE_SYS_ROLE')
alter table sys_user_and_role
   drop constraint FK_SYS_USER_AND_ROLE_REFERENCE_SYS_ROLE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('box_good')
            and   type = 'U')
   drop table box_good
go

if exists (select 1
            from  sysobjects
           where  id = object_id('box_good_online')
            and   type = 'U')
   drop table box_good_online
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('box_resource_statist')
            and   name  = 'Index_1'
            and   indid > 0
            and   indid < 255)
   drop index box_resource_statist.Index_1
go

if exists (select 1
            from  sysobjects
           where  id = object_id('box_resource_statist')
            and   type = 'U')
   drop table box_resource_statist
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('box_resource_statist_day')
            and   name  = 'Index_1'
            and   indid > 0
            and   indid < 255)
   drop index box_resource_statist_day.Index_1
go

if exists (select 1
            from  sysobjects
           where  id = object_id('box_resource_statist_day')
            and   type = 'U')
   drop table box_resource_statist_day
go

if exists (select 1
            from  sysobjects
           where  id = object_id('box_subject_edition')
            and   type = 'U')
   drop table box_subject_edition
go

if exists (select 1
            from  sysobjects
           where  id = object_id('box_update_sys')
            and   type = 'U')
   drop table box_update_sys
go

if exists (select 1
            from  sysobjects
           where  id = object_id('box_update_sys_log')
            and   type = 'U')
   drop table box_update_sys_log
go

if exists (select 1
            from  sysobjects
           where  id = object_id('sys_role')
            and   type = 'U')
   drop table sys_role
go

if exists (select 1
            from  sysobjects
           where  id = object_id('sys_role_and_auth')
            and   type = 'U')
   drop table sys_role_and_auth
go

if exists (select 1
            from  sysobjects
           where  id = object_id('sys_user')
            and   type = 'U')
   drop table sys_user
go

if exists (select 1
            from  sysobjects
           where  id = object_id('sys_user_and_role')
            and   type = 'U')
   drop table sys_user_and_role
go

/*==============================================================*/
/* Table: box_good                                              */
/*==============================================================*/
create table box_good (
   BoxId                int                  identity,
   Code                 varchar(100)         null,
   Name                 varchar(100)         null,
   Mac                  varchar(30)          null,
   ActivNumber          varchar(500)         null,
   Edition              int                  null,
   SysVersion           float                not null,
   Principal            varchar(30)          null,
   UpTime               datetime             null,
   State                int                  not null,
   IsCanUpdate          bit                  not null,
   IsActiv              bit                  not null,
   Total                int                  not null,
   Prov                 varchar(100)         null,
   City                 varchar(100)         null,
   Area                 varchar(100)         null,
   SchoolName           varchar(100)         null,
   IP                   varchar(100)         null,
   FirstRunTime         datetime             null,
   RunTime              datetime             null,
   UseUserName          varchar(20)          null,
   Remark               varchar(300)         null,
   CreateTime           datetime             not null,
   CreateUserId         int                  not null,
   UpdateTime           datetime             null,
   UpdateUserId         int                  null,
   ExpirTime            datetime             null,
   Validity             int                  null,
   constraint PK_BOX_GOOD primary key (BoxId)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '1.库存，2.已售，3.领用',
   'user', @CurrentUser, 'table', 'box_good', 'column', 'State'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '默认-是',
   'user', @CurrentUser, 'table', 'box_good', 'column', 'IsCanUpdate'
go

/*==============================================================*/
/* Table: box_good_online                                       */
/*==============================================================*/
create table box_good_online (
   BoxId                int                  not null,
   LastTime             datetime             null,
   LastIp               varchar(50)          null,
   constraint PK_BOX_GOOD_ONLINE primary key (BoxId)
)
go

/*==============================================================*/
/* Table: box_resource_statist                                  */
/*==============================================================*/
create table box_resource_statist (
   Id                   varchar(36)          not null,
   BoxId                int                  not null,
   Mac                  varchar(100)         not null,
   CreateDate           datetime             not null,
   Subject              int                  null,
   Edition              int                  null,
   Type                 int                  not null,
   constraint PK_BOX_RESOURCE_STATIST primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '每个商品每天统计记录一次',
   'user', @CurrentUser, 'table', 'box_resource_statist'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '1-教材，2-游戏-3.。。。。。',
   'user', @CurrentUser, 'table', 'box_resource_statist', 'column', 'Type'
go

/*==============================================================*/
/* Index: Index_1                                               */
/*==============================================================*/
create index Index_1 on box_resource_statist (
CreateDate DESC
)
go

/*==============================================================*/
/* Table: box_resource_statist_day                              */
/*==============================================================*/
create table box_resource_statist_day (
   Id                   varchar(36)          not null,
   BoxId                int                  not null,
   Mac                  varchar(100)         not null,
   SDate                datetime             not null,
   Subject              int                  null,
   Edition              int                  null,
   f0                   int                  not null,
   f1                   int                  not null,
   f2                   int                  not null,
   f3                   int                  not null,
   f4                   int                  not null,
   f5                   int                  not null,
   f6                   int                  not null,
   f7                   int                  not null,
   f8                   int                  not null,
   f9                   int                  not null,
   f10                  int                  not null,
   f11                  int                  not null,
   f12                  int                  not null,
   EditionTotal         int                  not null,
   SubjectTotal         int                  not null,
   constraint PK_BOX_RESOURCE_STATIST_DAY primary key (Id)
)
go

/*==============================================================*/
/* Index: Index_1                                               */
/*==============================================================*/
create index Index_1 on box_resource_statist_day (
SDate DESC
)
go

/*==============================================================*/
/* Table: box_subject_edition                                   */
/*==============================================================*/
create table box_subject_edition (
   Id                   varchar(36)          not null,
   BoxId                int                  not null,
   Subject              int                  not null,
   Edition              int                  not null,
   constraint PK_BOX_SUBJECT_EDITION primary key (Id)
)
go

/*==============================================================*/
/* Table: box_update_sys                                        */
/*==============================================================*/
create table box_update_sys (
   Id                   int                  identity,
   VNumber              float                not null,
   PackUrl              varchar(1000)        not null,
   Principal            varchar(50)          not null,
   Cause                varchar(3000)        not null,
   IsPublish            bit                  not null,
   constraint PK_BOX_UPDATE_SYS primary key (Id)
)
go

/*==============================================================*/
/* Table: box_update_sys_log                                    */
/*==============================================================*/
create table box_update_sys_log (
   UpdateId             int                  not null,
   BoxId                int                  not null,
   StartTime            datetime             not null,
   FinishTime           datetime             null,
   State                int                  not null,
   constraint PK_BOX_UPDATE_SYS_LOG primary key (UpdateId, BoxId)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '1-更新中，2-更新成功，3-更新失败',
   'user', @CurrentUser, 'table', 'box_update_sys_log', 'column', 'State'
go

/*==============================================================*/
/* Table: sys_role                                              */
/*==============================================================*/
create table sys_role (
   RoleId               int                  identity,
   RoleName             varchar(100)         not null,
   Remark               varchar(300)         null,
   constraint PK_SYS_ROLE primary key (RoleId)
)
go

/*==============================================================*/
/* Table: sys_role_and_auth                                     */
/*==============================================================*/
create table sys_role_and_auth (
   Id                   varchar(36)          not null,
   RoleId               int                  not null,
   LeftId               int                  not null,
   constraint PK_SYS_ROLE_AND_AUTH primary key (Id)
)
go

/*==============================================================*/
/* Table: sys_user                                              */
/*==============================================================*/
create table sys_user (
   UserId               int                  identity,
   Account              varchar(50)          not null,
   Password             varchar(50)          not null,
   Name                 varchar(50)          not null,
   State                int                  not null,
   constraint PK_SYS_USER primary key (UserId)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0-正常，1-停用，2-删除',
   'user', @CurrentUser, 'table', 'sys_user', 'column', 'State'
go

/*==============================================================*/
/* Table: sys_user_and_role                                     */
/*==============================================================*/
create table sys_user_and_role (
   UserId               int                  not null,
   RoleId               int                  not null,
   CreateTime           datetime             not null,
   constraint PK_SYS_USER_AND_ROLE primary key (UserId, RoleId)
)
go

alter table box_good_online
   add constraint FK_BOX_GOODONLINE_REFERENCE_BOX_GOOD foreign key (BoxId)
      references box_good (BoxId)
go

alter table box_subject_edition
   add constraint FK_BOX_SUBJ_REFERENCE_BOX_GOOD foreign key (BoxId)
      references box_good (BoxId)
go

alter table box_update_sys_log
   add constraint FK_OMS_BOX_UPDATESYS_REFERENCE_OMS_BOX_UPDATESYSLOG foreign key (UpdateId)
      references box_update_sys (Id)
go

alter table box_update_sys_log
   add constraint FK_BOX_UPDATESYSLOG_REFERENCE_BOX_GOOD foreign key (BoxId)
      references box_good (BoxId)
go

alter table sys_role_and_auth
   add constraint FK_SYS_ROLE_AND_AUTH_REFERENCE_SYS_ROLE foreign key (RoleId)
      references sys_role (RoleId)
go

alter table sys_user_and_role
   add constraint FK_SYS_USER_AND_ROLE_REFERENCE_SYS_USER foreign key (UserId)
      references sys_user (UserId)
go

alter table sys_user_and_role
   add constraint FK_SYS_USER_AND_ROLE_REFERENCE_SYS_ROLE foreign key (RoleId)
      references sys_role (RoleId)
go
