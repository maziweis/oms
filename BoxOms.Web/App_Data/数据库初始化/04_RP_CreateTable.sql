if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('rp_cdkey') and o.name = 'FK_RP_CDKEY_REFERENCE_RP_ENTER')
alter table rp_cdkey
   drop constraint FK_RP_CDKEY_REFERENCE_RP_ENTER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('rp_cdkey_and_edition') and o.name = 'FK_CDKEYANDEDITION_REFERENCE_CDKEY')
alter table rp_cdkey_and_edition
   drop constraint FK_CDKEYANDEDITION_REFERENCE_CDKEY
go

if exists (select 1
            from  sysobjects
           where  id = object_id('rp_cdkey')
            and   type = 'U')
   drop table rp_cdkey
go

if exists (select 1
            from  sysobjects
           where  id = object_id('rp_cdkey_and_edition')
            and   type = 'U')
   drop table rp_cdkey_and_edition
go

if exists (select 1
            from  sysobjects
           where  id = object_id('rp_enterprise')
            and   type = 'U')
   drop table rp_enterprise
go

if exists (select 1
            from  sysobjects
           where  id = object_id('rp_log_login')
            and   type = 'U')
   drop table rp_log_login
go

/*==============================================================*/
/* Table: rp_cdkey                                              */
/*==============================================================*/
create table rp_cdkey (
   Id                   varchar(36)          not null,
   EntId                int                  null,
   UseProv              int                  null,
   UseCity              int                  null,
   UseDist              int                  null,
   UseSchool            varchar(100)         null,
   Validity             int                  null,
   ExpirTime            datetime             null,
   AuthUserCount        int                  null,
   UseUser              varchar(50)          null,
   ActiveMac            varchar(50)          null,
   ActiveDisk           varchar(50)          null,
   ActiveTime           datetime             null,
   ActiveIp             varchar(30)          null,
   ActiveCode           varchar(300)         null,
   Remark               varchar(500)         null,
   constraint PK_RP_CDKEY primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'null就是无限期',
   'user', @CurrentUser, 'table', 'rp_cdkey', 'column', 'Validity'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'null为不限人数',
   'user', @CurrentUser, 'table', 'rp_cdkey', 'column', 'AuthUserCount'
go

/*==============================================================*/
/* Table: rp_cdkey_and_edition                                  */
/*==============================================================*/
create table rp_cdkey_and_edition (
   Id                   varchar(36)          not null,
   CdKey                varchar(36)          not null,
   Subject              int                  not null,
   Edition              int                  not null,
   constraint PK_RP_CDKEY_AND_EDITION primary key (Id)
)
go

/*==============================================================*/
/* Table: rp_enterprise                                         */
/*==============================================================*/
create table rp_enterprise (
   EntId                int                  identity,
   EntName              varchar(100)         not null,
   Remark               varchar(500)         null,
   constraint PK_RP_ENTERPRISE primary key (EntId)
)
go

/*==============================================================*/
/* Table: rp_log_login                                          */
/*==============================================================*/
if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('rp_log_login') and o.name = 'FK_RP_LOG_L_REFERENCE_RP_CDKEY')
alter table rp_log_login
   drop constraint FK_RP_LOG_L_REFERENCE_RP_CDKEY
go

if exists (select 1
            from  sysobjects
           where  id = object_id('rp_log_login')
            and   type = 'U')
   drop table rp_log_login
go

/*==============================================================*/
/* Table: rp_log_login                                          */
/*==============================================================*/
create table rp_log_login (
   Id                   varchar(36)          not null,
   LoginAccount         varchar(50)          null,
   LoginUserName        varchar(50)          null,
   LoginSchool          varchar(100)         null,
   LoginRoleName        varchar(20)          null,
   LoginEnte            varchar(100)         null,
   LoginDistName        varchar(100)         null,
   LoginKey             varchar(36)          null,
   LoginTime            datetime             null,
   LoginIP              varchar(50)          null,
   constraint PK_RP_LOG_LOGIN primary key (Id)
)
go

alter table rp_log_login
   add constraint FK_RP_LOG_L_REFERENCE_RP_CDKEY foreign key (LoginKey)
      references rp_cdkey (Id)
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('rp_resource_statist') and o.name = 'FK_RP_RESOU_REFERENCE_RP_CDKEY')
alter table rp_resource_statist
   drop constraint FK_RP_RESOU_REFERENCE_RP_CDKEY
go

if exists (select 1
            from  sysobjects
           where  id = object_id('rp_resource_statist')
            and   type = 'U')
   drop table rp_resource_statist
go

/*==============================================================*/
/* Table: rp_resource_statist                                   */
/*==============================================================*/
create table rp_resource_statist (
   Id                   varchar(36)          not null,
   Cd_key               varchar(36)          null,
   SubjectId            int                  null,
   EditionId            int                  null,
   EditionName          varchar(30)          null,
   Type                 int                  null,
   ResourceType         int                  null,
   CreateTime           datetime             null,
   UserName             varchar(30)          null,
   RoleName             varchar(30)          null,
   SchoolName           varchar(30)          null,
   constraint PK_RP_RESOURCE_STATIST primary key (Id)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '1：电子教材；2：电影课；3：云课堂',
   'user', @CurrentUser, 'table', 'rp_resource_statist', 'column', 'ResourceType'
go

alter table rp_resource_statist
   add constraint FK_RP_RESOU_REFERENCE_RP_CDKEY foreign key (Cd_key)
      references rp_cdkey (Id)
go
