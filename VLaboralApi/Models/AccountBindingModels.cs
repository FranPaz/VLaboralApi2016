using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VLaboralApi.Models
{
    public class CreateUserBindingModel
    {
        [Required]
        [Display(Name = "Valor del Tipo de Identificacion")]
        public string ValorIdentificacion { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
    }
    public class modeloCreacionUsuarioEmpresa : CreateUserBindingModel
    {
        [Required]
        [Display(Name = "Razon Social")]
        public string RazonSocial { get; set; }

        [Required]
        [Display(Name = "Tipo de Identificacion de Empresa")]
        public TipoIdentificacionEmpresa TipoIdentificacionEmpresa { get; set; }
    }

    public class modeloCreacionUsuarioProfesional : CreateUserBindingModel
    {

        [Required]
        [Display(Name = "Tipo de Identificacion")]
        public TipoIdentificacionProfesional TipoIdentificacionProfesional { get; set; }
    }

    public class ChangePasswordBindingModel
    {

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}