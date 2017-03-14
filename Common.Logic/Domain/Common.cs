

// This file was automatically generated.
// Do not make changes directly to this file - edit the template instead.
// 
// The following connection settings were used to generate this file
// 
//     Connection String Name: "Common"
//     Connection String:      "Data Source=data.mgcc.com.cn;Initial Catalog=GH_WebWechat;User ID=demo_ghwc;PWD=ghwc1234;"

// ReSharper disable RedundantUsingDirective
// ReSharper disable DoNotCallOverridableMethodsInConstructor
// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Newtonsoft.Json;
using System.ComponentModel;

//using DatabaseGeneratedOption = System.ComponentModel.DataAnnotations.DatabaseGeneratedOption;

namespace Common.Logic.Domain
{
    // ************************************************************************
    // Database context
    public partial class CommonEntities : Formula.FormulaDbContext
    {
        public IDbSet<FansCheckFriends> FansCheckFriends { get; set; } // FansCheckFriends
        public IDbSet<FansFriends> FansFriends { get; set; } // FansFriends
        public IDbSet<FansInfo> FansInfo { get; set; } // FansInfo
        public IDbSet<FansLogin> FansLogin { get; set; } // FansLogin
        public IDbSet<UserInfo> UserInfo { get; set; } // UserInfo

        static CommonEntities()
        {
            Database.SetInitializer<CommonEntities>(null);
        }

        public CommonEntities()
            : base("Name=Common")
        {
        }

        public CommonEntities(string connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new FansCheckFriendsConfiguration());
            modelBuilder.Configurations.Add(new FansFriendsConfiguration());
            modelBuilder.Configurations.Add(new FansInfoConfiguration());
            modelBuilder.Configurations.Add(new FansLoginConfiguration());
            modelBuilder.Configurations.Add(new UserInfoConfiguration());
        }
    }

    // ************************************************************************
    // POCO classes

	/// <summary></summary>	
	[Serializable]
	[Description("")]
    public partial class FansCheckFriends : Formula.BaseModel
    {
		/// <summary></summary>	
		[Description("")]
        public string ID { get{return _ID;} set{_ID = value??"";} } // ID (Primary key)
		private string _ID="";
		/// <summary></summary>	
		[Description("")]
        public string FansID { get{return _FansID;} set{_FansID = value??"";} } // FansID
		private string _FansID="";
		/// <summary></summary>	
		[Description("")]
        public string LoginID { get{return _LoginID;} set{_LoginID = value??"";} } // LoginID
		private string _LoginID="";
		/// <summary></summary>	
		[Description("")]
        public string DeletedNameList { get{return _DeletedNameList;} set{_DeletedNameList = value??"";} } // DeletedNameList
		private string _DeletedNameList="";
		/// <summary></summary>	
		[Description("")]
        public string BlockedNameList { get{return _BlockedNameList;} set{_BlockedNameList = value??"";} } // BlockedNameList
		private string _BlockedNameList="";
		/// <summary></summary>	
		[Description("")]
        public string DeletedIDList { get{return _DeletedIDList;} set{_DeletedIDList = value??"";} } // DeletedIDList
		private string _DeletedIDList="";
		/// <summary></summary>	
		[Description("")]
        public string BlockedIDList { get{return _BlockedIDList;} set{_BlockedIDList = value??"";} } // BlockedIDList
		private string _BlockedIDList="";
    }

	/// <summary></summary>	
	[Serializable]
	[Description("")]
    public partial class FansFriends : Formula.BaseModel
    {
		/// <summary></summary>	
		[Description("")]
        public string ID { get{return _ID;} set{_ID = value??"";} } // ID (Primary key)
		private string _ID="";
		/// <summary></summary>	
		[Description("")]
        public string FansID { get{return _FansID;} set{_FansID = value??"";} } // FansID
		private string _FansID="";
		/// <summary></summary>	
		[Description("")]
        public string OpenID { get{return _OpenID;} set{_OpenID = value??"";} } // OpenID
		private string _OpenID="";
		/// <summary></summary>	
		[Description("")]
        public string NickName { get{return _NickName;} set{_NickName = value??"";} } // NickName
		private string _NickName="";
		/// <summary></summary>	
		[Description("")]
        public string HeadImgUrl { get{return _HeadImgUrl;} set{_HeadImgUrl = value??"";} } // HeadImgUrl
		private string _HeadImgUrl="";
		/// <summary></summary>	
		[Description("")]
        public int? ContactFlag { get; set; } // ContactFlag
		/// <summary></summary>	
		[Description("")]
        public string RemarkName { get{return _RemarkName;} set{_RemarkName = value??"";} } // RemarkName
		private string _RemarkName="";
		/// <summary></summary>	
		[Description("")]
        public int? Sex { get; set; } // Sex
		/// <summary></summary>	
		[Description("")]
        public string Signature { get{return _Signature;} set{_Signature = value??"";} } // Signature
		private string _Signature="";
		/// <summary></summary>	
		[Description("")]
        public int? VerifyFlag { get; set; } // VerifyFlag
		/// <summary></summary>	
		[Description("")]
        public int? StarFriend { get; set; } // StarFriend
		/// <summary></summary>	
		[Description("")]
        public string Province { get{return _Province;} set{_Province = value??"";} } // Province
		private string _Province="";
		/// <summary></summary>	
		[Description("")]
        public string City { get{return _City;} set{_City = value??"";} } // City
		private string _City="";
		/// <summary></summary>	
		[Description("")]
        public string Alias { get{return _Alias;} set{_Alias = value??"";} } // Alias
		private string _Alias="";
		/// <summary></summary>	
		[Description("")]
        public int? SnsFlag { get; set; } // SnsFlag
		/// <summary></summary>	
		[Description("")]
        public string KeyWord { get{return _KeyWord;} set{_KeyWord = value??"";} } // KeyWord
		private string _KeyWord="";
    }

	/// <summary></summary>	
	[Serializable]
	[Description("")]
    public partial class FansInfo : Formula.BaseModel
    {
		/// <summary></summary>	
		[Description("")]
        public string ID { get{return _ID;} set{_ID = value??"";} } // ID (Primary key)
		private string _ID="";
		/// <summary></summary>	
		[Description("")]
        public string OpenID { get{return _OpenID;} set{_OpenID = value??"";} } // OpenID
		private string _OpenID="";
		/// <summary></summary>	
		[Description("")]
        public string NickName { get{return _NickName;} set{_NickName = value??"";} } // NickName
		private string _NickName="";
		/// <summary></summary>	
		[Description("")]
        public string HeadImgUrl { get{return _HeadImgUrl;} set{_HeadImgUrl = value??"";} } // HeadImgUrl
		private string _HeadImgUrl="";
		/// <summary></summary>	
		[Description("")]
        public string RemarkName { get{return _RemarkName;} set{_RemarkName = value??"";} } // RemarkName
		private string _RemarkName="";
		/// <summary></summary>	
		[Description("")]
        public int? Sex { get; set; } // Sex
		/// <summary></summary>	
		[Description("")]
        public string Signature { get{return _Signature;} set{_Signature = value??"";} } // Signature
		private string _Signature="";
		/// <summary></summary>	
		[Description("")]
        public int? VerifyFlag { get; set; } // VerifyFlag
		/// <summary></summary>	
		[Description("")]
        public int? SnsFlag { get; set; } // SnsFlag
    }

	/// <summary></summary>	
	[Serializable]
	[Description("")]
    public partial class FansLogin : Formula.BaseModel
    {
		/// <summary></summary>	
		[Description("")]
        public string ID { get{return _ID;} set{_ID = value??"";} } // ID (Primary key)
		private string _ID="";
		/// <summary></summary>	
		[Description("")]
        public string OpenID { get{return _OpenID;} set{_OpenID = value??"";} } // OpenID
		private string _OpenID="";
		/// <summary></summary>	
		[Description("")]
        public bool? IsReply { get; set; } // IsReply
		/// <summary></summary>	
		[Description("")]
        public bool? IsSendQR { get; set; } // IsSendQR
		/// <summary></summary>	
		[Description("")]
        public bool? IsLogin { get; set; } // IsLogin
		/// <summary></summary>	
		[Description("")]
        public bool? IsFinish { get; set; } // IsFinish
		/// <summary></summary>	
		[Description("")]
        public DateTime? ApplyDate { get; set; } // ApplyDate
		/// <summary></summary>	
		[Description("")]
        public DateTime? ReplyTime { get; set; } // ReplyTime
		/// <summary></summary>	
		[Description("")]
        public DateTime? SendQRTime { get; set; } // SendQRTime
		/// <summary></summary>	
		[Description("")]
        public DateTime? LoginTime { get; set; } // LoginTime
		/// <summary></summary>	
		[Description("")]
        public DateTime? LogoutTime { get; set; } // LogoutTime
		/// <summary></summary>	
		[Description("")]
        public string Cookies { get{return _Cookies;} set{_Cookies = value??"";} } // Cookies
		private string _Cookies="";
		/// <summary></summary>	
		[Description("")]
        public string SessionID { get{return _SessionID;} set{_SessionID = value??"";} } // SessionID
		private string _SessionID="";
		/// <summary></summary>	
		[Description("")]
        public string Skey { get{return _Skey;} set{_Skey = value??"";} } // Skey
		private string _Skey="";
		/// <summary></summary>	
		[Description("")]
        public string PassTicket { get{return _PassTicket;} set{_PassTicket = value??"";} } // PassTicket
		private string _PassTicket="";
		/// <summary></summary>	
		[Description("")]
        public string SyncKey { get{return _SyncKey;} set{_SyncKey = value??"";} } // SyncKey
		private string _SyncKey="";
    }

	/// <summary></summary>	
	[Serializable]
	[Description("")]
    public partial class UserInfo : Formula.BaseModel
    {
		/// <summary></summary>	
		[Description("")]
        public string ID { get{return _ID;} set{_ID = value??"";} } // ID (Primary key)
		private string _ID="";
		/// <summary></summary>	
		[Description("")]
        public string SystemName { get{return _SystemName;} set{_SystemName = value??"";} } // SystemName
		private string _SystemName="";
		/// <summary></summary>	
		[Description("")]
        public string UserName { get{return _UserName;} set{_UserName = value??"";} } // UserName
		private string _UserName="";
		/// <summary></summary>	
		[Description("")]
        public string Password { get{return _Password;} set{_Password = value??"";} } // Password
		private string _Password="";
		/// <summary></summary>	
		[Description("")]
        public string BelongType { get{return _BelongType;} set{_BelongType = value??"";} } // BelongType
		private string _BelongType="";
		/// <summary></summary>	
		[Description("")]
        public int? ErrorCount { get; set; } // ErrorCount
		/// <summary></summary>	
		[Description("")]
        public DateTime? LastLoginTime { get; set; } // LastLoginTime
		/// <summary></summary>	
		[Description("")]
        public bool? IsDelete { get; set; } // IsDelete
    }


    // ************************************************************************
    // POCO Configuration

    // FansCheckFriends
    internal partial class FansCheckFriendsConfiguration : EntityTypeConfiguration<FansCheckFriends>
    {
        public FansCheckFriendsConfiguration()
        {
			ToTable("FANSCHECKFRIENDS");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasMaxLength(50);
            Property(x => x.FansID).HasColumnName("FANSID").IsOptional().HasMaxLength(50);
            Property(x => x.LoginID).HasColumnName("LOGINID").IsOptional().HasMaxLength(50);
            Property(x => x.DeletedNameList).HasColumnName("DELETEDNAMELIST").IsOptional().HasMaxLength(1073741823);
            Property(x => x.BlockedNameList).HasColumnName("BLOCKEDNAMELIST").IsOptional().HasMaxLength(1073741823);
            Property(x => x.DeletedIDList).HasColumnName("DELETEDIDLIST").IsOptional().HasMaxLength(1073741823);
            Property(x => x.BlockedIDList).HasColumnName("BLOCKEDIDLIST").IsOptional().HasMaxLength(1073741823);
        }
    }

    // FansFriends
    internal partial class FansFriendsConfiguration : EntityTypeConfiguration<FansFriends>
    {
        public FansFriendsConfiguration()
        {
			ToTable("FANSFRIENDS");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasMaxLength(50);
            Property(x => x.FansID).HasColumnName("FANSID").IsOptional().HasMaxLength(50);
            Property(x => x.OpenID).HasColumnName("OPENID").IsOptional().HasMaxLength(200);
            Property(x => x.NickName).HasColumnName("NICKNAME").IsOptional().HasMaxLength(2000);
            Property(x => x.HeadImgUrl).HasColumnName("HEADIMGURL").IsOptional().HasMaxLength(500);
            Property(x => x.ContactFlag).HasColumnName("CONTACTFLAG").IsOptional();
            Property(x => x.RemarkName).HasColumnName("REMARKNAME").IsOptional().HasMaxLength(2000);
            Property(x => x.Sex).HasColumnName("SEX").IsOptional();
            Property(x => x.Signature).HasColumnName("SIGNATURE").IsOptional();
            Property(x => x.VerifyFlag).HasColumnName("VERIFYFLAG").IsOptional();
            Property(x => x.StarFriend).HasColumnName("STARFRIEND").IsOptional();
            Property(x => x.Province).HasColumnName("PROVINCE").IsOptional().HasMaxLength(50);
            Property(x => x.City).HasColumnName("CITY").IsOptional().HasMaxLength(50);
            Property(x => x.Alias).HasColumnName("ALIAS").IsOptional().HasMaxLength(200);
            Property(x => x.SnsFlag).HasColumnName("SNSFLAG").IsOptional();
            Property(x => x.KeyWord).HasColumnName("KEYWORD").IsOptional().HasMaxLength(200);
        }
    }

    // FansInfo
    internal partial class FansInfoConfiguration : EntityTypeConfiguration<FansInfo>
    {
        public FansInfoConfiguration()
        {
			ToTable("FANSINFO");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasMaxLength(50);
            Property(x => x.OpenID).HasColumnName("OPENID").IsOptional().HasMaxLength(200);
            Property(x => x.NickName).HasColumnName("NICKNAME").IsOptional().HasMaxLength(200);
            Property(x => x.HeadImgUrl).HasColumnName("HEADIMGURL").IsOptional().HasMaxLength(500);
            Property(x => x.RemarkName).HasColumnName("REMARKNAME").IsOptional().HasMaxLength(200);
            Property(x => x.Sex).HasColumnName("SEX").IsOptional();
            Property(x => x.Signature).HasColumnName("SIGNATURE").IsOptional();
            Property(x => x.VerifyFlag).HasColumnName("VERIFYFLAG").IsOptional();
            Property(x => x.SnsFlag).HasColumnName("SNSFLAG").IsOptional();
        }
    }

    // FansLogin
    internal partial class FansLoginConfiguration : EntityTypeConfiguration<FansLogin>
    {
        public FansLoginConfiguration()
        {
			ToTable("FANSLOGIN");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasMaxLength(50);
            Property(x => x.OpenID).HasColumnName("OPENID").IsOptional().HasMaxLength(200);
            Property(x => x.IsReply).HasColumnName("ISREPLY").IsOptional();
            Property(x => x.IsSendQR).HasColumnName("ISSENDQR").IsOptional();
            Property(x => x.IsLogin).HasColumnName("ISLOGIN").IsOptional();
            Property(x => x.IsFinish).HasColumnName("ISFINISH").IsOptional();
            Property(x => x.ApplyDate).HasColumnName("APPLYDATE").IsOptional();
            Property(x => x.ReplyTime).HasColumnName("REPLYTIME").IsOptional();
            Property(x => x.SendQRTime).HasColumnName("SENDQRTIME").IsOptional();
            Property(x => x.LoginTime).HasColumnName("LOGINTIME").IsOptional();
            Property(x => x.LogoutTime).HasColumnName("LOGOUTTIME").IsOptional();
            Property(x => x.Cookies).HasColumnName("COOKIES").IsOptional().HasMaxLength(1073741823);
            Property(x => x.SessionID).HasColumnName("SESSIONID").IsOptional().HasMaxLength(500);
            Property(x => x.Skey).HasColumnName("SKEY").IsOptional().HasMaxLength(500);
            Property(x => x.PassTicket).HasColumnName("PASSTICKET").IsOptional().HasMaxLength(500);
            Property(x => x.SyncKey).HasColumnName("SYNCKEY").IsOptional().HasMaxLength(1073741823);
        }
    }

    // UserInfo
    internal partial class UserInfoConfiguration : EntityTypeConfiguration<UserInfo>
    {
        public UserInfoConfiguration()
        {
			ToTable("USERINFO");
            HasKey(x => x.ID);

            Property(x => x.ID).HasColumnName("ID").IsRequired().HasMaxLength(50);
            Property(x => x.SystemName).HasColumnName("SYSTEMNAME").IsOptional().HasMaxLength(200);
            Property(x => x.UserName).HasColumnName("USERNAME").IsOptional().HasMaxLength(200);
            Property(x => x.Password).HasColumnName("PASSWORD").IsOptional().HasMaxLength(1000);
            Property(x => x.BelongType).HasColumnName("BELONGTYPE").IsOptional().HasMaxLength(50);
            Property(x => x.ErrorCount).HasColumnName("ERRORCOUNT").IsOptional();
            Property(x => x.LastLoginTime).HasColumnName("LASTLOGINTIME").IsOptional();
            Property(x => x.IsDelete).HasColumnName("ISDELETE").IsOptional();
        }
    }

}

