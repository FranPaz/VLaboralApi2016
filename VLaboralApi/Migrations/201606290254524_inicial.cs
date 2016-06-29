namespace VLaboralApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BlobUploadModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FileUrl = c.String(),
                        FileSizeInBytes = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Empresas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RazonSocial = c.String(),
                        NombreFantasia = c.String(),
                        UrlImagenPerfil = c.String(),
                        Descripcion = c.String(),
                        Direccion = c.String(),
                        Telefono = c.String(),
                        SitioWeb = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ofertas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        FechaInicioConvocatoria = c.String(),
                        FechaFinConvocatoria = c.String(),
                        Publica = c.Boolean(nullable: false),
                        EmpresaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Empresas", t => t.EmpresaId, cascadeDelete: true)
                .Index(t => t.EmpresaId);
            
            CreateTable(
                "dbo.OfertaEstadoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaEstado = c.String(),
                        TipoEstadoOfertaId = c.Int(nullable: false),
                        OfertaId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ofertas", t => t.OfertaId, cascadeDelete: true)
                .ForeignKey("dbo.TipoEstadoOfertas", t => t.TipoEstadoOfertaId, cascadeDelete: true)
                .Index(t => t.TipoEstadoOfertaId)
                .Index(t => t.OfertaId);
            
            CreateTable(
                "dbo.TipoEstadoOfertas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Puestoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        Ubicacion = c.String(),
                        Remuneracion = c.String(),
                        Vacantes = c.Int(nullable: false),
                        OfertaId = c.Int(nullable: false),
                        TipoContratoId = c.Int(nullable: false),
                        TipoDisponibilidadId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TipoDisponibilidads", t => t.TipoDisponibilidadId, cascadeDelete: true)
                .ForeignKey("dbo.Ofertas", t => t.OfertaId, cascadeDelete: true)
                .ForeignKey("dbo.TipoContratoes", t => t.TipoContratoId, cascadeDelete: true)
                .Index(t => t.OfertaId)
                .Index(t => t.TipoContratoId)
                .Index(t => t.TipoDisponibilidadId);
            
            CreateTable(
                "dbo.TipoDisponibilidads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Requisitoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Valor = c.String(),
                        Excluyente = c.Boolean(nullable: false),
                        PuestoId = c.Int(nullable: false),
                        TipoRequisitoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Puestoes", t => t.PuestoId, cascadeDelete: true)
                .ForeignKey("dbo.TipoRequisitoes", t => t.TipoRequisitoId, cascadeDelete: true)
                .Index(t => t.PuestoId)
                .Index(t => t.TipoRequisitoId);
            
            CreateTable(
                "dbo.TipoRequisitoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubRubroes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        RubroId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Rubroes", t => t.RubroId, cascadeDelete: true)
                .Index(t => t.RubroId);
            
            CreateTable(
                "dbo.Profesionals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        Nacionalidad = c.String(),
                        FechaNac = c.DateTime(),
                        Domicilio = c.String(),
                        ObjetivoProfesional = c.String(),
                        DescripcionCurricular = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IdentificacionProfesionals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Valor = c.String(),
                        TipoIdentificacionProfesionalId = c.Int(nullable: false),
                        ProfesionalId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Profesionals", t => t.ProfesionalId, cascadeDelete: true)
                .ForeignKey("dbo.TipoIdentificacionProfesionals", t => t.TipoIdentificacionProfesionalId, cascadeDelete: true)
                .Index(t => t.TipoIdentificacionProfesionalId)
                .Index(t => t.ProfesionalId);
            
            CreateTable(
                "dbo.TipoIdentificacionProfesionals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Rubroes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TipoContratoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Habilidads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false, maxLength: 100),
                        LastName = c.String(nullable: false, maxLength: 100),
                        Level = c.Byte(nullable: false),
                        JoinDate = c.DateTime(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ProfesionalSubRubroes",
                c => new
                    {
                        Profesional_Id = c.Int(nullable: false),
                        SubRubro_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Profesional_Id, t.SubRubro_Id })
                .ForeignKey("dbo.Profesionals", t => t.Profesional_Id, cascadeDelete: true)
                .ForeignKey("dbo.SubRubroes", t => t.SubRubro_Id, cascadeDelete: true)
                .Index(t => t.Profesional_Id)
                .Index(t => t.SubRubro_Id);
            
            CreateTable(
                "dbo.SubRubroPuestoes",
                c => new
                    {
                        SubRubro_Id = c.Int(nullable: false),
                        Puesto_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SubRubro_Id, t.Puesto_Id })
                .ForeignKey("dbo.SubRubroes", t => t.SubRubro_Id, cascadeDelete: true)
                .ForeignKey("dbo.Puestoes", t => t.Puesto_Id, cascadeDelete: true)
                .Index(t => t.SubRubro_Id)
                .Index(t => t.Puesto_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Puestoes", "TipoContratoId", "dbo.TipoContratoes");
            DropForeignKey("dbo.SubRubroes", "RubroId", "dbo.Rubroes");
            DropForeignKey("dbo.SubRubroPuestoes", "Puesto_Id", "dbo.Puestoes");
            DropForeignKey("dbo.SubRubroPuestoes", "SubRubro_Id", "dbo.SubRubroes");
            DropForeignKey("dbo.ProfesionalSubRubroes", "SubRubro_Id", "dbo.SubRubroes");
            DropForeignKey("dbo.ProfesionalSubRubroes", "Profesional_Id", "dbo.Profesionals");
            DropForeignKey("dbo.IdentificacionProfesionals", "TipoIdentificacionProfesionalId", "dbo.TipoIdentificacionProfesionals");
            DropForeignKey("dbo.IdentificacionProfesionals", "ProfesionalId", "dbo.Profesionals");
            DropForeignKey("dbo.Requisitoes", "TipoRequisitoId", "dbo.TipoRequisitoes");
            DropForeignKey("dbo.Requisitoes", "PuestoId", "dbo.Puestoes");
            DropForeignKey("dbo.Puestoes", "OfertaId", "dbo.Ofertas");
            DropForeignKey("dbo.Puestoes", "TipoDisponibilidadId", "dbo.TipoDisponibilidads");
            DropForeignKey("dbo.OfertaEstadoes", "TipoEstadoOfertaId", "dbo.TipoEstadoOfertas");
            DropForeignKey("dbo.OfertaEstadoes", "OfertaId", "dbo.Ofertas");
            DropForeignKey("dbo.Ofertas", "EmpresaId", "dbo.Empresas");
            DropIndex("dbo.SubRubroPuestoes", new[] { "Puesto_Id" });
            DropIndex("dbo.SubRubroPuestoes", new[] { "SubRubro_Id" });
            DropIndex("dbo.ProfesionalSubRubroes", new[] { "SubRubro_Id" });
            DropIndex("dbo.ProfesionalSubRubroes", new[] { "Profesional_Id" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.IdentificacionProfesionals", new[] { "ProfesionalId" });
            DropIndex("dbo.IdentificacionProfesionals", new[] { "TipoIdentificacionProfesionalId" });
            DropIndex("dbo.SubRubroes", new[] { "RubroId" });
            DropIndex("dbo.Requisitoes", new[] { "TipoRequisitoId" });
            DropIndex("dbo.Requisitoes", new[] { "PuestoId" });
            DropIndex("dbo.Puestoes", new[] { "TipoDisponibilidadId" });
            DropIndex("dbo.Puestoes", new[] { "TipoContratoId" });
            DropIndex("dbo.Puestoes", new[] { "OfertaId" });
            DropIndex("dbo.OfertaEstadoes", new[] { "OfertaId" });
            DropIndex("dbo.OfertaEstadoes", new[] { "TipoEstadoOfertaId" });
            DropIndex("dbo.Ofertas", new[] { "EmpresaId" });
            DropTable("dbo.SubRubroPuestoes");
            DropTable("dbo.ProfesionalSubRubroes");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Habilidads");
            DropTable("dbo.TipoContratoes");
            DropTable("dbo.Rubroes");
            DropTable("dbo.TipoIdentificacionProfesionals");
            DropTable("dbo.IdentificacionProfesionals");
            DropTable("dbo.Profesionals");
            DropTable("dbo.SubRubroes");
            DropTable("dbo.TipoRequisitoes");
            DropTable("dbo.Requisitoes");
            DropTable("dbo.TipoDisponibilidads");
            DropTable("dbo.Puestoes");
            DropTable("dbo.TipoEstadoOfertas");
            DropTable("dbo.OfertaEstadoes");
            DropTable("dbo.Ofertas");
            DropTable("dbo.Empresas");
            DropTable("dbo.BlobUploadModels");
        }
    }
}
