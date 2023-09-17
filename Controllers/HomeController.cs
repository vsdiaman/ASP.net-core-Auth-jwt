using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Shop.Services;
using Shop.Repositories;

namespace Shop.Controllers
{
    [Route("v1/account")]
    public class HomeController : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody]User model)
        {
            var user = UserRepository.Get(model.Username, model.Password);
            if(user == null)
            return NotFound(new {message = "User not found"});

            var token = TokenService.GenerateToken(user);
            user.Password = "";
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]

        public string anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        
        public string authenticated() => String.Format("Autenticado = {0}", User.Identity.Name);

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee, manager")]

        public string Anonymous() => "Funcionário";    

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]

        public string Employee() => "Gerente";

      }
    }