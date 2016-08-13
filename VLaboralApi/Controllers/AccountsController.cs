using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using VlaboralApi.Infrastructure;
using VLaboralApi.Models;


namespace VLaboralApi.Controllers
{
    [RoutePrefix("api/accounts")]
    //clase responsable de manejar las cuentas de usuario hereda de BaseApiController
    public class AccountsController : BaseApiController
    {
        [Authorize(Roles = "Admin")]
        [Route("users")]
        public IHttpActionResult GetUsers()
        {
            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}", Name = "GetUserById")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [Authorize(Roles = "Admin")]
        [Route("user/{username}")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        [AllowAnonymous]
        [Route("createEmpresa")]
        public async Task<IHttpActionResult> CreateUserEmpresa(modeloCreacionUsuarioEmpresa createUserModel) //fpaz: funcion para crear un usuario del tipo Empresa
        {            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                VLaboral_Context db = new VLaboral_Context();                

                //fpaz: agrego un nuevo usuario de la aplicacion usando identity framework
                var user = new ApplicationUser()
                {
                    UserName = createUserModel.Email,
                    Email = createUserModel.Email, 
                    FechaAlta = DateTime.Now
                };

                IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

                if (!addUserResult.Succeeded)
                {
                    return GetErrorResult(addUserResult);
                }
                else
                {
                    #region alta de nueva empresa
                    //fpaz: doy de alta una nueva instancia empresa que va a estar relacionada con el usuario del tipo empresa 
                    Empresa emp = new Empresa { RazonSocial = createUserModel.RazonSocial };                    
                    db.Empresas.Add(emp);
                    db.SaveChanges();
                    #endregion

                    #region Alta de Claims y Roles para el nuevo Usuario del tipo Empresa
                    //fpaz: agrego al Usuario de la Aplicacion los Claims que van a tener informacion de la empresa a la cual esta asociado el Usuario del Tipo Empresa
                    await this.AppUserManager.AddClaimAsync(user.Id, new Claim("app_usertype", "empresa"));
                    await this.AppUserManager.AddClaimAsync(user.Id, new Claim("empresaId", emp.Id.ToString()));

                    //fpaz: agrego los roles del usuario                    
                    await this.AppUserManager.AddToRolesAsync(user.Id, new string[] { "Empresa" });
                    #endregion


                    #region Envio de Email de Confirmacion
                    string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                    var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));

                    await this.AppUserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
                    #endregion

                    return Created(locationHeader, TheModelFactory.Create(user));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [AllowAnonymous]
        [Route("createProfesional")]
        public async Task<IHttpActionResult> CreateUserProfesional(modeloCreacionUsuarioProfesional createUserModel) //fpaz: funcion para crear un usuario del tipo Profesional
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                VLaboral_Context db = new VLaboral_Context();

                //fpaz: agrego un nuevo usuario de la aplicacion usando identity framework
                var user = new ApplicationUser()
                {
                    UserName = createUserModel.Email,
                    Email = createUserModel.Email,
                    FechaAlta = DateTime.Now
                };

                IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

                if (!addUserResult.Succeeded)
                {
                    return GetErrorResult(addUserResult);
                }
                else
                {
                    #region alta de nueva profesional
                    //fpaz: doy de alta una nueva instancia Profesional que va a estar relacionada con el usuario del tipo Profesional 
                    Profesional prof = new Profesional { Nombre= createUserModel.Nombre, Apellido=createUserModel.Apellido};

                    db.Profesionals.Add(prof);
                    db.SaveChanges();
                    #endregion

                    #region Alta de Claims y Roles para el nuevo Usuario del tipo Profesional
                    //fpaz: agrego al Usuario de la Aplicacion los Claims que van a tener informacion de la empresa a la cual esta asociado el Usuario del Tipo Empresa
                    await this.AppUserManager.AddClaimAsync(user.Id, new Claim("app_usertype", "profesional"));
                    await this.AppUserManager.AddClaimAsync(user.Id, new Claim("profesionalId", prof.Id.ToString()));

                    //fpaz: agrego los roles del usuario                    
                    await this.AppUserManager.AddToRolesAsync(user.Id, new string[] { "Profesional" });
                    #endregion


                    #region Envio de Email de Confirmacion
                    string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);

                    var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));

                    await this.AppUserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
                    #endregion

                    return Created(locationHeader, TheModelFactory.Create(user));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded) //fpaz: agrego la funcion para que la url que se manda en el email de confirmacion redirija a la pagina de confirmacion de cuenta
            {
                IHttpActionResult response;
                //we want a 303 with the ability to set location
                HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.RedirectMethod);
                //seteo la url a la que voy a redirigir y capturar en el frontend
                responseMsg.Headers.Location = new Uri(ConfigurationManager.AppSettings["urlWeb"]); //fpaz: url del frontend que se toma desde las configuraciones en el webconfig                 
                response = ResponseMessage(responseMsg);
                return response;
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {

            //Only SuperAdmin or Admin can delete users (Later when implement roles)

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();

            }

            return NotFound();

        }

        [Authorize(Roles = "Admin")]
        [Route("user/{id:guid}/roles")]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
            {
                return NotFound();
            }

            var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

            if (rolesNotExists.Count() > 0)
            {

                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }

            IdentityResult removeResult = await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }


    }
}
