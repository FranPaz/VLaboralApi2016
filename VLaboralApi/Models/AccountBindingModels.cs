﻿using System.ComponentModel.DataAnnotations;

namespace VLaboralApi.Models
{
    public class CreateUserBindingModel
    {       

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
        
    }

    public class modeloCreacionUsuarioProfesional : CreateUserBindingModel
    {

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }
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

    public class ForgotPasswordViewModel
    {
        public string Email { get; set; }
    }

    public class ResetPasswordViewModel
    {
        public string Code { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}